namespace TestWinform
{
    partial class MetaDataSelector
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
            this.cbbList = new System.Windows.Forms.ComboBox();
            this.dgvMetaDatas = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetaDatas)).BeginInit();
            this.SuspendLayout();
            // 
            // cbbList
            // 
            this.cbbList.FormattingEnabled = true;
            this.cbbList.Location = new System.Drawing.Point(111, 13);
            this.cbbList.Name = "cbbList";
            this.cbbList.Size = new System.Drawing.Size(406, 21);
            this.cbbList.TabIndex = 0;
            this.cbbList.SelectedIndexChanged += new System.EventHandler(this.cbbList_SelectedIndexChanged);
            // 
            // dgvMetaDatas
            // 
            this.dgvMetaDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMetaDatas.Location = new System.Drawing.Point(111, 51);
            this.dgvMetaDatas.Name = "dgvMetaDatas";
            this.dgvMetaDatas.Size = new System.Drawing.Size(406, 252);
            this.dgvMetaDatas.TabIndex = 1;
            // 
            // MetaDataSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 397);
            this.Controls.Add(this.dgvMetaDatas);
            this.Controls.Add(this.cbbList);
            this.Name = "MetaDataSelector";
            this.Text = "MetaDataSelector";
            this.Load += new System.EventHandler(this.MetaDataSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetaDatas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbList;
        private System.Windows.Forms.DataGridView dgvMetaDatas;
    }
}