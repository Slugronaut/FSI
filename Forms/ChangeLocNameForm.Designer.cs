namespace SAOT.Forms
{
    partial class ChangeLocNameForm
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
            this.textBoxOldLoc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAisle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxRack = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxBin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxLevel = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxOldLoc
            // 
            this.textBoxOldLoc.Enabled = false;
            this.textBoxOldLoc.Location = new System.Drawing.Point(92, 23);
            this.textBoxOldLoc.Name = "textBoxOldLoc";
            this.textBoxOldLoc.ReadOnly = true;
            this.textBoxOldLoc.Size = new System.Drawing.Size(186, 20);
            this.textBoxOldLoc.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Old Location:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "New Aisle:";
            // 
            // textBoxAisle
            // 
            this.textBoxAisle.Location = new System.Drawing.Point(79, 64);
            this.textBoxAisle.Name = "textBoxAisle";
            this.textBoxAisle.Size = new System.Drawing.Size(53, 20);
            this.textBoxAisle.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "New Rack:";
            // 
            // textBoxRack
            // 
            this.textBoxRack.Location = new System.Drawing.Point(225, 64);
            this.textBoxRack.Name = "textBoxRack";
            this.textBoxRack.Size = new System.Drawing.Size(53, 20);
            this.textBoxRack.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "New Bin:";
            // 
            // textBoxBin
            // 
            this.textBoxBin.Location = new System.Drawing.Point(225, 90);
            this.textBoxBin.Name = "textBoxBin";
            this.textBoxBin.Size = new System.Drawing.Size(53, 20);
            this.textBoxBin.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "New Level:";
            // 
            // textBoxLevel
            // 
            this.textBoxLevel.Location = new System.Drawing.Point(79, 90);
            this.textBoxLevel.Name = "textBoxLevel";
            this.textBoxLevel.Size = new System.Drawing.Size(53, 20);
            this.textBoxLevel.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Change";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChangeLocNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 199);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxBin);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxLevel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxRack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxAisle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxOldLoc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeLocNameForm";
            this.ShowIcon = false;
            this.Text = "Change Location Name";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOldLoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAisle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRack;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxBin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLevel;
        private System.Windows.Forms.Button button1;
    }
}