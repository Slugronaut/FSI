using IronBarCode;
using Ookii.Dialogs;
using SAOT.Forms;
using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SAOT
{
    public static class WindowUtils
    {
        /// <summary>
        /// Returns the index of a column with a given header name.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="headerId"></param>
        /// <returns></returns>
        public static int GetHeaderIndex(DataGridView grid, string headerId)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (grid.Columns[i].HeaderText == headerId)
                    return i;
            }

            return -1;
        }

        public static void HandleOpenLocationView(Object sender, DataGridViewCellMouseEventArgs args, User currentUser, Warehouse currentWarehouse, Project proj)
        {
            var grid = sender as DataGridView;
            if (grid == null)
            {
                var fGrid = sender as FilterableDataGridView;
                if (fGrid == null)
                {
                    Dialog.Message("Not a valid control for looking up document drawings.");
                    return;
                }
                grid = fGrid.GridView;
            }
            if (grid == null)
            {
                Dialog.Message("Could not locate grid view control.");
                return;
            }

            int colIndex = args.ColumnIndex;
            int rowIndex = args.RowIndex;
            var headerStr = grid.Columns[colIndex].HeaderText.ToUpper();
            if (rowIndex >= 0 && headerStr == "LOCATION")
            {
                var row = grid.Rows[args.RowIndex];
                var cell = row.Cells[colIndex];
                var locId = cell.Value as string;
                if (!string.IsNullOrWhiteSpace(locId))
                {
                    var loc = currentWarehouse.FindStorageLocation(locId);
                    if (loc == null)
                    {
                        Dialog.Message($"There was an error trying to access the location information '{locId}'.");
                        return;
                    }
                    var locForm = new LocationContentsForm(currentUser, proj, currentWarehouse, loc);
                    locForm.Show();
                }
            }
        }

        public static void HandleDrawingLookup(Object sender, DataGridViewCellMouseEventArgs args)
        {
            var grid = sender as DataGridView;
            if (grid == null)
            {
                var fGrid = sender as FilterableDataGridView;
                if(fGrid == null)
                {
                    Dialog.Message("Not a valid control for looking up document drawings.");
                    return;
                }
                grid = fGrid.GridView;
            }
            if(grid == null)
            {
                Dialog.Message("Could not locate grid view control.");
                return;
            }

            int colIndex = args.ColumnIndex;
            var headerStr = grid.Columns[colIndex].HeaderText.ToUpper();
            if (headerStr == "DOC" || headerStr == "KELOX" || headerStr == "OLD MATERIAL")
            {
                Task t = Task.Run(() =>
                {
                    var row = grid.Rows[args.RowIndex];
                    var cell = row.Cells[colIndex];
                    var docId = cell.Value as string;

                    var modAndRev = DrawingLookup.ModIdAndRev(docId);
                    if (modAndRev.Length == 5 && !string.IsNullOrEmpty(modAndRev[0]))
                    {
                        var file = DrawingLookup.FindDrawingFromModId(modAndRev[0], modAndRev[1], modAndRev[2], modAndRev[3], modAndRev[4]);
                        DrawingLookup.OpenPdfDoc(file);
                    }
                    else
                    {
                        Dialog.Message($"Could not determine Mod ID, part type, part number, or revision number.\nProcess: {modAndRev[1]}\nMod: {modAndRev[0]}\nPart Type: {modAndRev[2]}\nPart No: {modAndRev[3]}\nRev: {modAndRev[4]}");
                        return;
                    }
                });

            }

            //if we made it this far, this ain't a kelox number, just fall through
        }

        #region Packing Slip Vars
        const int oneInch = 96;
        const int qtrInch = (int)((float)oneInch * 0.25f);
        const int hlfInch = (int)((float)oneInch * 0.5f);
        static Font TitleFont = new Font("Calibri", 14);
        static Font PartsFont = new Font("Consolas", 10);
        static string PagenationText;
        static int OrderToPrintId;
        static List<PickListItem> PickList;
        static PickOrder Order;
        static Vendor ShipVendor;
        static Project CurrProj;
        #endregion


        #region Print Locations Sequence Logic
        static List<string> LocsToPrint = new List<string>(100);
        static Font LocsFont;

        static int LabelOffsetTop;
        static int LabelOffsetLeft;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="locs"></param>
        public static void PrintLocationSequence(Form parentForm, List<Aisle> locs, string fontName, float fontSize, float topMargin, float bottomMargin, float leftMargin)
        {
            var dialog = new System.Windows.Forms.PrintDialog();
            dialog.UseEXDialog = true;
            dialog.ShowNetwork = true;
            PrintDocument doc = new PrintDocument();
            doc.PrinterSettings.PrinterName = dialog.PrinterSettings.PrinterName;
            doc.PrintPage += OnPrintLocationSequence;
            doc.PrinterSettings.Copies = 1;
            doc.PrinterSettings.Duplex = Duplex.Simplex;
            LabelOffsetLeft = (int)(leftMargin * oneInch);
            LabelOffsetTop = (int)(topMargin * oneInch);
            dialog.Document = doc;
            LocsFont = new Font(fontName, fontSize, FontStyle.Bold);


            var result = dialog.ShowDialog(parentForm);
            if (result == DialogResult.OK)
            {
                doc.PrinterSettings = dialog.PrinterSettings;


                foreach(var aisle in locs)
                {
                    foreach(var rack in aisle.Racks)
                    {
                        foreach(var shelf in rack.Levels)
                        {
                            foreach(var bin in shelf.Bins)
                            {
                                LocsToPrint.Add($"{aisle.Id}{rack.Id}  {shelf.Id}{bin.Id}");
                            }
                        }
                    }
                }
                try
                {
                    doc.Print();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            dialog.Dispose();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static void OnPrintLocationSequence(Object sender, PrintPageEventArgs args)
        {
            var gr = args.Graphics;
            var text = string.Empty;
            const int locsPerPage = 1;

            int locPosX = 0;
            int locPosY = (int)((float)oneInch * 0.65);
            int bottomLine = (int)((float)oneInch * 2.85);


            for (int x = 0; x < locsPerPage; x++)
            {
                text = LocsToPrint[0];
                LocsToPrint.RemoveAt(0);
                gr.DrawString(text, LocsFont, Brushes.Black, locPosX + LabelOffsetLeft, locPosY + LabelOffsetTop);
            }


            gr.DrawLine(new Pen(Brushes.Black), new Point(0, bottomLine), new Point(args.PageBounds.Width, bottomLine));

            args.HasMorePages = LocsToPrint.Count > 0;
        }
        #endregion


        #region Print Packing Slip Logic
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="proj"></param>
        /// <param name="orderId"></param>
        public static void PrintPackingSlip(Form parentForm, Project proj, int orderId)
        {
            OrderToPrintId = orderId;
            CurrProj = proj;
            var dialog = new System.Windows.Forms.PrintDialog();
            dialog.UseEXDialog = true;
            dialog.ShowNetwork = true;
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += OnPrintPackingSlip;
            doc.PrinterSettings.Copies = 2;
            dialog.Document = doc;

            var result = dialog.ShowDialog(parentForm);

            if (result == DialogResult.OK)
            {
                doc.PrinterSettings = dialog.PrinterSettings;
                
                try
                {
                    PickList = PickOrdersDatabase.RequestPickList(orderId);
                    PickList = PickList.Where(item => item.Status != PickItemStatuses.Canceled).ToList();
                    Order = PickOrdersDatabase.RequestOrder(orderId);
                    ShipVendor = Vendor.RequestVendor(Order.ShipToId);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Unknown picklist id {orderId}. Could not print packing slip.");
                    return;
                }
                if (PickList == null || PickList.Count < 1 || Order == null)
                {
                    MessageBox.Show($"Order {orderId} has no items.");
                    return;
                }

                PagenationText = StringifyPickList(PickList);
                try
                {
                    doc.Print();
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            dialog.Dispose();
        }

        const int MatLen = 8;
        const int DocLen = 20;
        const int DescLen = 38;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picks"></param>
        /// <returns></returns>
        public static string StringifyPickList(List<PickListItem> picks)
        {
            StringBuilder sb = new StringBuilder(1000);

            sb.AppendFormat($"     {{0,-{MatLen}}} {{1,-{DocLen}}} {{2,-{DescLen}}}   Rqst/Picked", "Material", "Doc", "Desc");
            sb.AppendLine();
            sb.AppendLine();

            int line = 1;
            foreach (var pick in picks)
            {
                var mat = CurrProj.FindMaterial(pick.MaterialId);
                string docId = mat.DocumentId == null ? string.Empty : mat.DocumentId;
                string desc = mat.Description == null ? string.Empty : mat.Description;

                int docLen = Math.Min(docId.Length, DocLen);
                int descLen = Math.Min(desc.Length, DescLen);

                sb.AppendFormat($"#{line,-3} {pick.MaterialId, -MatLen} {docId.Substring(0,docLen),-DocLen} {desc.Substring(0,descLen),-DescLen}   {pick.PickedQty.ToString(),4}/{pick.RequestedQty.ToString(),-4} {mat.UoM}");
                sb.AppendLine();
                line++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OnPrintPackingSlip(Object sender, PrintPageEventArgs args)
        {
            DrawPackingSlipPage(args, args.Graphics, ShipVendor, Order);
        }

        static void DrawPackingSlipPage(PrintPageEventArgs args, Graphics gr, Vendor shipVendor, PickOrder order)
        {
            var tlPt = new Point(args.PageBounds.X, args.PageBounds.Y);
            var trPt = new Point(args.PageBounds.Width - hlfInch, args.PageBounds.Y);
            var blPt = new Point(args.PageBounds.X, args.PageBounds.Height - hlfInch);
            var brPt = new Point(args.PageBounds.Width - hlfInch, args.PageBounds.Height - hlfInch);

            Rectangle border = new Rectangle(tlPt.X, tlPt.Y, trPt.X - tlPt.X, blPt.Y - tlPt.Y);
            Rectangle content = new Rectangle(new Point(tlPt.X + hlfInch, tlPt.Y + (2 * oneInch)),
                                           new Size(args.PageBounds.Width - (2 * hlfInch) - hlfInch, args.PageBounds.Height - hlfInch - (2 * oneInch) - qtrInch));

            int dateWidth = (int)gr.MeasureString($"Order Date: {order.DateCreated}\nDate: {DateTime.Now}", TitleFont).Width;
            


            gr.DrawRectangle(Pens.Black, border);
            //gr.DrawRectangle(Pens.Black, content);
            gr.DrawString($"Shipto:\n{shipVendor.FullAddress}", TitleFont, Brushes.Black, new Point(tlPt.X + qtrInch, tlPt.Y + qtrInch));
            gr.DrawString($"Order No. {order.OrderId}\nOrder Date: {order.DateCreated}\nDate: {DateTime.Now}", TitleFont, Brushes.Black, new Point(trPt.X - qtrInch - dateWidth, trPt.Y + qtrInch));
            PagenationText = LayoutPackingSlipText(PagenationText, gr, content);
            args.HasMorePages = PagenationText.Length > 0;

        }

        static string LayoutPackingSlipText(string text, Graphics gr, Rectangle container)
        {
            var format = new StringFormat();
            var start = new Point(container.Top, container.Left);
            var size = new SizeF(container.Width, container.Height);
            var required = gr.MeasureString(text, PartsFont, size, format, out int charsFitted, out int linesFilled);

            gr.DrawString(text, PartsFont, Brushes.Black, container);
            return (charsFitted > 0) ? text.Substring(charsFitted) : string.Empty;
        }
        #endregion


        #region Print Material Label Logic
        static Material MatToPrint;
        static decimal QtyToPrint = -1;
        static decimal RequestQtyToPick = -1;
        static string Extra = null;
        static float labelScale = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mat"></param>
        public static void PrintMaterialLabel(Material mat, decimal qty = -1, decimal requestQty = -1, string extra = null)
        {
            int xSize = 300;
            int ySize = 600;
            QtyToPrint = qty;
            RequestQtyToPick = requestQty;
            Extra = extra;
            MatToPrint = mat;
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += OnPrintLabel;
            doc.PrinterSettings.PrinterName = Config.ReadConfigStr("LabelPrinter");
            if (!float.TryParse(Config.ReadConfigStr("LabelScale"), out labelScale))
                labelScale = 1;

            try
            {
                doc.Print();
            }
            catch(Exception)
            {
                Dialog.Message("There is no label printer selected.");
            }
        }

        static bool ForcePageSize(System.Drawing.Printing.PrintDocument MyPrintDocument, System.Drawing.Printing.PaperKind MyPaperKind)
        {
            for (int i = 0; i < MyPrintDocument.PrinterSettings.PaperSizes.Count; ++i)
            {
                if (MyPrintDocument.PrinterSettings.PaperSizes[i].Kind == MyPaperKind)
                {
                    MyPrintDocument.DefaultPageSettings.PaperSize = MyPrintDocument.PrinterSettings.PaperSizes[i];
                    return true;
                }
            }
            return false;
        }

        static void OnPrintLabel(Object sender, PrintPageEventArgs args)
        {
            var qty = QtyToPrint;
            var rQty = RequestQtyToPick;
            var mat = MatToPrint;
            var gr = args.Graphics;

            var temp = Config.ReadConfigStr(SelectLabelPrinterForm.LabelHorOffset);
            decimal horOffset = temp == null ? 0 : decimal.Parse(temp);

            //consts
            int xGutter = 1;// 4;
            int yGutter = -3;// 2;
            int xStart = 5 + (int)Math.Round(args.Graphics.DpiX * Convert.ToSingle(horOffset));
            int yStart = 5;

            string barcodeStr = "*" + mat.MatId + "*";
            Font idFont = new Font("Calibri", 14);
            Font docFont = new Font("Calibri", 10);
            Font descFont =  new Font("Calibri", 6);
           
            int barcodeHeight = (int)gr.MeasureString(mat.MatId, RssUtils.BarcodeFont).Height;
            int barcodeLen = (int)gr.MeasureString(mat.MatId, RssUtils.BarcodeFont).Width;

            int idHeight = (int)gr.MeasureString(mat.MatId, idFont).Height; 
            int idLen = (int)gr.MeasureString(mat.MatId, idFont).Width;

            int docHeight = (int)gr.MeasureString(mat.DocumentId, docFont).Height;



            //BARCODE
            int xPos = xStart;
            int yPos = yStart;
            gr.DrawString(barcodeStr, RssUtils.BarcodeFont, Brushes.Black, new Point(xPos, yPos));

            //DATE
            xPos += barcodeLen + xGutter + 20;
            //yPos += barcodeHeight + yGutter;
            gr.DrawString("Date: " + DateTime.Now.ToShortDateString(), descFont, Brushes.Black, new Point(xPos, yPos));

            //MATERIAL ID
            xPos = xStart;
            yPos += barcodeHeight + yGutter;
            gr.DrawString(mat.MatId, idFont, Brushes.Black, new Point(xPos, yPos));

            //KELOX ID
            yPos += idHeight + yGutter;
            gr.DrawString(mat.DocumentId, docFont, Brushes.Black, new Point(xPos, yPos));

            //DESCRIPTION
            xPos = xStart;
            yPos += docHeight + yGutter;
            gr.DrawString(mat.Description, descFont, Brushes.Black, new Point(xPos, yPos));

            //QTY
            if(qty > -1)
            {
                xPos = xStart + barcodeLen + xGutter + 20;
                yPos = yStart + docHeight + yGutter;
                gr.DrawString($"Qty: {qty} / {rQty}", idFont, Brushes.Black, new Point(xPos, yPos));

                yPos += docHeight + yGutter + 5;
                gr.DrawString($"{Extra}", idFont, Brushes.Black, new Point(xPos, yPos));
            }
        }

        public static void ProgressWrapper(Form parentForm, string desc, Action action)
        {
            using (var progress = new ProgressDialog())
            {
                DoWorkEventHandler workHandler = (object s, DoWorkEventArgs workArgs) =>
                {
                    action();
                };

                progress.WindowTitle = "Performing Long Task...";
                progress.Text = desc;
                progress.DoWork += workHandler;
                //progress.RunWorkerCompleted += workCompleteHandler;
                progress.ShowCancelButton = false;
                parentForm.Invoke(new Action(() => progress.ShowDialog(parentForm))); //this ensures the dialog is parented to this form
            }
        }

        public static List<string> SplitRawItemList(string raw)
        {
            return raw.Split(new[] { "\r" }, StringSplitOptions.None).Select(x => x.Trim()).ToList();

        }

        public static List<string> IdListFromClipboard(object sender, KeyEventArgs e)
        {
            List<string> ids = new List<string>();
            if (e.Control && e.KeyCode == Keys.V)
            {
                string pasta = Clipboard.GetText();
                var idList = SplitRawItemList(pasta);

                foreach (var id in idList)
                {
                    if (!string.IsNullOrEmpty(id))
                        ids.Add(id);
                }
            }
            return ids;
        }

        public static List<Material> MaterialListFromIdList(List<string> idList, Project proj)
        {
            List<Material> mats = new List<Material>();

            foreach (var id in idList)
            {
                if (string.IsNullOrEmpty(id))
                    continue;
                var mat = proj.GetMaterialFromUnknownId(id);
                if (mat == null)
                {
                    Dialog.Message($"Unknown material '{id}'. Skipping it.");
                }
                else
                {
                    mats.Add(mat);
                }
            }
            return mats;
        }
        #endregion

    }
}
