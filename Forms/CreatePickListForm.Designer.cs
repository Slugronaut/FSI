namespace SAOT.Forms
{
    partial class CreatePickListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePickListForm));
            this.buttonCreateOrder = new System.Windows.Forms.Button();
            this.comboVendorDest = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAddPickItem = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.materialIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pickedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.removeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pickItemVMBindingSource4 = new System.Windows.Forms.BindingSource(this.components);
            this.pickItemVMBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.pickItemVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.comboVendorInventory = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOrderId = new System.Windows.Forms.TextBox();
            this.pickItemVMBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pickItemVMBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.comboBoxOrderType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.printPackingSlipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSaveOrder = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.printPackingSlipToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonAppendPickItem = new System.Windows.Forms.Button();
            this.restoreItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCreateOrder
            // 
            this.buttonCreateOrder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCreateOrder.Location = new System.Drawing.Point(328, 438);
            this.buttonCreateOrder.Name = "buttonCreateOrder";
            this.buttonCreateOrder.Size = new System.Drawing.Size(160, 23);
            this.buttonCreateOrder.TabIndex = 0;
            this.buttonCreateOrder.Text = "Create Order";
            this.buttonCreateOrder.UseVisualStyleBackColor = true;
            this.buttonCreateOrder.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // comboVendorDest
            // 
            this.comboVendorDest.FormattingEnabled = true;
            this.comboVendorDest.Location = new System.Drawing.Point(87, 12);
            this.comboVendorDest.Name = "comboVendorDest";
            this.comboVendorDest.Size = new System.Drawing.Size(146, 21);
            this.comboVendorDest.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ship Address";
            // 
            // buttonAddPickItem
            // 
            this.buttonAddPickItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddPickItem.Location = new System.Drawing.Point(735, 374);
            this.buttonAddPickItem.Name = "buttonAddPickItem";
            this.buttonAddPickItem.Size = new System.Drawing.Size(75, 23);
            this.buttonAddPickItem.TabIndex = 7;
            this.buttonAddPickItem.Text = "+";
            this.buttonAddPickItem.UseVisualStyleBackColor = true;
            this.buttonAddPickItem.Click += new System.EventHandler(this.addPickItem_Click);
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
            this.statusDataGridViewTextBoxColumn,
            this.materialIdDataGridViewTextBoxColumn,
            this.docDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.qtyDataGridViewTextBoxColumn,
            this.pickedDataGridViewTextBoxColumn,
            this.uMDataGridViewTextBoxColumn,
            this.removeDataGridViewCheckBoxColumn});
            this.dataGridView1.DataSource = this.pickItemVMBindingSource4;
            this.dataGridView1.Location = new System.Drawing.Point(12, 59);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(798, 309);
            this.dataGridView1.TabIndex = 8;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // materialIdDataGridViewTextBoxColumn
            // 
            this.materialIdDataGridViewTextBoxColumn.DataPropertyName = "MaterialId";
            this.materialIdDataGridViewTextBoxColumn.HeaderText = "MaterialId";
            this.materialIdDataGridViewTextBoxColumn.Name = "materialIdDataGridViewTextBoxColumn";
            // 
            // docDataGridViewTextBoxColumn
            // 
            this.docDataGridViewTextBoxColumn.DataPropertyName = "Doc";
            this.docDataGridViewTextBoxColumn.HeaderText = "Doc";
            this.docDataGridViewTextBoxColumn.Name = "docDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // qtyDataGridViewTextBoxColumn
            // 
            this.qtyDataGridViewTextBoxColumn.DataPropertyName = "Qty";
            this.qtyDataGridViewTextBoxColumn.HeaderText = "Qty";
            this.qtyDataGridViewTextBoxColumn.Name = "qtyDataGridViewTextBoxColumn";
            // 
            // pickedDataGridViewTextBoxColumn
            // 
            this.pickedDataGridViewTextBoxColumn.DataPropertyName = "Picked";
            this.pickedDataGridViewTextBoxColumn.HeaderText = "Picked";
            this.pickedDataGridViewTextBoxColumn.Name = "pickedDataGridViewTextBoxColumn";
            // 
            // uMDataGridViewTextBoxColumn
            // 
            this.uMDataGridViewTextBoxColumn.DataPropertyName = "UM";
            this.uMDataGridViewTextBoxColumn.HeaderText = "UM";
            this.uMDataGridViewTextBoxColumn.Name = "uMDataGridViewTextBoxColumn";
            // 
            // removeDataGridViewCheckBoxColumn
            // 
            this.removeDataGridViewCheckBoxColumn.DataPropertyName = "Remove";
            this.removeDataGridViewCheckBoxColumn.HeaderText = "Remove";
            this.removeDataGridViewCheckBoxColumn.Name = "removeDataGridViewCheckBoxColumn";
            // 
            // pickItemVMBindingSource4
            // 
            this.pickItemVMBindingSource4.DataSource = typeof(SAOT.Forms.CreatePickListForm.PickItemVM);
            // 
            // pickItemVMBindingSource3
            // 
            this.pickItemVMBindingSource3.DataSource = typeof(SAOT.Forms.CreatePickListForm.PickItemVM);
            // 
            // pickItemVMBindingSource
            // 
            this.pickItemVMBindingSource.DataSource = typeof(SAOT.Forms.CreatePickListForm.PickItemVM);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Inventory Transfer";
            // 
            // comboVendorInventory
            // 
            this.comboVendorInventory.FormattingEnabled = true;
            this.comboVendorInventory.Location = new System.Drawing.Point(342, 12);
            this.comboVendorInventory.Name = "comboVendorInventory";
            this.comboVendorInventory.Size = new System.Drawing.Size(146, 21);
            this.comboVendorInventory.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 395);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Order Id";
            // 
            // textBoxOrderId
            // 
            this.textBoxOrderId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxOrderId.Location = new System.Drawing.Point(63, 392);
            this.textBoxOrderId.Name = "textBoxOrderId";
            this.textBoxOrderId.ReadOnly = true;
            this.textBoxOrderId.Size = new System.Drawing.Size(116, 20);
            this.textBoxOrderId.TabIndex = 12;
            // 
            // pickItemVMBindingSource1
            // 
            this.pickItemVMBindingSource1.DataSource = typeof(SAOT.Forms.CreatePickListForm.PickItemVM);
            // 
            // pickItemVMBindingSource2
            // 
            this.pickItemVMBindingSource2.DataSource = typeof(SAOT.Forms.CreatePickListForm.PickItemVM);
            // 
            // comboBoxOrderType
            // 
            this.comboBoxOrderType.FormattingEnabled = true;
            this.comboBoxOrderType.Items.AddRange(new object[] {
            "Vendor",
            "Production",
            "Inventory Count",
            "Vendor NCR",
            "Customer NCR"});
            this.comboBoxOrderType.Location = new System.Drawing.Point(583, 12);
            this.comboBoxOrderType.Name = "comboBoxOrderType";
            this.comboBoxOrderType.Size = new System.Drawing.Size(129, 21);
            this.comboBoxOrderType.TabIndex = 13;
            this.comboBoxOrderType.SelectedIndexChanged += new System.EventHandler(this.comboBoxOrderType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(517, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Order Type";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printPackingSlipToolStripMenuItem,
            this.cancelItemToolStripMenuItem,
            this.restoreItemToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // printPackingSlipToolStripMenuItem
            // 
            this.printPackingSlipToolStripMenuItem.Name = "printPackingSlipToolStripMenuItem";
            this.printPackingSlipToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.printPackingSlipToolStripMenuItem.Text = "Print Packing Slip";
            this.printPackingSlipToolStripMenuItem.Click += new System.EventHandler(this.printPackingSlipToolStripMenuItem_Click);
            // 
            // cancelItemToolStripMenuItem
            // 
            this.cancelItemToolStripMenuItem.Name = "cancelItemToolStripMenuItem";
            this.cancelItemToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.cancelItemToolStripMenuItem.Text = "Cancel Item";
            this.cancelItemToolStripMenuItem.Click += new System.EventHandler(this.cancelItemToolStripMenuItem_Click);
            // 
            // buttonSaveOrder
            // 
            this.buttonSaveOrder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonSaveOrder.Location = new System.Drawing.Point(342, 426);
            this.buttonSaveOrder.Name = "buttonSaveOrder";
            this.buttonSaveOrder.Size = new System.Drawing.Size(132, 23);
            this.buttonSaveOrder.TabIndex = 15;
            this.buttonSaveOrder.Text = "Update Order";
            this.buttonSaveOrder.UseVisualStyleBackColor = true;
            this.buttonSaveOrder.Click += new System.EventHandler(this.buttonSaveOrder_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printPackingSlipToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(138, 26);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // printPackingSlipToolStripMenuItem1
            // 
            this.printPackingSlipToolStripMenuItem1.Name = "printPackingSlipToolStripMenuItem1";
            this.printPackingSlipToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.printPackingSlipToolStripMenuItem1.Text = "Cancel Item";
            this.printPackingSlipToolStripMenuItem1.Click += new System.EventHandler(this.cancelItemToolStripMenuItem1_Click);
            // 
            // buttonAppendPickItem
            // 
            this.buttonAppendPickItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAppendPickItem.Location = new System.Drawing.Point(735, 385);
            this.buttonAppendPickItem.Name = "buttonAppendPickItem";
            this.buttonAppendPickItem.Size = new System.Drawing.Size(75, 23);
            this.buttonAppendPickItem.TabIndex = 16;
            this.buttonAppendPickItem.Text = "+";
            this.buttonAppendPickItem.UseVisualStyleBackColor = true;
            this.buttonAppendPickItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // restoreItemToolStripMenuItem
            // 
            this.restoreItemToolStripMenuItem.Name = "restoreItemToolStripMenuItem";
            this.restoreItemToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.restoreItemToolStripMenuItem.Text = "Restore Item";
            this.restoreItemToolStripMenuItem.Click += new System.EventHandler(this.restoreItemToolStripMenuItem_Click);
            // 
            // CreatePickListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 473);
            this.Controls.Add(this.buttonAppendPickItem);
            this.Controls.Add(this.buttonSaveOrder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxOrderType);
            this.Controls.Add(this.textBoxOrderId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboVendorInventory);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonAddPickItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboVendorDest);
            this.Controls.Add(this.buttonCreateOrder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "CreatePickListForm";
            this.Text = "Create Pick List";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickItemVMBindingSource2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCreateOrder;
        private System.Windows.Forms.ComboBox comboVendorDest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAddPickItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboVendorInventory;
        private System.Windows.Forms.BindingSource pickItemVMBindingSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOrderId;
        private System.Windows.Forms.BindingSource pickItemVMBindingSource1;
        private System.Windows.Forms.BindingSource pickItemVMBindingSource2;
        private System.Windows.Forms.BindingSource pickItemVMBindingSource3;
        private System.Windows.Forms.ComboBox comboBoxOrderType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem printPackingSlipToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn materialIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn docDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pickedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn removeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource pickItemVMBindingSource4;
        private System.Windows.Forms.Button buttonSaveOrder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem printPackingSlipToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cancelItemToolStripMenuItem;
        private System.Windows.Forms.Button buttonAppendPickItem;
        private System.Windows.Forms.ToolStripMenuItem restoreItemToolStripMenuItem;
    }
}