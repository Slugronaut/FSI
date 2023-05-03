using SAOT.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SAOT
{

    /// <summary>
    /// 
    /// </summary>
    public partial class LocationContentsForm : Form, IProjectProvider
    {
        public const int RowMaterialIndex = 2;
        readonly WeakReference CurrentUser;
        readonly Project Proj;
        readonly Warehouse Wh;
        StorageSpace Loc;
        bool DisableUpdate = false;

        public Project CurrentSelectedProject { get => Proj; }
        

        public LocationContentsForm(User currentUser, Project proj, Warehouse warehouse, StorageSpace loc)
        {
            
            Assert.IsNotNull(warehouse);
            Assert.IsNotNull(loc);

            InventoryAdjustmentForm.AddLocationForm(this);
            this.FormClosing += HandleSelfClosing;
            CurrentUser = new WeakReference(currentUser);
            Proj = proj;
            Wh = warehouse;
            Loc = loc;

            InitializeComponent();

            DisableUpdate = true;
            foreach(var type in Enum.GetNames(typeof(StorageSpace.LocationTypes)))
            {
                this.comboStorageType.Items.Add(type);
            }
            this.comboStorageType.SelectedItem = Enum.GetName(typeof(StorageSpace.LocationTypes), loc.Type);
            DisableUpdate = false;

            //redirect to the gridview within the filterable gridview
            filterableDataGridView1.ClearColumns();
            filterableDataGridView1.CreateColumns(FilterableDataGridView.ColumnControlTypes.Button, "");
            filterableDataGridView1.CreateColumns("Kelox", "SAP", "Qty", "Description");
            this.dataGridView1 = filterableDataGridView1.GridView;


            this.Text = $"Location Contents - {loc.LocationId}";
            this.labelLocationId.Text = loc.LocationId;
            UpdateLocationGridView(Proj, Loc);
            dataGridView1.AllowDrop = true;
            dataGridView1.DragDrop += dataGridView1_DragDrop;
            dataGridView1.DragOver += dataGridView1_DragOver;
            dataGridView1.CellPainting += grid_CellPainting;
            dataGridView1.MouseMove += dataGridView1_MouseMove;
            dataGridView1.MouseDown += dataGridView1_MouseDown;
            dataGridView1.MouseUp += dataGridView1_MouseUp;
            dataGridView1.CellContentClick += cellContent_Click;
            dataGridView1.MultiSelect = true;
            dataGridView1.AllowUserToResizeRows = false;
            MsgDispatch.AddListener<InventoryUpdated>(HandleLocationContentsUpdated);
            MsgDispatch.AddListener<CloseAllLocationFormsMsg>(HandleCloseAllLocationForms);

            dataGridView1.KeyDown += OnKeyDown;

            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridView1.CellMouseUp += dataGridView1_CellMouseUp;
            this.dataGridView1.CellMouseDoubleClick += WindowUtils.HandleDrawingLookup;
            MsgDispatch.AddListener<FilterUpdatedMessage>(HandleFilterUpdated);
            //TODO: Start DB lock here!!
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        bool Dragging;

        void HandleFilterUpdated(FilterUpdatedMessage msg)
        {
            HandleLocationContentsUpdated(null);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var idList = WindowUtils.IdListFromClipboard(sender, e);
            if (idList == null || idList.Count < 1)
                return;
            var user = CurrentUser.Target as User;
            if (user == null)
            {
                Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                return;
            }
            var result = Dialog.ConfirmAction("You are about to paste information to this location. If it is identfied as valid materials it will be stored in this location.\n\nDo you want to continue?", this);
            if (result == DialogResult.No)
                return;
            var matList = WindowUtils.MaterialListFromIdList(idList, Proj);

            foreach (var mat in matList)
            {
                //just moving from materials list, nothing fancy here
                Loc.SetQty(mat.MatId, 1);
                Warehouse.SetMaterialInLocation(user, Wh.WarehouseId, Loc.LocationId, mat.MatId, 1);
                
            }

            //TODO: can we make this asyncronous?
            MsgDispatch.PostMessage(new LocationContentsUpdated());
        }

        void HandleCloseAllLocationForms(CloseAllLocationFormsMsg msg)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void HandleLocationContentsUpdated(InventoryUpdated msg)
        {
            //we need to pull the latest version of the location from the source
            Loc = Warehouse.RequestStorageSpaceData(this.Wh.WarehouseId, Loc.LocationId);
            UpdateLocationGridView(Proj, Loc);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void cellContent_Click(Object sender, DataGridViewCellEventArgs args)
        {
            var user = CurrentUser.Target as User;
            if (user == null)
            {
                //Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                return;
            }

            var grid = sender as DataGridView;
            //if(grid.Columns[args.ColumnIndex] is DataGridViewButtonColumn && args.RowIndex >= 0)
            if(args.ColumnIndex == 0 && args.RowIndex >= 0)
            {
                var row = grid.Rows[args.RowIndex];
                string matId = row.Cells[RowMaterialIndex].Value as string;
                string locId = Loc.LocationId;
                if (Dialog.ConfirmAction($"Are you sure you want to remove the material '{matId}' from the location '{locId}'?") == DialogResult.Yes)
                {
                    Loc.RemoveItem(matId);
                    Warehouse.RemoveMaterialFromLocation(user, Wh.WarehouseId, locId, matId);
                    MsgDispatch.PostMessage(new LocationContentsUpdated());
                }
                else return;
            }
        }


        #region Drag n Drop
        //private int rowIndexOfItemUnderMouseToDrop;
        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            var user = CurrentUser.Target as User;
            if (
                //Dragging &&
                //(ModifierKeys & Keys.Control) != 0 &&
                (e.Button & MouseButtons.Right) == MouseButtons.Right
                )
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    //the mouse has left the initial textbox while dragging, begin dragging operation proper
                    if (user == null)
                    {
                        Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                        return;
                    }

                    var matColIndex = WindowUtils.GetHeaderIndex(this.dataGridView1, "SAP");
                    var idList = new List<string>();
                    var v = dataGridView1.SelectedCells;

                    List<int> selectedRows = new List<int>(10);
                    for(int i = 0; i < dataGridView1.SelectedCells.Count; i++)
                    {
                        var cell = dataGridView1.SelectedCells[i];
                        if (!selectedRows.Contains(cell.RowIndex))
                            selectedRows.Add(cell.RowIndex);
                    }

                    for(int i = 0; i < selectedRows.Count; i++)
                    {
                        idList.Add(dataGridView1.Rows[selectedRows[i]].Cells[matColIndex].Value as string);
                    }

                    
                    InvMovePackage.BeginMove(Loc.LocationId, idList);
                    DragDropEffects dropEffect = dataGridView1.DoDragDrop(
                        Loc.LocationId, 
                        (ModifierKeys & Keys.Control) != 0 ? DragDropEffects.Copy : DragDropEffects.Move);
                }
            }
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs args)
        {
            Dragging = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            Dragging = true;
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dataGridView1.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
            }
        }

        [Flags]
        public enum DragKeyStates : int
        {
            Shift = 4,
            Ctrl = 8,
            Alt = 32,
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            //TODO: handle mulit-item dragdrop!!!

            var user = CurrentUser.Target as User;
            if(user == null)
            {
                Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                return;
            }


            //get our source location and mat list
            StorageSpace srcLoc = Wh.FindStorageLocation(InvMovePackage.DragSource);

            List<Material> matList = new List<Material>(10);
            foreach(var itemId in InvMovePackage.DragList)
            {
                var mat = Proj.FindMaterial(itemId);
                if (mat != null) matList.Add(mat);
            }


            //are we dragging from another location or just the materials list?
            if (srcLoc == null)
            {
                //just moving from materials list, nothing fancy here
                foreach (var mat in matList)
                {
                    Loc.SetQty(mat.MatId, 1);
                    Warehouse.SetMaterialInLocation(user, Wh.WarehouseId, Loc.LocationId, mat.MatId, 1);
                }
            }
            else
            {
                //we're dragging from another location, will it be a copy operation or a move operation?
                if ((e.KeyState & 4) == 4)
                {
                    //shift is held, perform a copy
                    foreach (var mat in matList)
                    {
                        Loc.SetQty(mat.MatId, 1);
                        Warehouse.SetMaterialInLocation(user, Wh.WarehouseId, Loc.LocationId, mat.MatId, 1);
                    }
                }
                else
                {
                    //no shift, remove material from original location and move it to the new one
                    foreach (var mat in matList)
                    {
                        srcLoc?.RemoveItem(mat.MatId);
                        Loc.SetQty(mat.MatId, 1);
                        Warehouse.RemoveMaterialFromLocation(user, Wh.WarehouseId, InvMovePackage.DragSource, mat.MatId);
                        Warehouse.SetMaterialInLocation(user, Wh.WarehouseId, Loc.LocationId, mat.MatId, 1);
                    }
                }
            }


            //TODO: can we make this asyncronous?
            MsgDispatch.PostMessage(new LocationContentsUpdated());
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            this.Focus();
            //if (sender == this.dataGridView1) return;
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                //is shift being held?
                //if ((e.KeyState & (int)DragKeyStates.Shift) != 0)
                //   e.Effect = DragDropEffects.Copy;
                //else e.Effect = DragDropEffects.Move;
                e.Effect = DragDropEffects.Move;
            }
        }
        #endregion


        void HandleParentClosing(Object sender, EventArgs args)
        {
            this.Close();
        }

        void HandleSelfClosing(Object sender, EventArgs args)
        {
            this.FormClosing -= HandleSelfClosing;
            InventoryAdjustmentForm.ReoveLocationForm(this);
            MsgDispatch.RemoveListener<InventoryUpdated>(HandleLocationContentsUpdated);
            MsgDispatch.RemoveListener<CloseAllLocationFormsMsg>(HandleCloseAllLocationForms);
            MsgDispatch.RemoveListener<FilterUpdatedMessage>(HandleFilterUpdated);
        }

        private void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //I supposed your button column is at index 0
            if (e.ColumnIndex == 0)
            {
                var image = global::SAOT.Properties.Resources.images;
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                int xInset = 4;
                int yInset = 2;

                int xOffset = 0;
                int yOffset = 0;
                var bounds = new Rectangle(e.CellBounds.X + xInset + xOffset, e.CellBounds.Y + yInset + yOffset,
                                           e.CellBounds.Width - (xInset*2) + xOffset, e.CellBounds.Height - (yInset*2) + yOffset);

                e.Graphics.DrawImage(image, bounds);
                e.Handled = true;
            }
        }
        
        /// <summary>
        /// Updates a DataGridView with the contents of a SotrageSpace.
        /// </summary>
        /// <param name="locData"></param>
        /// <param name="grid"></param>
        public void UpdateLocationGridView(Project proj, StorageSpace locData)
        {
            
            if (this.filterableDataGridView1.Parent == null)
            {
                MessageBox.Show("Global message handler caught. Please inform a dev. Error code 001");
                return;
            }

            List<List<string>> rows = new List<List<string>>();

            var invalidIdList = new List<string>(2);
            foreach (var item in locData.Items)
            {
                var mat = proj.FindMaterial(item.Key);
                if (mat == null)
                    invalidIdList.Add(item.Key);
                else
                {
                    //update, we are now using a filterable grid list so all we need to o is compile the list of rows
                    rows.Add(new List<string>(new string[] { null, mat.DocumentId, mat.MatId, item.Value.ToString(), mat.Description }));
                }
            }

            //display message about missing stuff
            if (invalidIdList.Count > 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (var id in invalidIdList)
                    sb.AppendLine(id);
                Dialog.Message($"There were a number of materials that could not be properly indentified in this location. They are as follows.\n\n{sb.ToString()}");

            }


            filterableDataGridView1.PushModelToView(rows, true);
            if (dataGridView1.Rows.Count > 0)
            {
                var deleteButtonCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0] as DataGridViewButtonCell;
                deleteButtonCell.ToolTipText = "Removes this item from the current storage location.";
                deleteButtonCell.OwningColumn.Resizable = DataGridViewTriState.False;
                dataGridView1.Columns[0].Width = 24;
            }

        }

        int currentColIndex;
        int currentRowIndex;
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.Button == MouseButtons.Left) return;

            //in case we are deleting this row as we unclick it too
            if (e.RowIndex >= dataGridView1.Rows.Count)
                return;

            currentRowIndex = e.RowIndex;
            currentColIndex = e.ColumnIndex;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[currentRowIndex].Cells[currentColIndex].Selected = true;
        }

        private void printLableToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void comboStorageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DisableUpdate) return;
            //update local copy then push to warehouse
            Loc.Type = (StorageSpace.LocationTypes)this.comboStorageType.SelectedIndex;

            var user = CurrentUser.Target as User;
            if (user == null)
            {
                Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                return;
            }

            //in order for n update to happen we need to have a location in the db. Normally
            //empty locatiosn just don't exist
            if(Loc.Items.Count < 1)
                Warehouse.SetLocationContents(user, Wh.WarehouseId, Loc);
            
            Warehouse.SetLocationType(user, Wh.WarehouseId, Loc, Loc.Type);
            MsgDispatch.PostMessage(new LocationContentsUpdated());
        }
    }
}
