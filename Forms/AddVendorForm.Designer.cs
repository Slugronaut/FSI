namespace SAOT
{
    partial class AddVendorForm
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
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.IdTextbox = new System.Windows.Forms.TextBox();
            this.StreetTextbox = new System.Windows.Forms.TextBox();
            this.CityTextbox = new System.Windows.Forms.TextBox();
            this.StateTextbox = new System.Windows.Forms.TextBox();
            this.ZipTextbox = new System.Windows.Forms.TextBox();
            this.InUSCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.vendorGroup = new System.Windows.Forms.GroupBox();
            this.TransferIdTextbox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.vendorGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameTextbox
            // 
            this.NameTextbox.Location = new System.Drawing.Point(100, 25);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.Size = new System.Drawing.Size(143, 20);
            this.NameTextbox.TabIndex = 0;
            // 
            // IdTextbox
            // 
            this.IdTextbox.Location = new System.Drawing.Point(306, 25);
            this.IdTextbox.Name = "IdTextbox";
            this.IdTextbox.Size = new System.Drawing.Size(143, 20);
            this.IdTextbox.TabIndex = 1;
            // 
            // StreetTextbox
            // 
            this.StreetTextbox.Location = new System.Drawing.Point(100, 78);
            this.StreetTextbox.Name = "StreetTextbox";
            this.StreetTextbox.Size = new System.Drawing.Size(143, 20);
            this.StreetTextbox.TabIndex = 2;
            // 
            // CityTextbox
            // 
            this.CityTextbox.Location = new System.Drawing.Point(100, 104);
            this.CityTextbox.Name = "CityTextbox";
            this.CityTextbox.Size = new System.Drawing.Size(143, 20);
            this.CityTextbox.TabIndex = 3;
            // 
            // StateTextbox
            // 
            this.StateTextbox.Location = new System.Drawing.Point(100, 130);
            this.StateTextbox.Name = "StateTextbox";
            this.StateTextbox.Size = new System.Drawing.Size(143, 20);
            this.StateTextbox.TabIndex = 4;
            // 
            // ZipTextbox
            // 
            this.ZipTextbox.Location = new System.Drawing.Point(306, 130);
            this.ZipTextbox.Name = "ZipTextbox";
            this.ZipTextbox.Size = new System.Drawing.Size(143, 20);
            this.ZipTextbox.TabIndex = 5;
            // 
            // InUSCheckbox
            // 
            this.InUSCheckbox.AutoSize = true;
            this.InUSCheckbox.Enabled = false;
            this.InUSCheckbox.Location = new System.Drawing.Point(100, 156);
            this.InUSCheckbox.Name = "InUSCheckbox";
            this.InUSCheckbox.Size = new System.Drawing.Size(59, 17);
            this.InUSCheckbox.TabIndex = 6;
            this.InUSCheckbox.Text = "In U.S.";
            this.InUSCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Id No.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Street Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "City";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "State";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(278, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Zip";
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(213, 221);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 13;
            this.OkButton.Text = "Make it So";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // vendorGroup
            // 
            this.vendorGroup.Controls.Add(this.TransferIdTextbox);
            this.vendorGroup.Controls.Add(this.label7);
            this.vendorGroup.Controls.Add(this.NameTextbox);
            this.vendorGroup.Controls.Add(this.IdTextbox);
            this.vendorGroup.Controls.Add(this.label6);
            this.vendorGroup.Controls.Add(this.StreetTextbox);
            this.vendorGroup.Controls.Add(this.label5);
            this.vendorGroup.Controls.Add(this.CityTextbox);
            this.vendorGroup.Controls.Add(this.label4);
            this.vendorGroup.Controls.Add(this.StateTextbox);
            this.vendorGroup.Controls.Add(this.label3);
            this.vendorGroup.Controls.Add(this.ZipTextbox);
            this.vendorGroup.Controls.Add(this.label2);
            this.vendorGroup.Controls.Add(this.InUSCheckbox);
            this.vendorGroup.Controls.Add(this.label1);
            this.vendorGroup.Location = new System.Drawing.Point(12, 12);
            this.vendorGroup.Name = "vendorGroup";
            this.vendorGroup.Size = new System.Drawing.Size(478, 190);
            this.vendorGroup.TabIndex = 14;
            this.vendorGroup.TabStop = false;
            this.vendorGroup.Text = "Vendor Info";
            // 
            // TransferIdTextbox
            // 
            this.TransferIdTextbox.Location = new System.Drawing.Point(352, 51);
            this.TransferIdTextbox.Name = "TransferIdTextbox";
            this.TransferIdTextbox.Size = new System.Drawing.Size(97, 20);
            this.TransferIdTextbox.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(264, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Transfer Id No.";
            // 
            // AddVendorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 262);
            this.Controls.Add(this.vendorGroup);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddVendorForm";
            this.ShowIcon = false;
            this.Text = "Add Vendor";
            this.vendorGroup.ResumeLayout(false);
            this.vendorGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox NameTextbox;
        private System.Windows.Forms.TextBox IdTextbox;
        private System.Windows.Forms.TextBox StreetTextbox;
        private System.Windows.Forms.TextBox CityTextbox;
        private System.Windows.Forms.TextBox StateTextbox;
        private System.Windows.Forms.TextBox ZipTextbox;
        private System.Windows.Forms.CheckBox InUSCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.GroupBox vendorGroup;
        private System.Windows.Forms.TextBox TransferIdTextbox;
        private System.Windows.Forms.Label label7;
    }
}