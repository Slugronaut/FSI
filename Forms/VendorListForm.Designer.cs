namespace SAOT
{
    partial class VendorListForm
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
            this.VendorDataGrid = new System.Windows.Forms.DataGridView();
            this.AddVendorButton = new System.Windows.Forms.Button();
            this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransferId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InUSA = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StreetAdress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.VendorDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // VendorDataGrid
            // 
            this.VendorDataGrid.AllowUserToAddRows = false;
            this.VendorDataGrid.AllowUserToDeleteRows = false;
            this.VendorDataGrid.AllowUserToResizeRows = false;
            this.VendorDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VendorDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VendorDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VendorDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Name,
            this.Id,
            this.TransferId,
            this.InUSA,
            this.StreetAdress,
            this.City,
            this.State,
            this.Zip});
            this.VendorDataGrid.Location = new System.Drawing.Point(12, 41);
            this.VendorDataGrid.Name = "VendorDataGrid";
            this.VendorDataGrid.RowHeadersVisible = false;
            this.VendorDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VendorDataGrid.ShowEditingIcon = false;
            this.VendorDataGrid.Size = new System.Drawing.Size(760, 308);
            this.VendorDataGrid.TabIndex = 4;
            this.VendorDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.EditVendorButton_Click);
            // 
            // AddVendorButton
            // 
            this.AddVendorButton.Location = new System.Drawing.Point(12, 12);
            this.AddVendorButton.Name = "AddVendorButton";
            this.AddVendorButton.Size = new System.Drawing.Size(75, 23);
            this.AddVendorButton.TabIndex = 3;
            this.AddVendorButton.Text = "Add";
            this.AddVendorButton.UseVisualStyleBackColor = true;
            this.AddVendorButton.Click += new System.EventHandler(this.AddVendorButton_Click);
            // 
            // Name
            // 
            this.Name.HeaderText = "Name";
            this.Name.MinimumWidth = 20;
            this.Name.Name = "Name";
            this.Name.ReadOnly = true;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 20;
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // TransferId
            // 
            this.TransferId.HeaderText = "TransferId";
            this.TransferId.Name = "TransferId";
            this.TransferId.ReadOnly = true;
            // 
            // InUSA
            // 
            this.InUSA.HeaderText = "In USA";
            this.InUSA.MinimumWidth = 20;
            this.InUSA.Name = "InUSA";
            this.InUSA.ReadOnly = true;
            this.InUSA.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InUSA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // StreetAdress
            // 
            this.StreetAdress.HeaderText = "Street Address";
            this.StreetAdress.Name = "StreetAdress";
            this.StreetAdress.ReadOnly = true;
            // 
            // City
            // 
            this.City.HeaderText = "City";
            this.City.Name = "City";
            this.City.ReadOnly = true;
            // 
            // State
            // 
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // Zip
            // 
            this.Zip.HeaderText = "Zip";
            this.Zip.Name = "Zip";
            this.Zip.ReadOnly = true;
            // 
            // VendorListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.VendorDataGrid);
            this.Controls.Add(this.AddVendorButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.Text = "Vendor List";
            ((System.ComponentModel.ISupportInitialize)(this.VendorDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView VendorDataGrid;
        private System.Windows.Forms.Button AddVendorButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransferId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn InUSA;
        private System.Windows.Forms.DataGridViewTextBoxColumn StreetAdress;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zip;
    }
}