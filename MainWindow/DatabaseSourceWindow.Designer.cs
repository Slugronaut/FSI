using System;

namespace SAOT.ConfigWindow
{
    partial class DatabaseSourceWindow
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
            this.buttonChoosePath = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonUserDB = new System.Windows.Forms.Button();
            this.buttonCreateWarehouseDB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonChoosePath
            // 
            this.buttonChoosePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChoosePath.Location = new System.Drawing.Point(415, 30);
            this.buttonChoosePath.Name = "buttonChoosePath";
            this.buttonChoosePath.Size = new System.Drawing.Size(75, 23);
            this.buttonChoosePath.TabIndex = 0;
            this.buttonChoosePath.Text = "Choose";
            this.buttonChoosePath.UseVisualStyleBackColor = true;
            this.buttonChoosePath.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(116, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(293, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Data Source Path";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonUserDB
            // 
            this.buttonUserDB.Location = new System.Drawing.Point(116, 58);
            this.buttonUserDB.Name = "buttonUserDB";
            this.buttonUserDB.Size = new System.Drawing.Size(159, 23);
            this.buttonUserDB.TabIndex = 3;
            this.buttonUserDB.Text = "Create User Database";
            this.buttonUserDB.UseVisualStyleBackColor = true;
            this.buttonUserDB.Click += new System.EventHandler(this.buttonCreateUserDatabase);
            // 
            // buttonCreateWarehouseDB
            // 
            this.buttonCreateWarehouseDB.Location = new System.Drawing.Point(116, 87);
            this.buttonCreateWarehouseDB.Name = "buttonCreateWarehouseDB";
            this.buttonCreateWarehouseDB.Size = new System.Drawing.Size(159, 23);
            this.buttonCreateWarehouseDB.TabIndex = 4;
            this.buttonCreateWarehouseDB.Text = "Create Warehouse Database";
            this.buttonCreateWarehouseDB.UseVisualStyleBackColor = true;
            this.buttonCreateWarehouseDB.Click += new System.EventHandler(this.buttonCreateWarehouseDB_Click);
            // 
            // DatabaseSourceWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 138);
            this.Controls.Add(this.buttonCreateWarehouseDB);
            this.Controls.Add(this.buttonUserDB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonChoosePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(260, 50);
            this.Name = "DatabaseSourceWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Source";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            this.textBox1.Text = Config.DatabaseDir;
        }

        private System.Windows.Forms.Button buttonChoosePath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonUserDB;
        private System.Windows.Forms.Button buttonCreateWarehouseDB;
    }
}