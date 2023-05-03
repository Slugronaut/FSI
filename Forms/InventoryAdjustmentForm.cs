using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SAOT
{
    public partial class InventoryAdjustmentForm : Form, IProjectProvider
    {
        string SelectedWarehouse => this.comboWarehouseChoice.Text;
        User CurrentUser;
        Project Project;
        Warehouse WarehouseData;
        List<Aisle> AisleData;
        System.Threading.Timer Timer = null;
        Regex FilterReg = null;
        bool BeingResized = false;

        static List<LocationContentsForm> LocForms = new List<LocationContentsForm>();
        public Project CurrentSelectedProject { get => Project; }

        public InventoryAdjustmentForm(User currentUser, Project proj)
        {
            CurrentUser = currentUser;
            Project = proj;
            InitializeComponent();
            UpdateWarehouseChoiceList();
            UpdateLocationGrid();

            this.FormClosing += HandleSelfClosing;
            //this.listView1.MouseDoubleClick += HandleLocationSelected;
            this.textBoxFilter.KeyPress += CheckEnterKeyPress;
            this.FormClosing += OnFormClosing;
            MsgDispatch.AddListener<WarehouseModelUpdated>(HandleLocationContentsUpdated);
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            //this.Opacity = 0.6;
            locationsPanel.Visible = false;
            //locationsPanel.SuspendDrawing();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            //locationsPanel.ResumeDrawing();
            locationsPanel.Visible = true;
            //this.Opacity = 1.0;
        }

        void OnFormClosing(Object sender, EventArgs args)
        {
            StorageSpace.ListItemPool.RelenquishAll();
            MsgDispatch.RemoveListener<WarehouseModelUpdated>(HandleLocationContentsUpdated);
        }

        void HandleLocationContentsUpdated(WarehouseModelUpdated msg)
        {
            var scrollPos = new Point(Math.Abs(locationsPanel.AutoScrollPosition.X), Math.Abs(locationsPanel.AutoScrollPosition.Y));
            this.locationsPanel.Controls.Clear(true);
            UpdateLocationGrid();
            this.locationsPanel.AutoScrollPosition = scrollPos;
        }

        private void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            /*
            if (e.KeyChar == (char)Keys.Return)
            {
                int index = 0;
                foreach(var itemObj in listView1.Items)
                {
                    ListViewItem item = itemObj as ListViewItem;
                    if (item.ToolTipText == textBoxFilter.Text)
                    {
                        listView1.EnsureVisible(index);
                        item.Selected = true;
                        item.EnsureVisible();
                        return;
                    }
                    index++;
                }
            }
            */
        }

        static public void AddLocationForm(LocationContentsForm form)
        {
            if(form != null)
                LocForms.Add(form);
        }

        static public void ReoveLocationForm(LocationContentsForm form)
        {
            if (form != null)
                LocForms.Remove(form);
        }

        void HandleSelfClosing(Object sender, EventArgs args)
        {
            //close all child windows - i.e. FormContentsForms
            this.FormClosing -= HandleSelfClosing;
            LocationContentsForm[] temp = new LocationContentsForm[LocForms.Count];
            LocForms.CopyTo(temp);
            foreach (var form in temp)
                form.Close();

            LocForms.Clear();
        }

        void HandleLocationSelected(Object sender, MouseEventArgs args)
        {
            var listView = sender as ListView;

            for (int i = 0; i < listView.Items.Count; i++)
            {
                var item = listView.GetItemAt(args.X, args.Y);
                if (item != null)
                {
                    var locId = item.Group.Name + item.Text;
                    //get the StorageSpace object via the location id
                    var locForm = new LocationContentsForm(CurrentUser, Project, WarehouseData, WarehouseData.FindStorageLocation(locId));
                    locForm.Show();
                    return;
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            UpdateLocationGrid();
        }

        void UpdateWarehouseChoiceList()
        {
            var names = Warehouse.RequestWarehouseNames();
            if (names == null || names.Count < 1)
            {
                MessageBox.Show("There are no warehouse databases available. You can create a warehouse database and a default warehouse from the main menu under \"File -> Database Source\" and pressing the \"Create Warehouse Database\" button.");
                this.Close();
                return;
            }

            foreach (var name in names)
                this.comboWarehouseChoice.Items.Add(name);

            this.comboWarehouseChoice.SelectedIndex = 0;
        }

        void UpdateLocationGrid()
        {
            if (BeingResized) return;
            try
            {
                WarehouseData = Warehouse.RequestWarehouseData(SelectedWarehouse);
                AisleData = WarehouseData.AisleFormatStorage(
                    ImportLocationDataForm.LOC_AISLE_COLUMNS, 
                    ImportLocationDataForm.LOC_RACK_COLUMNS, 
                    ImportLocationDataForm.LOC_LEVEL_COLUMNS, 
                    ImportLocationDataForm.LOC_BIN_COLUMNS);
            }
            catch(Exception e)
            {
                Dialog.Message("There was an error while attempting to load the warehouse data.\n\n"+e.Message);
            }
            //StorageSpace.PropogateListView(AisleData, this.listView1, FilterReg);
            ButtonGrid.PropogateAisleView(WarehouseData, this.locationsPanel, AisleData, FilterReg, true, false, OnItemClicked);
        }

        void OnItemClicked(Button sender, ButtonGrid group)
        {
            var button = sender as Button;
            var locId = group.GroupId + button.Text;
            var locForm = new LocationContentsForm(CurrentUser, Project, WarehouseData, WarehouseData.FindStorageLocation(locId));
            locForm.Show();
        }
        

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            /*
            //TODO: start timer and don't perform update until a brief time has passed since the last keypress
            var filter = sender as TextBox;
            if (!filter.ContainsFocus)
                return;

            DisposeTimer();
            Timer = new System.Threading.Timer(TimerElapsed, filter, FilterDelay, FilterDelay);
            */
        }


        #region Timer
        int FilterDelay = 1500;

        void DisposeTimer()
        {
            if (Timer != null)
            {
                Timer.Dispose();
                Timer = null;
            }
        }

        void TimerElapsed(Object obj)
        {
            DoFilter(textBoxFilter);
            DisposeTimer();
        }

        void DoFilter(TextBox filter)
        {
            /*
            //invoke so that it's performed on the UI thread
            this.Invoke(new Action(() =>
            {
                string text = filter.Text;
                FilterReg = string.IsNullOrEmpty(text) ? null : new Regex(FilterUtil.WildCardToRegular(filter.Text, true), RegexOptions.IgnoreCase);
                StorageSpace.PropogateListView(AisleData, this.listView1, FilterReg);
            }));
            */
        }
        #endregion

    }
}
