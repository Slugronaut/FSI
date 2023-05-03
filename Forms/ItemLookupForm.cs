using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SAOT
{
    public partial class ItemLookupForm : Form, IProjectProvider
    {
        Project Proj;
        Warehouse Wh;
        public Project CurrentSelectedProject { get => Proj; }

        public ItemLookupForm(Project proj, Warehouse wh)
        {
            InitializeComponent();
            Proj = proj;
            Wh = wh;
            dataGridView1.DragDrop += HandleDrop;
            dataGridView1.KeyDown += OnKeyDown;
            dataGridView1.Rows.Clear();

            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridView1.CellMouseUp += dataGridView1_CellMouseUp;
           this.dataGridView1.CellMouseDoubleClick += HandleDoubleClick;
        }
        void HandleDoubleClick(Object sender, DataGridViewCellMouseEventArgs args)
        {
            var grid = this.dataGridView1;
            int col = args.ColumnIndex;

            WindowUtils.HandleDrawingLookup(sender, args);
            //WindowUtils.HandleOpenLocationView(sender, args, CurrentUser, CurrentWarehouse, Proj);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.Control && e.KeyCode == Keys.V)
            {
                string pasta = Clipboard.GetText();
                var idList = SplitRawItemList(pasta);

                foreach(var id in idList)
                {
                    if (string.IsNullOrEmpty(id))
                        continue;
                    var mat = Proj.GetMaterialFromUnknownId(id);
                    if(mat == null)
                    {
                        Dialog.Message($"Material id '{id}' is unknown. Skipping it.");
                    }
                    else
                    {
                        var loc = Wh.FindMaterialLocations(mat.MatId);
                        //loc, sap, kelox, desc
                        dataGridView1.Rows.Add(loc == null ? "No Loc" : loc.LocationId, mat.MatId, mat.DocumentId, mat.Description);
                    }
                }
                
            }
            */

            var idList = WindowUtils.IdListFromClipboard(sender, e);
            var matList = WindowUtils.MaterialListFromIdList(idList, Proj);

            foreach(var mat in matList)
            {
                var loc = Wh.FindFirstMaterialLocation(mat.MatId);
                //loc, sap, kelox, desc
                dataGridView1.Rows.Add(loc == null ? "No Loc" : loc.LocationId, mat.MatId, mat.DocumentId, mat.Description);
            }
        }

        private void HandleDrop(Object sender, DragEventArgs args)
        {
            //args.Data.GetData()
            //foreach(var s arg)
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        int currentColIndex;
        int currentRowIndex;
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.Button == MouseButtons.Left) return;

            currentRowIndex = e.RowIndex;
            currentColIndex = e.ColumnIndex;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[currentRowIndex].Cells[currentColIndex].Selected = true;
        }

        private void printLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             * grid.Columns[colIndex].HeaderText.ToUpper();
             * */
            int colIndex = -1;
            var row = dataGridView1.Rows[currentRowIndex];
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                var colName = dataGridView1.Columns[i].HeaderText.ToUpper();
                if (colName == "SAP" || colName == "ID" || colName == "MATERIAL")
                {
                    colIndex = i;
                    break;
                }
            }

            if (colIndex < 0)
            {
                Dialog.Message("Could not identify the item clicked on.");
                return;
            }

            var cell = row.Cells[colIndex];

            //TODO: need to get parentform's Project provider interface!!
            var form = this.FindForm() as IProjectProvider;
            var mat = form.CurrentSelectedProject.FindMaterial(cell.Value.ToString());
            if (mat == null)
            {
                Dialog.Message($"Could not identify the material '{cell.Value.ToString()}'.");
                return;
            }
            WindowUtils.PrintMaterialLabel(mat);
        }

    }
}
