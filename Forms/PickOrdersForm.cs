using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SAOT.Forms
{
    public partial class PickOrdersForm : Form
    {
        Warehouse Wh;
        Project Proj;
        User CurrentUser;
        public BindingList<PickOrderVM> PickOrdersVM = new BindingList<PickOrderVM>();
        //readonly List<Vendor> Vendors;
        //readonly List<User> Users;

        /// <summary>
        /// 
        /// </summary>
        public class PickOrderVM
        {
            public string OrderId { get; set; }
            //public string OrderType { get; set; }
            public string CreatedBy { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateModified { get; set; }
            public string ShipToId { get; set; }
            public string InventoryAdjustId { get; set; }
            public string Status { get; set; }
            public bool AdjustedInSAP { get; set; }
            public string Carrier { get; set; }
            public string CarrierTracking { get; set; }
            
            public PickOrderVM()
            {

            }

            public PickOrderVM(PickOrder order)
            {
                OrderId = order.OrderId.ToString().PadLeft(6, '0');
                CreatedBy = User.GetUserNameFromId(order.CreatedById) ?? "Unknown";
                DateCreated = order.DateCreated;
                DateModified = order.DateModified;
                ShipToId = order.OrderType == (int)OrderTypes.InventoryCount ? "n/a" : Vendor.GetVendorNameFromId(order.ShipToId) ?? "Unknown";
                InventoryAdjustId = order.OrderType == (int)OrderTypes.InventoryCount ? "n/a" : Vendor.GetVendorNameFromId(order.InventoryAdjustId) ?? "Unkown";
                AdjustedInSAP = order.AdjustedInSAP;
                Carrier = order.OrderType == (int)OrderTypes.InventoryCount ? "n/a" : SAOT.Model.Carrier.GetCarrierNameFromId(order.CarrierId) ?? "Unknown";
                CarrierTracking = order.CarrierTracking;

                Status = Enum.GetName(typeof(PickOrderStatuses), order.Status);
                if (order.OrderType == (int)OrderTypes.InventoryCount)
                {
                    Status = Status.Replace("Pick", "Count");
                }

            }

        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="proj"></param>
        public PickOrdersForm(User currentUser, Warehouse wh, Project proj)
        {
            Wh = wh;
            Proj = proj;
            CurrentUser = currentUser;
            InitializeComponent();

            PickOrdersDatabase.ConfirmDatabaseExists();

            //read pick orders then convert them to viewmodel format
            var orders = PickOrdersDatabase.RequestAllPickOrders(OrderTypes.All, true, true, true, true, true, true);
            for(int i = orders.Count -1; i >= 0; i--)
                PickOrdersVM.Add(new PickOrderVM(orders[i]));

            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            UpdateGridView();


        }

        void UpdateGridView()
        {
            this.dataGridView1.DataSource = typeof(PickOrderVM);
            if (PickOrdersVM.Count > 0)
                this.dataGridView1.DataSource = PickOrdersVM;
            else this.dataGridView1.DataSource = typeof(PickOrderVM);
        }

        private void buttonAddOrder_Click(object sender, EventArgs e)
        {
            var createOrderForm = new CreatePickListForm(CurrentUser, Proj);
            if (createOrderForm.ErrorFlag) return;
            createOrderForm.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            int col = e.ColumnIndex;
            string headerText = dataGridView1.Columns[col].HeaderText;
            var row = dataGridView1.Rows[e.RowIndex];
            var cell = row.Cells[col];

            if(headerText == "OrderId")
            {
                //TODO: replace 'CreatePickListForm' with 'ViewPickListForm' window. Shows slightly different info
                //OPEN ORDER VIEWER HERE
                var createPickListForm = new CreatePickListForm(CurrentUser, Wh, Proj, Convert.ToInt32(cell.Value));
                createPickListForm.Show();
                this.Close();
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printPackingSlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
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



            //DataGridViewCellEventArgs args = e as DataGridViewCellEventArgs;
            //DataGridView grid = sender as DataGridView;
            //grid.

            /*
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            int col = e.ColumnIndex;
            string headerText = dataGridView1.Columns[col].HeaderText;
            var row = dataGridView1.Rows[e.RowIndex];
            var cell = row.Cells[col];

            if (headerText == "OrderId")
            {
                //TODO: replace 'CreatePickListForm' with 'ViewPickListForm' window. Shows slightly different info
                //OPEN ORDER VIEWER HERE
                var createPickListForm = new CreatePickListForm(CurrentUser, Wh, Proj, Convert.ToInt32(cell.Value));
                createPickListForm.Show();
                this.Close();
            }

            */
            MessageBox.Show("Print slip for order ffff #.");
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createDuplicateOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            var user = CurrentUser;
            if (user == null)
            {
                MessageBox.Show("This action cannot be performed while logged off.");
                return;
            }
            if (!user.CanPickOrders)
            {
                MessageBox.Show("You do not have access to pick/ship orders.");
                return;
            }

            var idIndex = WindowUtils.GetHeaderIndex(this.dataGridView1, "OrderId");
            var row = this.dataGridView1.Rows[CurrentRowIndex];
            var cell = row.Cells[idIndex];
            int orderId = int.Parse(cell.Value as string);

            var order = PickOrdersDatabase.RequestOrder(orderId);
            if (order == null)
            {
                MessageBox.Show(this, $"Order {orderId} could not be found in the database.");
                return;
            }
            if (order.OrderType != (int)OrderTypes.PickForVendor && order.OrderType != (int)OrderTypes.VendorNCR)
            {
                MessageBox.Show(this, "Cannot process this type of order for adjustment.");
                return;
            }
            */
        }
    }
}
