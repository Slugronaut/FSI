using SAOT.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SAOT.Forms
{
    public partial class PickForm : Form
    {
        WeakReference CurrentUser;
        Warehouse Wh;
        Project Proj;
        int OrderId;
        OrderTypes OrderType;

        PickOrder PickOrder;
        List<PickListItem> PickList;
        Vendor ShipTo;
        Vendor AdjustTo;
        int CurrentItemIndex;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="proj"></param>
        /// <param name="orderId"></param>
        public PickForm(User currentUser, Warehouse warehouse, Project proj, int orderId)
        {
            CurrentUser = new WeakReference(currentUser);
            Wh = warehouse;
            Proj = proj;
            OrderId = orderId;

            InitializeComponent();

            PickOrder = PickOrdersDatabase.RequestOrder(orderId);
            PickList = PickOrdersDatabase.RequestPickList(orderId);

            var shipName = Vendor.GetVendorNameFromId(PickOrder.ShipToId);
            var adjustName = Vendor.GetVendorNameFromId(PickOrder.InventoryAdjustId);
            ShipTo = Vendor.RequestVendor(shipName);
            AdjustTo = Vendor.RequestVendor(adjustName);




            this.textBoxOrderId.Text = OrderId.ToString().PadLeft(6, '0');
            this.textBoxShipTo.Text = shipName;
            this.textBoxAdjustTo.Text = adjustName;
            this.textBoxShippingAddress.Text = ShipTo.FullAddress;
            this.textBoxDoc.DoubleClick += onDoubleClick;
            //this.listViewPickItems.AutoResizeColumn(0, 200);

            ColumnHeader header = new ColumnHeader();
            header.Text = "";
            header.Name = "col1";
            header.Width = listViewPickItems.Width;
            listViewPickItems.Columns.Clear();
            listViewPickItems.Columns.Add(header);

            listViewPickItems.View = System.Windows.Forms.View.SmallIcon;
            //listViewPickItems.View = View.Details;
            //listViewPickItems.HeaderStyle = ColumnHeaderStyle.None;
            //listViewPickItems.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            FillPickList();
            listViewPickItems.Items[0].Focused = true;
            listViewPickItems.Items[0].Selected = true;

            this.FormClosing += HandleFormClosing;

            OrderType = (OrderTypes)(PickOrder.OrderType);
            if(OrderType == OrderTypes.InventoryCount)
            {
                textBoxRequestQty.Visible = false;
                labelRequestedQty.Visible = false;
                buttonPick.Enabled = false;
                buttonPick.Visible = false;
                buttonConfirmCount.Enabled = true;
                buttonConfirmCount.Visible = true;
                labelPickedQty.Text = "Count Qty:";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void HandleFormClosing(object sender, EventArgs args)
        {
            this.FormClosing -= HandleFormClosing;
            var displayPicklistForm = new DisplayPickListsForm(CurrentUser.Target as User, this.Wh, Proj);
            displayPicklistForm.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string FormattedPickListItem(PickListItem item)
        {
            var mat = Proj.FindMaterial(item.MaterialId);
            return string.Format("{0, -14} x{1,-4}/{2,-7}{3,-2}", item.MaterialId, item.PickedQty, item.RequestedQty, mat.UoM);
        }

        /// <summary>
        /// 
        /// </summary>
        void FillPickList()
        {
            listViewPickItems.Clear();
            listViewPickItems.SmallImageList = this.imageList1;
            if (PickList != null && PickList.Count > 0)
            {
                foreach (var item in PickList)
                {
                    
                    ListViewItem li = new ListViewItem();
                    li.Tag = item.MaterialId;
                    li.Text = FormattedPickListItem(item);
                    if (item.Status == PickItemStatuses.Picked)
                        li.ImageIndex = 1;
                    else if (item.Status == PickItemStatuses.Canceled)
                        li.ImageIndex = 0;
                    else li.ImageIndex = -1;
                    listViewPickItems.Items.Add(li);
                }

                var curLi = listViewPickItems.Items[0];
                UpdatePickItemView(PickList[0]);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void ClearPickItemView()
        {
            textBoxMatId.Text = string.Empty;
            textBoxDoc.Text = string.Empty;
            textBoxDesc.Text = string.Empty;
            textBoxRequestQty.Text = string.Empty;
            numericPickedQty.Value = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRef"></param>
        /// <returns></returns>
        bool ConfirmUserAction(WeakReference userRef)
        {
            var user = userRef.Target as User;
            if (user == null || !CurrentUser.IsAlive || !user.CanPickOrders)
            {
                MessageBox.Show(this, "You do not have authorization to perform this task.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickItem"></param>
        void UpdatePickItemView(PickListItem pickItem)
        {
            if (!ConfirmUserAction(CurrentUser))
            {
                this.Close();
                return;
            }


            numericPickedQty.Enabled = false;
            if (pickItem == null)
            {
                ClearPickItemView();
                return;
            }

            var material = Proj.FindMaterial(pickItem.MaterialId);
            if(material == null)
            {
                ClearPickItemView();
                return;
            }

            numericPickedQty.Enabled = true;

            textBoxMatId.Text = material.MatId;
            textBoxDoc.Text = material.DocumentId;
            textBoxDesc.Text = material.Description;
            textBoxRequestQty.Text = pickItem.RequestedQty.ToString();
            textBoxUoM.Text = material.UoM;
            numericPickedQty.Value = pickItem.PickedQty;

            //HACK ALERT: todo fix this!!
            //harded coded logic for adjusting numeric decimal properties based on UoM
            if (material.UoM.ToUpper() == "M")
                numericPickedQty.DecimalPlaces = 3;
            else numericPickedQty.DecimalPlaces = 0;


            numericPickedQty.Enabled = true;
            buttonPick.Enabled = true;
            if (pickItem.Status == PickItemStatuses.Canceled)
            {
                buttonPick.Text = "CANCELLED";
                buttonPick.Enabled = false;
                buttonPick.BackColor = Color.Red;
            }
            if(pickItem.Status == PickItemStatuses.PickPending)
            {
                buttonPick.Text = "Pick";
                buttonPick.BackColor = Color.LightGreen;
                buttonConfirmCount.Text = "Confirm Count";
                buttonConfirmCount.BackColor = Color.LightGreen;
            }
            if(pickItem.Status == PickItemStatuses.Picked)
            {
                buttonPick.Text = "Revert Pick";
                buttonPick.BackColor = Color.Yellow;
                buttonConfirmCount.Text = "Revert Count";
                buttonConfirmCount.BackColor = Color.Yellow;
                numericPickedQty.Enabled = false;
            }

            //print all locations available
            listViewLocs.Clear();
            var locs = Wh.FindAllMaterialLocations(pickItem.MaterialId);
            listViewLocs.Items.AddRange(locs.Select( x => new ListViewItem(x.LocationId)).ToArray() );

        }

        private void listViewPickItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listView = sender as ListView;
            if (listView.SelectedIndices.Count > 0)
            {
                CurrentItemIndex = listView.SelectedIndices[0];
                UpdatePickItemView(PickList[CurrentItemIndex]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateCurrentPickItemInList()
        {
            var pickItem = PickList[CurrentItemIndex];
            listViewPickItems.Items[CurrentItemIndex].Text = FormattedPickListItem(pickItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPick_Click(object sender, EventArgs e)
        {
            if (!ConfirmUserAction(CurrentUser))
            {
                this.Close();
                return;
            }

            int currIndex = CurrentItemIndex;
            decimal pickedQty = this.numericPickedQty.Value;
            var currPick = PickList[CurrentItemIndex];
            Material mat = Proj.FindMaterial(currPick.MaterialId);


            #region Validate
            //confirm that pick value is a number and that its type matches the UoM
            if (currPick.Status == PickItemStatuses.PickPending)
            {
                if (pickedQty < 0)
                {
                    MessageBox.Show(this, "You must specify a pick quantity of zero or greater.", "Invalid Pick Quantity");
                    return;
                }
                if (mat.UoM.ToUpper() != "M")
                {
                    if (!(pickedQty % 1 == 0))
                    {
                        MessageBox.Show(this, $"The material '{mat.MatId}' must be picked in whole integer quantities.", "Pick Qauntity Invalid");
                        return;
                    }
                }
                if (pickedQty < currPick.RequestedQty)
                {
                    if (MessageBox.Show(this, $"The picked quantity is LESS than the requested quantity.\nAre you sure you want to continue?", "Pick Quantity Unexpected", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                else if (pickedQty > currPick.RequestedQty)
                {
                    if (MessageBox.Show(this, $"The picked quantity is GREATER than the requested quantity.\nAre you sure you want to continue?", "Pick Quantity Unexpected", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
            }
            #endregion


            #region Update Database
            switch (currPick.Status)
            {
                case PickItemStatuses.Canceled:
                    {
                        //technically should never happen since the button should already be disabled at this point but.... just in case
                        MessageBox.Show(this, $"The pick for material '{mat.MatId}' has been cancelled.", "Cancelled Pick");
                        break;
                    }
                case PickItemStatuses.Picked:
                    {
                        PickOrdersDatabase.ReversePick(CurrentUser.Target as User, currPick.PickItemId);
                        PickOrdersDatabase.SetOrderStateAsPending(CurrentUser.Target as User, OrderId);
                        currPick.Status = PickItemStatuses.PickPending;
                        currPick.PickedQty = -1;
                        UpdatePickListIcon(currIndex);
                        UpdatePickItemView(PickList[CurrentItemIndex]); //refresh view based on state
                        listViewPickItems.Items[CurrentItemIndex].Focused = true;
                        listViewPickItems.Items[CurrentItemIndex].Selected = true;
                        break;
                    }
                case PickItemStatuses.PickPending:
                    {
                        PickOrdersDatabase.PerformPick(CurrentUser.Target as User, currPick.PickItemId, pickedQty);
                        currPick.Status = PickItemStatuses.Picked;
                        currPick.PickedQty = pickedQty;
                        UpdatePickListIcon(currIndex);
                        break;
                    }
                default:
                    {
                        {
                            //also technically should never happen but.... just in case
                            MessageBox.Show(this, $"The status for material '{mat.MatId}' is in an unknown state.\nThis action cannot be performed.", "Unexpected Pick State");
                            break;
                        }
                    }
            }

            if (checkPrintTicket.Checked)
            {
                var loc = listViewLocs.Items.Count > 0 ? listViewLocs.Items[0].Text : string.Empty;
                WindowUtils.PrintMaterialLabel(mat, currPick.PickedQty, currPick.RequestedQty, loc);
            }
            #endregion



            #region Update View
            UpdateCurrentPickItemInList();

            //no need to go further, we've reverted this pick and will not bother incrementing now
            if (currPick.Status == PickItemStatuses.PickPending)
                return;

            if (!GetNextPickableItem(out PickListItem nextItem, out CurrentItemIndex))
            {
                MessageBox.Show(this, "Pick Order Complete.", "Pick Order Complete");
                ValidatePickOrderComplete((OrderTypes)this.PickOrder.OrderType);
                PickOrdersDatabase.CompletePick(CurrentUser.Target as User, OrderId);
                PrintPackingSlip();
                this.Close();
                return;
            }
            else
            {
                listViewPickItems.Items[CurrentItemIndex].Focused = true;
                listViewPickItems.Items[CurrentItemIndex].Selected = true;
            }
            #endregion


            //make the qty entry textbox selected by default
            this.labelPickedQty.Select();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfirmCount_Click(object sender, EventArgs e)
        {
            if (!ConfirmUserAction(CurrentUser))
            {
                this.Close();
                return;
            }

            int currIndex = CurrentItemIndex;
            decimal pickedQty = this.numericPickedQty.Value;
            var currPick = PickList[CurrentItemIndex];
            Material mat = Proj.FindMaterial(currPick.MaterialId);


            #region Validate
            //confirm that pick value is a number and that its type matches the UoM
            if (currPick.Status == PickItemStatuses.PickPending)
            {
                if (pickedQty < 0)
                {
                    MessageBox.Show(this, "You must specify a count quantity of zero or greater.", "Invalid Count Quantity");
                    return;
                }
                if (mat.UoM.ToUpper() != "M")
                {
                    if (!(pickedQty % 1 == 0))
                    {
                        MessageBox.Show(this, $"The material '{mat.MatId}' must be counted in whole integer quantities.", "Count Qauntity Invalid");
                        return;
                    }
                }

            }
            #endregion


            #region Update Database
            switch (currPick.Status)
            {
                case PickItemStatuses.Canceled:
                    {
                        //technically should never happen since the button should already be disabled at this point but.... just in case
                        MessageBox.Show(this, $"The count for material '{mat.MatId}' has been cancelled.", "Cancelled Count");
                        break;
                    }
                case PickItemStatuses.Picked:
                    {
                        PickOrdersDatabase.ReversePick(CurrentUser.Target as User, currPick.PickItemId);
                        PickOrdersDatabase.SetOrderStateAsPending(CurrentUser.Target as User, OrderId);
                        currPick.Status = PickItemStatuses.PickPending;
                        currPick.PickedQty = -1;
                        UpdatePickListIcon(currIndex);
                        UpdatePickItemView(PickList[CurrentItemIndex]); //refresh view based on state
                        listViewPickItems.Items[CurrentItemIndex].Focused = true;
                        listViewPickItems.Items[CurrentItemIndex].Selected = true;
                        break;
                    }
                case PickItemStatuses.PickPending:
                    {
                        PickOrdersDatabase.PerformPick(CurrentUser.Target as User, currPick.PickItemId, pickedQty);
                        currPick.Status = PickItemStatuses.Picked;
                        currPick.PickedQty = pickedQty;
                        UpdatePickListIcon(currIndex);
                        break;
                    }
                default:
                    {
                        {
                            //also technically should never happen but.... just in case
                            MessageBox.Show(this, $"The status for material '{mat.MatId}' is in an unknown state.\nThis action cannot be performed.", "Unexpected Item State");
                            break;
                        }
                    }
            }
            #endregion


            #region Increment
            UpdateCurrentPickItemInList();

            //no need to go further, we've reverted this pick and will not bother incrementing now
            if (currPick.Status == PickItemStatuses.PickPending)
                return;

            if (!GetNextPickableItem(out PickListItem nextItem, out CurrentItemIndex))
            {
                MessageBox.Show(this, "Inventory Count Order Complete.", "Count Order Complete");
                ValidatePickOrderComplete((OrderTypes)this.PickOrder.OrderType);
                PickOrdersDatabase.CompletePick(CurrentUser.Target as User, OrderId);
                PrintPackingSlip();
                this.Close();
                return;
            }
            else
            {
                listViewPickItems.Items[CurrentItemIndex].Focused = true;
                listViewPickItems.Items[CurrentItemIndex].Selected = true;
            }
            #endregion


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextItem"></param>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        bool GetNextPickableItem(out PickListItem nextItem, out int nextIndex)
        {
            nextItem = null;
            nextIndex = CurrentItemIndex;
            int counter = 0;
            while (counter < PickList.Count)
            {
                nextIndex++;
                if (nextIndex >= PickList.Count)
                    nextIndex = 0;
                counter++;

                if (PickList[nextIndex].Status == PickItemStatuses.PickPending)
                {
                    nextItem = PickList[nextIndex];
                    CurrentItemIndex = nextIndex;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        bool ValidatePickOrderComplete(OrderTypes orderType)
        {
            List<PickListItem> backOrderItems = new List<PickListItem>(10);
            foreach(var item in PickList)
            {
                if(item.Status != PickItemStatuses.Picked && item.Status != PickItemStatuses.Canceled)
                {
                    return false;
                }
                else if(item.Status == PickItemStatuses.Picked && orderType != OrderTypes.InventoryCount)
                {
                    if(item.PickedQty < item.RequestedQty)
                        backOrderItems.Add(item);
                }
            }


            if(backOrderItems.Count > 0)
            {
                MessageBox.Show(this, "Some items in this order have been underpicked. A backorder has been automatically filled.", "Underpicked Items Found", MessageBoxButtons.YesNo);
                //TODO: generate backorder
                MessageBox.Show(this, "Feature not yet implmented.");
                
            }

            return true;
        }

        public void PrintPackingSlip()
        {
            WindowUtils.PrintPackingSlip(this, this.Proj, OrderId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickIndex"></param>
        void UpdatePickListIcon(int pickIndex)
        {
            var li = listViewPickItems.Items[pickIndex];
            var item = PickList[pickIndex];

            if (item.Status == PickItemStatuses.Picked)
                li.ImageIndex = 1;
            else if (item.Status == PickItemStatuses.Canceled)
                li.ImageIndex = 0;
            else li.ImageIndex = -1;
        }

        private void textBoxDoc_TextChanged(object sender, EventArgs e)
        {

        }

        void onDoubleClick(object sender, EventArgs args)
        {
            DrawingLookup.StartLookupDrawingTask(this.textBoxDoc.Text);
        }
    }
}
