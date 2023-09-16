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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxLineWeight = new System.Windows.Forms.CheckBox();
            this.comboBoxPlotStyle = new System.Windows.Forms.ComboBox();
            this.comboBoxScale = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxLandscape = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BlockcountLBL = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelBlkName = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxFile = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LS_PrinterName
            // 
            this.LS_PrinterName.FormattingEnabled = true;
            this.LS_PrinterName.Location = new System.Drawing.Point(14, 51);
            this.LS_PrinterName.Name = "LS_PrinterName";
            this.LS_PrinterName.ScrollAlwaysVisible = true;
            this.LS_PrinterName.Size = new System.Drawing.Size(148, 186);
            this.LS_PrinterName.TabIndex = 0;
            this.LS_PrinterName.SelectedIndexChanged += new System.EventHandler(this.LS_PrinterName_SelectedIndexChanged);
            // 
            // LS_Size
            // 
            this.LS_Size.FormattingEnabled = true;
            this.LS_Size.Location = new System.Drawing.Point(168, 51);
            this.LS_Size.Name = "LS_Size";
            this.LS_Size.ScrollAlwaysVisible = true;
            this.LS_Size.Size = new System.Drawing.Size(169, 186);
            this.LS_Size.Sorted = true;
            this.LS_Size.TabIndex = 1;
            // 
            // Plot_BTN
            // 
            this.Plot_BTN.Location = new System.Drawing.Point(354, 216);
            this.Plot_BTN.Name = "Plot_BTN";
            this.Plot_BTN.Size = new System.Drawing.Size(75, 23);
            this.Plot_BTN.TabIndex = 3;
            this.Plot_BTN.Text = "BatchPlot";
            this.Plot_BTN.UseVisualStyleBackColor = true;
            this.Plot_BTN.Click += new System.EventHandler(this.Plot_BTN_Click);
            // 
            // BlockSelect_BTN
            // 
            this.BlockSelect_BTN.Location = new System.Drawing.Point(126, 26);
            this.BlockSelect_BTN.Name = "BlockSelect_BTN";
            this.BlockSelect_BTN.Size = new System.Drawing.Size(75, 23);
            this.BlockSelect_BTN.TabIndex = 2;
            this.BlockSelect_BTN.Text = "Select Block";
            this.BlockSelect_BTN.UseVisualStyleBackColor = true;
            this.BlockSelect_BTN.Click += new System.EventHandler(this.BlockSelect_BTN_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Printer List:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Paper Size:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(343, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Or enter Scale Below:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxFile);
            this.groupBox1.Controls.Add(this.checkBoxLineWeight);
            this.groupBox1.Controls.Add(this.comboBoxPlotStyle);
            this.groupBox1.Controls.Add(this.Plot_BTN);
            this.groupBox1.Controls.Add(this.comboBoxScale);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.checkBoxLandscape);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxScale);
            this.groupBox1.Controls.Add(this.LS_Size);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.LS_PrinterName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 245);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printer Properties";
            // 
            // checkBoxLineWeight
            // 
            this.checkBoxLineWeight.AutoSize = true;
            this.checkBoxLineWeight.Location = new System.Drawing.Point(346, 170);
            this.checkBoxLineWeight.Name = "checkBoxLineWeight";
            this.checkBoxLineWeight.Size = new System.Drawing.Size(83, 17);
            this.checkBoxLineWeight.TabIndex = 15;
            this.checkBoxLineWeight.Text = "Line Weight";
            this.checkBoxLineWeight.UseVisualStyleBackColor = true;
            // 
            // comboBoxPlotStyle
            // 
            this.comboBoxPlotStyle.FormattingEnabled = true;
            this.comboBoxPlotStyle.Location = new System.Drawing.Point(346, 121);
            this.comboBoxPlotStyle.Name = "comboBoxPlotStyle";
            this.comboBoxPlotStyle.Size = new System.Drawing.Size(97, 21);
            this.comboBoxPlotStyle.TabIndex = 14;
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.Location = new System.Drawing.Point(346, 42);
            this.comboBoxScale.Name = "comboBoxScale";
            this.comboBoxScale.Size = new System.Drawing.Size(97, 21);
            this.comboBoxScale.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(343, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Select Scale:";
            // 
            // checkBoxLandscape
            // 
            this.checkBoxLandscape.AutoSize = true;
            this.checkBoxLandscape.Location = new System.Drawing.Point(346, 147);
            this.checkBoxLandscape.Name = "checkBoxLandscape";
            this.checkBoxLandscape.Size = new System.Drawing.Size(79, 17);
            this.checkBoxLandscape.TabIndex = 10;
            this.checkBoxLandscape.Text = "Landscape";
            this.checkBoxLandscape.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(343, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Plot Style:";
            // 
            // textBoxScale
            // 
            this.textBoxScale.Location = new System.Drawing.Point(346, 82);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(97, 20);
            this.textBoxScale.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelBlkName);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.BlockcountLBL);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.BlockSelect_BTN);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 65);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Block Properties";
            // 
            // BlockcountLBL
            // 
            this.BlockcountLBL.AutoSize = true;
            this.BlockcountLBL.Location = new System.Drawing.Point(300, 31);
            this.BlockcountLBL.Name = "BlockcountLBL";
            this.BlockcountLBL.Size = new System.Drawing.Size(13, 13);
            this.BlockcountLBL.TabIndex = 5;
            this.BlockcountLBL.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(220, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Block Counts:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Select Sample Block:";
            // 
            // labelBlkName
            // 
            this.labelBlkName.AutoSize = true;
            this.labelBlkName.Location = new System.Drawing.Point(409, 31);
            this.labelBlkName.Name = "labelBlkName";
            this.labelBlkName.Size = new System.Drawing.Size(0, 13);
            this.labelBlkName.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(329, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Block Name:";
            // 
            // checkBoxFile
            // 
            this.checkBoxFile.AutoSize = true;
            this.checkBoxFile.Location = new System.Drawing.Point(346, 193);
            this.checkBoxFile.Name = "checkBoxFile";
            this.checkBoxFile.Size = new System.Drawing.Size(75, 17);
            this.checkBoxFile.TabIndex = 16;
            this.checkBoxFile.Text = "Plot to File";
            this.checkBoxFile.UseVisualStyleBackColor = true;
            // 
            // Batch_Plot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 336);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(500, 375);
            this.MinimumSize = new System.Drawing.Size(500, 375);
            this.Name = "Batch_Plot";
            this.Text = "Batch Plot Ver 1.0";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LS_PrinterName;
        private System.Windows.Forms.ListBox LS_Size;
        private System.Windows.Forms.Button Plot_BTN;
        private System.Windows.Forms.Button BlockSelect_BTN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.ComboBox comboBoxScale;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxLandscape;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label BlockcountLBL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxPlotStyle;
        private System.Windows.Forms.CheckBox checkBoxLineWeight;
        private System.Windows.Forms.Label labelBlkName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxFile;
    }
}