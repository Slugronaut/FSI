using SAOT.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SAOT
{
    public partial class LocationContentsForm : Form
    {
        public const int RowMaterialIndex = 2;

        User CurrentUser;
        Project Proj;
        Warehouse Wh;
        StorageSpace Loc;


        public LocationContentsForm(User currentUser, Project proj, Warehouse warehouse, StorageSpace loc)
        {
            Assert.IsNotNull(warehouse);
            Assert.IsNotNull(loc);

            InventoryAdjustmentForm.AddLocationForm(this);
            this.FormClosing += HandleSelfClosing;
            CurrentUser = currentUser;
            Proj = proj;
            Wh = warehouse;
            Loc = loc;

            InitializeComponent();

            this.Text = $"Location Contents - {loc.LocationId}";
            this.labelLocationId.Text = loc.LocationId;
            LocationContentsForm.UpdateLocationGridView(Proj, Loc, this.dataGridView1);
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
            MsgDispatch.AddListener<LocationContentsUpdated>(HandleLocationContentsUpdated);
            MsgDispatch.AddListener<CloseAllLocationFormsMsg>(HandleCloseAllLocationForms);
            //TODO: Start DB lock here!!
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        bool Dragging;

        void HandleCloseAllLocationForms(CloseAllLocationFormsMsg msg)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void HandleLocationContentsUpdated(LocationContentsUpdated msg)
        {
            LocationContentsForm.UpdateLocationGridView(Proj, Loc, this.dataGridView1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void cellContent_Click(Object sender, DataGridViewCellEventArgs args)
        {
            if (CurrentUser == null)
            {
                Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                return;
            }

            var grid = sender as DataGridView;
            if(grid.Columns[args.ColumnIndex] is DataGridViewButtonColumn && args.RowIndex >= 0)
            {
                var row = grid.Rows[args.RowIndex];
                string matId = row.Cells[RowMaterialIndex].Value as string;
                string locId = Loc.LocationId;
                if (Dialog.ConfirmAction($"Are you sure you want to remove the material '{matId}' from the location '{locId}'?") == DialogResult.Yes)
                {
                    Loc.RemoveItem(matId);
                    Warehouse.RemoveMaterialFromLocation(CurrentUser, Wh.WarehouseId, locId, matId);
                    MsgDispatch.PostMessage(new LocationContentsUpdated());
                }
                else return;
            }
        }


        #region Drag n Drop
        //private int rowIndexOfItemUnderMouseToDrop;
        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (
                Dragging &&
                (ModifierKeys & Keys.Control) != 0 &&
                (e.Button & MouseButtons.Left) == MouseButtons.Left
                )
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    if (CurrentUser == null)
                    {
                        Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                        return;
                    }

                    //TODO: Get all selected elements
                    var matId = dataGridView1.Rows[rowIndexFromMouseDown].Cells[2].Value as string;
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dataGridView1.DoDragDrop(
                    $"{matId}:{Loc.LocationId}",
                    DragDropEffects.Move);
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
            if(CurrentUser == null)
            {
                Dialog.Message($"This action cannot be performed by a user that is not logged in.");
                return;
            }
            Dragging = false;
            var matId = e.Data.GetData(DataFormats.Text).ToString();
            string srcLocId = null;
            StorageSpace srcLoc = null;

            //if this being moved from another location we can tell by the precence of the ':' delimeter
            if (matId.Contains(":"))
            {
                var split = matId.Split(':');
                matId = split[0];
                srcLocId = split[1];
                srcLoc = Wh.FindStorageLocation(srcLocId);
            }
            if (srcLoc == Loc) return;//don't allow dropping into self

            var material = Proj.FindMaterial(matId);
            if(matId == null)
            {
                Dialog.Message($"There was an error with the material id {matId}. The associated material data could not be found.");
                return;
            }
            else
            {
                //are we dragging from another locatoin or just the materials list?
                if (srcLocId == null)
                {
                    //just moving from materials list, nothing fancy here
                    Loc.SetQty(matId, 1);
                    Warehouse.SetMaterialInLocation(CurrentUser, Wh.WarehouseId, Loc.LocationId, matId, 1);
                }
                else
                {
                    //we're dragging from another location, will it be a copy operation or a move operation?
                    if ((e.KeyState & 4) == 4)
                    {
                        //shift is held, perform a copy
                        Loc.SetQty(matId, 1);
                        Warehouse.SetMaterialInLocation(CurrentUser, Wh.WarehouseId, Loc.LocationId, matId, 1);
                    }
                    else
                    {
                        //no shift, remove material from original location and move it to the new one
                        srcLoc?.RemoveItem(matId);
                        Loc.SetQty(matId, 1);
                        Warehouse.SetMaterialInLocation(CurrentUser, Wh.WarehouseId, Loc.LocationId, matId, 1);
                        Warehouse.RemoveMaterialFromLocation(CurrentUser, Wh.WarehouseId, srcLocId, matId);
                    }
                }

                //TODO: can we make this asyncronous?
                MsgDispatch.PostMessage(new LocationContentsUpdated());
            }
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
            MsgDispatch.RemoveListener<LocationContentsUpdated>(HandleLocationContentsUpdated);
            MsgDispatch.RemoveListener<CloseAllLocationFormsMsg>(HandleCloseAllLocationForms);
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
        public static void UpdateLocationGridView(Project proj, StorageSpace locData, DataGridView grid)
        {
            if (grid.Parent == null)
            {
                MessageBox.Show("Looks like James fucked up and forgot to remove the global message handlers for the location windows again. Go yell at him about it.");
                return;
            }

            grid.Rows.Clear();
            grid.Columns[0].Resizable = DataGridViewTriState.False;
            grid.Columns[0].Width = 25;

            var invalidIdList = new List<string>(2);
            foreach(var item in locData.Items)
            {
                var mat = proj.FindMaterial(item.Key);
                if (mat == null)
                    invalidIdList.Add(item.Key);
                else
                {
                    //FORMAT: delete button, kelox, sap, qty, desc
                    grid.Rows.Add(null, mat.DocumentId, mat.MatId, item.Value.ToString(), mat.Description);

                    var deleteButtonCell = grid.Rows[grid.Rows.Count - 1].Cells[0] as DataGridViewButtonCell;
                    deleteButtonCell.ToolTipText = "Removes this item from the current storage location.";
                    deleteButtonCell.OwningColumn.Resizable = DataGridViewTriState.False;
                }
            }

            if(invalidIdList.Count > 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (var id in invalidIdList)
                    sb.AppendLine(id);
                Dialog.Message($"There were a number of materials that could not be properly indentified in this location. They are as follows.\n\n{sb.ToString()}");

            }
        }
    }
}
