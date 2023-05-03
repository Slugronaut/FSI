using Newtonsoft.Json;
using SAOT.Model;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SAOT.Forms
{
    public partial class DisplayPickListsForm : Form
    {

        const int PendingIndex = 0;
        const int PickingIndex = 1;
        const int PickedIndex = 2;
        const int ShippedIndex = 3;
        const int AdjustedIndex = 4;
        const int ClosedIndex = 5;
        const int CanceledIndex = 6;

        public class PickOrderVM
        {
            public string OrderId { get; set; }
            public string OrderType { get; set; }
            public string ShipTo { get; set; }
            public string Created { get; set; }
            public string PickCount { get; set; }
            public string Status { get; set; }
        }

        Warehouse Wh;
        Project Proj;
        WeakReference CurrentUser;
        BindingList<PickOrderVM> PickOrdersVM = new BindingList<PickOrderVM>();



        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="proj"></param>
        public DisplayPickListsForm(User currentUser, Warehouse warehouse, Project proj)
        {
            CurrentUser = new WeakReference(currentUser);
            Wh = warehouse;
            Proj = proj;
            InitializeComponent();

            this.checkedListBoxOrderTypes.SetItemChecked(PendingIndex, true);
            this.checkedListBoxOrderTypes.SetItemChecked(PickingIndex, true);
            this.checkedListBoxOrderTypes.SetItemChecked(PickedIndex, true);
            this.checkedListBoxOrderTypes.SetItemChecked(ShippedIndex, true);
            this.checkedListBoxOrderTypes.SetItemChecked(AdjustedIndex, true);
            this.checkedListBoxOrderTypes.SetItemChecked(ClosedIndex, true);
            this.checkedListBoxOrderTypes.SetItemChecked(CanceledIndex, false);

            this.dataGridView1.CellMouseUp += this.dataGridView1_CellMouseUp;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;

            RequestOrdersAndUpdateModel();
            UpdateView();
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
            return dataGridView1.SelectedCells[0];
            //return dataGridView1.Rows[CurrentRowIndex].Cells[CurrentColIndex]
        }

        DataGridViewRow SelectedRow()
        {
            return dataGridView1.SelectedRows[0];
        }

        Material SelectedMaterial(Project proj, string headerId)
        {
            var row = SelectedRow();
            int headerIndex = WindowUtils.GetHeaderIndex(dataGridView1, headerId);
            var matIdCell = row.Cells[headerIndex];
            return proj.FindMaterial(matIdCell.Value as string);
        }
        #endregion

        public void RequestOrdersAndUpdateModel()
        {
            var orders = PickOrdersDatabase.RequestAllPickOrders(OrderTypes.All,
                this.checkedListBoxOrderTypes.GetItemChecked(PendingIndex),
                this.checkedListBoxOrderTypes.GetItemChecked(PickingIndex),
                this.checkedListBoxOrderTypes.GetItemChecked(PickedIndex),
                this.checkedListBoxOrderTypes.GetItemChecked(ShippedIndex),
                this.checkedListBoxOrderTypes.GetItemChecked(AdjustedIndex),
                this.checkedListBoxOrderTypes.GetItemChecked(ClosedIndex)
                );

            PickOrdersVM.Clear();
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    var pickList = PickOrdersDatabase.RequestPickList(order.OrderId);
                    int count = pickList == null ? 0 : pickList.Count;
                    var shipTo = Vendor.GetVendorNameFromId(order.ShipToId);
                    string orderType = "Unknown Type";
                    switch ((OrderTypes)order.OrderType)
                    {
                        case OrderTypes.InventoryCount:
                            {
                                orderType = "Inventory Count";
                                break;
                            }
                        case OrderTypes.PickForProduction:
                            {
                                orderType = "Prod Pick";
                                break;
                            }
                        case OrderTypes.PickForVendor:
                            {
                                orderType = "Vendor Pick";
                                break;
                            }
                        case OrderTypes.VendorNCR:
                            {
                                orderType = "Vendor NCR";
                                break;
                            }
                        case OrderTypes.CustomerNCR:
                            {
                                orderType = "Customer NCR";
                                break;
                            }
                    }

                    var povm = new PickOrderVM()
                    {
                        OrderId = order.OrderId.ToString().PadLeft(6, '0'),
                        OrderType = orderType,
                        ShipTo = order.OrderType == (int)OrderTypes.InventoryCount ? "n/a" : (shipTo == null ? "Unknown" : shipTo),
                        Created = order.DateCreated.ToString(),
                        PickCount = count.ToString(),

                        Status = Enum.GetName(typeof(PickOrderStatuses), order.Status),
                    };

                    if (order.OrderType == (int)OrderTypes.InventoryCount)
                    {
                        povm.Status = povm.Status.Replace("Pick", "Count");
                    }

                    PickOrdersVM.Add(povm);

                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void UpdateView()
        {
            this.dataGridView1.DataSource = typeof(PickOrderVM);
            this.dataGridView1.DataSource = PickOrdersVM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            var user = CurrentUser.Target as User;
            if(user == null)
            {
                MessageBox.Show("This action cannot be performed while logged off.");
                return;
            }
            if(!user.CanPickOrders)
            {
                MessageBox.Show("You do not have access to pick orders.");
                return;
            }


            int col = e.ColumnIndex;
            string headerText = dataGridView1.Columns[col].HeaderText;
            var row = dataGridView1.Rows[e.RowIndex];
            var cell = row.Cells[col];

            var idCell = row.Cells[0].Value;
            var pickOrderForm = new PickForm(user, Wh, Proj, Convert.ToInt32(idCell));
            pickOrderForm.Show();
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static int GetSelectedItemPickId(DataGridView grid, int rowIndex)
        {
            var idIndex = WindowUtils.GetHeaderIndex(grid, "OrderId");
            var row = grid.Rows[rowIndex];
            var cell = row.Cells[idIndex];
            if (!int.TryParse(cell.Value as string, out int orderId))
            {
                MessageBox.Show("Could not locate the order for this item.");
                return -1;
            }

            //get all items assoaciated with this order
            return orderId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printPackingSlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var id = GetSelectedItemPickId(this.dataGridView1, CurrentRowIndex);
            if(id >= 0)
            {
                WindowUtils.PrintPackingSlip(this, this.Proj, id);
            }
        }

        private void markAsShippedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = CurrentUser.Target as User;
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
            if(order == null)
            {
                MessageBox.Show(this, $"Order {orderId} could not be found in the database.");
                return;
            }
            if(order.OrderType != (int)OrderTypes.PickForVendor && order.OrderType != (int)OrderTypes.VendorNCR && order.OrderType != (int)OrderTypes.CustomerNCR)
            {
                MessageBox.Show(this, "Cannot process this type of order for shipment.");
                return;
            }

            var trackingForm = new ProvideTrackingNumberForm(user, order.CarrierId, order.CarrierTracking, orderId);
            trackingForm.ShowDialog(this);
            PickOrdersDatabase.ShipOrder(user, orderId);
            this.RequestOrdersAndUpdateModel();
            this.UpdateView();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBoxOrderTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RequestOrdersAndUpdateModel();
            this.UpdateView();
        }

        private void markAsAdjustedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = CurrentUser.Target as User;
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

            PickOrdersDatabase.SetAdjusted(user, orderId, true);
            this.RequestOrdersAndUpdateModel();
            this.UpdateView();
        }
    }
}
