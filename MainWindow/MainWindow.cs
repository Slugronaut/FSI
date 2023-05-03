using SAOT.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Form
    {
        User CurrentUser;
        Project Proj;
        Warehouse CurrentWarehouse;
        string SelectedWarehouse => this.comboWarehouseChoice.Text;

        public enum MenuModes
        {
            Unconfigured,
            ReadyForLogin,
            LoggedIn,
        }

        public MainWindow()
        {
            InitializeComponent();
            MsgDispatch.AddListener<WarehouseCreatedMessage>(HandleWarehouseCreated);
            MsgDispatch.AddListener<UserLoggedInEvent>(HandleUserLoggedIn);
            MsgDispatch.AddListener<UserLoggedOutEvent>(HandleUserLoggedOut);
            MsgDispatch.AddListener<LocationContentsUpdated>(HandleLocationContentsUpdated);
            MsgDispatch.AddListener<FilterUpdatedMessage>(HandleFilterUpdated);

            this.filterableDataGridView1.GridView.CellMouseDoubleClick += HandleDoubleClick;
        }

        void OpenPdfDoc(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                Dialog.Message($"No document found for {file}.");
                return;
            }
            else
            {
                //Dialog.Message($"{file}.");
                //ThreadStart ths = new ThreadStart(delegate () { System.Diagnostics.Process.Start(file); });
                System.Diagnostics.Process.Start(file);
            }
        }

        void HandleDoubleClick(Object sender, DataGridViewCellMouseEventArgs args)
        {
            var grid = filterableDataGridView1.GridView;
            int col = args.ColumnIndex;

            if(grid.Columns[col].HeaderText == "Kelox")
            {
                Task t = Task.Run(() => 
                {
                    var row = grid.Rows[args.RowIndex];
                    var cell = row.Cells[col];
                    var docId = cell.Value as string;

                    var modAndRev = ModIdAndRev(docId);
                    if (modAndRev.Length == 4 && !string.IsNullOrEmpty(modAndRev[0]))
                    {
                        var file = FindDrawingFromModId(modAndRev[0], modAndRev[1], modAndRev[2], modAndRev[3]);
                        OpenPdfDoc(file);
                    }
                    else
                    {
                        Dialog.Message("Could not determine Mod ID, part type, part number, or revision number.");
                        return;
                    }
                });

            }
            if (grid.Columns[col].HeaderText == "Location" && args.RowIndex >= 0)
            {
                var row = grid.Rows[args.RowIndex];
                var cell = row.Cells[col];
                var locId = cell.Value as string;
                if (!string.IsNullOrWhiteSpace(locId))
                {
                    var loc = this.CurrentWarehouse.FindStorageLocation(locId);
                    if(loc == null)
                    {
                        Dialog.Message($"There was an error trying to access the location information '{locId}'.");
                        return;
                    }
                    var locForm = new LocationContentsForm(CurrentUser, Proj, CurrentWarehouse, loc);
                    locForm.Show();
                }
            }
        }

        const string regStr = @"\(([^)]*)\)";
        static Regex reg = new Regex(regStr, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string[] ModIdAndRev(string docId)
        {
            string rev = "0";
            var token = Regex.Replace(docId, @"\s", "");
            if (token.Contains("KR002"))
            {
                int revIndex = token.LastIndexOf("_");
                if (revIndex > -1)
                    rev = token.Substring(revIndex+1, 1);

                var modIndex = token.IndexOf("DR");
                modIndex += 2;
                string mod = token.Substring(modIndex, 3);

                string partType = token.Substring(modIndex + 3, 1); //p, s, c, g?
                string partNum = token.Substring(modIndex + 4, 3);
                return new string[] { mod, partType, partNum, rev };
                /*
                var match = reg.Match(token);
                if (match.Success)
                {
                    var modId = match.Value.Trim();
                    modId = modId.Remove(0, 1);
                    modId = modId.Remove(modId.Length - 1, 1);

                    var temp = modId.Split('D');
                    var modNum = temp[1];
                    modNum = modNum.Split('R')[1];
                    modNum = modNum.Split('G')[0];

                    var revNum = "0";
                    temp = modId.Split('_');
                    if (temp != null && temp.Length > 1)
                        revNum = temp[1];

                    return new string[] { modNum, revNum };
                }
                */
                ///else MessageBox.Show(this, "Mod token found but could not parse. " + match.Value);
            }

            return new string[] { string.Empty, string.Empty, string.Empty, string.Empty };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modId"></param>
        /// <returns></returns>
        public static string FindDrawingFromModId(string modId, string partType, string partNum, string rev)
        {
            var rootDir = Config.ReadConfigStr(Config.DrawingsRootDirId);
            if(string.IsNullOrEmpty(rootDir) || !Directory.Exists(rootDir))
            {
                Dialog.Message("The directory for the document drawings has not been properly set.");
                return string.Empty;
            }
            //gather a list of all folders for a given mod, for any revision level
            int revCount = string.IsNullOrEmpty(rev) ? 0 : int.Parse(rev);
            while (revCount >= 0)
            {
                string[] dirs = null;
                if(modId == "000")
                {
                    foreach (var file in Directory.EnumerateFiles($"{rootDir}", $"*kr002*{modId}*{partType}*{partNum}*.pdf", SearchOption.AllDirectories))
                    {
                        return file;
                    }
                    Dialog.Message("Searching for drawings of triple-zero materials is not currently implemented.");
                    return null;
                }
                else dirs = Directory.GetDirectories(rootDir, modId + "_BOM*_REV*" + revCount, SearchOption.TopDirectoryOnly);
                if (dirs.Length == 0)
                    Console.WriteLine("No directories found for mod " + modId + " at revision " + rev);
                foreach (var dir in dirs)
                {
                    //try all dirs that match
                    var files = Directory.GetFiles(dir, $"kr002*{modId}*{partType}*{partNum}*.pdf", SearchOption.AllDirectories);

                    if (files.Length > 1)
                        Console.WriteLine("Warning: For some reason there is more than one g-level drawing for mod " + modId + "Rev + " + rev);
                    foreach (var file in files)
                    {
                        //check for a pdf that is a g-level drawing
                        Console.WriteLine("Found a drawing file at: " + file);
                        return file;
                    }
                }
                revCount--;
            }


            return string.Empty;
        }

        void HandleFilterUpdated(FilterUpdatedMessage msg)
        {
            if (msg.Parent == this)
            {
                PushModelToView(CurrentWarehouse, Proj);
            }
        }

        void HandleLocationContentsUpdated(LocationContentsUpdated msg)
        {
            //this.errorProvider1.SetError(this.buttonRefresh, "The inventory view is out of date. Click refresh to update it.");
            buttonRefresh_Click(null, null);
        }

        public void AppBegin()
        {
            Proj = new Project("Avelia");
            UpdateWarehouseChoiceList();
            UpdateGridSync();
        }

        void HandleWarehouseCreated(WarehouseCreatedMessage msg)
        {
            UpdateWarehouseChoiceList();
        }

        void UpdateWarehouseChoiceList()
        {
            var names = Warehouse.RequestWarehouseNames();
            if (names == null || names.Count < 1)
            {
                //MessageBox.Show("There are no warehouse databases available. You can create a warehouse database and a default warehouse from the main menu under \"File -> Database Source\" and pressing the \"Create Warehouse Database\" button.");
                return;
            }

            this.comboWarehouseChoice.Items.Clear();
            foreach (var name in names)
                this.comboWarehouseChoice.Items.Add(name);

            this.comboWarehouseChoice.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void HandleUserLoggedIn(UserLoggedInEvent msg)
        {
            CurrentUser = msg.User;
            this.MenuMode = MenuModes.LoggedIn;
            //Proj = new Project("Avelia");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void HandleUserLoggedOut(UserLoggedOutEvent msg)
        {
            CurrentUser = null;
            this.MenuMode = MenuModes.ReadyForLogin;
            //Proj = null;
        }

        /// <summary>
        /// Sets the state of the main menu based on the app's internal state.
        /// 
        /// TODO: Fix this hardcoded mess! It relies on specific ordering of menu items!!
        /// </summary>
        public MenuModes MenuMode
        {
            set
            {
                ToolStripMenuItem fileMenu = this.menuStrip1.Items[0] as ToolStripMenuItem;
                var loginItem = fileMenu.DropDownItems[0];
                var logoutItem = fileMenu.DropDownItems[1];
                var configureItem = fileMenu.DropDownItems[3];

                ToolStripMenuItem maintenanceMenu = this.menuStrip1.Items[1] as ToolStripMenuItem;
                ToolStripMenuItem actionMenu = this.menuStrip1.Items[2] as ToolStripMenuItem;

                switch (value)
                {
                    case MenuModes.Unconfigured:
                        {
                            loginItem.Enabled = false;
                            logoutItem.Enabled = false;
                            configureItem.Enabled = true;
                            maintenanceMenu.Enabled = false;
                            actionMenu.Enabled = false;
                            break;
                        }
                    case MenuModes.ReadyForLogin:
                        {
                            loginItem.Enabled = true;
                            logoutItem.Enabled = false;
                            configureItem.Enabled = true;
                            maintenanceMenu.Enabled = false;
                            actionMenu.Enabled = false;
                            break;
                        }
                    case MenuModes.LoggedIn:
                        {
                            loginItem.Enabled = false;
                            logoutItem.Enabled = true;
                            configureItem.Enabled = false;
                            maintenanceMenu.Enabled = true;
                            actionMenu.Enabled = true;

                            var modMatsItem = maintenanceMenu.DropDownItems[0];
                            var modLocationsItem = maintenanceMenu.DropDownItems[1];
                            var modUsersMenu = maintenanceMenu.DropDownItems[2];

                            modMatsItem.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageMaterials);
                            modLocationsItem.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageLocations);
                            modUsersMenu.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageUsers);

                            var adjustInventoryItem = actionMenu.DropDownItems[0];
                            var createPickOrderItem = actionMenu.DropDownItems[1];
                            var pickOrderItem = actionMenu.DropDownItems[2];

                            createPickOrderItem.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanCreateOrder);
                            pickOrderItem.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanPickOrders);
                            adjustInventoryItem.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageInventory);
                           
                            break;
                        }
                }
            }
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new LoginWindow())
                dialog.ShowDialog(this);

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentUser != null)
            {
                if(MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    MsgDispatch.PostMessage(new UserLoggedOutEvent(CurrentUser));
            }
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var configForm = new ConfigWindow.DatabaseSourceWindow())
                configForm.ShowDialog(this);
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var userListDialog = new UserListForm(CurrentUser))
                userListDialog.ShowDialog(this);
        }

        private void vendorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var vendorListDialog = new VendorListForm(CurrentUser))
                vendorListDialog.ShowDialog(this);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void actionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void materialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var matsForms = new MaterialsListForm(Proj);
            matsForms.Show();
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        InventoryAdjustmentForm InvAdjustForm;
        /// <summary>
        /// Open window for inventory adjustments here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InvAdjustForm == null)
            {
                InvAdjustForm = new InventoryAdjustmentForm(CurrentUser, Proj);
                InvAdjustForm.Show();
                InvAdjustForm.FormClosing += HandleInvAdjustClosed;
            }
            else
            {
                InvAdjustForm.Focus();
            }
        }

        void HandleInvAdjustClosed(Object sender, EventArgs args)
        {
            InvAdjustForm.FormClosing -= HandleInvAdjustClosed;
            InvAdjustForm = null;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /// <summary>
        /// Append locations from a spreadsheet to the given warehouse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var importDialog = new ImportLocationDataForm(CurrentUser, this.Proj, this.CurrentWarehouse))
                importDialog.ShowDialog(this);
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.errorProvider1.Clear();
            if(!Warehouse.DatabaseExists)
            {
                Dialog.Message("There are currently no warehouse databases to read. You can create one using the File->Database Source menu item.");
                return;
            }

            //TODO: This needs to be asyncronous!!
            await UpdateGridTask();
        }

        void UpdateGridSync()
        {
            this.errorProvider1.Clear();
            if (!Warehouse.DatabaseExists)
            {
                Dialog.Message("There are currently no warehouse databases to read. You can create one using the File->Database Source menu item.");
                return;
            }

            UpdateWarehouseChoiceList();
            UpdateModelFromDatabase(SelectedWarehouse);
            PushModelToView(CurrentWarehouse, Proj);
        }

        private async Task UpdateGridTask() => await Task.Factory.StartNew(() =>
                                                          {
                                                              this.filterableDataGridView1.Invoke(new Action(() =>
                                                              {
                                                                  UpdateWarehouseChoiceList();
                                                                  UpdateModelFromDatabase(SelectedWarehouse);
                                                                  PushModelToView(CurrentWarehouse, Proj);
                                                              }));
                                                          });

        public void UpdateModelFromDatabase(string warehouseId)
        {
            CurrentWarehouse = Warehouse.RequestWarehouseData(warehouseId);
        }

        public void PushModelToView(Warehouse wh, Project proj)
        {
            List<List<string>> data = new List<List<string>>();
            foreach (var locData in wh.Storage)
            {
                var invalidIdList = new List<string>(2);
                foreach (var item in locData.Items)
                {
                    var mat = proj.FindMaterial(item.Key);
                    if (mat == null)
                        invalidIdList.Add(item.Key);
                    else
                    {
                        data.Add(new List<string>(){locData.LocationId, mat.DocumentId, mat.MatId, mat.Description });
                    }
                }

                if (invalidIdList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder(100);
                    foreach (var id in invalidIdList)
                        sb.AppendLine(id);
                    Dialog.Message($"There were a number of materials that could not be properly indentified in this location. They are as follows.\n\n{sb.ToString()}");

                }

            }

            //1) update gridview with all model data
            //2) apply filters to model data to only display what's needed
            filterableDataGridView1.PushModelToView(data, true);
        }

        private void filterableDataGridView1_Load(object sender, EventArgs e)
        {

        }

        private void itemLocationsLookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var itemLocLookup = new ItemLookupForm(this.Proj, this.CurrentWarehouse))
                itemLocLookup.ShowDialog(this);
        }
    }
}
