namespace Sections
{
    partial class CreateTunnel
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
            this.ErrorNOStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TimeElapseStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.راهنماToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.CreateBTN = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.LS_SLG = new System.Windows.Forms.ListBox();
            this.chkSurface = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LS_Prof = new System.Windows.Forms.ListBox();
            this.chkConvex = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LS_Alg = new System.Windows.Forms.ListBox();
            this.check3dpoly = new System.Windows.Forms.CheckBox();
            this.checkBoxSection = new System.Windows.Forms.CheckBox();
            this.checkBoxHatch = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SectionFromFile_TxtBox = new System.Windows.Forms.TextBox();
            this.chkformat = new System.Windows.Forms.CheckBox();
            this.chksampleline = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Tolbox = new System.Windows.Forms.TextBox();
            this.chkwhitoutslg = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ErrorNOStripStatus
            // 
            this.ErrorNOStripStatus.Name = "ErrorNOStripStatus";
            this.ErrorNOStripStatus.Size = new System.Drawing.Size(49, 17);
            this.ErrorNOStripStatus.Text = "Errors: 0";
            // 
            // ProgBar
            // 
            this.ProgBar.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.ProgBar.MergeIndex = 0;
            this.ProgBar.Name = "ProgBar";
            this.ProgBar.Size = new System.Drawing.Size(100, 16);
            this.ProgBar.Step = 1;
            this.ProgBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgBar,
            this.TimeElapseStripStatus,
            this.ErrorNOStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 287);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(500, 22);
            this.statusStrip1.TabIndex = 57;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TimeElapseStripStatus
            // 
            this.TimeElapseStripStatus.Name = "TimeElapseStripStatus";
            this.TimeElapseStripStatus.Size = new System.Drawing.Size(125, 17);
            this.TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00";
            // 
            // راهنماToolStripMenuItem
            // 
            this.راهنماToolStripMenuItem.Name = "راهنماToolStripMenuItem";
            this.راهنماToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.راهنماToolStripMenuItem.Text = "About";
            this.راهنماToolStripMenuItem.Click += new System.EventHandler(this.About_ToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click_1);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem1.Text = "Open";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Sample Line Group:";
            // 
            // CreateBTN
            // 
            this.CreateBTN.Location = new System.Drawing.Point(182, 250);
            this.CreateBTN.Name = "CreateBTN";
            this.CreateBTN.Size = new System.Drawing.Size(75, 23);
            this.CreateBTN.TabIndex = 53;
            this.CreateBTN.Text = "Create";
            this.CreateBTN.UseVisualStyleBackColor = true;
            this.CreateBTN.Click += new System.EventHandler(this.CreateBTN_Click_1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.راهنماToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(500, 24);
            this.menuStrip1.TabIndex = 56;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // LS_SLG
            // 
            this.LS_SLG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LS_SLG.FormattingEnabled = true;
            this.LS_SLG.Location = new System.Drawing.Point(5, 189);
            this.LS_SLG.Name = "LS_SLG";
            this.LS_SLG.ScrollAlwaysVisible = true;
            this.LS_SLG.Size = new System.Drawing.Size(152, 82);
            this.LS_SLG.TabIndex = 55;
            // 
            // chkSurface
            // 
            this.chkSurface.AutoSize = true;
            this.chkSurface.Location = new System.Drawing.Point(171, 210);
            this.chkSurface.Name = "chkSurface";
            this.chkSurface.Size = new System.Drawing.Size(156, 17);
            this.chkSurface.TabIndex = 58;
            this.chkSurface.Text = "Create Surface and Points?";
            this.chkSurface.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(168, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 59;
            this.label3.Text = "Profile:";
            // 
            // LS_Prof
            // 
            this.LS_Prof.FormattingEnabled = true;
            this.LS_Prof.Location = new System.Drawing.Point(171, 58);
            this.LS_Prof.Name = "LS_Prof";
            this.LS_Prof.ScrollAlwaysVisible = true;
            this.LS_Prof.Size = new System.Drawing.Size(152, 43);
            this.LS_Prof.TabIndex = 60;
            // 
            // chkConvex
            // 
            this.chkConvex.AutoSize = true;
            this.chkConvex.Location = new System.Drawing.Point(171, 227);
            this.chkConvex.Name = "chkConvex";
            this.chkConvex.Size = new System.Drawing.Size(134, 17);
            this.chkConvex.TabIndex = 61;
            this.chkConvex.Text = "Convex Hull(Optional)?";
            this.chkConvex.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "Select Alignment:";
            // 
            // LS_Alg
            // 
            this.LS_Alg.FormattingEnabled = true;
            this.LS_Alg.Location = new System.Drawing.Point(5, 58);
            this.LS_Alg.Name = "LS_Alg";
            this.LS_Alg.ScrollAlwaysVisible = true;
            this.LS_Alg.Size = new System.Drawing.Size(152, 108);
            this.LS_Alg.TabIndex = 63;
            this.LS_Alg.SelectedIndexChanged += new System.EventHandler(this.LS_Alg_SelectedIndexChanged);
            // 
            // check3dpoly
            // 
            this.check3dpoly.AutoSize = true;
            this.check3dpoly.Location = new System.Drawing.Point(171, 159);
            this.check3dpoly.Name = "check3dpoly";
            this.check3dpoly.Size = new System.Drawing.Size(115, 17);
            this.check3dpoly.TabIndex = 64;
            this.check3dpoly.Text = "Create 3Dpolyline?";
            this.check3dpoly.UseVisualStyleBackColor = true;
            // 
            // checkBoxSection
            // 
            this.checkBoxSection.AutoSize = true;
            this.checkBoxSection.Location = new System.Drawing.Point(171, 176);
            this.checkBoxSection.Name = "checkBoxSection";
            this.checkBoxSection.Size = new System.Drawing.Size(133, 17);
            this.checkBoxSection.TabIndex = 65;
            this.checkBoxSection.Text = "Create Section Views?";
            this.checkBoxSection.UseVisualStyleBackColor = true;
            // 
            // checkBoxHatch
            // 
            this.checkBoxHatch.AutoSize = true;
            this.checkBoxHatch.Location = new System.Drawing.Point(171, 193);
            this.checkBoxHatch.Name = "checkBoxHatch";
            this.checkBoxHatch.Size = new System.Drawing.Size(95, 17);
            this.checkBoxHatch.TabIndex = 66;
            this.checkBoxHatch.Text = "Create Hatch?";
            this.checkBoxHatch.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(339, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "Point List:";
            // 
            // SectionFromFile_TxtBox
            // 
            this.SectionFromFile_TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SectionFromFile_TxtBox.BackColor = System.Drawing.SystemColors.Window;
            this.SectionFromFile_TxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.SectionFromFile_TxtBox.Location = new System.Drawing.Point(342, 84);
            this.SectionFromFile_TxtBox.MaxLength = 30;
            this.SectionFromFile_TxtBox.Multiline = true;
            this.SectionFromFile_TxtBox.Name = "SectionFromFile_TxtBox";
            this.SectionFromFile_TxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SectionFromFile_TxtBox.Size = new System.Drawing.Size(146, 200);
            this.SectionFromFile_TxtBox.TabIndex = 51;
            // 
            // chkformat
            // 
            this.chkformat.AutoSize = true;
            this.chkformat.Location = new System.Drawing.Point(171, 142);
            this.chkformat.Name = "chkformat";
            this.chkformat.Size = new System.Drawing.Size(116, 17);
            this.chkformat.TabIndex = 67;
            this.chkformat.Text = "PXYZ,STA format?";
            this.chkformat.UseVisualStyleBackColor = true;
            this.chkformat.CheckedChanged += new System.EventHandler(this.chkformat_CheckedChanged);
            // 
            // chksampleline
            // 
            this.chksampleline.AutoSize = true;
            this.chksampleline.Location = new System.Drawing.Point(171, 125);
            this.chksampleline.Name = "chksampleline";
            this.chksampleline.Size = new System.Drawing.Size(143, 17);
            this.chksampleline.TabIndex = 68;
            this.chksampleline.Text = "Project on SampleLines?";
            this.chksampleline.UseVisualStyleBackColor = true;
            this.chksampleline.CheckedChanged += new System.EventHandler(this.chksampleline_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(339, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 69;
            this.label1.Text = "Tolerance:";
            // 
            // Tolbox
            // 
            this.Tolbox.Location = new System.Drawing.Point(403, 34);
            this.Tolbox.Name = "Tolbox";
            this.Tolbox.Size = new System.Drawing.Size(76, 20);
            this.Tolbox.TabIndex = 70;
            // 
            // chkwhitoutslg
            // 
            this.chkwhitoutslg.AutoSize = true;
            this.chkwhitoutslg.Location = new System.Drawing.Point(171, 108);
            this.chkwhitoutslg.Name = "chkwhitoutslg";
            this.chkwhitoutslg.Size = new System.Drawing.Size(84, 17);
            this.chkwhitoutslg.TabIndex = 71;
            this.chkwhitoutslg.Text = "High Slope?";
            this.chkwhitoutslg.UseVisualStyleBackColor = true;
            this.chkwhitoutslg.CheckedChanged += new System.EventHandler(this.chkwhitoutslg_CheckedChanged);
            // 
            // CreateTunnel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 309);
            this.Controls.Add(this.chkwhitoutslg);
            this.Controls.Add(this.Tolbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chksampleline);
            this.Controls.Add(this.chkformat);
            this.Controls.Add(this.checkBoxHatch);
            this.Controls.Add(this.checkBoxSection);
            this.Controls.Add(this.check3dpoly);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LS_Alg);
            this.Controls.Add(this.chkConvex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LS_Prof);
            this.Controls.Add(this.chkSurface);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CreateBTN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SectionFromFile_TxtBox);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.LS_SLG);
            this.MinimumSize = new System.Drawing.Size(515, 335);
            this.Name = "CreateTunnel";
            this.Text = "CreateTunnel";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel ErrorNOStripStatus;
        private System.Windows.Forms.ToolStripProgressBar ProgBar;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel TimeElapseStripStatus;
        private System.Windows.Forms.ToolStripMenuItem راهنماToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CreateBTN;
        //private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ListBox LS_SLG;
        private System.Windows.Forms.CheckBox chkSurface;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox LS_Prof;
        private System.Windows.Forms.CheckBox chkConvex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox LS_Alg;
        private System.Windows.Forms.CheckBox check3dpoly;
        private System.Windows.Forms.CheckBox checkBoxSection;
        private System.Windows.Forms.CheckBox checkBoxHatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SectionFromFile_TxtBox;
        private System.Windows.Forms.CheckBox chkformat;
        private System.Windows.Forms.CheckBox chksampleline;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Tolbox;
        private System.Windows.Forms.CheckBox chkwhitoutslg;
    }
}