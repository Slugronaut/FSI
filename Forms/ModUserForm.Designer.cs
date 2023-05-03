namespace SAOT
{
    partial class ModUserForm
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
            this.AccessRightsCheckList = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PasswordTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EmailTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ChangePasswordButton = new System.Windows.Forms.Button();
            this.UsernameTextbox = new System.Windows.Forms.TextBox();
            this.RightsGroupbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.RightsGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AccessRightsCheckList
            // 
            this.AccessRightsCheckList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccessRightsCheckList.FormattingEnabled = true;
            this.AccessRightsCheckList.Items.AddRange(new object[] {
            "Admin",
            "Create Orders",
            "Pick Orders",
            "Manage Materials",
            "Manage Locations",
            "Manage Users"});
            this.AccessRightsCheckList.Location = new System.Drawing.Point(6, 19);
            this.AccessRightsCheckList.Name = "AccessRightsCheckList";
            this.AccessRightsCheckList.Size = new System.Drawing.Size(164, 94);
            this.AccessRightsCheckList.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PasswordTextbox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.EmailTextbox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ChangePasswordButton);
            this.groupBox1.Controls.Add(this.UsernameTextbox);
            this.groupBox1.Controls.Add(this.RightsGroupbox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 153);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Location = new System.Drawing.Point(98, 84);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.Size = new System.Drawing.Size(118, 20);
            this.PasswordTextbox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "New Password";
            // 
            // EmailTextbox
            // 
            this.EmailTextbox.Location = new System.Drawing.Point(98, 58);
            this.EmailTextbox.Name = "EmailTextbox";
            this.EmailTextbox.Size = new System.Drawing.Size(118, 20);
            this.EmailTextbox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Email";
            // 
            // ChangePasswordButton
            // 
            this.ChangePasswordButton.Location = new System.Drawing.Point(98, 110);
            this.ChangePasswordButton.Name = "ChangePasswordButton";
            this.ChangePasswordButton.Size = new System.Drawing.Size(118, 23);
            this.ChangePasswordButton.TabIndex = 5;
            this.ChangePasswordButton.Text = "Change Password";
            this.ChangePasswordButton.UseVisualStyleBackColor = true;
            this.ChangePasswordButton.Click += new System.EventHandler(this.ChangePasswordButton_Click);
            // 
            // UsernameTextbox
            // 
            this.UsernameTextbox.Location = new System.Drawing.Point(98, 32);
            this.UsernameTextbox.Name = "UsernameTextbox";
            this.UsernameTextbox.ReadOnly = true;
            this.UsernameTextbox.Size = new System.Drawing.Size(118, 20);
            this.UsernameTextbox.TabIndex = 1;
            // 
            // RightsGroupbox
            // 
            this.RightsGroupbox.Controls.Add(this.AccessRightsCheckList);
            this.RightsGroupbox.Location = new System.Drawing.Point(222, 19);
            this.RightsGroupbox.Name = "RightsGroupbox";
            this.RightsGroupbox.Size = new System.Drawing.Size(176, 123);
            this.RightsGroupbox.TabIndex = 2;
            this.RightsGroupbox.TabStop = false;
            this.RightsGroupbox.Text = "Access Rights";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 182);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(92, 23);
            this.SaveButton.TabIndex = 10;
            this.SaveButton.Text = "Save Changes";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ModUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 217);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modify User";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.RightsGroupbox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox AccessRightsCheckList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox UsernameTextbox;
        private System.Windows.Forms.GroupBox RightsGroupbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ChangePasswordButton;
        private System.Windows.Forms.TextBox EmailTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SaveButton;
    }
}