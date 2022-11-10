namespace Sections
{
    partial class SectionViewEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DVG1 = new System.Windows.Forms.DataGridView();
            this.SectionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Station = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MinZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeftOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RightOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LS_Align = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LS_SLG = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DrawBTN = new System.Windows.Forms.Button();
            this.TXTA = new System.Windows.Forms.TextBox();
            this.textRow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LS_SVG = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.TXTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TXTC = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TXTD = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TXTE = new System.Windows.Forms.TextBox();
            this.chkbox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TXTCol = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TXTSpace = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.TimeElapseStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ErrorNOStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MinMaxBTN = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.MinZBox = new System.Windows.Forms.TextBox();
            this.MaxZBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.DVG1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DVG1
            // 
            this.DVG1.AllowDrop = true;
            this.DVG1.AllowUserToAddRows = false;
            this.DVG1.AllowUserToDeleteRows = false;
            this.DVG1.AllowUserToOrderColumns = true;
            this.DVG1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Format = "N3";
            dataGridViewCellStyle1.NullValue = null;
            this.DVG1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DVG1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DVG1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DVG1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DVG1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DVG1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.DVG1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DVG1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SectionName,
            this.Station,
            this.MinZ,
            this.MaxZ,
            this.LeftOffset,
            this.RightOffset});
            this.DVG1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Format = "N3";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DVG1.DefaultCellStyle = dataGridViewCellStyle2;
            this.DVG1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DVG1.Location = new System.Drawing.Point(12, 187);
            this.DVG1.Name = "DVG1";
            this.DVG1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DVG1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DVG1.Size = new System.Drawing.Size(569, 235);
            this.DVG1.TabIndex = 0;
            this.DVG1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DVG1_CellContentClick);
            this.DVG1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DVG1_CellValueChanged);
            this.DVG1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DVG1_KeyDown);
            // 
            // SectionName
            // 
            this.SectionName.HeaderText = "Section Name";
            this.SectionName.Name = "SectionName";
            this.SectionName.ReadOnly = true;
            this.SectionName.Width = 99;
            // 
            // Station
            // 
            this.Station.HeaderText = "Station";
            this.Station.Name = "Station";
            this.Station.ReadOnly = true;
            this.Station.Width = 65;
            // 
            // MinZ
            // 
            this.MinZ.HeaderText = "Minimum Z";
            this.MinZ.Name = "MinZ";
            this.MinZ.Width = 83;
            // 
            // MaxZ
            // 
            this.MaxZ.HeaderText = "Maximum Z";
            this.MaxZ.Name = "MaxZ";
            this.MaxZ.Width = 86;
            // 
            // LeftOffset
            // 
            this.LeftOffset.HeaderText = "Left Offset";
            this.LeftOffset.Name = "LeftOffset";
            this.LeftOffset.Width = 81;
            // 
            // RightOffset
            // 
            this.RightOffset.HeaderText = "Right Offset";
            this.RightOffset.Name = "RightOffset";
            this.RightOffset.Width = 88;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // LS_Align
            // 
            this.LS_Align.FormattingEnabled = true;
            this.LS_Align.Location = new System.Drawing.Point(12, 47);
            this.LS_Align.Name = "LS_Align";
            this.LS_Align.Size = new System.Drawing.Size(120, 56);
            this.LS_Align.TabIndex = 17;
            this.LS_Align.SelectedIndexChanged += new System.EventHandler(this.LS_Align_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Alignment:";
            // 
            // LS_SLG
            // 
            this.LS_SLG.FormattingEnabled = true;
            this.LS_SLG.Location = new System.Drawing.Point(138, 47);
            this.LS_SLG.Name = "LS_SLG";
            this.LS_SLG.Size = new System.Drawing.Size(117, 56);
            this.LS_SLG.TabIndex = 19;
            this.LS_SLG.SelectedIndexChanged += new System.EventHandler(this.LS_SLG_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Sample Line Group:";
            // 
            // DrawBTN
            // 
            this.DrawBTN.Location = new System.Drawing.Point(109, 112);
            this.DrawBTN.Name = "DrawBTN";
            this.DrawBTN.Size = new System.Drawing.Size(75, 23);
            this.DrawBTN.TabIndex = 8;
            this.DrawBTN.Text = "Draw";
            this.DrawBTN.UseVisualStyleBackColor = true;
            this.DrawBTN.Click += new System.EventHandler(this.DrawBTN_Click);
            // 
            // TXTA
            // 
            this.TXTA.Location = new System.Drawing.Point(114, 30);
            this.TXTA.Name = "TXTA";
            this.TXTA.Size = new System.Drawing.Size(26, 20);
            this.TXTA.TabIndex = 2;
            // 
            // textRow
            // 
            this.textRow.Location = new System.Drawing.Point(61, 30);
            this.textRow.Name = "textRow";
            this.textRow.Size = new System.Drawing.Size(26, 20);
            this.textRow.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "A=";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Row=";
            // 
            // LS_SVG
            // 
            this.LS_SVG.FormattingEnabled = true;
            this.LS_SVG.Location = new System.Drawing.Point(261, 47);
            this.LS_SVG.Name = "LS_SVG";
            this.LS_SVG.Size = new System.Drawing.Size(117, 56);
            this.LS_SVG.TabIndex = 21;
            this.LS_SVG.SelectedIndexChanged += new System.EventHandler(this.LS_SVG_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(261, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Section View Group:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(824, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
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
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(92, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "B=";
            // 
            // TXTB
            // 
            this.TXTB.Location = new System.Drawing.Point(114, 59);
            this.TXTB.Name = "TXTB";
            this.TXTB.Size = new System.Drawing.Size(26, 20);
            this.TXTB.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(142, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "C=";
            // 
            // TXTC
            // 
            this.TXTC.Location = new System.Drawing.Point(164, 30);
            this.TXTC.Name = "TXTC";
            this.TXTC.Size = new System.Drawing.Size(26, 20);
            this.TXTC.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(143, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "D=";
            // 
            // TXTD
            // 
            this.TXTD.Location = new System.Drawing.Point(164, 59);
            this.TXTD.Name = "TXTD";
            this.TXTD.Size = new System.Drawing.Size(26, 20);
            this.TXTD.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "E=";
            // 
            // TXTE
            // 
            this.TXTE.Location = new System.Drawing.Point(61, 86);
            this.TXTE.Name = "TXTE";
            this.TXTE.Size = new System.Drawing.Size(26, 20);
            this.TXTE.TabIndex = 6;
            // 
            // chkbox
            // 
            this.chkbox.AutoSize = true;
            this.chkbox.Location = new System.Drawing.Point(121, 89);
            this.chkbox.Name = "chkbox";
            this.chkbox.Size = new System.Drawing.Size(63, 17);
            this.chkbox.TabIndex = 7;
            this.chkbox.Text = "By Row";
            this.chkbox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Col=";
            // 
            // TXTCol
            // 
            this.TXTCol.Location = new System.Drawing.Point(61, 60);
            this.TXTCol.Name = "TXTCol";
            this.TXTCol.Size = new System.Drawing.Size(26, 20);
            this.TXTCol.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Space=";
            // 
            // TXTSpace
            // 
            this.TXTSpace.Location = new System.Drawing.Point(61, 115);
            this.TXTSpace.Name = "TXTSpace";
            this.TXTSpace.Size = new System.Drawing.Size(26, 20);
            this.TXTSpace.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 171);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Section View Editor";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgBar,
            this.TimeElapseStripStatus,
            this.ErrorNOStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 425);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(824, 22);
            this.statusStrip1.TabIndex = 29;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.DrawBTN);
            this.groupBox1.Controls.Add(this.TXTSpace);
            this.groupBox1.Controls.Add(this.TXTA);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textRow);
            this.groupBox1.Controls.Add(this.TXTCol);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkbox);
            this.groupBox1.Controls.Add(this.TXTB);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.TXTE);
            this.groupBox1.Controls.Add(this.TXTC);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.TXTD);
            this.groupBox1.Location = new System.Drawing.Point(384, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 137);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sort Section Views:";
            // 
            // MinMaxBTN
            // 
            this.MinMaxBTN.Location = new System.Drawing.Point(6, 19);
            this.MinMaxBTN.Name = "MinMaxBTN";
            this.MinMaxBTN.Size = new System.Drawing.Size(117, 23);
            this.MinMaxBTN.TabIndex = 26;
            this.MinMaxBTN.Text = "Set to MinMax";
            this.MinMaxBTN.UseVisualStyleBackColor = true;
            this.MinMaxBTN.Click += new System.EventHandler(this.MinMaxBTN_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(129, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "Min Z:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(203, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Max Z:";
            // 
            // MinZBox
            // 
            this.MinZBox.Location = new System.Drawing.Point(170, 19);
            this.MinZBox.Name = "MinZBox";
            this.MinZBox.Size = new System.Drawing.Size(26, 20);
            this.MinZBox.TabIndex = 26;
            // 
            // MaxZBox
            // 
            this.MaxZBox.Location = new System.Drawing.Point(244, 19);
            this.MaxZBox.Name = "MaxZBox";
            this.MaxZBox.Size = new System.Drawing.Size(26, 20);
            this.MaxZBox.TabIndex = 28;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.MinMaxBTN);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.MaxZBox);
            this.groupBox2.Controls.Add(this.MinZBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(366, 51);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Set Minimum and Maximum Elevation:";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AllowDrop = true;
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(587, 38);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(229, 384);
            this.propertyGrid1.TabIndex = 32;
            // 
            // SectionViewEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 447);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.LS_SVG);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LS_Align);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LS_SLG);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DVG1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(840, 486);
            this.Name = "SectionViewEditor";
            this.Text = "SectionViewEditor Ver 1.1";
            ((System.ComponentModel.ISupportInitialize)(this.DVG1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DVG1;
        private System.Windows.Forms.ListBox LS_Align;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox LS_SLG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button DrawBTN;
        private System.Windows.Forms.TextBox TXTA;
        private System.Windows.Forms.TextBox textRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox LS_SVG;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TXTB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TXTC;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TXTD;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TXTE;
        private System.Windows.Forms.CheckBox chkbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TXTCol;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TXTSpace;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar ProgBar;
        private System.Windows.Forms.ToolStripStatusLabel TimeElapseStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel ErrorNOStripStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button MinMaxBTN;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox MinZBox;
        private System.Windows.Forms.TextBox MaxZBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Station;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeftOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn RightOffset;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}