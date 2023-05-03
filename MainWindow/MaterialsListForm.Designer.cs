namespace SAOT
{
    partial class MaterialsListForm
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
            this.filterableDataGridView1 = new SAOT.FilterableDataGridView();
            this.SuspendLayout();
            // 
            // filterableDataGridView1
            // 
            this.filterableDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterableDataGridView1.Location = new System.Drawing.Point(12, 12);
            this.filterableDataGridView1.Name = "filterableDataGridView1";
            this.filterableDataGridView1.Size = new System.Drawing.Size(776, 426);
            this.filterableDataGridView1.TabIndex = 0;
            this.filterableDataGridView1.Load += new System.EventHandler(this.filterableDataGridView1_Load);
            // 
            // MaterialsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.filterableDataGridView1);
            this.Name = "MaterialsListForm";
            this.Text = "Materials Master List";
            this.ResumeLayout(false);

        }

        #endregion

        private FilterableDataGridView filterableDataGridView1;
    }
}