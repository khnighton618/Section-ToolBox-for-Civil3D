namespace SectionToolBox
{
    partial class Subdividing
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
            this.SelPolyBTN = new System.Windows.Forms.Button();
            this.NObtn = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CreateBTN = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.AngleBTN = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.DesireArea = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.AreaRes = new System.Windows.Forms.TextBox();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelangle = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SelPolyBTN
            // 
            this.SelPolyBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.SelPolyBTN.Location = new System.Drawing.Point(171, 75);
            this.SelPolyBTN.Name = "SelPolyBTN";
            this.SelPolyBTN.Size = new System.Drawing.Size(75, 27);
            this.SelPolyBTN.TabIndex = 0;
            this.SelPolyBTN.Text = "Select";
            this.SelPolyBTN.UseVisualStyleBackColor = true;
            this.SelPolyBTN.Click += new System.EventHandler(this.SelPolyBTN_Click);
            // 
            // NObtn
            // 
            this.NObtn.Location = new System.Drawing.Point(171, 49);
            this.NObtn.Name = "NObtn";
            this.NObtn.Size = new System.Drawing.Size(76, 20);
            this.NObtn.TabIndex = 1;
            this.NObtn.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.Location = new System.Drawing.Point(14, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of Subdivision:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.Location = new System.Drawing.Point(63, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select Polyline";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label3.Location = new System.Drawing.Point(39, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Create Subdivision";
            // 
            // CreateBTN
            // 
            this.CreateBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.CreateBTN.Location = new System.Drawing.Point(171, 141);
            this.CreateBTN.Name = "CreateBTN";
            this.CreateBTN.Size = new System.Drawing.Size(75, 27);
            this.CreateBTN.TabIndex = 4;
            this.CreateBTN.Text = "Create";
            this.CreateBTN.UseVisualStyleBackColor = true;
            this.CreateBTN.Click += new System.EventHandler(this.CreateBTN_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label4.Location = new System.Drawing.Point(94, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Set Angle";
            // 
            // AngleBTN
            // 
            this.AngleBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.AngleBTN.Location = new System.Drawing.Point(171, 108);
            this.AngleBTN.Name = "AngleBTN";
            this.AngleBTN.Size = new System.Drawing.Size(75, 27);
            this.AngleBTN.TabIndex = 6;
            this.AngleBTN.Text = "Angle";
            this.AngleBTN.UseVisualStyleBackColor = true;
            this.AngleBTN.Click += new System.EventHandler(this.AngleBTN_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label5.Location = new System.Drawing.Point(77, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Desire Area:";
            // 
            // DesireArea
            // 
            this.DesireArea.Location = new System.Drawing.Point(171, 23);
            this.DesireArea.Name = "DesireArea";
            this.DesireArea.Size = new System.Drawing.Size(76, 20);
            this.DesireArea.TabIndex = 8;
            this.DesireArea.Text = "200";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label6.Location = new System.Drawing.Point(63, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Area Residual:";
            // 
            // AreaRes
            // 
            this.AreaRes.Location = new System.Drawing.Point(170, 174);
            this.AreaRes.Name = "AreaRes";
            this.AreaRes.Size = new System.Drawing.Size(76, 20);
            this.AreaRes.TabIndex = 11;
            this.AreaRes.Text = "1e-4";
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.labelArea.Location = new System.Drawing.Point(252, 84);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(90, 16);
            this.labelArea.TabIndex = 15;
            this.labelArea.Text = "Polyline Area:";
            // 
            // labelangle
            // 
            this.labelangle.AutoSize = true;
            this.labelangle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.labelangle.Location = new System.Drawing.Point(252, 114);
            this.labelangle.Name = "labelangle";
            this.labelangle.Size = new System.Drawing.Size(45, 16);
            this.labelangle.TabIndex = 16;
            this.labelangle.Text = "Angle:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label7.Location = new System.Drawing.Point(252, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 16);
            this.label7.TabIndex = 17;
            this.label7.Text = "Elapsed Time: 00:00:00 ";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(255, 51);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(87, 17);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Only Division";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Subdividing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 231);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelangle);
            this.Controls.Add(this.labelArea);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.AreaRes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DesireArea);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AngleBTN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CreateBTN);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NObtn);
            this.Controls.Add(this.SelPolyBTN);
            this.MinimumSize = new System.Drawing.Size(450, 270);
            this.Name = "Subdividing";
            this.Text = "Subdivision By E.Khalili Ver 1.0";
            this.MouseHover += new System.EventHandler(this.Subdividing_MouseHover);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelPolyBTN;
        private System.Windows.Forms.TextBox NObtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CreateBTN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AngleBTN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DesireArea;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox AreaRes;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelangle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}