namespace Sections
{
    partial class STR
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STR));
            this.label1 = new System.Windows.Forms.Label();
            this.LS_Cor = new System.Windows.Forms.ListBox();
            this.LS_EG = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LS_Final = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LS_SLG = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BTN_OK = new System.Windows.Forms.Button();
            this.LS_Align = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LeftETWWidth = new System.Windows.Forms.TextBox();
            this.RightETWWidth = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsXLSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.TimeElapseStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ErrorNOStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox_Codes = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.listBox_LinkCodes = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkDraw = new System.Windows.Forms.CheckBox();
            this.chkEGFG = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Corridor:";
            // 
            // LS_Cor
            // 
            this.LS_Cor.FormattingEnabled = true;
            this.LS_Cor.Location = new System.Drawing.Point(12, 51);
            this.LS_Cor.Name = "LS_Cor";
            this.LS_Cor.Size = new System.Drawing.Size(141, 43);
            this.LS_Cor.TabIndex = 1;
            this.LS_Cor.SelectedIndexChanged += new System.EventHandler(this.LS_Cor_SelectedIndexChanged);
            // 
            // LS_EG
            // 
            this.LS_EG.FormattingEnabled = true;
            this.LS_EG.Location = new System.Drawing.Point(291, 51);
            this.LS_EG.Name = "LS_EG";
            this.LS_EG.Size = new System.Drawing.Size(144, 43);
            this.LS_EG.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Existing Ground:";
            // 
            // LS_Final
            // 
            this.LS_Final.FormattingEnabled = true;
            this.LS_Final.Location = new System.Drawing.Point(12, 117);
            this.LS_Final.Name = "LS_Final";
            this.LS_Final.Size = new System.Drawing.Size(141, 43);
            this.LS_Final.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Corridor Surface:";
            // 
            // LS_SLG
            // 
            this.LS_SLG.FormattingEnabled = true;
            this.LS_SLG.Location = new System.Drawing.Point(162, 117);
            this.LS_SLG.Name = "LS_SLG";
            this.LS_SLG.Size = new System.Drawing.Size(117, 43);
            this.LS_SLG.TabIndex = 5;
            this.LS_SLG.SelectedIndexChanged += new System.EventHandler(this.LS_SLG_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Sample Line Group:";
            // 
            // BTN_OK
            // 
            this.BTN_OK.Location = new System.Drawing.Point(177, 218);
            this.BTN_OK.Name = "BTN_OK";
            this.BTN_OK.Size = new System.Drawing.Size(75, 23);
            this.BTN_OK.TabIndex = 10;
            this.BTN_OK.Text = "Calculate";
            this.BTN_OK.UseVisualStyleBackColor = true;
            this.BTN_OK.Click += new System.EventHandler(this.BTN_OK_Click);
            // 
            // LS_Align
            // 
            this.LS_Align.FormattingEnabled = true;
            this.LS_Align.Location = new System.Drawing.Point(159, 51);
            this.LS_Align.Name = "LS_Align";
            this.LS_Align.Size = new System.Drawing.Size(120, 43);
            this.LS_Align.TabIndex = 2;
            this.LS_Align.SelectedIndexChanged += new System.EventHandler(this.LS_Align_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Alignment:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Left ETW Width:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Right ETW Width:";
            // 
            // LeftETWWidth
            // 
            this.LeftETWWidth.Location = new System.Drawing.Point(101, 171);
            this.LeftETWWidth.Name = "LeftETWWidth";
            this.LeftETWWidth.Size = new System.Drawing.Size(43, 20);
            this.LeftETWWidth.TabIndex = 8;
            this.LeftETWWidth.TextChanged += new System.EventHandler(this.LeftETWWidth_TextChanged);
            // 
            // RightETWWidth
            // 
            this.RightETWWidth.Location = new System.Drawing.Point(101, 221);
            this.RightETWWidth.Name = "RightETWWidth";
            this.RightETWWidth.Size = new System.Drawing.Size(43, 20);
            this.RightETWWidth.TabIndex = 9;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(9, 199);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Symmetric";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(582, 24);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsXLSToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveAsXLSToolStripMenuItem
            // 
            this.saveAsXLSToolStripMenuItem.Name = "saveAsXLSToolStripMenuItem";
            this.saveAsXLSToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.saveAsXLSToolStripMenuItem.Text = "Save as XLS";
            this.saveAsXLSToolStripMenuItem.Click += new System.EventHandler(this.saveAsXLSToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.saveToolStripMenuItem.Text = "Save Text File";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgBar,
            this.TimeElapseStripStatus,
            this.ErrorNOStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 250);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(582, 22);
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
            // listBox_Codes
            // 
            this.listBox_Codes.FormattingEnabled = true;
            this.listBox_Codes.Location = new System.Drawing.Point(291, 117);
            this.listBox_Codes.Name = "listBox_Codes";
            this.listBox_Codes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_Codes.Size = new System.Drawing.Size(144, 121);
            this.listBox_Codes.TabIndex = 6;
            this.listBox_Codes.SelectedIndexChanged += new System.EventHandler(this.ListBox_Codes_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(288, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Point Code for ETW width:";
            // 
            // listBox_LinkCodes
            // 
            this.listBox_LinkCodes.FormattingEnabled = true;
            this.listBox_LinkCodes.Location = new System.Drawing.Point(441, 51);
            this.listBox_LinkCodes.Name = "listBox_LinkCodes";
            this.listBox_LinkCodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_LinkCodes.Size = new System.Drawing.Size(135, 186);
            this.listBox_LinkCodes.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(440, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(138, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Link Code for T Calculation:";
            // 
            // chkDraw
            // 
            this.chkDraw.AutoSize = true;
            this.chkDraw.Checked = true;
            this.chkDraw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDraw.Location = new System.Drawing.Point(150, 198);
            this.chkDraw.Name = "chkDraw";
            this.chkDraw.Size = new System.Drawing.Size(128, 17);
            this.chkDraw.TabIndex = 38;
            this.chkDraw.Text = "Draw CC,CF,T Lines?";
            this.chkDraw.UseVisualStyleBackColor = true;
            // 
            // chkEGFG
            // 
            this.chkEGFG.AutoSize = true;
            this.chkEGFG.Location = new System.Drawing.Point(150, 175);
            this.chkEGFG.Name = "chkEGFG";
            this.chkEGFG.Size = new System.Drawing.Size(120, 17);
            this.chkEGFG.TabIndex = 39;
            this.chkEGFG.Text = "Draw EG,FG Lines?";
            this.chkEGFG.UseVisualStyleBackColor = true;
            // 
            // STR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(582, 272);
            this.Controls.Add(this.chkEGFG);
            this.Controls.Add(this.chkDraw);
            this.Controls.Add(this.listBox_LinkCodes);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.listBox_Codes);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.RightETWWidth);
            this.Controls.Add(this.LeftETWWidth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LS_Align);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BTN_OK);
            this.Controls.Add(this.LS_SLG);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LS_Final);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LS_EG);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LS_Cor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(598, 311);
            this.Name = "STR";
            this.Text = " Stripping Length Ver 1.3.5";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox LS_Cor;
        private System.Windows.Forms.ListBox LS_EG;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox LS_Final;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox LS_SLG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BTN_OK;
        private System.Windows.Forms.ListBox LS_Align;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox LeftETWWidth;
        private System.Windows.Forms.TextBox RightETWWidth;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProgBar;
        private System.Windows.Forms.ToolStripStatusLabel TimeElapseStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel ErrorNOStripStatus;
        private System.Windows.Forms.ListBox listBox_Codes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox listBox_LinkCodes;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkDraw;
        private System.Windows.Forms.CheckBox chkEGFG;
        private System.Windows.Forms.ToolStripMenuItem saveAsXLSToolStripMenuItem;
    }
}