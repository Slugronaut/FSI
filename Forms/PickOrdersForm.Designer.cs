namespace SAOT.Forms
{
    partial class PickOrdersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickOrdersForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.orderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateCreatedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateModifiedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipToIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inventoryAdjustIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.adjustedInSAPDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.carrierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carrierTrackingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pickOrderVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonAddOrder = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.printPackingSlipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDuplicateOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderIdDataGridViewTextBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.dateCreatedDataGridViewTextBoxColumn,
            this.dateModifiedDataGridViewTextBoxColumn,
            this.shipToIdDataGridViewTextBoxColumn,
            this.inventoryAdjustIdDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.adjustedInSAPDataGridViewCheckBoxColumn,
            this.carrierDataGridViewTextBoxColumn,
            this.carrierTrackingDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.pickOrderVMBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 43);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(808, 311);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // orderIdDataGridViewTextBoxColumn
            // 
            this.orderIdDataGridViewTextBoxColumn.DataPropertyName = "OrderId";
            this.orderIdDataGridViewTextBoxColumn.HeaderText = "OrderId";
            this.orderIdDataGridViewTextBoxColumn.Name = "orderIdDataGridViewTextBoxColumn";
            this.orderIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dateCreatedDataGridViewTextBoxColumn
            // 
            this.dateCreatedDataGridViewTextBoxColumn.DataPropertyName = "DateCreated";
            this.dateCreatedDataGridViewTextBoxColumn.HeaderText = "DateCreated";
            this.dateCreatedDataGridViewTextBoxColumn.Name = "dateCreatedDataGridViewTextBoxColumn";
            this.dateCreatedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dateModifiedDataGridViewTextBoxColumn
            // 
            this.dateModifiedDataGridViewTextBoxColumn.DataPropertyName = "DateModified";
            this.dateModifiedDataGridViewTextBoxColumn.HeaderText = "DateModified";
            this.dateModifiedDataGridViewTextBoxColumn.Name = "dateModifiedDataGridViewTextBoxColumn";
            this.dateModifiedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // shipToIdDataGridViewTextBoxColumn
            // 
            this.shipToIdDataGridViewTextBoxColumn.DataPropertyName = "ShipToId";
            this.shipToIdDataGridViewTextBoxColumn.HeaderText = "ShipToId";
            this.shipToIdDataGridViewTextBoxColumn.Name = "shipToIdDataGridViewTextBoxColumn";
            this.shipToIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // inventoryAdjustIdDataGridViewTextBoxColumn
            // 
            this.inventoryAdjustIdDataGridViewTextBoxColumn.DataPropertyName = "InventoryAdjustId";
            this.inventoryAdjustIdDataGridViewTextBoxColumn.HeaderText = "InventoryAdjustId";
            this.inventoryAdjustIdDataGridViewTextBoxColumn.Name = "inventoryAdjustIdDataGridViewTextBoxColumn";
            this.inventoryAdjustIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.inventoryAdjustIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // adjustedInSAPDataGridViewCheckBoxColumn
            // 
            this.adjustedInSAPDataGridViewCheckBoxColumn.DataPropertyName = "AdjustedInSAP";
            this.adjustedInSAPDataGridViewCheckBoxColumn.HeaderText = "AdjustedInSAP";
            this.adjustedInSAPDataGridViewCheckBoxColumn.Name = "adjustedInSAPDataGridViewCheckBoxColumn";
            this.adjustedInSAPDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // carrierDataGridViewTextBoxColumn
            // 
            this.carrierDataGridViewTextBoxColumn.DataPropertyName = "Carrier";
            this.carrierDataGridViewTextBoxColumn.HeaderText = "Carrier";
            this.carrierDataGridViewTextBoxColumn.Name = "carrierDataGridViewTextBoxColumn";
            this.carrierDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // carrierTrackingDataGridViewTextBoxColumn
            // 
            this.carrierTrackingDataGridViewTextBoxColumn.DataPropertyName = "CarrierTracking";
            this.carrierTrackingDataGridViewTextBoxColumn.HeaderText = "CarrierTracking";
            this.carrierTrackingDataGridViewTextBoxColumn.Name = "carrierTrackingDataGridViewTextBoxColumn";
            this.carrierTrackingDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pickOrderVMBindingSource
            // 
            this.pickOrderVMBindingSource.DataSource = typeof(SAOT.Forms.PickOrdersForm.PickOrderVM);
            // 
            // buttonAddOrder
            // 
            this.buttonAddOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddOrder.Location = new System.Drawing.Point(745, 360);
            this.buttonAddOrder.Name = "buttonAddOrder";
            this.buttonAddOrder.Size = new System.Drawing.Size(75, 23);
            this.buttonAddOrder.TabIndex = 10;
            this.buttonAddOrder.Text = "+";
            this.buttonAddOrder.UseVisualStyleBackColor = true;
            this.buttonAddOrder.Click += new System.EventHandler(this.buttonAddOrder_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printPackingSlipToolStripMenuItem,
            this.createDuplicateOrderToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(195, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // printPackingSlipToolStripMenuItem
            // 
            this.printPackingSlipToolStripMenuItem.Name = "printPackingSlipToolStripMenuItem";
            this.printPackingSlipToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.printPackingSlipToolStripMenuItem.Text = "Print Packing Slip";
            this.printPackingSlipToolStripMenuItem.Click += new System.EventHandler(this.printPackingSlipToolStripMenuItem_Click);
            // 
            // createDuplicateOrderToolStripMenuItem
            // 
            this.createDuplicateOrderToolStripMenuItem.Name = "createDuplicateOrderToolStripMenuItem";
            this.createDuplicateOrderToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.createDuplicateOrderToolStripMenuItem.Text = "Create Duplicate Order";
            this.createDuplicateOrderToolStripMenuItem.Click += new System.EventHandler(this.createDuplicateOrderToolStripMenuItem_Click);
            // 
            // PickOrdersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 395);
            this.Controls.Add(this.buttonAddOrder);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PickOrdersForm";
            this.Text = "Pick Orders Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource pickOrderVMBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateCreatedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateModifiedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipToIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn inventoryAdjustIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn adjustedInSAPDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carrierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carrierTrackingDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button buttonAddOrder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem printPackingSlipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDuplicateOrderToolStripMenuItem;
    }
}