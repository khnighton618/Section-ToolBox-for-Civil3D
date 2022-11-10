namespace SectionVer2.Other_App.BatchPlot
{
    partial class Batch_Plot
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
            this.LS_PrinterName = new System.Windows.Forms.ListBox();
            this.LS_Size = new System.Windows.Forms.ListBox();
            this.Plot_BTN = new System.Windows.Forms.Button();
            this.BlockSelect_BTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LS_PrinterName
            // 
            this.LS_PrinterName.FormattingEnabled = true;
            this.LS_PrinterName.Location = new System.Drawing.Point(12, 19);
            this.LS_PrinterName.MultiColumn = true;
            this.LS_PrinterName.Name = "LS_PrinterName";
            this.LS_PrinterName.ScrollAlwaysVisible = true;
            this.LS_PrinterName.Size = new System.Drawing.Size(117, 160);
            this.LS_PrinterName.TabIndex = 0;
            this.LS_PrinterName.SelectedIndexChanged += new System.EventHandler(this.LS_PrinterName_SelectedIndexChanged);
            // 
            // LS_Size
            // 
            this.LS_Size.FormattingEnabled = true;
            this.LS_Size.Location = new System.Drawing.Point(135, 19);
            this.LS_Size.MultiColumn = true;
            this.LS_Size.Name = "LS_Size";
            this.LS_Size.ScrollAlwaysVisible = true;
            this.LS_Size.Size = new System.Drawing.Size(99, 160);
            this.LS_Size.Sorted = true;
            this.LS_Size.TabIndex = 1;
            // 
            // Plot_BTN
            // 
            this.Plot_BTN.Location = new System.Drawing.Point(240, 48);
            this.Plot_BTN.Name = "Plot_BTN";
            this.Plot_BTN.Size = new System.Drawing.Size(75, 23);
            this.Plot_BTN.TabIndex = 3;
            this.Plot_BTN.Text = "BatchPlot";
            this.Plot_BTN.UseVisualStyleBackColor = true;
            this.Plot_BTN.Click += new System.EventHandler(this.Plot_BTN_Click);
            // 
            // BlockSelect_BTN
            // 
            this.BlockSelect_BTN.Location = new System.Drawing.Point(240, 19);
            this.BlockSelect_BTN.Name = "BlockSelect_BTN";
            this.BlockSelect_BTN.Size = new System.Drawing.Size(75, 23);
            this.BlockSelect_BTN.TabIndex = 2;
            this.BlockSelect_BTN.Text = "Select Block";
            this.BlockSelect_BTN.UseVisualStyleBackColor = true;
            this.BlockSelect_BTN.Click += new System.EventHandler(this.BlockSelect_BTN_Click);
            // 
            // Batch_Plot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 187);
            this.Controls.Add(this.BlockSelect_BTN);
            this.Controls.Add(this.Plot_BTN);
            this.Controls.Add(this.LS_Size);
            this.Controls.Add(this.LS_PrinterName);
            this.MaximumSize = new System.Drawing.Size(341, 226);
            this.MinimumSize = new System.Drawing.Size(341, 226);
            this.Name = "Batch_Plot";
            this.Text = "Batch_Plot";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LS_PrinterName;
        private System.Windows.Forms.ListBox LS_Size;
        private System.Windows.Forms.Button Plot_BTN;
        private System.Windows.Forms.Button BlockSelect_BTN;
    }
}