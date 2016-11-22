﻿namespace PKHeX
{
    partial class QR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QR));
            this.PB_QR = new System.Windows.Forms.PictureBox();
            this.FontLabel = new System.Windows.Forms.Label();
            this.NUD_Box = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NUD_Slot = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.NUD_Copies = new System.Windows.Forms.NumericUpDown();
            this.B_Refresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PB_QR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Slot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Copies)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_QR
            // 
            this.PB_QR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_QR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PB_QR.Location = new System.Drawing.Point(2, 1);
            this.PB_QR.Name = "PB_QR";
            this.PB_QR.Size = new System.Drawing.Size(405, 455);
            this.PB_QR.TabIndex = 0;
            this.PB_QR.TabStop = false;
            this.PB_QR.Click += new System.EventHandler(this.PB_QR_Click);
            // 
            // FontLabel
            // 
            this.FontLabel.AutoSize = true;
            this.FontLabel.Location = new System.Drawing.Point(388, 393);
            this.FontLabel.Name = "FontLabel";
            this.FontLabel.Size = new System.Drawing.Size(19, 13);
            this.FontLabel.TabIndex = 1;
            this.FontLabel.Text = "<3";
            this.FontLabel.Visible = false;
            // 
            // NUD_Box
            // 
            this.NUD_Box.Location = new System.Drawing.Point(38, 465);
            this.NUD_Box.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.NUD_Box.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_Box.Name = "NUD_Box";
            this.NUD_Box.Size = new System.Drawing.Size(61, 20);
            this.NUD_Box.TabIndex = 2;
            this.NUD_Box.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 467);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Box:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 467);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Slot:";
            // 
            // NUD_Slot
            // 
            this.NUD_Slot.Location = new System.Drawing.Point(139, 465);
            this.NUD_Slot.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.NUD_Slot.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_Slot.Name = "NUD_Slot";
            this.NUD_Slot.Size = new System.Drawing.Size(61, 20);
            this.NUD_Slot.TabIndex = 4;
            this.NUD_Slot.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(211, 467);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Copies:";
            // 
            // NUD_Copies
            // 
            this.NUD_Copies.Location = new System.Drawing.Point(259, 465);
            this.NUD_Copies.Maximum = new decimal(new int[] {
            960,
            0,
            0,
            0});
            this.NUD_Copies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_Copies.Name = "NUD_Copies";
            this.NUD_Copies.Size = new System.Drawing.Size(52, 20);
            this.NUD_Copies.TabIndex = 6;
            this.NUD_Copies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // B_Refresh
            // 
            this.B_Refresh.Location = new System.Drawing.Point(317, 464);
            this.B_Refresh.Name = "B_Refresh";
            this.B_Refresh.Size = new System.Drawing.Size(80, 23);
            this.B_Refresh.TabIndex = 8;
            this.B_Refresh.Text = "Refresh";
            this.B_Refresh.UseVisualStyleBackColor = true;
            this.B_Refresh.Click += new System.EventHandler(this.updateBoxSlotCopies);
            // 
            // QR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 407);
            this.Controls.Add(this.B_Refresh);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NUD_Copies);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NUD_Slot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NUD_Box);
            this.Controls.Add(this.FontLabel);
            this.Controls.Add(this.PB_QR);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PKHeX QR Code (Click QR to Copy Image)";
            ((System.ComponentModel.ISupportInitialize)(this.PB_QR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Slot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Copies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_QR;
        private System.Windows.Forms.Label FontLabel;
        private System.Windows.Forms.NumericUpDown NUD_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NUD_Slot;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NUD_Copies;
        private System.Windows.Forms.Button B_Refresh;
    }
}