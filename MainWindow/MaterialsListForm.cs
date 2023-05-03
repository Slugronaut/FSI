using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAOT
{
    public partial class MaterialsListForm : Form
    {
        Project Proj;

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        //private int rowIndexOfItemUnderMouseToDrop;

        DataGridView DataGrid { get => filterableDataGridView1.GridView; }

        public MaterialsListForm(Project proj)
        {
            Proj = proj;
            InitializeComponent();
            this.FormClosing += HandleClosing;

            DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGrid.MultiSelect = true;
            DataGrid.AllowDrop = true;
            DataGrid.MouseMove += dataGridView1_MouseMove;
            DataGrid.MouseDown += dataGridView1_MouseDown;
            //this.filterableDataGridView1.GridView.Dra


            filterableDataGridView1.ClearColumns();
            filterableDataGridView1.CreateColumns("Id", "Doc", "Description");
            PopulateFilteredMaterialsList(Proj, filterableDataGridView1);
            MsgDispatch.AddListener<FilterUpdatedMessage>(HandleFilterUpdated);
            DataGrid.MultiSelect = true;
            DataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }

        void HandleClosing(Object sender, EventArgs args)
        {
            MsgDispatch.RemoveListener<FilterUpdatedMessage>(HandleFilterUpdated);
        }

        void HandleFilterUpdated(FilterUpdatedMessage msg)
        {
            PopulateFilteredMaterialsList(Proj, filterableDataGridView1);
        }

        public static void PopulateFilteredMaterialsList(Project proj, FilterableDataGridView view)
        {
            List<List<string>> data = new List<List<string>>();
            foreach (var mat in proj.MaterialsList)
            {
                data.Add(new List<string>() { mat.MatId, mat.DocumentId, mat.Description });
                //view.GridView.Rows.Add(mat.MatId, mat.DocumentId, mat.Description);
            }

            view.PushModelToView(data, true);
        }

        private void filterableDataGridView1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    var matId = DataGrid.Rows[rowIndexFromMouseDown].Cells[0].Value as string;
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = DataGrid.DoDragDrop(
                    matId,
                    DragDropEffects.Move);
                }
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = DataGrid.HitTest(e.X, e.Y).RowIndex;
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
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }
    }
}
