using Ookii.Dialogs;
using SAOT.Forms;
using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SAOT
{
    public partial class ImportLocationDataForm : Form, IProjectProvider
    {
        public const int LOC_AISLE_COLUMNS = 2;
        public const int LOC_RACK_COLUMNS = 3;
        public const int LOC_LEVEL_COLUMNS = 1;
        public const int LOC_BIN_COLUMNS = 3;

        //public const int LOCATION_COLUMN_COUNT = 3;
        User CurrentUser;
        Warehouse Wh;
        Project Proj;

        string SelectedImportWarehouse => this.comboImportLocsTarget.Text;
        string SelectedLocCreationWarehouse => this.comboCreateLocs.Text;

        public Project CurrentSelectedProject { get => Proj; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        public ImportLocationDataForm(User currentUser, Project proj, Warehouse wh)
        {
            CurrentUser = currentUser;
            Wh = wh;
            Proj = proj;
            var names = Warehouse.RequestWarehouseNames();

            InitializeComponent();

            if(names == null || names.Count < 1)
            {
                MessageBox.Show("There are no warehouse databases available. You can create a warehouse database and a default warehouse from the main menu under \"File -> Database Source\" and pressing the \"Create Warehouse Database\" button.");
                this.Close();
                return;
            }

            foreach (var name in names)
            {
                this.comboCreateLocs.Items.Add(name);
                this.comboImportLocsTarget.Items.Add(name);
            }

            this.comboCreateLocs.SelectedIndex = 0;
            this.comboImportLocsTarget.SelectedIndex = 0;

            textAisleStart.Validating += OnValidateRangeStart;
            textRackStart.Validating += OnValidateRangeStart;
            textLevelStart.Validating += OnValidateRangeStart;
            textBinStart.Validating += OnValidateRangeStart;

            textAisleEnd.Validating += OnValidateRangeEnd;
            textRackEnd.Validating += OnValidateRangeEnd;
            textLevelEnd.Validating += OnValidateRangeEnd;
            textBinEnd.Validating += OnValidateRangeEnd;

            this.Shown += OnFormShown;
            this.FormClosing += OnFormClosing;

            //fill font list
            InstalledFontCollection coll = new InstalledFontCollection();
            this.comboPrintFont.Items.AddRange(coll.Families.Select(x => x.Name).ToArray());

            var printFont = SAOT.Config.ReadConfigStr("LocationPrintFontName");
            if(string.IsNullOrEmpty(printFont))
            {
                printFont = "Consolas";
                Config.WriteConfigStr("LocationPrintFontName", printFont);
                Config.SaveConfig();
            }
            this.comboPrintFont.SelectedItem = printFont;
            
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            //this.Opacity = 0.6;
            //locationsPanel.Visible = false;
            locationsPanel.SuspendDrawing();
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            locationsPanel.ResumeDrawing();
            //locationsPanel.Visible = true;
            //this.Opacity = 1.0;
        }

        void OnFormClosing(Object sender, EventArgs args)
        {
            StorageSpace.ListItemPool.RelenquishAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnFormShown(Object sender, EventArgs args)
        {
            var chosenWarehouse = SelectedLocCreationWarehouse;
            using (var progress = new ProgressDialog())
            {
                DoWorkEventHandler workHandler = (object s, DoWorkEventArgs workArgs) =>
                {
                    ListViewUpdatePending = true;
                    Thread.Sleep(100);
                    this.Invoke(new Action(() =>
                    {
                        UpdateWarehouseLocView(chosenWarehouse);
                        ListViewUpdatePending = false;
                    }));
                    while (ListViewUpdatePending)
                        Thread.Sleep(1);
                };

                RunWorkerCompletedEventHandler workCompleteHandler = (object s, RunWorkerCompletedEventArgs workArgs) =>
                {
                };

                progress.WindowTitle = "Downloading Locations...";
                progress.Text = "Downloading locations...";
                progress.DoWork += workHandler;
                progress.RunWorkerCompleted += workCompleteHandler;
                progress.ShowCancelButton = false;
                //progress.Show();
                Owner.Invoke(new Action(() => progress.ShowDialog(this))); //this ensures the dialog is parented to this form
            }

            this.Shown -= OnFormShown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Control FindControlRecursive(Control control, string id)
        {
            foreach (Control ctl in control.Controls)
            {
                if (ctl.Name == id)
                    return ctl;

                Control child = FindControlRecursive(ctl, id);
                if (child != null)
                    return child;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnValidateRangeStart(Object sender, CancelEventArgs args)
        {
            TextBox textBox = sender as TextBox;

            var parent = textBox.Parent;
            string panelId = parent.Name.Substring("panel".Length);
            string id = string.Format("radio{0}Letters", panelId);
            var found = FindControlRecursive(parent, id);
            var lettersRadio = found as RadioButton;


            if (lettersRadio.Checked)
            {
                if (!textBox.Text.All(x => char.IsLetter(x)))
                {
                    args.Cancel = true;
                    this.errorProvider1.SetError(textBox, "Location information must be in the form of a letter.");
                }
                else errorProvider1.Clear();
            }
            else
            {
                if (!textBox.Text.All(x => char.IsNumber(x)))
                {
                    args.Cancel = true;
                    this.errorProvider1.SetError(textBox, "Location information must be in the form of a number.");
                }
                else errorProvider1.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnValidateRangeEnd(Object sender, CancelEventArgs args)
        {
            TextBox textBox = sender as TextBox;

            var parent = textBox.Parent;
            string panelId = parent.Name.Substring("panel".Length);
            string id = string.Format("radio{0}Letters", panelId);
            var found = FindControlRecursive(parent, id);
            var lettersRadio = found as RadioButton;


            if (lettersRadio.Checked)
            {
                if (!textBox.Text.All(x => char.IsLetter(x)))
                {
                    args.Cancel = true;
                    this.errorProvider1.SetError(textBox, "Location information must be in the form of a letter.");
                }
                else errorProvider1.Clear();
            }
            else
            {
                if (!textBox.Text.All(x => char.IsNumber(x)))
                {
                    args.Cancel = true;
                    this.errorProvider1.SetError(textBox, "Location information must be in the form of a number.");
                }
                else errorProvider1.Clear();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImportLocs_Click(object sender, EventArgs args)
        {
            Import(true, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImportContents_Click(object sender, EventArgs e)
        {
            Import(false, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyMatQty"></param>
        /// <param name="vaerifyMats"></param>
        void Import(bool createLocations, bool applyMatQty, bool vaerifyMats)
        {
            if (this.radioImportLocsReplace.Checked)
            {
                if (Dialog.ConfirmAction($"You are about to import a large amount of information that may potentially modify the contents of many storage locations in the warehouse {SelectedImportWarehouse}. Are you sure?", this)
                    == Dialog.yesButton)
                {
                    using (var dialog = new VistaOpenFileDialog())
                    {
                        dialog.InitialDirectory = Config.ReadConfigStr(Config.LastDirectory);
                        dialog.Multiselect = false;
                        var result = dialog.ShowDialog(this);

                        if (result == DialogResult.OK || result == DialogResult.Yes)
                        {
                            try
                            {
                                var locs = Warehouse.ParseStorageSpreadsheet(dialog.FileName, false, false);
                                if (locs == null || locs.Count < 1)
                                {
                                    MessageBox.Show("There was no location data contained within the file \"" + dialog.FileName + "\". Be sure that it is formatted with a header titled \"Location\".");
                                    return;
                                }
                                else
                                {
                                    foreach (var importLoc in locs)
                                    {
                                        if(importLoc == null)
                                        {
                                            if (Dialog.ConfirmAction($"One of the imported locations was invalid. Do you want to continue further imports?") == DialogResult.Yes)
                                                continue;
                                            else
                                            {
                                                MsgDispatch.PostMessage(new LocationContentsUpdated());
                                                return;
                                            }
                                        }
                                        var whLoc = Wh.FindStorageLocation(importLoc.LocationId);
                                        if(whLoc == null)
                                        {
                                            if(Dialog.ConfirmAction($"There was no location '{importLoc.LocationId}' in the warehouse '{SelectedImportWarehouse}'. Do you want to continue further imports?") == DialogResult.Yes)
                                                continue;
                                            else
                                            {
                                                MsgDispatch.PostMessage(new LocationContentsUpdated());
                                                return;
                                            }
                                        }
                                        whLoc.CopyContents(importLoc, false);
                                        foreach (var matId in importLoc.Items.Keys)
                                        {
                                            var mat = Proj.FindMaterial(matId);
                                            if (mat != null)
                                            {
                                                Warehouse.SetMaterialInLocation(CurrentUser, Wh.WarehouseId, importLoc.LocationId, mat.MatId, 1);
                                            }
                                        }
                                        //MessageBox.Show(this, "Not yet implemented.");
                                        //MessageBox.Show("Imported " + locs.Count + " locations into the Warehouse \"" + SelectedImportWarehouse + "\".");
                                        //TODO: actually import the data into the model and then push to the database
                                        //Warehouse.CreateStorageLocations(CurrentUser, SelectedImportWarehouse, locs);
                                    }
                                    MsgDispatch.PostMessage(new LocationContentsUpdated());
                                }
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("An error occurred while attempting to parse your location data spreadsheet.\n\n" + err.Message);
                                return;
                            }
                        }

                        var fileName = dialog.FileName;
                        var lastIndex = fileName.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                        var path = fileName.Substring(0, lastIndex + 1);
                        Config.ChangeConfigStr(Config.LastDirectory, path);
                        Config.SaveConfig();
                    }
                }
                else return;

            }
            else MessageBox.Show("This feature is not yet supported.");
        }


       

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textRackStart_TextChanged(object sender, EventArgs e)
        {

        }

        private void textAisleEnd_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonDeleteAllLocations(object sender, EventArgs e)
        {
            if(Dialog.ConfirmAction("This will delete all locations and their contents from the system.\n\nIf you would like to make a backup of your information first, the source database file can be found in the directory "+Config.DirectoryOfDb("Warehouse.db")+"\n\nAre you sure you want to continue with this process?")
                 == Dialog.yesButton)
            {
                locationsPanel.Controls.Clear(true);
                Warehouse.DeleteAllStorageLocations(CurrentUser, SelectedLocCreationWarehouse);
                MessageBox.Show("All location data and their contents have been removed from the warehouse \"" + SelectedLocCreationWarehouse+"\".");
            }
        }

        private void textAisleStart_TextChanged(object sender, EventArgs e)
        {

        }

        bool ListViewUpdatePending = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateLocationSeq_Click(object sender, EventArgs e)
        {
            var chosenWarehouse = SelectedLocCreationWarehouse;
            List<Aisle> locs = null;
            using (var progress = new ProgressDialog())
            {
                DoWorkEventHandler workHandler = (object s, DoWorkEventArgs workArgs) =>
                {
                    ListViewUpdatePending = true;
                    Thread.Sleep(500);
                    locs = StorageSpace.GenerateLocationSequence(
                        LOC_AISLE_COLUMNS,
                        LOC_RACK_COLUMNS,
                        LOC_LEVEL_COLUMNS,
                        LOC_BIN_COLUMNS,
                        textAisleStart.Text, textAisleEnd.Text, radioAisleLetters.Checked,
                        textRackStart.Text, textRackEnd.Text, radioRackLetters.Checked,
                        textLevelStart.Text, textLevelEnd.Text, radioLevelLetters.Checked,
                        textBinStart.Text, textBinEnd.Text, radioBinLetters.Checked);
                    Thread.Sleep(1);
                    //TODO: update the warehouse DB here!!
                    Warehouse.CreateStorageLocations(CurrentUser, chosenWarehouse, locs);

                    Thread.Sleep(1);
                    this.Invoke(new Action(() =>
                    {
                        UpdateWarehouseLocView(chosenWarehouse);
                        ListViewUpdatePending = false;
                    }));
                    while (ListViewUpdatePending)
                        Thread.Sleep(1);
                };

                //RunWorkerCompletedEventHandler workCompleteHandler = (object s, RunWorkerCompletedEventArgs workArgs) =>
                //{
                //};

                progress.WindowTitle = "Creating Locations...";
                progress.Text = "Creating locations...";
                progress.DoWork += workHandler;
                //progress.RunWorkerCompleted += workCompleteHandler;
                progress.ShowCancelButton = false;
                this.Invoke(new Action(() => progress.ShowDialog(this))); //this ensures the dialog is parented to this form
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouseId"></param>
        private void UpdateWarehouseLocView(string warehouseId)
        {
            var warehouse = Warehouse.RequestWarehouseData(SelectedLocCreationWarehouse);
            var whLocs = warehouse.AisleFormatStorage(
                LOC_AISLE_COLUMNS,
                LOC_RACK_COLUMNS,
                LOC_LEVEL_COLUMNS,
                LOC_BIN_COLUMNS);
            ButtonGrid.PropogateAisleView(warehouse, locationsPanel, whLocs, null, true, false, OnLocationClicked);
            //StorageSpace.PropogateListView(whLocs, listView1, null);
        }

        void OnLocationClicked(Button button, ButtonGrid buttonGrid)
        {
            var form = new ChangeLocNameForm(this.CurrentUser as User, this.Wh, this.Proj, button.AccessibleName);
            form.ShowDialog(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printLocs_Click(object sender, EventArgs e)
        {
            var chosenWarehouse = SelectedLocCreationWarehouse;
            List<Aisle> locs = StorageSpace.GenerateLocationSequence(
                         LOC_AISLE_COLUMNS,
                         LOC_RACK_COLUMNS,
                         LOC_LEVEL_COLUMNS,
                         LOC_BIN_COLUMNS,
                         textAisleStart.Text, textAisleEnd.Text, radioAisleLetters.Checked,
                         textRackStart.Text, textRackEnd.Text, radioRackLetters.Checked,
                         textLevelStart.Text, textLevelEnd.Text, radioLevelLetters.Checked,
                         textBinStart.Text, textBinEnd.Text, radioBinLetters.Checked);


            using (var progress = new ProgressDialog())
            {
                WindowUtils.PrintLocationSequence(this, locs, comboPrintFont.Text, (float)numericFontSize.Value, (float)numericTopMargin.Value, (float)numericBottomMargin.Value, (float)numericLeftMargin.Value);
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboPrintFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            var printFont = this.comboPrintFont.Text;
            Config.WriteConfigStr("LocationPrintFontName", printFont);
            Config.SaveConfig();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {

        }
    }
}
