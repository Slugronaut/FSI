namespace SAOT.Forms
{
    partial class PickForm
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
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("$loc1");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxOrderId = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxShippingAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxAdjustTo = new System.Windows.Forms.TextBox();
            this.textBoxShipTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonConfirmCount = new System.Windows.Forms.Button();
            this.textBoxUoM = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.textBoxRequestQty = new System.Windows.Forms.TextBox();
            this.buttonPick = new System.Windows.Forms.Button();
            this.textBoxDoc = new System.Windows.Forms.TextBox();
            this.textBoxMatId = new System.Windows.Forms.TextBox();
            this.numericPickedQty = new System.Windows.Forms.NumericUpDown();
            this.labelPickedQty = new System.Windows.Forms.Label();
            this.labelRequestedQty = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listViewLocs = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.listViewPickItems = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.checkPrintTicket = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPickedQty)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkPrintTicket);
            this.groupBox1.Controls.Add(this.textBoxOrderId);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBoxShippingAddress);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBoxAdjustTo);
            this.groupBox1.Controls.Add(this.textBoxShipTo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(842, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pick Order Info";
            // 
            // textBoxOrderId
            // 
            this.textBoxOrderId.Location = new System.Drawing.Point(59, 19);
            this.textBoxOrderId.Name = "textBoxOrderId";
            this.textBoxOrderId.ReadOnly = true;
            this.textBoxOrderId.Size = new System.Drawing.Size(132, 20);
            this.textBoxOrderId.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Order ID:";
            // 
            // textBoxShippingAddress
            // 
            this.textBoxShippingAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxShippingAddress.Location = new System.Drawing.Point(539, 19);
            this.textBoxShippingAddress.Multiline = true;
            this.textBoxShippingAddress.Name = "textBoxShippingAddress";
            this.textBoxShippingAddress.ReadOnly = true;
            this.textBoxShippingAddress.Size = new System.Drawing.Size(315, 59);
            this.textBoxShippingAddress.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(441, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Shipping Address:";
            // 
            // textBoxAdjustTo
            // 
            this.textBoxAdjustTo.Location = new System.Drawing.Point(258, 43);
            this.textBoxAdjustTo.Name = "textBoxAdjustTo";
            this.textBoxAdjustTo.ReadOnly = true;
            this.textBoxAdjustTo.Size = new System.Drawing.Size(132, 20);
            this.textBoxAdjustTo.TabIndex = 3;
            // 
            // textBoxShipTo
            // 
            this.textBoxShipTo.Location = new System.Drawing.Point(258, 19);
            this.textBoxShipTo.Name = "textBoxShipTo";
            this.textBoxShipTo.ReadOnly = true;
            this.textBoxShipTo.Size = new System.Drawing.Size(132, 20);
            this.textBoxShipTo.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Adjust To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(205, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ship To:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Controls.Add(this.buttonConfirmCount);
            this.groupBox2.Controls.Add(this.textBoxUoM);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textBoxDesc);
            this.groupBox2.Controls.Add(this.textBoxRequestQty);
            this.groupBox2.Controls.Add(this.buttonPick);
            this.groupBox2.Controls.Add(this.textBoxDoc);
            this.groupBox2.Controls.Add(this.textBoxMatId);
            this.groupBox2.Controls.Add(this.numericPickedQty);
            this.groupBox2.Controls.Add(this.labelPickedQty);
            this.groupBox2.Controls.Add(this.labelRequestedQty);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.listViewLocs);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(220, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(634, 368);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current Pick Item";
            // 
            // buttonConfirmCount
            // 
            this.buttonConfirmCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonConfirmCount.Enabled = false;
            this.buttonConfirmCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConfirmCount.Location = new System.Drawing.Point(269, 311);
            this.buttonConfirmCount.Name = "buttonConfirmCount";
            this.buttonConfirmCount.Size = new System.Drawing.Size(210, 51);
            this.buttonConfirmCount.TabIndex = 16;
            this.buttonConfirmCount.Text = "Confirm Count";
            this.buttonConfirmCount.UseVisualStyleBackColor = true;
            this.buttonConfirmCount.Visible = false;
            this.buttonConfirmCount.Click += new System.EventHandler(this.buttonConfirmCount_Click);
            // 
            // textBoxUoM
            // 
            this.textBoxUoM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUoM.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUoM.Location = new System.Drawing.Point(490, 227);
            this.textBoxUoM.MaxLength = 8;
            this.textBoxUoM.Name = "textBoxUoM";
            this.textBoxUoM.ReadOnly = true;
            this.textBoxUoM.Size = new System.Drawing.Size(130, 35);
            this.textBoxUoM.TabIndex = 15;
            this.textBoxUoM.Text = "$UoM";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(425, 234);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 24);
            this.label11.TabIndex = 14;
            this.label11.Text = "UoM";
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDesc.Location = new System.Drawing.Point(73, 70);
            this.textBoxDesc.MaxLength = 8;
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.ReadOnly = true;
            this.textBoxDesc.Size = new System.Drawing.Size(573, 29);
            this.textBoxDesc.TabIndex = 13;
            this.textBoxDesc.Text = "$desc";
            // 
            // textBoxRequestQty
            // 
            this.textBoxRequestQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRequestQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRequestQty.Location = new System.Drawing.Point(490, 125);
            this.textBoxRequestQty.MaxLength = 8;
            this.textBoxRequestQty.Name = "textBoxRequestQty";
            this.textBoxRequestQty.ReadOnly = true;
            this.textBoxRequestQty.Size = new System.Drawing.Size(130, 35);
            this.textBoxRequestQty.TabIndex = 12;
            this.textBoxRequestQty.Text = "$qtyToPick";
            // 
            // buttonPick
            // 
            this.buttonPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPick.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPick.Location = new System.Drawing.Point(258, 311);
            this.buttonPick.Name = "buttonPick";
            this.buttonPick.Size = new System.Drawing.Size(210, 51);
            this.buttonPick.TabIndex = 11;
            this.buttonPick.Text = "Pick";
            this.buttonPick.UseVisualStyleBackColor = true;
            this.buttonPick.Click += new System.EventHandler(this.buttonPick_Click);
            // 
            // textBoxDoc
            // 
            this.textBoxDoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDoc.Location = new System.Drawing.Point(289, 21);
            this.textBoxDoc.MaxLength = 8;
            this.textBoxDoc.Name = "textBoxDoc";
            this.textBoxDoc.ReadOnly = true;
            this.textBoxDoc.Size = new System.Drawing.Size(357, 35);
            this.textBoxDoc.TabIndex = 10;
            this.textBoxDoc.Text = "$doc";
            this.textBoxDoc.TextChanged += new System.EventHandler(this.textBoxDoc_TextChanged);
            // 
            // textBoxMatId
            // 
            this.textBoxMatId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMatId.Location = new System.Drawing.Point(73, 21);
            this.textBoxMatId.MaxLength = 8;
            this.textBoxMatId.Name = "textBoxMatId";
            this.textBoxMatId.ReadOnly = true;
            this.textBoxMatId.Size = new System.Drawing.Size(174, 35);
            this.textBoxMatId.TabIndex = 9;
            this.textBoxMatId.Text = "$matId";
            // 
            // numericPickedQty
            // 
            this.numericPickedQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericPickedQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericPickedQty.Location = new System.Drawing.Point(490, 176);
            this.numericPickedQty.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericPickedQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericPickedQty.Name = "numericPickedQty";
            this.numericPickedQty.Size = new System.Drawing.Size(130, 35);
            this.numericPickedQty.TabIndex = 8;
            // 
            // labelPickedQty
            // 
            this.labelPickedQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPickedQty.AutoSize = true;
            this.labelPickedQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPickedQty.Location = new System.Drawing.Point(370, 182);
            this.labelPickedQty.Name = "labelPickedQty";
            this.labelPickedQty.Size = new System.Drawing.Size(105, 24);
            this.labelPickedQty.TabIndex = 6;
            this.labelPickedQty.Text = "Picked Qty:";
            // 
            // labelRequestedQty
            // 
            this.labelRequestedQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRequestedQty.AutoSize = true;
            this.labelRequestedQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRequestedQty.Location = new System.Drawing.Point(357, 132);
            this.labelRequestedQty.Name = "labelRequestedQty";
            this.labelRequestedQty.Size = new System.Drawing.Size(118, 24);
            this.labelRequestedQty.TabIndex = 5;
            this.labelRequestedQty.Text = "Request Qty:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Locations:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Desc:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(253, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Doc:";
            // 
            // listViewLocs
            // 
            this.listViewLocs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewLocs.Enabled = false;
            this.listViewLocs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewLocs.HideSelection = false;
            this.listViewLocs.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.listViewLocs.Location = new System.Drawing.Point(9, 125);
            this.listViewLocs.Name = "listViewLocs";
            this.listViewLocs.Size = new System.Drawing.Size(208, 237);
            this.listViewLocs.TabIndex = 1;
            this.listViewLocs.UseCompatibleStateImageBehavior = false;
            this.listViewLocs.View = System.Windows.Forms.View.List;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Material ID:";
            // 
            // listViewPickItems
            // 
            this.listViewPickItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewPickItems.AutoArrange = false;
            this.listViewPickItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewPickItems.HideSelection = false;
            this.listViewPickItems.LabelWrap = false;
            this.listViewPickItems.Location = new System.Drawing.Point(12, 102);
            this.listViewPickItems.MultiSelect = false;
            this.listViewPickItems.Name = "listViewPickItems";
            this.listViewPickItems.ShowGroups = false;
            this.listViewPickItems.ShowItemToolTips = true;
            this.listViewPickItems.Size = new System.Drawing.Size(191, 368);
            this.listViewPickItems.TabIndex = 2;
            this.listViewPickItems.UseCompatibleStateImageBehavior = false;
            this.listViewPickItems.View = System.Windows.Forms.View.List;
            this.listViewPickItems.SelectedIndexChanged += new System.EventHandler(this.listViewPickItems_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "delete.png");
            this.imageList1.Images.SetKeyName(1, "checkgreen.png");
            // 
            // checkPrintTicket
            // 
            this.checkPrintTicket.AutoSize = true;
            this.checkPrintTicket.Checked = true;
            this.checkPrintTicket.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPrintTicket.Location = new System.Drawing.Point(9, 61);
            this.checkPrintTicket.Name = "checkPrintTicket";
            this.checkPrintTicket.Size = new System.Drawing.Size(80, 17);
            this.checkPrintTicket.TabIndex = 8;
            this.checkPrintTicket.Text = "Print Ticket";
            this.checkPrintTicket.UseVisualStyleBackColor = true;
            // 
            // PickForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 482);
            this.Controls.Add(this.listViewPickItems);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(770, 430);
            this.Name = "PickForm";
            this.Text = "Pick Order";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPickedQty)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxDoc;
        private System.Windows.Forms.TextBox textBoxMatId;
        private System.Windows.Forms.NumericUpDown numericPickedQty;
        private System.Windows.Forms.Label labelPickedQty;
        private System.Windows.Forms.Label labelRequestedQty;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listViewLocs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRequestQty;
        private System.Windows.Forms.Button buttonPick;
        private System.Windows.Forms.TextBox textBoxShippingAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxAdjustTo;
        private System.Windows.Forms.TextBox textBoxShipTo;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.TextBox textBoxOrderId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListView listViewPickItems;
        private System.Windows.Forms.TextBox textBoxUoM;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button buttonConfirmCount;
        private System.Windows.Forms.CheckBox checkPrintTicket;
    }
}