namespace Sections
{
    partial class CreateSectionFromXYZ
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
            this.LS_SLG = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CreateBTN = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SectionFromFile_TxtBox = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.راهنماToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnselALG = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.TimeElapseStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ErrorNOStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.Tol = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkCHAIN = new System.Windows.Forms.CheckBox();
            this.chksurf = new System.Windows.Forms.CheckBox();
            this.Alignment_chkBox = new System.Windows.Forms.CheckBox();
            this.BoundaryCHK = new System.Windows.Forms.CheckBox();
            this.samplefrompointsCHK = new System.Windows.Forms.CheckBox();
            this.polyCHK = new System.Windows.Forms.CheckBox();
            this.POgroupCHK = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LS_SLG
            // 
            this.LS_SLG.FormattingEnabled = true;
            this.LS_SLG.Location = new System.Drawing.Point(4, 88);
            this.LS_SLG.Name = "LS_SLG";
            this.LS_SLG.ScrollAlwaysVisible = true;
            this.LS_SLG.Size = new System.Drawing.Size(152, 56);
            this.LS_SLG.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Sample Line Group:";
            // 
            // CreateBTN
            // 
            this.CreateBTN.Location = new System.Drawing.Point(175, 252);
            this.CreateBTN.Name = "CreateBTN";
            this.CreateBTN.Size = new System.Drawing.Size(75, 23);
            this.CreateBTN.TabIndex = 43;
            this.CreateBTN.Text = "Create";
            this.CreateBTN.UseVisualStyleBackColor = true;
            this.CreateBTN.Click += new System.EventHandler(this.CreateBTN_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Point List:";
            // 
            // SectionFromFile_TxtBox
            // 
            this.SectionFromFile_TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SectionFromFile_TxtBox.BackColor = System.Drawing.SystemColors.Window;
            this.SectionFromFile_TxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.SectionFromFile_TxtBox.Location = new System.Drawing.Point(4, 179);
            this.SectionFromFile_TxtBox.MaxLength = 30;
            this.SectionFromFile_TxtBox.Multiline = true;
            this.SectionFromFile_TxtBox.Name = "SectionFromFile_TxtBox";
            this.SectionFromFile_TxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SectionFromFile_TxtBox.Size = new System.Drawing.Size(152, 89);
            this.SectionFromFile_TxtBox.TabIndex = 41;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(4, 43);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 38;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Select Alingment:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.راهنماToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(331, 24);
            this.menuStrip1.TabIndex = 46;
            this.menuStrip1.Text = "menuStrip1";
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
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(170, 22);
            this.toolStripMenuItem1.Text = "Open";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.saveToolStripMenuItem.Text = "Save-Total Format";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click_1);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click_1);
            // 
            // راهنماToolStripMenuItem
            // 
            this.راهنماToolStripMenuItem.Name = "راهنماToolStripMenuItem";
            this.راهنماToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.راهنماToolStripMenuItem.Text = "Help";
            this.راهنماToolStripMenuItem.Click += new System.EventHandler(this.راهنماToolStripMenuItem_Click);
            // 
            // btnselALG
            // 
            this.btnselALG.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnselALG.ForeColor = System.Drawing.SystemColors.Control;
            this.btnselALG.Image = global::SectionToolBox.Properties.Resources.Untitled_5;
            this.btnselALG.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnselALG.Location = new System.Drawing.Point(131, 40);
            this.btnselALG.Name = "btnselALG";
            this.btnselALG.Size = new System.Drawing.Size(25, 26);
            this.btnselALG.TabIndex = 39;
            this.btnselALG.UseVisualStyleBackColor = false;
            this.btnselALG.Click += new System.EventHandler(this.Select_Alignment_BTN_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgBar,
            this.TimeElapseStripStatus,
            this.ErrorNOStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 282);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(331, 22);
            this.statusStrip1.TabIndex = 47;
            this.statusStrip1.Text = "statusStrip1";
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
            // TimeElapseStripStatus
            // 
            this.TimeElapseStripStatus.Name = "TimeElapseStripStatus";
            this.TimeElapseStripStatus.Size = new System.Drawing.Size(125, 17);
            this.TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00";
            // 
            // ErrorNOStripStatus
            // 
            this.ErrorNOStripStatus.Name = "ErrorNOStripStatus";
            this.ErrorNOStripStatus.Size = new System.Drawing.Size(49, 17);
            this.ErrorNOStripStatus.Text = "Errors: 0";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(175, 68);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(116, 17);
            this.checkBox1.TabIndex = 48;
            this.checkBox1.Text = "Sample Line Group";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Tol
            // 
            this.Tol.Enabled = false;
            this.Tol.Location = new System.Drawing.Point(231, 40);
            this.Tol.Name = "Tol";
            this.Tol.Size = new System.Drawing.Size(48, 20);
            this.Tol.TabIndex = 49;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 50;
            this.label3.Text = "Tolerance:";
            // 
            // chkCHAIN
            // 
            this.chkCHAIN.AutoSize = true;
            this.chkCHAIN.Location = new System.Drawing.Point(175, 114);
            this.chkCHAIN.Name = "chkCHAIN";
            this.chkCHAIN.Size = new System.Drawing.Size(106, 17);
            this.chkCHAIN.TabIndex = 51;
            this.chkCHAIN.Text = "Chainage Format";
            this.chkCHAIN.UseVisualStyleBackColor = true;
            // 
            // chksurf
            // 
            this.chksurf.AutoSize = true;
            this.chksurf.Location = new System.Drawing.Point(175, 183);
            this.chksurf.Name = "chksurf";
            this.chksurf.Size = new System.Drawing.Size(97, 17);
            this.chksurf.TabIndex = 52;
            this.chksurf.Text = "Create Surface";
            this.chksurf.UseVisualStyleBackColor = true;
            // 
            // Alignment_chkBox
            // 
            this.Alignment_chkBox.AutoSize = true;
            this.Alignment_chkBox.Location = new System.Drawing.Point(175, 137);
            this.Alignment_chkBox.Name = "Alignment_chkBox";
            this.Alignment_chkBox.Size = new System.Drawing.Size(147, 17);
            this.Alignment_chkBox.TabIndex = 53;
            this.Alignment_chkBox.Text = "None-Standard Alignment";
            this.Alignment_chkBox.UseVisualStyleBackColor = true;
            this.Alignment_chkBox.CheckedChanged += new System.EventHandler(this.Alignment_chkBox_CheckedChanged);
            // 
            // BoundaryCHK
            // 
            this.BoundaryCHK.AutoSize = true;
            this.BoundaryCHK.Location = new System.Drawing.Point(175, 206);
            this.BoundaryCHK.Name = "BoundaryCHK";
            this.BoundaryCHK.Size = new System.Drawing.Size(111, 17);
            this.BoundaryCHK.TabIndex = 54;
            this.BoundaryCHK.Text = "Surface Boundary";
            this.BoundaryCHK.UseVisualStyleBackColor = true;
            // 
            // samplefrompointsCHK
            // 
            this.samplefrompointsCHK.AutoSize = true;
            this.samplefrompointsCHK.Location = new System.Drawing.Point(175, 91);
            this.samplefrompointsCHK.Name = "samplefrompointsCHK";
            this.samplefrompointsCHK.Size = new System.Drawing.Size(143, 17);
            this.samplefrompointsCHK.TabIndex = 55;
            this.samplefrompointsCHK.Text = "Sample Lines from points";
            this.samplefrompointsCHK.UseVisualStyleBackColor = true;
            // 
            // polyCHK
            // 
            this.polyCHK.AutoSize = true;
            this.polyCHK.Location = new System.Drawing.Point(175, 160);
            this.polyCHK.Name = "polyCHK";
            this.polyCHK.Size = new System.Drawing.Size(104, 17);
            this.polyCHK.TabIndex = 56;
            this.polyCHK.Text = "3dpoly on Points";
            this.polyCHK.UseVisualStyleBackColor = true;
            // 
            // POgroupCHK
            // 
            this.POgroupCHK.AutoSize = true;
            this.POgroupCHK.Location = new System.Drawing.Point(175, 229);
            this.POgroupCHK.Name = "POgroupCHK";
            this.POgroupCHK.Size = new System.Drawing.Size(113, 17);
            this.POgroupCHK.TabIndex = 57;
            this.POgroupCHK.Text = "Create PointGroup";
            this.POgroupCHK.UseVisualStyleBackColor = true;
            // 
            // CreateSectionFromXYZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 304);
            this.Controls.Add(this.POgroupCHK);
            this.Controls.Add(this.polyCHK);
            this.Controls.Add(this.samplefrompointsCHK);
            this.Controls.Add(this.BoundaryCHK);
            this.Controls.Add(this.Alignment_chkBox);
            this.Controls.Add(this.chksurf);
            this.Controls.Add(this.chkCHAIN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Tol);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.LS_SLG);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CreateBTN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SectionFromFile_TxtBox);
            this.Controls.Add(this.btnselALG);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(347, 343);
            this.Name = "CreateSectionFromXYZ";
            this.Text = "Create Section From PXYZ Ver1.4";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LS_SLG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CreateBTN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SectionFromFile_TxtBox;
        private System.Windows.Forms.Button btnselALG;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        //private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem راهنماToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProgBar;
        private System.Windows.Forms.ToolStripStatusLabel TimeElapseStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel ErrorNOStripStatus;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox Tol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkCHAIN;
        private System.Windows.Forms.CheckBox chksurf;
        private System.Windows.Forms.CheckBox Alignment_chkBox;
        private System.Windows.Forms.CheckBox BoundaryCHK;
        private System.Windows.Forms.CheckBox samplefrompointsCHK;
        private System.Windows.Forms.CheckBox polyCHK;
        private System.Windows.Forms.CheckBox POgroupCHK;
    }
}