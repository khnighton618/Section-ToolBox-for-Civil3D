namespace SectionToolBox
{
    partial class Flow_Path
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
            this.labelBlkName = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BlockSelect_BTN = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LSboxProf = new System.Windows.Forms.ListBox();
            this.IntervalTXT = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TextOff = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AllalgCHK = new System.Windows.Forms.CheckBox();
            this.BTN_Create = new System.Windows.Forms.Button();
            this.Select_Alignment_BTN = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelBlkName
            // 
            this.labelBlkName.AutoSize = true;
            this.labelBlkName.Location = new System.Drawing.Point(91, 65);
            this.labelBlkName.Name = "labelBlkName";
            this.labelBlkName.Size = new System.Drawing.Size(0, 13);
            this.labelBlkName.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Block Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Select Sample Block:";
            // 
            // BlockSelect_BTN
            // 
            this.BlockSelect_BTN.Location = new System.Drawing.Point(117, 26);
            this.BlockSelect_BTN.Name = "BlockSelect_BTN";
            this.BlockSelect_BTN.Size = new System.Drawing.Size(75, 23);
            this.BlockSelect_BTN.TabIndex = 2;
            this.BlockSelect_BTN.Text = "Select Block";
            this.BlockSelect_BTN.UseVisualStyleBackColor = true;
            this.BlockSelect_BTN.Click += new System.EventHandler(this.BlockSelect_BTN_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelBlkName);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.BlockSelect_BTN);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(203, 90);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Block Properties";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.Location = new System.Drawing.Point(11, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "Select Alignment";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(9, 56);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(158, 21);
            this.comboBox1.TabIndex = 22;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(11, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Select Profile:";
            // 
            // LSboxProf
            // 
            this.LSboxProf.FormattingEnabled = true;
            this.LSboxProf.Location = new System.Drawing.Point(12, 108);
            this.LSboxProf.Name = "LSboxProf";
            this.LSboxProf.Size = new System.Drawing.Size(155, 56);
            this.LSboxProf.TabIndex = 24;
            this.LSboxProf.SelectedIndexChanged += new System.EventHandler(this.LSboxProf_SelectedIndexChanged);
            // 
            // IntervalTXT
            // 
            this.IntervalTXT.Location = new System.Drawing.Point(62, 177);
            this.IntervalTXT.Name = "IntervalTXT";
            this.IntervalTXT.Size = new System.Drawing.Size(74, 20);
            this.IntervalTXT.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 180);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Interval:";
            // 
            // TextOff
            // 
            this.TextOff.Location = new System.Drawing.Point(63, 206);
            this.TextOff.Name = "TextOff";
            this.TextOff.Size = new System.Drawing.Size(74, 20);
            this.TextOff.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Offset:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AllalgCHK);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.TextOff);
            this.groupBox1.Controls.Add(this.Select_Alignment_BTN);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.IntervalTXT);
            this.groupBox1.Controls.Add(this.LSboxProf);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 239);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alignment Properties";
            // 
            // AllalgCHK
            // 
            this.AllalgCHK.AutoSize = true;
            this.AllalgCHK.Checked = true;
            this.AllalgCHK.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllalgCHK.Location = new System.Drawing.Point(15, 20);
            this.AllalgCHK.Name = "AllalgCHK";
            this.AllalgCHK.Size = new System.Drawing.Size(131, 17);
            this.AllalgCHK.TabIndex = 30;
            this.AllalgCHK.Text = "Select all  alignments?";
            this.AllalgCHK.UseVisualStyleBackColor = true;
            this.AllalgCHK.CheckedChanged += new System.EventHandler(this.AllalgCHK_CheckedChanged);
            // 
            // BTN_Create
            // 
            this.BTN_Create.Location = new System.Drawing.Point(73, 363);
            this.BTN_Create.Name = "BTN_Create";
            this.BTN_Create.Size = new System.Drawing.Size(75, 23);
            this.BTN_Create.TabIndex = 31;
            this.BTN_Create.Text = "Create";
            this.BTN_Create.UseVisualStyleBackColor = true;
            this.BTN_Create.Click += new System.EventHandler(this.BTN_Create_Click);
            // 
            // Select_Alignment_BTN
            // 
            this.Select_Alignment_BTN.Image = global::SectionToolBox.Properties.Resources.Untitled_5;
            this.Select_Alignment_BTN.Location = new System.Drawing.Point(172, 51);
            this.Select_Alignment_BTN.Name = "Select_Alignment_BTN";
            this.Select_Alignment_BTN.Size = new System.Drawing.Size(25, 28);
            this.Select_Alignment_BTN.TabIndex = 21;
            this.Select_Alignment_BTN.UseVisualStyleBackColor = true;
            this.Select_Alignment_BTN.Click += new System.EventHandler(this.Select_Alignment_BTN_Click);
            // 
            // Flow_Path
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 397);
            this.Controls.Add(this.BTN_Create);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Flow_Path";
            this.Text = "Flow_Path";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBlkName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BlockSelect_BTN;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Select_Alignment_BTN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox LSboxProf;
        private System.Windows.Forms.TextBox IntervalTXT;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextOff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BTN_Create;
        private System.Windows.Forms.CheckBox AllalgCHK;
    }
}