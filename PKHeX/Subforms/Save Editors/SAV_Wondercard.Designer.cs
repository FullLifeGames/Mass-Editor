﻿namespace PKHeX
{
    partial class SAV_Wondercard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SAV_Wondercard));
            this.B_Save = new System.Windows.Forms.Button();
            this.B_Cancel = new System.Windows.Forms.Button();
            this.B_Output = new System.Windows.Forms.Button();
            this.B_Import = new System.Windows.Forms.Button();
            this.LB_Received = new System.Windows.Forms.ListBox();
            this.mnuDel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.flagDel = new System.Windows.Forms.ToolStripMenuItem();
            this.L_Received = new System.Windows.Forms.Label();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.L_Details = new System.Windows.Forms.Label();
            this.L_QR = new System.Windows.Forms.Label();
            this.PB_Preview = new System.Windows.Forms.PictureBox();
            this.mnuVSD = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.FLP_Gifts = new System.Windows.Forms.FlowLayoutPanel();
            this.mnuDel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            this.mnuVSD.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(391, 326);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 0;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // B_Cancel
            // 
            this.B_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Cancel.Location = new System.Drawing.Point(320, 326);
            this.B_Cancel.Name = "B_Cancel";
            this.B_Cancel.Size = new System.Drawing.Size(71, 23);
            this.B_Cancel.TabIndex = 1;
            this.B_Cancel.Text = "Cancel";
            this.B_Cancel.UseVisualStyleBackColor = true;
            this.B_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // B_Output
            // 
            this.B_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Output.Location = new System.Drawing.Point(391, 12);
            this.B_Output.Name = "B_Output";
            this.B_Output.Size = new System.Drawing.Size(75, 23);
            this.B_Output.TabIndex = 2;
            this.B_Output.Text = "Output";
            this.B_Output.UseVisualStyleBackColor = true;
            this.B_Output.Click += new System.EventHandler(this.B_Output_Click);
            // 
            // B_Import
            // 
            this.B_Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Import.Location = new System.Drawing.Point(391, 34);
            this.B_Import.Name = "B_Import";
            this.B_Import.Size = new System.Drawing.Size(75, 23);
            this.B_Import.TabIndex = 3;
            this.B_Import.Text = "Import";
            this.B_Import.UseVisualStyleBackColor = true;
            this.B_Import.Click += new System.EventHandler(this.B_Import_Click);
            // 
            // LB_Received
            // 
            this.LB_Received.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_Received.ContextMenuStrip = this.mnuDel;
            this.LB_Received.FormattingEnabled = true;
            this.LB_Received.Location = new System.Drawing.Point(12, 31);
            this.LB_Received.Name = "LB_Received";
            this.LB_Received.Size = new System.Drawing.Size(78, 316);
            this.LB_Received.Sorted = true;
            this.LB_Received.TabIndex = 5;
            // 
            // mnuDel
            // 
            this.mnuDel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flagDel});
            this.mnuDel.Name = "mnuVSD";
            this.mnuDel.Size = new System.Drawing.Size(108, 26);
            // 
            // flagDel
            // 
            this.flagDel.Name = "flagDel";
            this.flagDel.Size = new System.Drawing.Size(107, 22);
            this.flagDel.Text = "Delete";
            this.flagDel.Click += new System.EventHandler(this.clearRecievedFlag);
            // 
            // L_Received
            // 
            this.L_Received.AutoSize = true;
            this.L_Received.Location = new System.Drawing.Point(12, 13);
            this.L_Received.Name = "L_Received";
            this.L_Received.Size = new System.Drawing.Size(75, 13);
            this.L_Received.TabIndex = 7;
            this.L_Received.Text = "Received List:";
            // 
            // RTB
            // 
            this.RTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB.Location = new System.Drawing.Point(111, 59);
            this.RTB.Name = "RTB";
            this.RTB.ReadOnly = true;
            this.RTB.Size = new System.Drawing.Size(355, 100);
            this.RTB.TabIndex = 10;
            this.RTB.Text = "";
            // 
            // L_Details
            // 
            this.L_Details.AutoSize = true;
            this.L_Details.Location = new System.Drawing.Point(108, 43);
            this.L_Details.Name = "L_Details";
            this.L_Details.Size = new System.Drawing.Size(42, 13);
            this.L_Details.TabIndex = 11;
            this.L_Details.Text = "Details:";
            // 
            // L_QR
            // 
            this.L_QR.AutoSize = true;
            this.L_QR.Location = new System.Drawing.Point(249, 7);
            this.L_QR.Name = "L_QR";
            this.L_QR.Size = new System.Drawing.Size(26, 13);
            this.L_QR.TabIndex = 62;
            this.L_QR.Text = "QR!";
            this.L_QR.Click += new System.EventHandler(this.L_QR_Click);
            // 
            // PB_Preview
            // 
            this.PB_Preview.BackColor = System.Drawing.Color.Transparent;
            this.PB_Preview.Location = new System.Drawing.Point(242, 23);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(40, 30);
            this.PB_Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PB_Preview.TabIndex = 63;
            this.PB_Preview.TabStop = false;
            // 
            // mnuVSD
            // 
            this.mnuVSD.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuView,
            this.mnuSet,
            this.mnuDelete});
            this.mnuVSD.Name = "mnuVSD";
            this.mnuVSD.Size = new System.Drawing.Size(108, 70);
            // 
            // mnuView
            // 
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(107, 22);
            this.mnuView.Text = "View";
            this.mnuView.Click += new System.EventHandler(this.clickView);
            // 
            // mnuSet
            // 
            this.mnuSet.Name = "mnuSet";
            this.mnuSet.Size = new System.Drawing.Size(107, 22);
            this.mnuSet.Text = "Set";
            this.mnuSet.Click += new System.EventHandler(this.clickSet);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(107, 22);
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.clickDelete);
            // 
            // FLP_Gifts
            // 
            this.FLP_Gifts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_Gifts.AutoScroll = true;
            this.FLP_Gifts.Location = new System.Drawing.Point(111, 164);
            this.FLP_Gifts.Name = "FLP_Gifts";
            this.FLP_Gifts.Size = new System.Drawing.Size(355, 153);
            this.FLP_Gifts.TabIndex = 92;
            // 
            // SAV_Wondercard
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.FLP_Gifts);
            this.Controls.Add(this.PB_Preview);
            this.Controls.Add(this.L_QR);
            this.Controls.Add(this.L_Details);
            this.Controls.Add(this.RTB);
            this.Controls.Add(this.L_Received);
            this.Controls.Add(this.LB_Received);
            this.Controls.Add(this.B_Import);
            this.Controls.Add(this.B_Output);
            this.Controls.Add(this.B_Cancel);
            this.Controls.Add(this.B_Save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "SAV_Wondercard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wonder Card I/O";
            this.mnuDel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            this.mnuVSD.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.Button B_Cancel;
        private System.Windows.Forms.Button B_Output;
        private System.Windows.Forms.Button B_Import;
        private System.Windows.Forms.ListBox LB_Received;
        private System.Windows.Forms.Label L_Received;
        private System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.Label L_Details;
        private System.Windows.Forms.Label L_QR;
        private System.Windows.Forms.PictureBox PB_Preview;
        private System.Windows.Forms.ContextMenuStrip mnuVSD;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuSet;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ContextMenuStrip mnuDel;
        private System.Windows.Forms.ToolStripMenuItem flagDel;
        private System.Windows.Forms.FlowLayoutPanel FLP_Gifts;
    }
}