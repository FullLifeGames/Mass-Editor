﻿namespace PKHeX
{
    partial class BatchEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchEditor));
            this.RB_SAV = new System.Windows.Forms.RadioButton();
            this.RB_Path = new System.Windows.Forms.RadioButton();
            this.FLP_RB = new System.Windows.Forms.FlowLayoutPanel();
            this.TB_Folder = new System.Windows.Forms.TextBox();
            this.RTB_Instructions = new System.Windows.Forms.RichTextBox();
            this.B_Go = new System.Windows.Forms.Button();
            this.PB_Show = new System.Windows.Forms.ProgressBar();
            this.CB_Format = new System.Windows.Forms.ComboBox();
            this.CB_Property = new System.Windows.Forms.ComboBox();
            this.CB_Require = new System.Windows.Forms.ComboBox();
            this.B_Add = new System.Windows.Forms.Button();
            this.FLP_RB.SuspendLayout();
            this.SuspendLayout();
            // 
            // RB_SAV
            // 
            this.RB_SAV.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RB_SAV.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_SAV.AutoSize = true;
            this.RB_SAV.Checked = true;
            this.RB_SAV.Location = new System.Drawing.Point(0, 0);
            this.RB_SAV.Margin = new System.Windows.Forms.Padding(0);
            this.RB_SAV.Name = "RB_SAV";
            this.RB_SAV.Size = new System.Drawing.Size(61, 23);
            this.RB_SAV.TabIndex = 0;
            this.RB_SAV.TabStop = true;
            this.RB_SAV.Text = "Save File";
            this.RB_SAV.UseVisualStyleBackColor = true;
            this.RB_SAV.Click += new System.EventHandler(this.B_SAV_Click);
            // 
            // RB_Path
            // 
            this.RB_Path.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RB_Path.Appearance = System.Windows.Forms.Appearance.Button;
            this.RB_Path.AutoSize = true;
            this.RB_Path.Location = new System.Drawing.Point(61, 0);
            this.RB_Path.Margin = new System.Windows.Forms.Padding(0);
            this.RB_Path.Name = "RB_Path";
            this.RB_Path.Size = new System.Drawing.Size(55, 23);
            this.RB_Path.TabIndex = 1;
            this.RB_Path.Text = "Folder...";
            this.RB_Path.UseVisualStyleBackColor = true;
            this.RB_Path.Click += new System.EventHandler(this.B_Open_Click);
            // 
            // FLP_RB
            // 
            this.FLP_RB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_RB.Controls.Add(this.RB_SAV);
            this.FLP_RB.Controls.Add(this.RB_Path);
            this.FLP_RB.Controls.Add(this.TB_Folder);
            this.FLP_RB.Location = new System.Drawing.Point(12, 10);
            this.FLP_RB.Name = "FLP_RB";
            this.FLP_RB.Size = new System.Drawing.Size(370, 24);
            this.FLP_RB.TabIndex = 2;
            // 
            // TB_Folder
            // 
            this.TB_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Folder.Location = new System.Drawing.Point(118, 2);
            this.TB_Folder.Margin = new System.Windows.Forms.Padding(2);
            this.TB_Folder.Name = "TB_Folder";
            this.TB_Folder.ReadOnly = true;
            this.TB_Folder.Size = new System.Drawing.Size(250, 20);
            this.TB_Folder.TabIndex = 4;
            this.TB_Folder.Visible = false;
            // 
            // RTB_Instructions
            // 
            this.RTB_Instructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTB_Instructions.Location = new System.Drawing.Point(12, 68);
            this.RTB_Instructions.Name = "RTB_Instructions";
            this.RTB_Instructions.Size = new System.Drawing.Size(370, 157);
            this.RTB_Instructions.TabIndex = 5;
            this.RTB_Instructions.Text = "";
            // 
            // B_Go
            // 
            this.B_Go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Go.Location = new System.Drawing.Point(325, 230);
            this.B_Go.Name = "B_Go";
            this.B_Go.Size = new System.Drawing.Size(57, 23);
            this.B_Go.TabIndex = 6;
            this.B_Go.Text = "Run";
            this.B_Go.UseVisualStyleBackColor = true;
            this.B_Go.Click += new System.EventHandler(this.B_Go_Click);
            // 
            // PB_Show
            // 
            this.PB_Show.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_Show.Location = new System.Drawing.Point(12, 231);
            this.PB_Show.Name = "PB_Show";
            this.PB_Show.Size = new System.Drawing.Size(307, 21);
            this.PB_Show.TabIndex = 7;
            // 
            // CB_Format
            // 
            this.CB_Format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Format.FormattingEnabled = true;
            this.CB_Format.Items.AddRange(new object[] {
            "All",
            "pk7",
            "pk6",
            "pk5",
            "pk4",
            "pk3",
            "Any"});
            this.CB_Format.Location = new System.Drawing.Point(11, 40);
            this.CB_Format.Name = "CB_Format";
            this.CB_Format.Size = new System.Drawing.Size(44, 21);
            this.CB_Format.TabIndex = 8;
            this.CB_Format.SelectedIndexChanged += new System.EventHandler(this.CB_Format_SelectedIndexChanged);
            // 
            // CB_Property
            // 
            this.CB_Property.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Property.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_Property.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Property.DropDownWidth = 200;
            this.CB_Property.FormattingEnabled = true;
            this.CB_Property.Location = new System.Drawing.Point(61, 40);
            this.CB_Property.Name = "CB_Property";
            this.CB_Property.Size = new System.Drawing.Size(140, 21);
            this.CB_Property.TabIndex = 9;
            // 
            // CB_Require
            // 
            this.CB_Require.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Require.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Require.FormattingEnabled = true;
            this.CB_Require.Items.AddRange(new object[] {
            "Set Equal To",
            "Require Equals",
            "Require Not Equals"});
            this.CB_Require.Location = new System.Drawing.Point(207, 40);
            this.CB_Require.Name = "CB_Require";
            this.CB_Require.Size = new System.Drawing.Size(111, 21);
            this.CB_Require.TabIndex = 10;
            // 
            // B_Add
            // 
            this.B_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Add.Location = new System.Drawing.Point(324, 39);
            this.B_Add.Name = "B_Add";
            this.B_Add.Size = new System.Drawing.Size(57, 23);
            this.B_Add.TabIndex = 11;
            this.B_Add.Text = "Add";
            this.B_Add.UseVisualStyleBackColor = true;
            this.B_Add.Click += new System.EventHandler(this.B_Add_Click);
            // 
            // BatchEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 261);
            this.Controls.Add(this.B_Add);
            this.Controls.Add(this.CB_Require);
            this.Controls.Add(this.CB_Property);
            this.Controls.Add(this.CB_Format);
            this.Controls.Add(this.PB_Show);
            this.Controls.Add(this.B_Go);
            this.Controls.Add(this.RTB_Instructions);
            this.Controls.Add(this.FLP_RB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(410, 300);
            this.Name = "BatchEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Editor";
            this.FLP_RB.ResumeLayout(false);
            this.FLP_RB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton RB_SAV;
        private System.Windows.Forms.RadioButton RB_Path;
        private System.Windows.Forms.FlowLayoutPanel FLP_RB;
        private System.Windows.Forms.TextBox TB_Folder;
        private System.Windows.Forms.RichTextBox RTB_Instructions;
        private System.Windows.Forms.Button B_Go;
        private System.Windows.Forms.ProgressBar PB_Show;
        private System.Windows.Forms.ComboBox CB_Format;
        private System.Windows.Forms.ComboBox CB_Property;
        private System.Windows.Forms.ComboBox CB_Require;
        private System.Windows.Forms.Button B_Add;
    }
}