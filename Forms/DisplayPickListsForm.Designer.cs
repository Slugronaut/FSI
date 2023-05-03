namespace SAOT.Forms
{
    partial class DisplayPickListsForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.orderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pickCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pickOrderVMBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.pickOrderVMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pickOrderVMBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pickOrderVMBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.printPackingSlipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsShippedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkedListBoxOrderTypes = new System.Windows.Forms.CheckedListBox();
            this.markAsAdjustedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource2)).BeginInit();
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
            this.orderTypeDataGridViewTextBoxColumn,
            this.shipToDataGridViewTextBoxColumn,
            this.createdDataGridViewTextBoxColumn,
            this.pickCountDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.pickOrderVMBindingSource3;
            this.dataGridView1.Location = new System.Drawing.Point(12, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(747, 262);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // orderIdDataGridViewTextBoxColumn
            // 
            this.orderIdDataGridViewTextBoxColumn.DataPropertyName = "OrderId";
            this.orderIdDataGridViewTextBoxColumn.HeaderText = "OrderId";
            this.orderIdDataGridViewTextBoxColumn.Name = "orderIdDataGridViewTextBoxColumn";
            // 
            // orderTypeDataGridViewTextBoxColumn
            // 
            this.orderTypeDataGridViewTextBoxColumn.DataPropertyName = "OrderType";
            this.orderTypeDataGridViewTextBoxColumn.HeaderText = "OrderType";
            this.orderTypeDataGridViewTextBoxColumn.Name = "orderTypeDataGridViewTextBoxColumn";
            // 
            // shipToDataGridViewTextBoxColumn
            // 
            this.shipToDataGridViewTextBoxColumn.DataPropertyName = "ShipTo";
            this.shipToDataGridViewTextBoxColumn.HeaderText = "ShipTo";
            this.shipToDataGridViewTextBoxColumn.Name = "shipToDataGridViewTextBoxColumn";
            // 
            // createdDataGridViewTextBoxColumn
            // 
            this.createdDataGridViewTextBoxColumn.DataPropertyName = "Created";
            this.createdDataGridViewTextBoxColumn.HeaderText = "Created";
            this.createdDataGridViewTextBoxColumn.Name = "createdDataGridViewTextBoxColumn";
            // 
            // pickCountDataGridViewTextBoxColumn
            // 
            this.pickCountDataGridViewTextBoxColumn.DataPropertyName = "PickCount";
            this.pickCountDataGridViewTextBoxColumn.HeaderText = "PickCount";
            this.pickCountDataGridViewTextBoxColumn.Name = "pickCountDataGridViewTextBoxColumn";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // pickOrderVMBindingSource3
            // 
            this.pickOrderVMBindingSource3.DataSource = typeof(SAOT.Forms.DisplayPickListsForm.PickOrderVM);
            // 
            // pickOrderVMBindingSource
            // 
            this.pickOrderVMBindingSource.DataSource = typeof(SAOT.Forms.DisplayPickListsForm.PickOrderVM);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available Picklists";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(485, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Order Type Filter";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // pickOrderVMBindingSource1
            // 
            this.pickOrderVMBindingSource1.DataSource = typeof(SAOT.Forms.PickOrdersForm.PickOrderVM);
            // 
            // pickOrderVMBindingSource2
            // 
            this.pickOrderVMBindingSource2.DataSource = typeof(SAOT.Forms.DisplayPickListsForm.PickOrderVM);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printPackingSlipToolStripMenuItem,
            this.markAsShippedToolStripMenuItem,
            this.markAsAdjustedToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // printPackingSlipToolStripMenuItem
            // 
            this.printPackingSlipToolStripMenuItem.Name = "printPackingSlipToolStripMenuItem";
            this.printPackingSlipToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.printPackingSlipToolStripMenuItem.Text = "Print Packing Slip";
            this.printPackingSlipToolStripMenuItem.Click += new System.EventHandler(this.printPackingSlipToolStripMenuItem_Click);
            // 
            // markAsShippedToolStripMenuItem
            // 
            this.markAsShippedToolStripMenuItem.Name = "markAsShippedToolStripMenuItem";
            this.markAsShippedToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.markAsShippedToolStripMenuItem.Text = "Mark As Shipped";
            this.markAsShippedToolStripMenuItem.Click += new System.EventHandler(this.markAsShippedToolStripMenuItem_Click);
            // 
            // checkedListBoxOrderTypes
            // 
            this.checkedListBoxOrderTypes.CheckOnClick = true;
            this.checkedListBoxOrderTypes.FormattingEnabled = true;
            this.checkedListBoxOrderTypes.Items.AddRange(new object[] {
            "Pending",
            "Picking/Counting",
            "Picked/Counted",
            "Shipped",
            "Adjusted",
            "Closed",
            "Canceled"});
            this.checkedListBoxOrderTypes.Location = new System.Drawing.Point(576, 12);
            this.checkedListBoxOrderTypes.Name = "checkedListBoxOrderTypes";
            this.checkedListBoxOrderTypes.Size = new System.Drawing.Size(183, 79);
            this.checkedListBoxOrderTypes.TabIndex = 5;
            this.checkedListBoxOrderTypes.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxOrderTypes_SelectedIndexChanged);
            // 
            // markAsAdjustedToolStripMenuItem
            // 
            this.markAsAdjustedToolStripMenuItem.Name = "markAsAdjustedToolStripMenuItem";
            this.markAsAdjustedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.markAsAdjustedToolStripMenuItem.Text = "Mark As Adjusted";
            this.markAsAdjustedToolStripMenuItem.Click += new System.EventHandler(this.markAsAdjustedToolStripMenuItem_Click);
            // 
            // DisplayPickListsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 382);
            this.Controls.Add(this.checkedListBoxOrderTypes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "DisplayPickListsForm";
            this.Text = "Display Orders";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickOrderVMBindingSource2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource pickOrderVMBindingSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource pickOrderVMBindingSource1;
        private System.Windows.Forms.BindingSource pickOrderVMBindingSource2;
        private System.Windows.Forms.BindingSource pickOrderVMBindingSource3;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pickCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem printPackingSlipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAsShippedToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox checkedListBoxOrderTypes;
        private System.Windows.Forms.ToolStripMenuItem markAsAdjustedToolStripMenuItem;
    }
}