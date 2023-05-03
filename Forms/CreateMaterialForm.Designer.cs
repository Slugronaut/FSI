namespace SAOT.Forms
{
    partial class CreateMaterialForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDocId = new System.Windows.Forms.TextBox();
            this.textBoxSapId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCustomerId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxUoM = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxGroupId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(305, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Doc Id";
            // 
            // textBoxDocId
            // 
            this.textBoxDocId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDocId.Location = new System.Drawing.Point(81, 32);
            this.textBoxDocId.Name = "textBoxDocId";
            this.textBoxDocId.Size = new System.Drawing.Size(361, 20);
            this.textBoxDocId.TabIndex = 2;
            // 
            // textBoxSapId
            // 
            this.textBoxSapId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSapId.Location = new System.Drawing.Point(81, 6);
            this.textBoxSapId.Name = "textBoxSapId";
            this.textBoxSapId.Size = new System.Drawing.Size(361, 20);
            this.textBoxSapId.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SAP Id";
            // 
            // textBoxCustomerId
            // 
            this.textBoxCustomerId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCustomerId.Location = new System.Drawing.Point(81, 58);
            this.textBoxCustomerId.Name = "textBoxCustomerId";
            this.textBoxCustomerId.Size = new System.Drawing.Size(361, 20);
            this.textBoxCustomerId.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Customer Id";
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDesc.Location = new System.Drawing.Point(81, 84);
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.Size = new System.Drawing.Size(361, 20);
            this.textBoxDesc.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Description";
            // 
            // textBoxUoM
            // 
            this.textBoxUoM.Location = new System.Drawing.Point(81, 110);
            this.textBoxUoM.Name = "textBoxUoM";
            this.textBoxUoM.Size = new System.Drawing.Size(74, 20);
            this.textBoxUoM.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "UoM";
            // 
            // textBoxGroupId
            // 
            this.textBoxGroupId.Location = new System.Drawing.Point(81, 136);
            this.textBoxGroupId.Name = "textBoxGroupId";
            this.textBoxGroupId.Size = new System.Drawing.Size(74, 20);
            this.textBoxGroupId.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Group";
            // 
            // CreateMaterialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 265);
            this.Controls.Add(this.textBoxGroupId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxUoM);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxDesc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxCustomerId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxSapId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDocId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "CreateMaterialForm";
            this.Text = "CreateMaterialForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDocId;
        private System.Windows.Forms.TextBox textBoxSapId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCustomerId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxUoM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxGroupId;
        private System.Windows.Forms.Label label6;
    }
}