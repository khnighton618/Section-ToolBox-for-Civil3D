namespace Sections
{
    partial class CreateSectionFromFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateSectionFromFile));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Select_Alignment_BTN = new System.Windows.Forms.Button();
            this.Create_section_BTN = new System.Windows.Forms.Button();
            this.Point_Group_TxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SectionFromFile_TxtBox = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.TimeElapseStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ErrorNOStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkSLG = new System.Windows.Forms.CheckBox();
            this.chkpo = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(301, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // Select_Alignment_BTN
            // 
            this.Select_Alignment_BTN.Image = ((System.Drawing.Image)(resources.GetObject("Select_Alignment_BTN.Image")));
            this.Select_Alignment_BTN.Location = new System.Drawing.Point(269, 48);
            this.Select_Alignment_BTN.Name = "Select_Alignment_BTN";
            this.Select_Alignment_BTN.Size = new System.Drawing.Size(25, 21);
            this.Select_Alignment_BTN.TabIndex = 2;
            this.Select_Alignment_BTN.UseVisualStyleBackColor = true;
            this.Select_Alignment_BTN.Click += new System.EventHandler(this.selalgbtn_Click);
            // 
            // Create_section_BTN
            // 
            this.Create_section_BTN.Location = new System.Drawing.Point(166, 162);
            this.Create_section_BTN.Name = "Create_section_BTN";
            this.Create_section_BTN.Size = new System.Drawing.Size(97, 23);
            this.Create_section_BTN.TabIndex = 3;
            this.Create_section_BTN.Text = "Create Section";
            this.Create_section_BTN.UseVisualStyleBackColor = true;
            this.Create_section_BTN.Click += new System.EventHandler(this.createsectionBtn_Click);
            // 
            // Point_Group_TxtBox
            // 
            this.Point_Group_TxtBox.BackColor = System.Drawing.SystemColors.Window;
            this.Point_Group_TxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Point_Group_TxtBox.Location = new System.Drawing.Point(166, 90);
            this.Point_Group_TxtBox.Name = "Point_Group_TxtBox";
            this.Point_Group_TxtBox.Size = new System.Drawing.Size(97, 20);
            this.Point_Group_TxtBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.Location = new System.Drawing.Point(168, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name Style:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label7.Location = new System.Drawing.Point(12, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Section From File:";
            // 
            // SectionFromFile_TxtBox
            // 
            this.SectionFromFile_TxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SectionFromFile_TxtBox.BackColor = System.Drawing.SystemColors.Control;
            this.SectionFromFile_TxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SectionFromFile_TxtBox.Location = new System.Drawing.Point(12, 51);
            this.SectionFromFile_TxtBox.MaxLength = 30;
            this.SectionFromFile_TxtBox.Multiline = true;
            this.SectionFromFile_TxtBox.Name = "SectionFromFile_TxtBox";
            this.SectionFromFile_TxtBox.ReadOnly = true;
            this.SectionFromFile_TxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SectionFromFile_TxtBox.Size = new System.Drawing.Size(148, 146);
            this.SectionFromFile_TxtBox.TabIndex = 10;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(166, 48);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(97, 21);
            this.comboBox1.TabIndex = 19;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.Location = new System.Drawing.Point(168, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "Select Alignment";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgBar,
            this.TimeElapseStripStatus,
            this.ErrorNOStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 200);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(301, 22);
            this.statusStrip1.TabIndex = 34;
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
            this.TimeElapseStripStatus.Size = new System.Drawing.Size(124, 17);
            this.TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00";
            // 
            // ErrorNOStripStatus
            // 
            this.ErrorNOStripStatus.Name = "ErrorNOStripStatus";
            this.ErrorNOStripStatus.Size = new System.Drawing.Size(49, 17);
            this.ErrorNOStripStatus.Text = "Errors: 0";
            // 
            // chkSLG
            // 
            this.chkSLG.AutoSize = true;
            this.chkSLG.Location = new System.Drawing.Point(166, 116);
            this.chkSLG.Name = "chkSLG";
            this.chkSLG.Size = new System.Drawing.Size(124, 17);
            this.chkSLG.TabIndex = 35;
            this.chkSLG.Text = "Create Sample Line?";
            this.chkSLG.UseVisualStyleBackColor = true;
            // 
            // chkpo
            // 
            this.chkpo.AutoSize = true;
            this.chkpo.Location = new System.Drawing.Point(166, 139);
            this.chkpo.Name = "chkpo";
            this.chkpo.Size = new System.Drawing.Size(95, 17);
            this.chkpo.TabIndex = 36;
            this.chkpo.Text = "Create Points?";
            this.chkpo.UseVisualStyleBackColor = true;
            // 
            // CreateSectionFromFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(301, 222);
            this.Controls.Add(this.chkpo);
            this.Controls.Add(this.chkSLG);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.SectionFromFile_TxtBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Point_Group_TxtBox);
            this.Controls.Add(this.Create_section_BTN);
            this.Controls.Add(this.Select_Alignment_BTN);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(317, 242);
            this.Name = "CreateSectionFromFile";
            this.Text = "Create Section From File Ver 1.2";
            this.Load += new System.EventHandler(this.CreateSectionFromFile_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button Select_Alignment_BTN;
        private System.Windows.Forms.Button Create_section_BTN;
        private System.Windows.Forms.TextBox Point_Group_TxtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox SectionFromFile_TxtBox;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProgBar;
        private System.Windows.Forms.ToolStripStatusLabel TimeElapseStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel ErrorNOStripStatus;
        private System.Windows.Forms.CheckBox chkSLG;
        private System.Windows.Forms.CheckBox chkpo;
    }
}