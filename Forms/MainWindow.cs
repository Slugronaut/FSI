using SAOT.Forms;
using SAOT.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Form, IProjectProvider
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

        public Project CurrentSelectedProject { get => Proj; }

        void HandleDoubleClick(Object sender, DataGridViewCellMouseEventArgs args)
        {
            var grid = filterableDataGridView1.GridView;
            int col = args.ColumnIndex;

            WindowUtils.HandleDrawingLookup(sender, args);
            WindowUtils.HandleOpenLocationView(sender, args, CurrentUser, CurrentWarehouse, Proj);
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
            Carrier.ConfirmDatabaseExists();
        }


        #region MVC
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
            GC.Collect();
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
                            maintenanceMenu.Enabled = true;
                            actionMenu.Enabled = false;

                            var modMatsItem = maintenanceMenu.DropDownItems[0];
                            var modLocationsItem = maintenanceMenu.DropDownItems[1];
                            var modUsersMenu = maintenanceMenu.DropDownItems[2];
                            var modVendorsMenu = maintenanceMenu.DropDownItems[3];
                            var modWarehousesMenu = maintenanceMenu.DropDownItems[4];

                            modMatsItem.Enabled = true;
                            modLocationsItem.Enabled = false;
                            modUsersMenu.Enabled = false;
                            modVendorsMenu.Enabled = false;
                            modWarehousesMenu.Enabled = false;

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
                            var modVendorsMenu = maintenanceMenu.DropDownItems[3];
                            var modWarehousesMenu = maintenanceMenu.DropDownItems[4];

                            modMatsItem.Enabled = true;// (CurrentUser.IsAdmin || CurrentUser.CanManageMaterials);
                            modLocationsItem.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageLocations);
                            modUsersMenu.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageUsers);
                            modVendorsMenu.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageLocations);
                            modWarehousesMenu.Enabled = (CurrentUser.IsAdmin || CurrentUser.CanManageLocations);

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
        /*
        private async Task UpdateGridTask() => await Task.Factory.StartNew(() =>
        {
            this.filterableDataGridView1.Invoke(new Action(() =>
            {
                UpdateWarehouseChoiceList();
                UpdateModelFromDatabase(SelectedWarehouse);
                PushModelToView(CurrentWarehouse, Proj);
                MsgDispatch.PostMessage(new InventoryUpdated());
            }));
        });
        */
        private void UpdateGridTask()
        {
            UpdateWarehouseChoiceList();
            UpdateModelFromDatabase(SelectedWarehouse);
            PushModelToView(CurrentWarehouse, Proj);
            MsgDispatch.PostMessage(new InventoryUpdated());
        }

        public void UpdateModelFromDatabase(string warehouseId)
        {
            CurrentWarehouse = Warehouse.RequestWarehouseData(warehouseId);
            MsgDispatch.PostMessage(new WarehouseModelUpdated());
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
                        data.Add(new List<string>() { locData.LocationId, mat.DocumentId, mat.MatId, mat.Description });
                    }
                }

                if (invalidIdList.Count > 0)
                {
                        StringBuilder sb = new StringBuilder(100);
                        for(int i = 0; i < System.Math.Min(invalidIdList.Count, 10); i++)
                        sb.AppendLine(invalidIdList[i]);

                    if (invalidIdList.Count <= 10)
                        Dialog.Message($"There were a number of materials that could not be properly indentified in this location. They are as follows.\n\n{sb.ToString()}");
                    else Dialog.Message($"There were more than ten materials that could not be properly indentified in this location. The first ten are as follows.\n\n{sb.ToString()}");
                }

            }

            //1) update gridview with all model data
            //2) apply filters to model data to only display what's needed
            filterableDataGridView1.PushModelToView(data, true);

            //update cell colors based on storage types
            var grid = filterableDataGridView1.GridView;
            for(int i = 0; i < grid.Rows.Count; i++)
            {
                var locCell = grid.Rows[i].Cells[0];

                var text = locCell.Value as string;
                var loc = wh.FindStorageLocation(text);
                locCell.Style.BackColor = StorageSpace.TypeToColor(loc.Type);
                
            }


        }
#endregion


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
            using (var dialog = new SelectLabelPrinterForm())
                dialog.ShowDialog(this);
        }

        private void actionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void materialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var matsForms = new MaterialsListForm(ref CurrentUser, Proj);
            matsForms.Show();
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //we keep a global reference to this form because we only want one instance of it open at any time
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
            //Task updateTask;
            //updateTask = UpdateGridTask();
            //updateTask.Res
            //await updateTask;
            //MsgDispatch.PostMessage(new InventoryUpdated());
            UpdateGridTask();
            MsgDispatch.PostMessage(new InventoryUpdated());
        }

        private void filterableDataGridView1_Load(object sender, EventArgs e)
        {

        }

        private void batchLocationLookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var itemLocLookup = new ItemLookupForm(this.Proj, this.CurrentWarehouse);
            itemLocLookup.Show();
        }

        private void exportUniqueLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonRefresh_Click(null, null);
            ExportUniqueLocationsToText(CurrentWarehouse);
        }

        void ExportUniqueLocationsToText(Warehouse wh)
        {
            //we are taking the lazy-ass approach here.
            //the last location assigned for a material is where it shall be.
            var uniques = new Dictionary<string, string>(1000);
            foreach(var loc in wh.Storage)
            {
                foreach (var matId in loc.Items.Keys)
                {
                    //look for wierd made-up-ass numbers
                    if(int.Parse(matId) > 40000000 && int.Parse(matId) < 70000000)
                        uniques[matId] = loc.LocationId;
                }
            }

            ExportUniqueLocationSpreadsheet(uniques);

        }

        void ExportUniqueLocationSpreadsheet(Dictionary<string, string> matLocs)
        {
            string templatePath = Path.Combine(Config.AppPath, "TemplateExportSpreadsheet.xlsx");
            string filePath = Path.Combine(Config.AppPath, "export.xlsx");
            using (var dialog = new Ookii.Dialogs.VistaSaveFileDialog())
            {
                dialog.Title = "Material File Export";
                dialog.DefaultExt = "xlsx";
                dialog.AddExtension = true;// ("xlsx");
                var result = dialog.ShowDialog(this);
                if (result != DialogResult.OK)
                    return;

                filePath = dialog.FileName;
            }

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (FastExcel.FastExcel spread = new FastExcel.FastExcel(new System.IO.FileInfo(templatePath), new System.IO.FileInfo(filePath)))
            {
                var worksheet = new FastExcel.Worksheet();
                var rows = new List<FastExcel.Row>(matLocs.Count);

                //first row is headers
                var cells = new List<FastExcel.Cell>(2);
                cells.Add(new FastExcel.Cell(1, "Material"));
                cells.Add(new FastExcel.Cell(2, "Location"));
                rows.Add(new FastExcel.Row(1, cells));

                
                //each row after is data
                int rowNum = 2;
                foreach(var kv in matLocs)
                {
                    cells = new List<FastExcel.Cell>(2);
                    cells.Add(new FastExcel.Cell(1, string.IsNullOrEmpty(kv.Key)?"NO ID":kv.Key));
                    cells.Add(new FastExcel.Cell(2, string.IsNullOrEmpty(kv.Value)?"NO LOCATION":kv.Value));
                    rows.Add(new FastExcel.Row(rowNum, cells));
                    rowNum++;
                }
                

                worksheet.Rows = rows;
                spread.Write(worksheet, "sheet1");
            }
        }

        private void createOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var viewPickOrdersForm = new PickOrdersForm(CurrentUser, CurrentWarehouse, Proj);
            viewPickOrdersForm.Show();
        }

        private void pickOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var displayPicklistForm = new DisplayPickListsForm(this.CurrentUser, this.CurrentWarehouse, this.Proj);
            displayPicklistForm.Show();
        }

        private void exportAllLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonRefresh_Click(null, null);
            ExportAllLocationsToText(CurrentWarehouse);
        }

        void ExportAllLocationsToText(Warehouse wh)
        {
            //we are taking the lazy-ass approach here.
            //the last location assigned for a material is where it shall be.
            var uniques = new Dictionary<string, string>(1000);
            List<string> mats = new List<string>(1000);
            List<string> locs = new List<string>(1000);
            foreach (var loc in wh.Storage)
            {
                foreach (var matId in loc.Items.Keys)
                {
                    //look for wierd made-up-ass numbers
                    if (int.Parse(matId) > 40000000 && int.Parse(matId) < 70000000)
                    {
                        mats.Add(matId);
                        locs.Add(loc.LocationId);
                       // uniques[matId] = loc.LocationId;
                    }
                }
            }

            ExportAllLocationSpreadsheet(mats, locs);

        }

        void ExportAllLocationSpreadsheet(List<string> mats, List<string> locs)
        {
            string templatePath = Path.Combine(Config.AppPath, "TemplateExportSpreadsheet.xlsx");
            string filePath = Path.Combine(Config.AppPath, "export.xlsx");
            using (var dialog = new Ookii.Dialogs.VistaSaveFileDialog())
            {
                dialog.Title = "Material File Export";
                dialog.DefaultExt = "xlsx";
                dialog.AddExtension = true;// ("xlsx");
                var result = dialog.ShowDialog(this);
                if (result != DialogResult.OK)
                    return;

                filePath = dialog.FileName;
            }

            if (mats.Count != locs.Count)
                throw new Exception("Material count does not equal location count.");

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (FastExcel.FastExcel spread = new FastExcel.FastExcel(new System.IO.FileInfo(templatePath), new System.IO.FileInfo(filePath)))
            {
                var worksheet = new FastExcel.Worksheet();
                var rows = new List<FastExcel.Row>(mats.Count);

                //first row is headers
                var cells = new List<FastExcel.Cell>(2);
                cells.Add(new FastExcel.Cell(1, "Material"));
                cells.Add(new FastExcel.Cell(2, "Location"));
                rows.Add(new FastExcel.Row(1, cells));


                //each row after is data
                int rowNum = 2;
                for(int x = 0; x < mats.Count; x++)
                {
                    var kv = new KeyValuePair<string, string>(mats[x], locs[x]);
                    cells = new List<FastExcel.Cell>(2);
                    cells.Add(new FastExcel.Cell(1, string.IsNullOrEmpty(kv.Key) ? "NO ID" : kv.Key));
                    cells.Add(new FastExcel.Cell(2, string.IsNullOrEmpty(kv.Value) ? "NO LOCATION" : kv.Value));
                    rows.Add(new FastExcel.Row(rowNum, cells));
                    rowNum++;
                }


                worksheet.Rows = rows;
                spread.Write(worksheet, "sheet1");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wh"></param>
        void ExportEverythingEver(Warehouse wh)
        {
            //we need to get every location in the world and export it along with it's item data
            //List<string> allLocs = new List<string>(1000);
            Dictionary<string, List<string>> locToItems = new Dictionary<string, List<string>>(1000);

            foreach (var loc in wh.Storage)
            {
                var items = new List<string>(10);
                
                foreach (var matId in loc.Items.Keys)
                {
                    //look for wierd made-up-ass numbers
                    if (int.Parse(matId) > 40000000 && int.Parse(matId) < 70000000)
                    {
                        items.Add(matId);
                    }
                }
                locToItems.Add(loc.LocationId, items);
            }


            ExportEverythingToSpreadsheet(locToItems);
            //ExportAllLocationSpreadsheet(mats, locs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locs"></param>
        void ExportEverythingToSpreadsheet(Dictionary<string, List<string>> locToItems)
        {
            string templatePath = Path.Combine(Config.AppPath, "TemplateExportSpreadsheet.xlsx");
            string filePath = Path.Combine(Config.AppPath, "export.xlsx");
            using (var dialog = new Ookii.Dialogs.VistaSaveFileDialog())
            {
                dialog.Title = "Material File Export";
                dialog.DefaultExt = "xlsx";
                dialog.AddExtension = true;// ("xlsx");
                var result = dialog.ShowDialog(this);
                if (result != DialogResult.OK)
                    return;

                filePath = dialog.FileName;
            }


            if (File.Exists(filePath))
                File.Delete(filePath);

            using (FastExcel.FastExcel spread = new FastExcel.FastExcel(new System.IO.FileInfo(templatePath), new System.IO.FileInfo(filePath)))
            {
                var worksheet = new FastExcel.Worksheet();
                var rows = new List<FastExcel.Row>(locToItems.Count);

                //first row is headers
                var cells = new List<FastExcel.Cell>(2);
                cells.Add(new FastExcel.Cell(1, "Location"));
                cells.Add(new FastExcel.Cell(2, "Material"));
                rows.Add(new FastExcel.Row(1, cells));


                //each row after is data
                int rowNum = 2;
                foreach(string loc in locToItems.Keys)
                {
                    if (locToItems[loc].Count < 1)
                    {
                        cells = new List<FastExcel.Cell>(2);
                        cells.Add(new FastExcel.Cell(1, loc));
                        rows.Add(new FastExcel.Row(rowNum, cells));
                        rowNum++;
                    }
                    else
                    {
                        foreach (var item in locToItems[loc])
                        {
                            cells = new List<FastExcel.Cell>(2);
                            cells.Add(new FastExcel.Cell(1, loc));
                            cells.Add(new FastExcel.Cell(2, item));
                            rows.Add(new FastExcel.Row(rowNum, cells));
                            rowNum++;
                        }
                    }
                }


                worksheet.Rows = rows;
                spread.Write(worksheet, "sheet1");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportLocationsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonRefresh_Click(null, null);
            ExportLocationsOnlyToText(CurrentWarehouse);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportEverythingEverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonRefresh_Click(null, null);
            ExportEverythingEver(CurrentWarehouse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wh"></param>
        void ExportLocationsOnlyToText(Warehouse wh)
        {
            //we are taking the lazy-ass approach here.
            //the last location assigned for a material is where it shall be.
            List<string> locs = new List<string>(1000);
            foreach (var loc in wh.Storage)
            {
                locs.Add(loc.LocationId);
                foreach (var matId in loc.Items.Keys)
                {
                    //look for wierd made-up-ass numbers
                    if (int.Parse(matId) > 40000000 && int.Parse(matId) < 70000000)
                    {
                        locs.Add(matId);
                        // uniques[matId] = loc.LocationId;
                    }
                }
            }

            ExportLocationOnlySpreadsheet(wh.Storage.Select(x => x.LocationId).ToList());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mats"></param>
        /// <param name="locs"></param>
        void ExportLocationOnlySpreadsheet(List<string> locs)
        {
            string templatePath = Path.Combine(Config.AppPath, "TemplateExportSpreadsheet.xlsx");
            string filePath = Path.Combine(Config.AppPath, "export.xlsx");
            using (var dialog = new Ookii.Dialogs.VistaSaveFileDialog())
            {
                dialog.Title = "Material File Export";
                dialog.DefaultExt = "xlsx";
                dialog.AddExtension = true;// ("xlsx");
                var result = dialog.ShowDialog(this);
                if (result != DialogResult.OK)
                    return;

                filePath = dialog.FileName;
            }

            
            if (File.Exists(filePath))
                File.Delete(filePath);

            using (FastExcel.FastExcel spread = new FastExcel.FastExcel(new System.IO.FileInfo(templatePath), new System.IO.FileInfo(filePath)))
            {
                var worksheet = new FastExcel.Worksheet();
                var rows = new List<FastExcel.Row>(locs.Count);

                //first row is headers
                var cells = new List<FastExcel.Cell>(2);
                cells.Add(new FastExcel.Cell(1, "Locations"));
                rows.Add(new FastExcel.Row(1, cells));


                //each row after is data
                int rowNum = 2;
                for (int x = 0; x < locs.Count; x++)
                {
                    cells = new List<FastExcel.Cell>(2);
                    cells.Add(new FastExcel.Cell(1, string.IsNullOrEmpty(locs[x]) ? "NO ID" : locs[x]));
                    rows.Add(new FastExcel.Row(rowNum, cells));
                    rowNum++;
                }


                worksheet.Rows = rows;
                spread.Write(worksheet, "sheet1");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportMultiLocationMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string templatePath = Path.Combine(Config.AppPath, "TemplateExportSpreadsheet.xlsx");
            string filePath = Path.Combine(Config.AppPath, "export.xlsx");
            using (var dialog = new Ookii.Dialogs.VistaSaveFileDialog())
            {
                dialog.Title = "Material File Export";
                dialog.DefaultExt = "xlsx";
                dialog.AddExtension = true;// ("xlsx");
                var result = dialog.ShowDialog(this);
                if (result != DialogResult.OK)
                    return;

                filePath = dialog.FileName;
            }

            if (File.Exists(filePath))
                File.Delete(filePath);

            //go through every material and see how many locations it has
            Dictionary<Material, List<StorageSpace>> matLocs = new Dictionary<Material, List<StorageSpace>>(100);
            foreach (var mat in this.CurrentSelectedProject.MaterialsList)
            {
                //not at all efficient but who actually cares?
                var locs = this.CurrentWarehouse.FindAllMaterialLocations(mat.MatId);
                if (locs?.Count > 1)
                    matLocs[mat] = locs;
            }


            using (FastExcel.FastExcel spread = new FastExcel.FastExcel(new System.IO.FileInfo(templatePath), new System.IO.FileInfo(filePath)))
            {
                var worksheet = new FastExcel.Worksheet();
                var rows = new List<FastExcel.Row>(matLocs.Count);

                //first row is headers
                var cells = new List<FastExcel.Cell>(4);
                cells.Add(new FastExcel.Cell(1, "Material"));
                cells.Add(new FastExcel.Cell(2, "Doc"));
                cells.Add(new FastExcel.Cell(3, "Desc"));
                cells.Add(new FastExcel.Cell(4, "Locations"));
                rows.Add(new FastExcel.Row(1, cells));


                //each row after is data
                int rowNum = 2;
                foreach(var kvp in matLocs)
                {
                    int x = 0;
                    foreach(var loc in kvp.Value)
                    {
                        Material mat = kvp.Key;
                        cells = new List<FastExcel.Cell>(4);
                        cells.Add(new FastExcel.Cell(1, x == 0 ? mat.MatId : string.Empty));
                        cells.Add(new FastExcel.Cell(2, x == 0 ? mat.DocumentId : string.Empty));
                        cells.Add(new FastExcel.Cell(3, x == 0 ? mat.Description : string.Empty));
                        cells.Add(new FastExcel.Cell(4, loc.LocationId));
                        rows.Add(new FastExcel.Row(rowNum, cells));
                        x++;
                        rowNum++;
                    }
                    
                }


                worksheet.Rows = rows;
                spread.Write(worksheet, "sheet1");
            }
        }
    }
}
