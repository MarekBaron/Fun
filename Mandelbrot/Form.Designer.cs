﻿namespace Baron.Mandelbrot
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblGenerationTime = new System.Windows.Forms.Label();
            this.cbxGenerator = new System.Windows.Forms.ComboBox();
            this.lblGenerator = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(907, 525);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // lblGenerationTime
            // 
            this.lblGenerationTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGenerationTime.AutoSize = true;
            this.lblGenerationTime.Location = new System.Drawing.Point(884, 551);
            this.lblGenerationTime.Name = "lblGenerationTime";
            this.lblGenerationTime.Size = new System.Drawing.Size(35, 13);
            this.lblGenerationTime.TabIndex = 1;
            this.lblGenerationTime.Text = "label1";
            this.lblGenerationTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxGenerator
            // 
            this.cbxGenerator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxGenerator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGenerator.FormattingEnabled = true;
            this.cbxGenerator.Location = new System.Drawing.Point(72, 543);
            this.cbxGenerator.Name = "cbxGenerator";
            this.cbxGenerator.Size = new System.Drawing.Size(351, 21);
            this.cbxGenerator.TabIndex = 2;
            this.cbxGenerator.SelectedIndexChanged += new System.EventHandler(this.cbxGenerator_SelectedIndexChanged);
            // 
            // lblGenerator
            // 
            this.lblGenerator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblGenerator.AutoSize = true;
            this.lblGenerator.Location = new System.Drawing.Point(12, 546);
            this.lblGenerator.Name = "lblGenerator";
            this.lblGenerator.Size = new System.Drawing.Size(54, 13);
            this.lblGenerator.TabIndex = 3;
            this.lblGenerator.Text = "Generator";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 573);
            this.Controls.Add(this.lblGenerator);
            this.Controls.Add(this.cbxGenerator);
            this.Controls.Add(this.lblGenerationTime);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseWheel);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblGenerationTime;
        private System.Windows.Forms.ComboBox cbxGenerator;
        private System.Windows.Forms.Label lblGenerator;
    }
}

