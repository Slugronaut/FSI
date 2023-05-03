namespace SAOT
{
    partial class LocationContentsForm
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
            this.labelLocationId = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemPrintLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.filterableDataGridView1 = new SAOT.FilterableDataGridView();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SAP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Kelox = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboStorageType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelLocationId
            // 
            this.labelLocationId.AutoSize = true;
            this.labelLocationId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLocationId.Location = new System.Drawing.Point(13, 13);
            this.labelLocationId.Name = "labelLocationId";
            this.labelLocationId.Size = new System.Drawing.Size(51, 20);
            this.labelLocationId.TabIndex = 1;
            this.labelLocationId.Text = "label1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPrintLabel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 26);
            // 
            // menuItemPrintLabel
            // 
            this.menuItemPrintLabel.Name = "menuItemPrintLabel";
            this.menuItemPrintLabel.Size = new System.Drawing.Size(130, 22);
            this.menuItemPrintLabel.Text = "Print Label";
            this.menuItemPrintLabel.Click += new System.EventHandler(this.printLableToolStripMenuItem_Click);
            // 
            // filterableDataGridView1
            // 
            this.filterableDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterableDataGridView1.Location = new System.Drawing.Point(12, 67);
            this.filterableDataGridView1.Name = "filterableDataGridView1";
            this.filterableDataGridView1.Size = new System.Drawing.Size(776, 371);
            this.filterableDataGridView1.TabIndex = 2;
            // 
            // Description
            // 
            this.Description.FillWeight = 210F;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.FillWeight = 50F;
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            // 
            // SAP
            // 
            this.SAP.FillWeight = 88.21272F;
            this.SAP.HeaderText = "SAP";
            this.SAP.Name = "SAP";
            this.SAP.ReadOnly = true;
            // 
            // Kelox
            // 
            this.Kelox.FillWeight = 88.21272F;
            this.Kelox.HeaderText = "Kelox";
            this.Kelox.Name = "Kelox";
            this.Kelox.ReadOnly = true;
            // 
            // Delete
            // 
            this.Delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Delete.FillWeight = 25F;
            this.Delete.Frozen = true;
            this.Delete.HeaderText = "";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Width = 25;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Delete,
            this.Kelox,
            this.SAP,
            this.Qty,
            this.Description});
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(109, 168);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(776, 126);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Visible = false;
            // 
            // comboStorageType
            // 
            this.comboStorageType.FormattingEnabled = true;
            this.comboStorageType.Location = new System.Drawing.Point(590, 15);
            this.comboStorageType.Name = "comboStorageType";
            this.comboStorageType.Size = new System.Drawing.Size(198, 21);
            this.comboStorageType.TabIndex = 3;
            this.comboStorageType.SelectedIndexChanged += new System.EventHandler(this.comboStorageType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(513, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Storage Type";
            // 
            // LocationContentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboStorageType);
            this.Controls.Add(this.filterableDataGridView1);
            this.Controls.Add(this.labelLocationId);
            this.Controls.Add(this.dataGridView1);
            this.Name = "LocationContentsForm";
            this.Text = "Location Contents";
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelLocationId;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemPrintLabel;
        private FilterableDataGridView filterableDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn SAP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Kelox;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboStorageType;
        private System.Windows.Forms.Label label1;
    }
}