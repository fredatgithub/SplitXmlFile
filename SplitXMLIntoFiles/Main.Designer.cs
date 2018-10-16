namespace SplitXMLIntoFiles
{
    partial class Main
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
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.btnSplit = new System.Windows.Forms.Button();
            this.txtFileSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // openFileDlg
            //
            this.openFileDlg.Filter = "xml|*.xml";
            this.openFileDlg.Title = "Huge XML Selector";
            //
            // btnSplit
            //
            this.btnSplit.Location = new System.Drawing.Point(16, 51);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(101, 23);
            this.btnSplit.TabIndex = 0;
            this.btnSplit.Text = "Split XML File";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.BtnSplitClick);
            //
            // txtFileSize
            //
            this.txtFileSize.Location = new System.Drawing.Point(16, 25);
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.Size = new System.Drawing.Size(100, 20);
            this.txtFileSize.TabIndex = 1;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "New File Size in MB:";
            //
            // Main
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 85);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileSize);
            this.Controls.Add(this.btnSplit);
            this.Name = "Main";
            this.Text = "XML File Splitter";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Windows Form Designer generated code

        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.TextBox txtFileSize;
        private System.Windows.Forms.Label label1;
    }
}