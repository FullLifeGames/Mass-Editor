﻿namespace Mass_Editor
{
    partial class MemoryAmie
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoryAmie));
            this.BTN_Save = new System.Windows.Forms.Button();
            this.BTN_Cancel = new System.Windows.Forms.Button();
            this.M_OT_Friendship = new System.Windows.Forms.MaskedTextBox();
            this.L_OT_Friendship = new System.Windows.Forms.Label();
            this.L_Geo0 = new System.Windows.Forms.Label();
            this.L_Geo1 = new System.Windows.Forms.Label();
            this.L_Region = new System.Windows.Forms.Label();
            this.L_Country = new System.Windows.Forms.Label();
            this.L_Geo2 = new System.Windows.Forms.Label();
            this.L_Geo3 = new System.Windows.Forms.Label();
            this.L_Geo4 = new System.Windows.Forms.Label();
            this.L_OT_Quality = new System.Windows.Forms.Label();
            this.L_OT_TextLine = new System.Windows.Forms.Label();
            this.LOTV = new System.Windows.Forms.Label();
            this.L_OT_Feeling = new System.Windows.Forms.Label();
            this.GB_M_OT = new System.Windows.Forms.GroupBox();
            this.RTB_OT = new System.Windows.Forms.RichTextBox();
            this.CB_OTVar = new System.Windows.Forms.ComboBox();
            this.CB_OTMemory = new System.Windows.Forms.ComboBox();
            this.CB_OTQual = new System.Windows.Forms.ComboBox();
            this.CB_OTFeel = new System.Windows.Forms.ComboBox();
            this.L_OT_Affection = new System.Windows.Forms.Label();
            this.M_OT_Affection = new System.Windows.Forms.MaskedTextBox();
            this.GB_Residence = new System.Windows.Forms.GroupBox();
            this.Region4 = new System.Windows.Forms.ComboBox();
            this.Region3 = new System.Windows.Forms.ComboBox();
            this.Region2 = new System.Windows.Forms.ComboBox();
            this.Region1 = new System.Windows.Forms.ComboBox();
            this.Region0 = new System.Windows.Forms.ComboBox();
            this.CB_Country4 = new System.Windows.Forms.ComboBox();
            this.CB_Country3 = new System.Windows.Forms.ComboBox();
            this.CB_Country2 = new System.Windows.Forms.ComboBox();
            this.CB_Country1 = new System.Windows.Forms.ComboBox();
            this.CB_Country0 = new System.Windows.Forms.ComboBox();
            this.L_Enjoyment = new System.Windows.Forms.Label();
            this.L_Fullness = new System.Windows.Forms.Label();
            this.M_Enjoyment = new System.Windows.Forms.MaskedTextBox();
            this.M_Fullness = new System.Windows.Forms.MaskedTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Tab_OTMemory = new System.Windows.Forms.TabPage();
            this.Tab_CTMemory = new System.Windows.Forms.TabPage();
            this.GB_M_CT = new System.Windows.Forms.GroupBox();
            this.RTB_CT = new System.Windows.Forms.RichTextBox();
            this.CB_CTVar = new System.Windows.Forms.ComboBox();
            this.CB_CTMemory = new System.Windows.Forms.ComboBox();
            this.CB_CTQual = new System.Windows.Forms.ComboBox();
            this.CB_CTFeel = new System.Windows.Forms.ComboBox();
            this.L_CT_Affection = new System.Windows.Forms.Label();
            this.L_CT_Friendship = new System.Windows.Forms.Label();
            this.M_CT_Affection = new System.Windows.Forms.MaskedTextBox();
            this.M_CT_Friendship = new System.Windows.Forms.MaskedTextBox();
            this.LCTV = new System.Windows.Forms.Label();
            this.L_CT_Feeling = new System.Windows.Forms.Label();
            this.L_CT_TextLine = new System.Windows.Forms.Label();
            this.L_CT_Quality = new System.Windows.Forms.Label();
            this.Tab_Residence = new System.Windows.Forms.TabPage();
            this.L_Handler = new System.Windows.Forms.Label();
            this.CB_Handler = new System.Windows.Forms.ComboBox();
            this.L_Arguments = new System.Windows.Forms.Label();
            this.GB_M_OT.SuspendLayout();
            this.GB_Residence.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Tab_OTMemory.SuspendLayout();
            this.Tab_CTMemory.SuspendLayout();
            this.GB_M_CT.SuspendLayout();
            this.Tab_Residence.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTN_Save
            // 
            this.BTN_Save.Location = new System.Drawing.Point(286, 266);
            this.BTN_Save.Name = "BTN_Save";
            this.BTN_Save.Size = new System.Drawing.Size(76, 23);
            this.BTN_Save.TabIndex = 28;
            this.BTN_Save.Text = "Save";
            this.BTN_Save.UseVisualStyleBackColor = true;
            this.BTN_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // BTN_Cancel
            // 
            this.BTN_Cancel.Location = new System.Drawing.Point(204, 266);
            this.BTN_Cancel.Name = "BTN_Cancel";
            this.BTN_Cancel.Size = new System.Drawing.Size(76, 23);
            this.BTN_Cancel.TabIndex = 27;
            this.BTN_Cancel.Text = "Cancel";
            this.BTN_Cancel.UseVisualStyleBackColor = true;
            this.BTN_Cancel.Click += new System.EventHandler(this.B_Cancel_Click);
            // 
            // M_OT_Friendship
            // 
            this.M_OT_Friendship.Location = new System.Drawing.Point(86, 16);
            this.M_OT_Friendship.Mask = "000";
            this.M_OT_Friendship.Name = "M_OT_Friendship";
            this.M_OT_Friendship.Size = new System.Drawing.Size(24, 20);
            this.M_OT_Friendship.TabIndex = 23;
            this.M_OT_Friendship.TextChanged += new System.EventHandler(this.update255_MTB);
            // 
            // L_OT_Friendship
            // 
            this.L_OT_Friendship.AutoSize = true;
            this.L_OT_Friendship.Location = new System.Drawing.Point(6, 19);
            this.L_OT_Friendship.Name = "L_OT_Friendship";
            this.L_OT_Friendship.Size = new System.Drawing.Size(58, 13);
            this.L_OT_Friendship.TabIndex = 52;
            this.L_OT_Friendship.Text = "Friendship:";
            // 
            // L_Geo0
            // 
            this.L_Geo0.Location = new System.Drawing.Point(-1, 31);
            this.L_Geo0.Name = "L_Geo0";
            this.L_Geo0.Size = new System.Drawing.Size(80, 13);
            this.L_Geo0.TabIndex = 68;
            this.L_Geo0.Text = "Latest:";
            this.L_Geo0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.L_Geo0.Click += new System.EventHandler(this.clickResetLocation);
            // 
            // L_Geo1
            // 
            this.L_Geo1.Location = new System.Drawing.Point(0, 64);
            this.L_Geo1.Name = "L_Geo1";
            this.L_Geo1.Size = new System.Drawing.Size(80, 13);
            this.L_Geo1.TabIndex = 69;
            this.L_Geo1.Text = "Past 1:";
            this.L_Geo1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.L_Geo1.Click += new System.EventHandler(this.clickResetLocation);
            // 
            // L_Region
            // 
            this.L_Region.Location = new System.Drawing.Point(226, 12);
            this.L_Region.Name = "L_Region";
            this.L_Region.Size = new System.Drawing.Size(80, 13);
            this.L_Region.TabIndex = 73;
            this.L_Region.Text = "Region";
            this.L_Region.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Country
            // 
            this.L_Country.Location = new System.Drawing.Point(104, 12);
            this.L_Country.Name = "L_Country";
            this.L_Country.Size = new System.Drawing.Size(80, 13);
            this.L_Country.TabIndex = 74;
            this.L_Country.Text = "Country";
            this.L_Country.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Geo2
            // 
            this.L_Geo2.Location = new System.Drawing.Point(0, 97);
            this.L_Geo2.Name = "L_Geo2";
            this.L_Geo2.Size = new System.Drawing.Size(80, 13);
            this.L_Geo2.TabIndex = 76;
            this.L_Geo2.Text = "Past 2:";
            this.L_Geo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.L_Geo2.Click += new System.EventHandler(this.clickResetLocation);
            // 
            // L_Geo3
            // 
            this.L_Geo3.Location = new System.Drawing.Point(0, 130);
            this.L_Geo3.Name = "L_Geo3";
            this.L_Geo3.Size = new System.Drawing.Size(80, 13);
            this.L_Geo3.TabIndex = 77;
            this.L_Geo3.Text = "Past 3:";
            this.L_Geo3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.L_Geo3.Click += new System.EventHandler(this.clickResetLocation);
            // 
            // L_Geo4
            // 
            this.L_Geo4.Location = new System.Drawing.Point(-1, 162);
            this.L_Geo4.Name = "L_Geo4";
            this.L_Geo4.Size = new System.Drawing.Size(80, 13);
            this.L_Geo4.TabIndex = 78;
            this.L_Geo4.Text = "Past 4:";
            this.L_Geo4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.L_Geo4.Click += new System.EventHandler(this.clickResetLocation);
            // 
            // L_OT_Quality
            // 
            this.L_OT_Quality.AutoSize = true;
            this.L_OT_Quality.Location = new System.Drawing.Point(6, 91);
            this.L_OT_Quality.Name = "L_OT_Quality";
            this.L_OT_Quality.Size = new System.Drawing.Size(49, 13);
            this.L_OT_Quality.TabIndex = 80;
            this.L_OT_Quality.Text = "Intensity:";
            // 
            // L_OT_TextLine
            // 
            this.L_OT_TextLine.AutoSize = true;
            this.L_OT_TextLine.Location = new System.Drawing.Point(6, 41);
            this.L_OT_TextLine.Name = "L_OT_TextLine";
            this.L_OT_TextLine.Size = new System.Drawing.Size(74, 13);
            this.L_OT_TextLine.TabIndex = 82;
            this.L_OT_TextLine.Text = "Memory Type:";
            // 
            // LOTV
            // 
            this.LOTV.AutoSize = true;
            this.LOTV.Location = new System.Drawing.Point(6, 65);
            this.LOTV.Name = "LOTV";
            this.LOTV.Size = new System.Drawing.Size(62, 13);
            this.LOTV.TabIndex = 83;
            this.LOTV.Text = "VARIABLE:";
            // 
            // L_OT_Feeling
            // 
            this.L_OT_Feeling.AutoSize = true;
            this.L_OT_Feeling.Location = new System.Drawing.Point(6, 114);
            this.L_OT_Feeling.Name = "L_OT_Feeling";
            this.L_OT_Feeling.Size = new System.Drawing.Size(44, 13);
            this.L_OT_Feeling.TabIndex = 86;
            this.L_OT_Feeling.Text = "Feeling:";
            // 
            // GB_M_OT
            // 
            this.GB_M_OT.Controls.Add(this.RTB_OT);
            this.GB_M_OT.Controls.Add(this.CB_OTVar);
            this.GB_M_OT.Controls.Add(this.CB_OTMemory);
            this.GB_M_OT.Controls.Add(this.CB_OTQual);
            this.GB_M_OT.Controls.Add(this.CB_OTFeel);
            this.GB_M_OT.Controls.Add(this.L_OT_Affection);
            this.GB_M_OT.Controls.Add(this.M_OT_Affection);
            this.GB_M_OT.Controls.Add(this.L_OT_Feeling);
            this.GB_M_OT.Controls.Add(this.LOTV);
            this.GB_M_OT.Controls.Add(this.L_OT_TextLine);
            this.GB_M_OT.Controls.Add(this.M_OT_Friendship);
            this.GB_M_OT.Controls.Add(this.L_OT_Friendship);
            this.GB_M_OT.Controls.Add(this.L_OT_Quality);
            this.GB_M_OT.Location = new System.Drawing.Point(7, 7);
            this.GB_M_OT.Name = "GB_M_OT";
            this.GB_M_OT.Size = new System.Drawing.Size(332, 188);
            this.GB_M_OT.TabIndex = 87;
            this.GB_M_OT.TabStop = false;
            this.GB_M_OT.Text = "Memories with Original Trainer";
            // 
            // RTB_OT
            // 
            this.RTB_OT.Location = new System.Drawing.Point(0, 141);
            this.RTB_OT.Name = "RTB_OT";
            this.RTB_OT.ReadOnly = true;
            this.RTB_OT.Size = new System.Drawing.Size(332, 47);
            this.RTB_OT.TabIndex = 103;
            this.RTB_OT.Text = "";
            // 
            // CB_OTVar
            // 
            this.CB_OTVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_OTVar.FormattingEnabled = true;
            this.CB_OTVar.Location = new System.Drawing.Point(86, 61);
            this.CB_OTVar.Name = "CB_OTVar";
            this.CB_OTVar.Size = new System.Drawing.Size(170, 21);
            this.CB_OTVar.TabIndex = 96;
            this.CB_OTVar.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // CB_OTMemory
            // 
            this.CB_OTMemory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_OTMemory.DropDownWidth = 440;
            this.CB_OTMemory.FormattingEnabled = true;
            this.CB_OTMemory.Location = new System.Drawing.Point(86, 38);
            this.CB_OTMemory.Name = "CB_OTMemory";
            this.CB_OTMemory.Size = new System.Drawing.Size(240, 21);
            this.CB_OTMemory.TabIndex = 95;
            this.CB_OTMemory.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // CB_OTQual
            // 
            this.CB_OTQual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_OTQual.FormattingEnabled = true;
            this.CB_OTQual.Items.AddRange(new object[] {
            "The Pokémon remembers",
            "The Pokémon remembers",
            "The Pokémon remembers",
            "The Pokémon remembers",
            "The Pokémon fondly remembers",
            "The Pokémon clearly remembers",
            "The Pokémon definitely remembers"});
            this.CB_OTQual.Location = new System.Drawing.Point(86, 88);
            this.CB_OTQual.Name = "CB_OTQual";
            this.CB_OTQual.Size = new System.Drawing.Size(240, 21);
            this.CB_OTQual.TabIndex = 90;
            this.CB_OTQual.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // CB_OTFeel
            // 
            this.CB_OTFeel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_OTFeel.FormattingEnabled = true;
            this.CB_OTFeel.Items.AddRange(new object[] {
            "it was happy",
            "it had fun",
            "it was glad",
            "it grinned",
            "it got overwhelmed by emotion",
            "its feelings were indescribable",
            "it felt good",
            "it got teary eyed",
            "it got lighthearted",
            "it got nervous",
            "it felt comfortable",
            "it was restless",
            "it got a bit carried away",
            "it felt sorry",
            "it got emotional",
            "it felt nostalgic",
            "it had some difficulty",
            "it felt exhausted",
            "it couldn’t be true to its feelings",
            "it felt proud",
            "they ended up in a foul mood",
            "it got angry",
            "it got jealous",
            "it got sleepy"});
            this.CB_OTFeel.Location = new System.Drawing.Point(86, 111);
            this.CB_OTFeel.Name = "CB_OTFeel";
            this.CB_OTFeel.Size = new System.Drawing.Size(170, 21);
            this.CB_OTFeel.TabIndex = 89;
            this.CB_OTFeel.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // L_OT_Affection
            // 
            this.L_OT_Affection.AutoSize = true;
            this.L_OT_Affection.Location = new System.Drawing.Point(149, 19);
            this.L_OT_Affection.Name = "L_OT_Affection";
            this.L_OT_Affection.Size = new System.Drawing.Size(52, 13);
            this.L_OT_Affection.TabIndex = 88;
            this.L_OT_Affection.Text = "Affection:";
            // 
            // M_OT_Affection
            // 
            this.M_OT_Affection.Location = new System.Drawing.Point(227, 16);
            this.M_OT_Affection.Mask = "000";
            this.M_OT_Affection.Name = "M_OT_Affection";
            this.M_OT_Affection.Size = new System.Drawing.Size(24, 20);
            this.M_OT_Affection.TabIndex = 24;
            this.M_OT_Affection.TextChanged += new System.EventHandler(this.update255_MTB);
            // 
            // GB_Residence
            // 
            this.GB_Residence.Controls.Add(this.Region4);
            this.GB_Residence.Controls.Add(this.Region3);
            this.GB_Residence.Controls.Add(this.Region2);
            this.GB_Residence.Controls.Add(this.Region1);
            this.GB_Residence.Controls.Add(this.Region0);
            this.GB_Residence.Controls.Add(this.CB_Country4);
            this.GB_Residence.Controls.Add(this.CB_Country3);
            this.GB_Residence.Controls.Add(this.CB_Country2);
            this.GB_Residence.Controls.Add(this.CB_Country1);
            this.GB_Residence.Controls.Add(this.CB_Country0);
            this.GB_Residence.Controls.Add(this.L_Geo4);
            this.GB_Residence.Controls.Add(this.L_Geo3);
            this.GB_Residence.Controls.Add(this.L_Geo2);
            this.GB_Residence.Controls.Add(this.L_Country);
            this.GB_Residence.Controls.Add(this.L_Region);
            this.GB_Residence.Controls.Add(this.L_Geo1);
            this.GB_Residence.Controls.Add(this.L_Geo0);
            this.GB_Residence.Location = new System.Drawing.Point(7, 7);
            this.GB_Residence.Name = "GB_Residence";
            this.GB_Residence.Size = new System.Drawing.Size(332, 188);
            this.GB_Residence.TabIndex = 89;
            this.GB_Residence.TabStop = false;
            this.GB_Residence.Text = "Pokémon has Resided in:";
            // 
            // Region4
            // 
            this.Region4.DropDownWidth = 180;
            this.Region4.FormattingEnabled = true;
            this.Region4.Location = new System.Drawing.Point(204, 158);
            this.Region4.Name = "Region4";
            this.Region4.Size = new System.Drawing.Size(102, 21);
            this.Region4.TabIndex = 88;
            // 
            // Region3
            // 
            this.Region3.DropDownWidth = 180;
            this.Region3.FormattingEnabled = true;
            this.Region3.Location = new System.Drawing.Point(204, 126);
            this.Region3.Name = "Region3";
            this.Region3.Size = new System.Drawing.Size(102, 21);
            this.Region3.TabIndex = 87;
            // 
            // Region2
            // 
            this.Region2.DropDownWidth = 180;
            this.Region2.FormattingEnabled = true;
            this.Region2.Location = new System.Drawing.Point(204, 93);
            this.Region2.Name = "Region2";
            this.Region2.Size = new System.Drawing.Size(102, 21);
            this.Region2.TabIndex = 86;
            // 
            // Region1
            // 
            this.Region1.DropDownWidth = 180;
            this.Region1.FormattingEnabled = true;
            this.Region1.Location = new System.Drawing.Point(204, 60);
            this.Region1.Name = "Region1";
            this.Region1.Size = new System.Drawing.Size(102, 21);
            this.Region1.TabIndex = 85;
            // 
            // Region0
            // 
            this.Region0.DropDownWidth = 180;
            this.Region0.FormattingEnabled = true;
            this.Region0.Location = new System.Drawing.Point(204, 27);
            this.Region0.Name = "Region0";
            this.Region0.Size = new System.Drawing.Size(102, 21);
            this.Region0.TabIndex = 84;
            // 
            // CB_Country4
            // 
            this.CB_Country4.DropDownWidth = 180;
            this.CB_Country4.FormattingEnabled = true;
            this.CB_Country4.Location = new System.Drawing.Point(82, 158);
            this.CB_Country4.Name = "CB_Country4";
            this.CB_Country4.Size = new System.Drawing.Size(102, 21);
            this.CB_Country4.TabIndex = 83;
            this.CB_Country4.SelectedIndexChanged += new System.EventHandler(this.changeCountry);
            // 
            // CB_Country3
            // 
            this.CB_Country3.DropDownWidth = 180;
            this.CB_Country3.FormattingEnabled = true;
            this.CB_Country3.Location = new System.Drawing.Point(82, 126);
            this.CB_Country3.Name = "CB_Country3";
            this.CB_Country3.Size = new System.Drawing.Size(102, 21);
            this.CB_Country3.TabIndex = 82;
            this.CB_Country3.SelectedIndexChanged += new System.EventHandler(this.changeCountry);
            // 
            // CB_Country2
            // 
            this.CB_Country2.DropDownWidth = 180;
            this.CB_Country2.FormattingEnabled = true;
            this.CB_Country2.Location = new System.Drawing.Point(82, 93);
            this.CB_Country2.Name = "CB_Country2";
            this.CB_Country2.Size = new System.Drawing.Size(102, 21);
            this.CB_Country2.TabIndex = 81;
            this.CB_Country2.SelectedIndexChanged += new System.EventHandler(this.changeCountry);
            // 
            // CB_Country1
            // 
            this.CB_Country1.DropDownWidth = 180;
            this.CB_Country1.FormattingEnabled = true;
            this.CB_Country1.Location = new System.Drawing.Point(82, 60);
            this.CB_Country1.Name = "CB_Country1";
            this.CB_Country1.Size = new System.Drawing.Size(102, 21);
            this.CB_Country1.TabIndex = 80;
            this.CB_Country1.SelectedIndexChanged += new System.EventHandler(this.changeCountry);
            // 
            // CB_Country0
            // 
            this.CB_Country0.DropDownWidth = 180;
            this.CB_Country0.FormattingEnabled = true;
            this.CB_Country0.Location = new System.Drawing.Point(82, 27);
            this.CB_Country0.Name = "CB_Country0";
            this.CB_Country0.Size = new System.Drawing.Size(102, 21);
            this.CB_Country0.TabIndex = 79;
            this.CB_Country0.SelectedIndexChanged += new System.EventHandler(this.changeCountry);
            // 
            // L_Enjoyment
            // 
            this.L_Enjoyment.Location = new System.Drawing.Point(94, 271);
            this.L_Enjoyment.Name = "L_Enjoyment";
            this.L_Enjoyment.Size = new System.Drawing.Size(70, 13);
            this.L_Enjoyment.TabIndex = 99;
            this.L_Enjoyment.Text = "Enjoyment:";
            this.L_Enjoyment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Fullness
            // 
            this.L_Fullness.Location = new System.Drawing.Point(1, 271);
            this.L_Fullness.Name = "L_Fullness";
            this.L_Fullness.Size = new System.Drawing.Size(60, 13);
            this.L_Fullness.TabIndex = 98;
            this.L_Fullness.Text = "Fullness:";
            this.L_Fullness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // M_Enjoyment
            // 
            this.M_Enjoyment.Location = new System.Drawing.Point(167, 268);
            this.M_Enjoyment.Mask = "000";
            this.M_Enjoyment.Name = "M_Enjoyment";
            this.M_Enjoyment.Size = new System.Drawing.Size(24, 20);
            this.M_Enjoyment.TabIndex = 18;
            this.M_Enjoyment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.M_Enjoyment.TextChanged += new System.EventHandler(this.update255_MTB);
            // 
            // M_Fullness
            // 
            this.M_Fullness.Location = new System.Drawing.Point(64, 268);
            this.M_Fullness.Mask = "000";
            this.M_Fullness.Name = "M_Fullness";
            this.M_Fullness.Size = new System.Drawing.Size(24, 20);
            this.M_Fullness.TabIndex = 17;
            this.M_Fullness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.M_Fullness.TextChanged += new System.EventHandler(this.update255_MTB);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Tab_OTMemory);
            this.tabControl1.Controls.Add(this.Tab_CTMemory);
            this.tabControl1.Controls.Add(this.Tab_Residence);
            this.tabControl1.Location = new System.Drawing.Point(7, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(355, 228);
            this.tabControl1.TabIndex = 100;
            // 
            // Tab_OTMemory
            // 
            this.Tab_OTMemory.Controls.Add(this.GB_M_OT);
            this.Tab_OTMemory.Location = new System.Drawing.Point(4, 22);
            this.Tab_OTMemory.Name = "Tab_OTMemory";
            this.Tab_OTMemory.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_OTMemory.Size = new System.Drawing.Size(347, 202);
            this.Tab_OTMemory.TabIndex = 1;
            this.Tab_OTMemory.Text = "Memories with OT";
            this.Tab_OTMemory.UseVisualStyleBackColor = true;
            // 
            // Tab_CTMemory
            // 
            this.Tab_CTMemory.Controls.Add(this.GB_M_CT);
            this.Tab_CTMemory.Location = new System.Drawing.Point(4, 22);
            this.Tab_CTMemory.Name = "Tab_CTMemory";
            this.Tab_CTMemory.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_CTMemory.Size = new System.Drawing.Size(347, 202);
            this.Tab_CTMemory.TabIndex = 2;
            this.Tab_CTMemory.Text = "Memories with notOT";
            this.Tab_CTMemory.UseVisualStyleBackColor = true;
            // 
            // GB_M_CT
            // 
            this.GB_M_CT.Controls.Add(this.RTB_CT);
            this.GB_M_CT.Controls.Add(this.CB_CTVar);
            this.GB_M_CT.Controls.Add(this.CB_CTMemory);
            this.GB_M_CT.Controls.Add(this.CB_CTQual);
            this.GB_M_CT.Controls.Add(this.CB_CTFeel);
            this.GB_M_CT.Controls.Add(this.L_CT_Affection);
            this.GB_M_CT.Controls.Add(this.L_CT_Friendship);
            this.GB_M_CT.Controls.Add(this.M_CT_Affection);
            this.GB_M_CT.Controls.Add(this.M_CT_Friendship);
            this.GB_M_CT.Controls.Add(this.LCTV);
            this.GB_M_CT.Controls.Add(this.L_CT_Feeling);
            this.GB_M_CT.Controls.Add(this.L_CT_TextLine);
            this.GB_M_CT.Controls.Add(this.L_CT_Quality);
            this.GB_M_CT.Location = new System.Drawing.Point(7, 7);
            this.GB_M_CT.Name = "GB_M_CT";
            this.GB_M_CT.Size = new System.Drawing.Size(332, 188);
            this.GB_M_CT.TabIndex = 89;
            this.GB_M_CT.TabStop = false;
            this.GB_M_CT.Text = "Memories with Current Trainer";
            // 
            // RTB_CT
            // 
            this.RTB_CT.Location = new System.Drawing.Point(0, 141);
            this.RTB_CT.Name = "RTB_CT";
            this.RTB_CT.ReadOnly = true;
            this.RTB_CT.Size = new System.Drawing.Size(332, 47);
            this.RTB_CT.TabIndex = 104;
            this.RTB_CT.Text = "";
            // 
            // CB_CTVar
            // 
            this.CB_CTVar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_CTVar.FormattingEnabled = true;
            this.CB_CTVar.Location = new System.Drawing.Point(86, 61);
            this.CB_CTVar.Name = "CB_CTVar";
            this.CB_CTVar.Size = new System.Drawing.Size(170, 21);
            this.CB_CTVar.TabIndex = 95;
            this.CB_CTVar.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // CB_CTMemory
            // 
            this.CB_CTMemory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_CTMemory.DropDownWidth = 440;
            this.CB_CTMemory.FormattingEnabled = true;
            this.CB_CTMemory.Location = new System.Drawing.Point(86, 38);
            this.CB_CTMemory.Name = "CB_CTMemory";
            this.CB_CTMemory.Size = new System.Drawing.Size(240, 21);
            this.CB_CTMemory.TabIndex = 94;
            this.CB_CTMemory.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // CB_CTQual
            // 
            this.CB_CTQual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_CTQual.FormattingEnabled = true;
            this.CB_CTQual.Items.AddRange(new object[] {
            "The Pokémon remembers",
            "The Pokémon remembers",
            "The Pokémon remembers",
            "The Pokémon remembers",
            "The Pokémon fondly remembers",
            "The Pokémon clearly remembers",
            "The Pokémon definitely remembers"});
            this.CB_CTQual.Location = new System.Drawing.Point(86, 88);
            this.CB_CTQual.Name = "CB_CTQual";
            this.CB_CTQual.Size = new System.Drawing.Size(240, 21);
            this.CB_CTQual.TabIndex = 93;
            this.CB_CTQual.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // CB_CTFeel
            // 
            this.CB_CTFeel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_CTFeel.FormattingEnabled = true;
            this.CB_CTFeel.Items.AddRange(new object[] {
            "it was happy",
            "it had fun",
            "it was glad",
            "it grinned",
            "it got overwhelmed by emotion",
            "its feelings were indescribable",
            "it felt good",
            "it got teary eyed",
            "it got lighthearted",
            "it got nervous",
            "it felt comfortable",
            "it was restless",
            "it got a bit carried away",
            "it felt sorry",
            "it got emotional",
            "it felt nostalgic",
            "it had some difficulty",
            "it felt exhausted",
            "it couldn’t be true to its feelings",
            "it felt proud",
            "they ended up in a foul mood",
            "it got angry",
            "it got jealous",
            "it got sleepy"});
            this.CB_CTFeel.Location = new System.Drawing.Point(86, 111);
            this.CB_CTFeel.Name = "CB_CTFeel";
            this.CB_CTFeel.Size = new System.Drawing.Size(170, 21);
            this.CB_CTFeel.TabIndex = 92;
            this.CB_CTFeel.SelectedIndexChanged += new System.EventHandler(this.changeMemory);
            // 
            // L_CT_Affection
            // 
            this.L_CT_Affection.AutoSize = true;
            this.L_CT_Affection.Location = new System.Drawing.Point(149, 19);
            this.L_CT_Affection.Name = "L_CT_Affection";
            this.L_CT_Affection.Size = new System.Drawing.Size(52, 13);
            this.L_CT_Affection.TabIndex = 91;
            this.L_CT_Affection.Text = "Affection:";
            // 
            // L_CT_Friendship
            // 
            this.L_CT_Friendship.AutoSize = true;
            this.L_CT_Friendship.Location = new System.Drawing.Point(6, 19);
            this.L_CT_Friendship.Name = "L_CT_Friendship";
            this.L_CT_Friendship.Size = new System.Drawing.Size(58, 13);
            this.L_CT_Friendship.TabIndex = 90;
            this.L_CT_Friendship.Text = "Friendship:";
            // 
            // M_CT_Affection
            // 
            this.M_CT_Affection.Location = new System.Drawing.Point(227, 16);
            this.M_CT_Affection.Mask = "000";
            this.M_CT_Affection.Name = "M_CT_Affection";
            this.M_CT_Affection.Size = new System.Drawing.Size(24, 20);
            this.M_CT_Affection.TabIndex = 16;
            this.M_CT_Affection.TextChanged += new System.EventHandler(this.update255_MTB);
            // 
            // M_CT_Friendship
            // 
            this.M_CT_Friendship.Location = new System.Drawing.Point(86, 16);
            this.M_CT_Friendship.Mask = "000";
            this.M_CT_Friendship.Name = "M_CT_Friendship";
            this.M_CT_Friendship.Size = new System.Drawing.Size(24, 20);
            this.M_CT_Friendship.TabIndex = 15;
            this.M_CT_Friendship.TextChanged += new System.EventHandler(this.update255_MTB);
            // 
            // LCTV
            // 
            this.LCTV.AutoSize = true;
            this.LCTV.Location = new System.Drawing.Point(6, 65);
            this.LCTV.Name = "LCTV";
            this.LCTV.Size = new System.Drawing.Size(59, 13);
            this.LCTV.TabIndex = 58;
            this.LCTV.Text = "VARIABLE";
            // 
            // L_CT_Feeling
            // 
            this.L_CT_Feeling.AutoSize = true;
            this.L_CT_Feeling.Location = new System.Drawing.Point(6, 114);
            this.L_CT_Feeling.Name = "L_CT_Feeling";
            this.L_CT_Feeling.Size = new System.Drawing.Size(44, 13);
            this.L_CT_Feeling.TabIndex = 56;
            this.L_CT_Feeling.Text = "Feeling:";
            // 
            // L_CT_TextLine
            // 
            this.L_CT_TextLine.AutoSize = true;
            this.L_CT_TextLine.Location = new System.Drawing.Point(6, 41);
            this.L_CT_TextLine.Name = "L_CT_TextLine";
            this.L_CT_TextLine.Size = new System.Drawing.Size(74, 13);
            this.L_CT_TextLine.TabIndex = 55;
            this.L_CT_TextLine.Text = "Memory Type:";
            // 
            // L_CT_Quality
            // 
            this.L_CT_Quality.AutoSize = true;
            this.L_CT_Quality.Location = new System.Drawing.Point(6, 91);
            this.L_CT_Quality.Name = "L_CT_Quality";
            this.L_CT_Quality.Size = new System.Drawing.Size(49, 13);
            this.L_CT_Quality.TabIndex = 54;
            this.L_CT_Quality.Text = "Intensity:";
            // 
            // Tab_Residence
            // 
            this.Tab_Residence.Controls.Add(this.GB_Residence);
            this.Tab_Residence.Location = new System.Drawing.Point(4, 22);
            this.Tab_Residence.Name = "Tab_Residence";
            this.Tab_Residence.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Residence.Size = new System.Drawing.Size(347, 202);
            this.Tab_Residence.TabIndex = 0;
            this.Tab_Residence.Text = "Residence";
            this.Tab_Residence.UseVisualStyleBackColor = true;
            // 
            // L_Handler
            // 
            this.L_Handler.Location = new System.Drawing.Point(44, 242);
            this.L_Handler.Name = "L_Handler";
            this.L_Handler.Size = new System.Drawing.Size(120, 13);
            this.L_Handler.TabIndex = 101;
            this.L_Handler.Text = "Current Handler:";
            this.L_Handler.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_Handler
            // 
            this.CB_Handler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Handler.Enabled = false;
            this.CB_Handler.FormattingEnabled = true;
            this.CB_Handler.Location = new System.Drawing.Point(175, 239);
            this.CB_Handler.Name = "CB_Handler";
            this.CB_Handler.Size = new System.Drawing.Size(117, 21);
            this.CB_Handler.TabIndex = 100;
            // 
            // L_Arguments
            // 
            this.L_Arguments.AutoSize = true;
            this.L_Arguments.Location = new System.Drawing.Point(338, 4);
            this.L_Arguments.Name = "L_Arguments";
            this.L_Arguments.Size = new System.Drawing.Size(33, 13);
            this.L_Arguments.TabIndex = 102;
            this.L_Arguments.Text = "(args)";
            this.L_Arguments.Visible = false;
            // 
            // MemoryAmie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(374, 297);
            this.Controls.Add(this.L_Arguments);
            this.Controls.Add(this.L_Handler);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.CB_Handler);
            this.Controls.Add(this.BTN_Cancel);
            this.Controls.Add(this.BTN_Save);
            this.Controls.Add(this.L_Enjoyment);
            this.Controls.Add(this.L_Fullness);
            this.Controls.Add(this.M_Fullness);
            this.Controls.Add(this.M_Enjoyment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MemoryAmie";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Memories / Amie Editor";
            this.GB_M_OT.ResumeLayout(false);
            this.GB_M_OT.PerformLayout();
            this.GB_Residence.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.Tab_OTMemory.ResumeLayout(false);
            this.Tab_CTMemory.ResumeLayout(false);
            this.GB_M_CT.ResumeLayout(false);
            this.GB_M_CT.PerformLayout();
            this.Tab_Residence.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_Save;
        private System.Windows.Forms.Button BTN_Cancel;
        private System.Windows.Forms.MaskedTextBox M_OT_Friendship;
        private System.Windows.Forms.Label L_OT_Friendship;
        private System.Windows.Forms.Label L_Geo0;
        private System.Windows.Forms.Label L_Geo1;
        private System.Windows.Forms.Label L_Region;
        private System.Windows.Forms.Label L_Country;
        private System.Windows.Forms.Label L_Geo2;
        private System.Windows.Forms.Label L_Geo3;
        private System.Windows.Forms.Label L_Geo4;
        private System.Windows.Forms.Label L_OT_Quality;
        private System.Windows.Forms.Label L_OT_TextLine;
        private System.Windows.Forms.Label LOTV;
        private System.Windows.Forms.Label L_OT_Feeling;
        private System.Windows.Forms.GroupBox GB_M_OT;
        private System.Windows.Forms.GroupBox GB_Residence;
        private System.Windows.Forms.Label L_OT_Affection;
        private System.Windows.Forms.MaskedTextBox M_OT_Affection;
        private System.Windows.Forms.Label L_Enjoyment;
        private System.Windows.Forms.Label L_Fullness;
        private System.Windows.Forms.MaskedTextBox M_Enjoyment;
        private System.Windows.Forms.MaskedTextBox M_Fullness;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Tab_Residence;
        private System.Windows.Forms.TabPage Tab_OTMemory;
        private System.Windows.Forms.Label L_Handler;
        private System.Windows.Forms.ComboBox CB_Handler;
        private System.Windows.Forms.ComboBox CB_Country4;
        private System.Windows.Forms.ComboBox CB_Country3;
        private System.Windows.Forms.ComboBox CB_Country2;
        private System.Windows.Forms.ComboBox CB_Country1;
        private System.Windows.Forms.ComboBox CB_Country0;
        private System.Windows.Forms.TabPage Tab_CTMemory;
        private System.Windows.Forms.GroupBox GB_M_CT;
        private System.Windows.Forms.Label L_CT_Affection;
        private System.Windows.Forms.Label L_CT_Friendship;
        private System.Windows.Forms.MaskedTextBox M_CT_Affection;
        private System.Windows.Forms.MaskedTextBox M_CT_Friendship;
        private System.Windows.Forms.Label LCTV;
        private System.Windows.Forms.Label L_CT_Feeling;
        private System.Windows.Forms.Label L_CT_TextLine;
        private System.Windows.Forms.Label L_CT_Quality;
        private System.Windows.Forms.ComboBox CB_OTQual;
        private System.Windows.Forms.ComboBox CB_OTFeel;
        private System.Windows.Forms.ComboBox CB_CTQual;
        private System.Windows.Forms.ComboBox CB_CTFeel;
        private System.Windows.Forms.ComboBox CB_OTVar;
        private System.Windows.Forms.ComboBox CB_OTMemory;
        private System.Windows.Forms.ComboBox CB_CTVar;
        private System.Windows.Forms.ComboBox CB_CTMemory;
        private System.Windows.Forms.RichTextBox RTB_OT;
        private System.Windows.Forms.RichTextBox RTB_CT;
        private System.Windows.Forms.Label L_Arguments;
        private System.Windows.Forms.ComboBox Region4;
        private System.Windows.Forms.ComboBox Region3;
        private System.Windows.Forms.ComboBox Region2;
        private System.Windows.Forms.ComboBox Region1;
        private System.Windows.Forms.ComboBox Region0;
    }
}