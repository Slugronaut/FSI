namespace SAOT
{
    partial class UserListForm
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
            this.AddUserButton = new System.Windows.Forms.Button();
            this.UserDataGrid = new System.Windows.Forms.DataGridView();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccessRights = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.UserDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // AddUserButton
            // 
            this.AddUserButton.Location = new System.Drawing.Point(12, 12);
            this.AddUserButton.Name = "AddUserButton";
            this.AddUserButton.Size = new System.Drawing.Size(75, 23);
            this.AddUserButton.TabIndex = 1;
            this.AddUserButton.Text = "Add";
            this.AddUserButton.UseVisualStyleBackColor = true;
            this.AddUserButton.Click += new System.EventHandler(this.AddUserButton_Click);
            // 
            // UserDataGrid
            // 
            this.UserDataGrid.AllowUserToAddRows = false;
            this.UserDataGrid.AllowUserToDeleteRows = false;
            this.UserDataGrid.AllowUserToResizeRows = false;
            this.UserDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UserDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UserDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Username,
            this.Email,
            this.AccessRights});
            this.UserDataGrid.Location = new System.Drawing.Point(12, 41);
            this.UserDataGrid.Name = "UserDataGrid";
            this.UserDataGrid.RowHeadersVisible = false;
            this.UserDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UserDataGrid.ShowEditingIcon = false;
            this.UserDataGrid.Size = new System.Drawing.Size(500, 308);
            this.UserDataGrid.TabIndex = 2;
            this.UserDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.UserDataGrid_CellContentClick);
            // 
            // Username
            // 
            this.Username.HeaderText = "Username";
            this.Username.MinimumWidth = 20;
            this.Username.Name = "Username";
            this.Username.ReadOnly = true;
            // 
            // Email
            // 
            this.Email.HeaderText = "Email";
            this.Email.MinimumWidth = 20;
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // AccessRights
            // 
            this.AccessRights.HeaderText = "AccessRights";
            this.AccessRights.MinimumWidth = 20;
            this.AccessRights.Name = "AccessRights";
            this.AccessRights.ReadOnly = true;
            // 
            // UserListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 361);
            this.Controls.Add(this.UserDataGrid);
            this.Controls.Add(this.AddUserButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserListForm";
            this.ShowIcon = false;
            this.Text = "Add or Modify Users";
            ((System.ComponentModel.ISupportInitialize)(this.UserDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button AddUserButton;
        private System.Windows.Forms.DataGridView UserDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccessRights;
    }
}