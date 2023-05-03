using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SAOT.Forms
{
    public partial class CreatePickListForm : Form
    {
        public bool ErrorFlag { get; private set; }
        List<PickItemVM> AppendedItems;
        int OrderId;
        User CurrentUser;
        Warehouse Wh;
        Project Proj;
        List<Vendor> Vendors;
        Vendor SelectedShipId { get => comboVendorDest.SelectedIndex >= 0 ? Vendors[comboVendorDest.SelectedIndex] : null; }
        Vendor SelectedInvAdjustId { get => comboVendorInventory.SelectedIndex >= 0 ? Vendors[comboVendorInventory.SelectedIndex] : null; }
        OrderTypes OrderType { get => (OrderTypes)(1 << (comboBoxOrderType.SelectedIndex)); }

        AutoCompleteStringCollection _CachedMaterialIdList;
        AutoCompleteStringCollection CachedMaterialIdList
        {
            get
            {
                if (_CachedMaterialIdList == null)
                {
                    _CachedMaterialIdList = new AutoCompleteStringCollection();
                    _CachedMaterialIdList.AddRange(Proj.MaterialsList.Select(x => x.MatId).ToArray());
                }

                return _CachedMaterialIdList;
            }
        }


        public BindingList<PickItemVM> PickListVM = new BindingList<PickItemVM>();
        public class PickItemVM
        {
            public string Status { get; set; }
            public string MaterialId { get; set; }
            public string Doc { get; set; }
            public string Description { get; set; }
            public decimal Qty { get; set; }
            public decimal Picked { get; set; }
            public string UM { get; set; }
            public bool Remove { get; set; }

            public PickItemVM()
            {
                Status = Enum.GetName(typeof(PickItemStatuses), PickItemStatuses.PickPending);
            }

            public PickItemVM(PickListItem item, Project proj)
            {
                var mat = proj.FindMaterial(item.MaterialId);

                Status = Enum.GetName(typeof(PickItemStatuses), item.Status);
                MaterialId = item.MaterialId;
                Doc = mat?.DocumentId;
                Description = mat?.Description;
                Qty = item.RequestedQty;
                Picked = item.PickedQty;
                UM = mat.UoM;
                Remove = item.Status == PickItemStatuses.Canceled ? true : false;
            }
        }

        List<PickItemVM> NewItems = new List<PickItemVM>();
        /// <summary>
        /// Create a read-only view of an existing order.
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="order"></param>
        public CreatePickListForm(User currentUser, Warehouse wh, Project proj, int orderId)
        {
            OrderId = orderId;
            CurrentUser = currentUser;
            Wh = wh;
            Proj = proj;
            InitializeComponent();
            textBoxOrderId.Text = orderId.ToString();
            Vendors = Vendor.RequestAllVendors();

            //this.buttonAddPickItem.Enabled = false;
            //this.buttonAddPickItem.Visible = false;
            NewItems.Clear();
            this.buttonSaveOrder.Enabled = true;
            this.buttonSaveOrder.Visible = true;
            this.buttonCreateOrder.Enabled = false;
            this.buttonCreateOrder.Visible = false;
            this.comboVendorDest.Enabled = false;
            this.comboVendorInventory.Enabled = false;

            PickOrder order = PickOrdersDatabase.RequestOrder(orderId);
            if(order == null)
            {
                MessageBox.Show($"The pick order #{orderId} could not be found in the database. Read Error #02");
                ErrorFlag = true;
                this.Close();
                return;
            }

            //convert picklist from database to picklist viewmodel, then update view
            PullModelFromDB(orderId);
            UpdateViewFromModel();
            

            this.Text = "Pick List (Viewer Mode)";
            var pickOrder = order;// PickOrdersDatabase.RequestOrder(orderId);
            if(pickOrder != null)
            {
                var dest = Vendor.GetVendorNameFromId(pickOrder.ShipToId);
                if(!string.IsNullOrEmpty(dest))
                {
                    this.comboVendorDest.Items.Add(dest);
                    comboVendorDest.SelectedIndex = 0;
                }
                this.comboVendorDest.Items.Add(dest);

                var inv = Vendor.GetVendorNameFromId(pickOrder.InventoryAdjustId);
                if (!string.IsNullOrEmpty(inv))
                {
                    this.comboVendorInventory.Items.Add(inv);
                    comboVendorInventory.SelectedIndex = 0;
                }
            }

            #region EDIT-MODE STUFF
            this.buttonAddPickItem.Visible = false;
            this.buttonAddPickItem.Enabled = false;
            //dataGridView1.ReadOnly = true;
            AppendedItems = new List<PickItemVM>(10);
            dataGridView1.KeyDown += OnKeyDown;
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Status")].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Doc")].DefaultCellStyle.BackColor = Color.LightGray;   //document
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Description")].DefaultCellStyle.BackColor = Color.LightGray;   //description
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "UM")].DefaultCellStyle.BackColor = Color.LightGray;   //UoM
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Remove")].DefaultCellStyle.BackColor = Color.LightGray;

            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            #endregion


            InitStdControls();
        }

        /// <summary>
        /// Create an order that allows adding items.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="proj"></param>
        public CreatePickListForm(User currentUser, Project proj)
        {
            OrderId = PickOrdersDatabase.NextOrderId();
            CurrentUser = currentUser;
            Proj = proj;
            InitializeComponent();
            textBoxOrderId.Text = OrderId.ToString();
            Vendors = Vendor.RequestAllVendors();


            if (Vendors == null || Vendors.Count < 1)
            {
                MessageBox.Show("You must have at least one shipping destination defined in the vendors list before you can create an order.");
                ErrorFlag = true;
                this.Close();
                return;
            }
            foreach (var vendor in Vendors)
            {
                comboVendorDest.Items.Add(vendor.Name);
                comboVendorInventory.Items.Add(vendor.Name);
            }

            PickOrdersDatabase.ConfirmDatabaseExists();

            #region EDIT-MODE STUFF
            this.buttonAppendPickItem.Visible = false;
            this.buttonAppendPickItem.Enabled = false;
            dataGridView1.KeyDown += OnKeyDown;
            this.buttonSaveOrder.Enabled = false;
            this.buttonSaveOrder.Visible = false;
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Status")].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Doc")].DefaultCellStyle.BackColor = Color.LightGray;   //document
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Description")].DefaultCellStyle.BackColor = Color.LightGray;   //description
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "UM")].DefaultCellStyle.BackColor = Color.LightGray;   //UoM
            dataGridView1.Columns[WindowUtils.GetHeaderIndex(dataGridView1, "Remove")].DefaultCellStyle.BackColor = Color.LightGray;

            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            #endregion

            this.Text = "Pick List (Creation Mode)";
            InitStdControls();
        }

        /// <summary>
        /// 
        /// </summary>
        void PullModelFromDB(int orderId)
        {
            PickListVM.Clear();
            var pickList = PickOrdersDatabase.RequestPickList(orderId);
            if (pickList != null)
            {
                foreach (var pickItem in pickList)
                    PickListVM.Add(new PickItemVM(pickItem, Proj));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void InitStdControls()
        {
            this.dataGridView1.CellMouseUp += this.dataGridView1_CellMouseUp;
            this.FormClosed += formClosed;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.contextMenuStrip1.Opened += this.contextMenuStrip2_Opening;
        }

        #region Track Cell Selection
        int CurrentColIndex;
        int CurrentRowIndex;
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.Button == MouseButtons.Left) return;

            CurrentRowIndex = e.RowIndex;
            CurrentColIndex = e.ColumnIndex;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[CurrentRowIndex].Cells[CurrentColIndex].Selected = true;
        }

        DataGridViewCell SelectedCell()
        {
            return dataGridView1.SelectedCells.Count == 0 ? null : dataGridView1.SelectedCells[0];
        }

        DataGridViewRow SelectedRow()
        {
            return (dataGridView1.SelectedCells.Count < 1) ? null : dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex];
        }

        Material SelectedMaterial(string headerId)
        {
            var row = SelectedRow();
            int headerIndex = WindowUtils.GetHeaderIndex(dataGridView1, headerId);
            var matIdCell = row.Cells[headerIndex];
            return Proj.FindMaterial(matIdCell.Value as string);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void formClosed(object sender, EventArgs e)
        {
            var orderView = new PickOrdersForm(CurrentUser, Wh, Proj);
            orderView.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs args)
        {
            var dataGrid = sender as DataGridView;
            var row = dataGrid.Rows[args.RowIndex];
            var cell = row.Cells[args.ColumnIndex];

            //TODO: HACK ALERT: hard-coded columns here dude
            var statusCell = row.Cells[0];
            var idCell = row.Cells[1];
            var docCell = row.Cells[2];
            var descCell = row.Cells[3];
            var qtyCell = row.Cells[4];
            var pickedCell = row.Cells[5];
            var umCell = row.Cells[6];

            string matId = idCell.Value as string;
            var mat = Proj.FindMaterial(matId);


            //adjusting material number, update all other crap
            if (args.ColumnIndex == WindowUtils.GetHeaderIndex(dataGridView1, "MaterialId"))
            {
                if (mat == null)
                {
                    statusCell.Value = string.Empty;
                    docCell.Value = string.Empty;
                    descCell.Value = string.Empty;
                    qtyCell.Value = new decimal(0);
                    pickedCell.Value = new decimal(0);
                    umCell.Value = string.Empty;
                }
                else
                {
                    statusCell.Value = Enum.GetName(typeof(PickItemStatuses), PickItemStatuses.PickPending);
                    docCell.Value = mat.DocumentId;
                    descCell.Value = mat.Description;
                    qtyCell.Value = new decimal(0);
                    umCell.Value = mat.UoM;
                }
            }

            

            //var button = row.Cells[5] as DataGridViewButtonCell;
            //button.Value = Cancel;
        }

        /// <summary>
        /// Ensures that the quantity give is valid for the supplied material's UoM and adjust if not.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static void ValidateQtyForUoM(DataGridViewCell cell, Material mat)
        {
            /*
            if (mat != null)
            {
                if (mat.UoM.ToUpper() == "M")
                {
                    if (!decimal.TryParse(cell.Value as string, out decimal temp))
                        cell.Value = 0;
                }
                else
                {
                    if (!decimal.TryParse(cell.Value as string, out decimal tempF))
                    {
                        if (!int.TryParse(cell.Value as string, out int tempI))
                            cell.Value = 0;
                    }
                    else
                    {
                        cell.Value = (int)tempF;
                    }
                }
            }
            */
        }

        DataGridViewRow GetLastGridRow()
        {
            return dataGridView1.Rows.Count == 0 ? null : dataGridView1.Rows[dataGridView1.Rows.Count-1];
        }

        private void addPickItem_Click(object sender, EventArgs e)
        {
            PickListVM.Add(new PickItemVM());
            UpdateViewFromModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastRow"></param>
        /// <param name="selectedRow"></param>
        /// <param name="colIndex"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void PasteQtys(DataGridViewRow lastRow, DataGridViewRow selectedRow, int colIndex, object sender, KeyEventArgs args)
        {
            var rawList = WindowUtils.IdListFromClipboard(sender, args);
            var umIndex = WindowUtils.GetHeaderIndex(dataGridView1, "UM");
            var matIndex = WindowUtils.GetHeaderIndex(dataGridView1, "MaterialId");

            //first, let's make sure all of the qtys are actually decimal numbers
            if (!rawList.All(x => decimal.TryParse(x, out decimal r)))
            {
                MessageBox.Show(this, "One or more values being pasted is not a valid numerical value.");
                return;
            }

            List<decimal> qtyList = rawList.Select(x => decimal.Parse(x)).ToList();

            for(int i = selectedRow.Index, j = 0; i < dataGridView1.Rows.Count && j < rawList.Count; i++, j++)
            {
                var row = dataGridView1.Rows[i];
                var qtyCell = row.Cells[colIndex];
                var umCell = row.Cells[umIndex];
                var matCell = row.Cells[matIndex];

                if (string.IsNullOrEmpty(matCell.Value as string))
                {
                    MessageBox.Show(this, "You must specify a material before you can apply any quantities to it.");
                    continue;
                }

                if ((umCell.Value as string).ToUpper() != "M" && rawList[j].Contains("."))
                {
                    MessageBox.Show(this, "One or more materials does not allow for decimal value quantities.");
                    continue;
                }

                qtyCell.Value = rawList[j];

            }

        }

        /// <summary>
        /// 
        /// </summary>
        void PasteMaterials(DataGridViewRow lastRow, DataGridViewRow selectedRow, int colIndex, object sender, KeyEventArgs args)
        {
            var idList = WindowUtils.IdListFromClipboard(sender, args);
            var matList = WindowUtils.MaterialListFromIdList(idList, Proj);

            //if we are doing a multi copy-paste operation, it must be performed on the last row of the grid,
            //otherwise it will default to a single copy-paste operation
            if (matList.Count < 1) return;
            if (matList.Count == 1 || lastRow != selectedRow)
            {
                //single copy-paste
                selectedRow.Cells[colIndex].Value = int.Parse(matList[0].MatId);
            }
            else if (matList.Count > 1 && lastRow == selectedRow)
            {
                //paste first item into currently selected cell...
                selectedRow.Cells[colIndex].Value = int.Parse(matList[0].MatId);

                //multi copy-paste the rest into newly created cells.
                for (int i = 1; i < matList.Count; i++)
                {
                    var mat = matList[i];
                    //var loc = Wh.FindFirstMaterialLocation(mat.MatId);
                    //loc, sap, kelox, desc
                    //dataGridView1.Rows.Add(loc == null ? "No Loc" : loc.LocationId, mat.MatId, mat.DocumentId, mat.Description);
                    this.addPickItem_Click(dataGridView1, null);

                    //find the cell column for the SAP info
                    lastRow = GetLastGridRow();
                    lastRow.Cells[colIndex].Value = int.Parse(mat.MatId);
                }
            }

            UpdateViewFromModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnKeyDown(object sender, KeyEventArgs args)
        {
            var selectedRow = SelectedRow();
            var lastRow = GetLastGridRow();
            if (lastRow == null) return;

            //first: confirm we are on the SAP cell
            var materialColIndex = WindowUtils.GetHeaderIndex(dataGridView1, "MaterialId");
            var qtyColIndex = WindowUtils.GetHeaderIndex(dataGridView1, "Qty");
            if (dataGridView1.SelectedCells.Count != 1)// || dataGridView1.SelectedCells[0].ColumnIndex != materialColIndex)
                return;

            //handle the paste command...
            if (args.Control && args.KeyCode == Keys.V)
            {
                if (dataGridView1.SelectedCells[0].ColumnIndex == materialColIndex)
                    PasteMaterials(lastRow, selectedRow, materialColIndex, sender, args);
                else if (dataGridView1.SelectedCells[0].ColumnIndex == qtyColIndex)
                    PasteQtys(lastRow, selectedRow, qtyColIndex, sender, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateViewFromModel()
        {
            //forces databindg to push updates. stupid but works *shrugs*
            this.dataGridView1.DataSource = typeof(PickItemVM); //assign type instead of null so that we don't erase our headers
            this.dataGridView1.DataSource = PickListVM;

            int index = WindowUtils.GetHeaderIndex(this.dataGridView1, "MaterialId");
            this.GetLastGridRow().Cells[index].ReadOnly = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs args)
        {
            int colIndex = dataGridView1.CurrentCell.ColumnIndex;
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            string headerText = dataGridView1.Columns[colIndex].HeaderText;
            var cell = dataGridView1.Rows[rowIndex].Cells[colIndex];

            var matIdCell = dataGridView1.Rows[rowIndex].Cells[WindowUtils.GetHeaderIndex(dataGridView1, "MaterialId")];
            string matId = matIdCell.Value as string;
            var mat = Proj.FindMaterial(matId);

            if (headerText.Equals("MaterialId"))
            {
                TextBox tb = args.Control as TextBox;

                if (tb != null)
                {
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    tb.AutoCompleteCustomSource = CachedMaterialIdList;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }
            else
            {
                TextBox tb = args.Control as TextBox;
                if (tb != null)
                {
                    tb.AutoCompleteMode = AutoCompleteMode.None;
                }

                //adjusting qty
                //if (headerText.Equals("Qty"))
                //{
                //    ValidateQtyForUoM(cell, mat);
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void VerifyOrderReadiness()
        {
            if (PickListVM.Count < 1)
                throw new Exception("You must have at least one item in your item list.");

            if (OrderType != OrderTypes.InventoryCount && OrderType != OrderTypes.PickForProduction)
            {
                if (SelectedShipId == null)
                    throw new Exception("Please select an id for shipping destination.");

                if (SelectedInvAdjustId == null)
                    throw new Exception("Please select an id for inventory adjustments.");

                if (comboBoxOrderType.SelectedIndex < 0)
                    throw new Exception("Please select an order type.");
            }

            int count = 0;
            foreach (var item in PickListVM)
            {
                if (OrderType != OrderTypes.InventoryCount)
                {
                    if (string.IsNullOrEmpty(item.MaterialId))
                        throw new Exception($"Material #{count} has not been specified. Please complete the item or remove it from the pick list.");
                    if (item.Qty <= 0)
                        throw new Exception($"You must specify a pick quantity greater than zero for the material #{count}: \n'{item.MaterialId} / {item.Doc}'");
                }
                count++;
            }

        }

        private void comboBoxOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(OrderType == OrderTypes.InventoryCount || OrderType == OrderTypes.PickForProduction)
            {
                this.comboVendorDest.Enabled = false;
                this.comboVendorInventory.Enabled = false;
            }
            else
            {
                this.comboVendorDest.Enabled = true;
                this.comboVendorInventory.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printPackingSlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will print a slip for order #.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void buttonCreate_Click(object sender, EventArgs args)
        {
            try
            {
                VerifyOrderReadiness();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
                return;
            }

            try
            {
                var mats = this.PickListVM.Select(x => Proj.FindMaterial(x.MaterialId)).ToList();
                var qtys = this.PickListVM.Select(x => x.Qty).ToList();
                for (int i = 0; i < qtys.Count; i++)
                {
                    if (qtys[i] < 0)
                        throw new Exception($"Invalid quantity for material '{mats[i].MatId}'");
                    string sF = qtys[i].ToString();
                    if (sF.Contains(".") && mats[i].UoM.ToUpper() != "M")
                        throw new Exception($"Invalid unit for material '{mats[i].MatId}'. Please supply a whole integer.");
                    if (!sF.IsNumeric())
                        throw new Exception($"Invalid unit for material '{mats[i].MatId}'. Must be a number.");
                }
                int orderId = PickOrdersDatabase.CreatePickOrder(CurrentUser, SelectedShipId, SelectedInvAdjustId, OrderType, mats, qtys);

                MessageBox.Show(this, $"The pick order '{orderId}' has been successfully created.");
                this.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveOrder_Click(object sender, EventArgs args)
        {
            try
            {
                VerifyOrderReadiness();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
                return;
            }

            try
            {
                if (AppendedItems == null)
                {
                    MessageBox.Show(this, "This order has not been changed.");
                }
                else
                {
                    var mats = this.AppendedItems.Select(x => Proj.FindMaterial(x.MaterialId)).ToList();
                    var qtys = this.AppendedItems.Select(x => x.Qty).ToList();
                    for (int i = 0; i < qtys.Count; i++)
                    {
                        if (qtys[i] < 0)
                            throw new Exception($"Invalid quantity for material '{mats[i].MatId}'");
                        string sF = qtys[i].ToString();
                        if (sF.Contains(".") && mats[i].UoM.ToUpper() != "M")
                            throw new Exception($"Invalid unit for material '{mats[i].MatId}'. Please supply a whole integer.");
                        if (!sF.IsNumeric())
                            throw new Exception($"Invalid unit for material '{mats[i].MatId}'. Must be a number.");
                    }
                    PickOrdersDatabase.AddPickItems(CurrentUser, this.OrderId, mats, qtys);

                    MessageBox.Show(this, $"The pick order '{OrderId}' has been successfully updated.");
                }
                this.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelItemToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void cancelItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mat = SelectedMaterial("MaterialId");
            if(mat == null)
            {
                MessageBox.Show(this, "Unknown material. Could not perform cancelation.");
                return;
            }

            if(!PickOrdersDatabase.CancelPickItem(this.CurrentUser, OrderId, mat))
            {
                MessageBox.Show(this, "This item is already cancelled in the picklist.");
                return;
            }

            //TODO: We should really just update the locally cached
            //copy here instead of downloading the entire model again!!
            PullModelFromDB(this.OrderId);
            UpdateViewFromModel();
        }

        private void restoreItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mat = SelectedMaterial("MaterialId");
            if (mat == null)
            {
                MessageBox.Show(this, "Unknown material. Could not perform restoration.");
                return;
            }

            if(!PickOrdersDatabase.RestoreCanceledPickItem(this.CurrentUser, OrderId, mat))
            {
                MessageBox.Show(this, "This item is already active in the picklist.");
                return;
            }

            //TODO: We should really just update the locally cached
            //copy here instead of downloading the entire model again!!
            PullModelFromDB(this.OrderId);
            UpdateViewFromModel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var item = new PickItemVM();
            AppendedItems.Add(item);
            PickListVM.Add(item);
            UpdateViewFromModel();
        }

        const int CancelIndex = 1;
        const int RestoreIndex = 2;
        private void contextMenuStrip2_Opening(object sender, EventArgs e)
        {
            /*
            DataGridViewCellEventArgs args = e as DataGridViewCellEventArgs;
            var row = SelectedRow();
            int headerIndex = WindowUtils.GetHeaderIndex(dataGridView1, "Status");
            var statusCell = row.Cells[headerIndex];

            var status = (PickItemStatuses)Enum.Parse(typeof(PickItemStatuses), statusCell.Value as string);
            if(status == PickItemStatuses.Canceled)
            {
                this.contextMenuStrip1.Items[CancelIndex].Enabled = false;
                this.contextMenuStrip1.Items[RestoreIndex].Enabled = true;
            }
            else
            {
                this.contextMenuStrip1.Items[CancelIndex].Enabled = true;
                this.contextMenuStrip1.Items[RestoreIndex].Enabled = false;
            }
            */
        }
    }
}
