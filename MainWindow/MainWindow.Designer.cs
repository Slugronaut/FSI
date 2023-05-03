namespace SAOT
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.maintenanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vendorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warehousesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pickOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateQRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.filterableDataGridView1 = new SAOT.FilterableDataGridView();
            this.comboWarehouseChoice = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.easyAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemLocationsLookupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionsToolStripMenuItem,
            this.maintenanceToolStripMenuItem,
            this.actionsToolStripMenuItem1,
            this.easyAccessToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(775, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.toolStripSeparator1,
            this.configureToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.actionsToolStripMenuItem.Text = "File";
            this.actionsToolStripMenuItem.Click += new System.EventHandler(this.actionsToolStripMenuItem_Click);
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Enabled = false;
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Enabled = false;
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.configureToolStripMenuItem.Text = "Database Source";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(158, 6);
            // 
            // maintenanceToolStripMenuItem
            // 
            this.maintenanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.materialsToolStripMenuItem,
            this.locationsToolStripMenuItem,
            this.usersToolStripMenuItem,
            this.vendorsToolStripMenuItem,
            this.warehousesToolStripMenuItem});
            this.maintenanceToolStripMenuItem.Name = "maintenanceToolStripMenuItem";
            this.maintenanceToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.maintenanceToolStripMenuItem.Text = "Maintenance";
            // 
            // materialsToolStripMenuItem
            // 
            this.materialsToolStripMenuItem.Enabled = false;
            this.materialsToolStripMenuItem.Name = "materialsToolStripMenuItem";
            this.materialsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.materialsToolStripMenuItem.Text = "Materials";
            this.materialsToolStripMenuItem.Click += new System.EventHandler(this.materialsToolStripMenuItem_Click);
            // 
            // locationsToolStripMenuItem
            // 
            this.locationsToolStripMenuItem.Enabled = false;
            this.locationsToolStripMenuItem.Name = "locationsToolStripMenuItem";
            this.locationsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.locationsToolStripMenuItem.Text = "Locations";
            this.locationsToolStripMenuItem.Click += new System.EventHandler(this.locationsToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Enabled = false;
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // vendorsToolStripMenuItem
            // 
            this.vendorsToolStripMenuItem.Name = "vendorsToolStripMenuItem";
            this.vendorsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.vendorsToolStripMenuItem.Text = "Vendors";
            this.vendorsToolStripMenuItem.Click += new System.EventHandler(this.vendorsToolStripMenuItem_Click);
            // 
            // warehousesToolStripMenuItem
            // 
            this.warehousesToolStripMenuItem.Name = "warehousesToolStripMenuItem";
            this.warehousesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.warehousesToolStripMenuItem.Text = "Warehouses";
            // 
            // actionsToolStripMenuItem1
            // 
            this.actionsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stockToolStripMenuItem,
            this.createOrderToolStripMenuItem,
            this.pickOrderToolStripMenuItem,
            this.generateQRToolStripMenuItem});
            this.actionsToolStripMenuItem1.Name = "actionsToolStripMenuItem1";
            this.actionsToolStripMenuItem1.Size = new System.Drawing.Size(59, 20);
            this.actionsToolStripMenuItem1.Text = "Actions";
            // 
            // stockToolStripMenuItem
            // 
            this.stockToolStripMenuItem.Name = "stockToolStripMenuItem";
            this.stockToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.stockToolStripMenuItem.Text = "Adjust Inventory";
            this.stockToolStripMenuItem.Click += new System.EventHandler(this.stockToolStripMenuItem_Click);
            // 
            // createOrderToolStripMenuItem
            // 
            this.createOrderToolStripMenuItem.Name = "createOrderToolStripMenuItem";
            this.createOrderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createOrderToolStripMenuItem.Text = "Create Order";
            // 
            // pickOrderToolStripMenuItem
            // 
            this.pickOrderToolStripMenuItem.Name = "pickOrderToolStripMenuItem";
            this.pickOrderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pickOrderToolStripMenuItem.Text = "Pick Order";
            // 
            // generateQRToolStripMenuItem
            // 
            this.generateQRToolStripMenuItem.Name = "generateQRToolStripMenuItem";
            this.generateQRToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.generateQRToolStripMenuItem.Text = "Generate QR";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.BackgroundImage = global::SAOT.Properties.Resources.refresh;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefresh.Location = new System.Drawing.Point(717, 27);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(32, 32);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // filterableDataGridView1
            // 
            this.filterableDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterableDataGridView1.Location = new System.Drawing.Point(12, 65);
            this.filterableDataGridView1.Name = "filterableDataGridView1";
            this.filterableDataGridView1.Size = new System.Drawing.Size(751, 375);
            this.filterableDataGridView1.TabIndex = 3;
            this.filterableDataGridView1.Load += new System.EventHandler(this.filterableDataGridView1_Load);
            // 
            // comboWarehouseChoice
            // 
            this.comboWarehouseChoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboWarehouseChoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWarehouseChoice.FormattingEnabled = true;
            this.comboWarehouseChoice.Location = new System.Drawing.Point(525, 34);
            this.comboWarehouseChoice.Name = "comboWarehouseChoice";
            this.comboWarehouseChoice.Size = new System.Drawing.Size(186, 21);
            this.comboWarehouseChoice.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(423, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Target Warehouse";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // easyAccessToolStripMenuItem
            // 
            this.easyAccessToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemLocationsLookupToolStripMenuItem});
            this.easyAccessToolStripMenuItem.Name = "easyAccessToolStripMenuItem";
            this.easyAccessToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.easyAccessToolStripMenuItem.Text = "Easy Access";
            // 
            // itemLocationsLookupToolStripMenuItem
            // 
            this.itemLocationsLookupToolStripMenuItem.Name = "itemLocationsLookupToolStripMenuItem";
            this.itemLocationsLookupToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.itemLocationsLookupToolStripMenuItem.Text = "Item Locations Lookup";
            this.itemLocationsLookupToolStripMenuItem.Click += new System.EventHandler(this.itemLocationsLookupToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 452);
            this.Controls.Add(this.comboWarehouseChoice);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.filterableDataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "FSI 2.Oh";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem maintenanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem materialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vendorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem createOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pickOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockToolStripMenuItem;
        private FilterableDataGridView filterableDataGridView1;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ToolStripMenuItem warehousesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateQRToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboWarehouseChoice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolStripMenuItem easyAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemLocationsLookupToolStripMenuItem;
    }
}

