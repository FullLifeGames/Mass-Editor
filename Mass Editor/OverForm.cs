using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mass_Editor
{
    public partial class OverForm : Form
    {
        Form1 f;

        public OverForm()
        {
            // Using another Initialize Method to use objects from another Form
        //    InitializeComponent();
            InitializeComponents();
            enableAll();
        }

        #region Drag and Drop
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string File in FileList)
                this.listView1.Items.Add(File);
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.Equals(Keys.Delete))
            {
                ListView.SelectedListViewItemCollection lc = listView1.SelectedItems;
                foreach (ListViewItem l in lc)
                {
                    listView1.Items.RemoveAt(listView1.Items.IndexOf(l));
                }
            }
        }
        #endregion

        private void disableAll()
        {
            bool b = false;
            CHK_Bak.Enabled = b;
            CB_Ball.Enabled = b;
            CHK_ChangeOT.Enabled = b;
            CHK_DeleteNicknames.Enabled = b;
            CB_EggLocation.Enabled = b;
            CB_EncounterType.Enabled = b;
            CHK_Frienship.Enabled = b;
            CB_GameOrigin.Enabled = b;
            CHK_Level.Enabled = b;
            CHK_Met.Enabled = b;
            CB_MetLocation.Enabled = b;
            CHK_Perfect_IVs.Enabled = b;
            CHK_Reroll.Enabled = b;
            CHK_Shiny.Enabled = b;
            CHK_Unshiny.Enabled = b;
            TB_MetLevel.Enabled = b;
            CAL_EggDate.Enabled = b;
            CAL_MetDate.Enabled = b;
            CHK_AsEgg.Enabled = b;
            CHK_Fateful.Enabled = b;
            B_Mass_Edit.Enabled = b;
            textBox1.Enabled = b;
            textBox2.Enabled = b;
            textBox3.Enabled = b;
            textBox4.Enabled = b;
            textBox5.Enabled = b;
            textBox6.Enabled = b;
            listView1.Enabled = b;
            groupBox1.Enabled = b;
            groupBox2.Enabled = b;
            groupBox3.Enabled = b;
            GB_Met.Enabled = b;
            groupBox4.Enabled = b;
            CHK_Country.Enabled = b;
        }

        private void enableAll()
        {
            bool b = true;
            CHK_Bak.Enabled = b;
            CB_Ball.Enabled = b;
            CHK_ChangeOT.Enabled = b;
            CHK_DeleteNicknames.Enabled = b;
            CB_EggLocation.Enabled = b;
            CB_EncounterType.Enabled = b;
            CHK_Frienship.Enabled = b;
            CB_GameOrigin.Enabled = b;
            CHK_Level.Enabled = b;
            CHK_Met.Enabled = b;
            CB_MetLocation.Enabled = b;
            CHK_Perfect_IVs.Enabled = b;
            CHK_Reroll.Enabled = b;
            CHK_Shiny.Enabled = b;
            CHK_Unshiny.Enabled = b;
            TB_MetLevel.Enabled = b;
            CAL_EggDate.Enabled = b;
            CAL_MetDate.Enabled = b;
            CHK_AsEgg.Enabled = b;
            CHK_Fateful.Enabled = b;
            B_Mass_Edit.Enabled = b;
            textBox1.Enabled = b;
            textBox2.Enabled = b;
            textBox3.Enabled = b;
            textBox4.Enabled = b;
            textBox5.Enabled = b;
            textBox6.Enabled = b;
            listView1.Enabled = b;
            groupBox1.Enabled = CHK_ChangeOT.Checked;
            groupBox2.Enabled = b;
            groupBox3.Enabled = b;
            GB_Met.Enabled = CHK_Met.Checked;
            groupBox4.Enabled = CHK_Country.Checked;
            CHK_Country.Enabled = b;
        }

        private void B_Mass_Edit_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                running = true;
                disableAll();
                List<int> modes = new List<int>();
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = listView1.Items.Count;
                bool bak = CHK_Bak.Checked;
                string[] ret = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text };
                string friendship = textBox5.Text;
                string level = textBox6.Text;
                bool[] countrybool = { checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, checkBox5.Checked };
                bool[] metbool = { checkBox6.Checked, checkBox7.Checked, checkBox8.Checked, checkBox9.Checked, checkBox10.Checked, checkBox11.Checked, checkBox12.Checked, checkBox13.Checked, checkBox14.Checked, checkBox15.Checked };
                bool[] otbool = { checkBox16.Checked, checkBox17.Checked, checkBox18.Checked, checkBox19.Checked };
                int[] otindexes = { CB_Language.SelectedIndex, CB_Country.SelectedIndex, CB_SubRegion.SelectedIndex, CB_3DSReg.SelectedIndex };
                List<string> litems = new List<string>();
                foreach (ListViewItem l in listView1.Items)
                {
                    litems.Add(l.Text);
                }


                Met m = new Met(CB_GameOrigin.SelectedIndex, CB_MetLocation.SelectedIndex, CB_Ball.SelectedIndex, TB_MetLevel.Text, CAL_MetDate.Value, CHK_Fateful.Checked, CB_EncounterType.Enabled, CB_EncounterType.SelectedIndex, CHK_AsEgg.Checked, CB_EggLocation.SelectedIndex, CAL_EggDate.Value);

                if (CHK_Shiny.Checked)
                {
                    modes.Add(0);
                }
                if (CHK_Unshiny.Checked)
                {
                    modes.Add(1);
                }
                if (CHK_ChangeOT.Checked)
                {
                    modes.Add(2);
                }
                if (CHK_DeleteNicknames.Checked)
                {
                    modes.Add(3);
                }
                if (CHK_Perfect_IVs.Checked)
                {
                    modes.Add(4);
                }
                if (CHK_Reroll.Checked)
                {
                    modes.Add(5);
                }
                if (CHK_Frienship.Checked)
                {
                    modes.Add(6);
                }
                if (CHK_Level.Checked)
                {
                    modes.Add(7);
                }
                if (CHK_Met.Checked)
                {
                    modes.Add(8);
                }
                if (CHK_Country.Checked)
                {
                    modes.Add(9);
                }

                // thread for free UI
                thread = new Thread(delegate() { Form1 f1 = new Form1(litems, modes, this.progressBar1, ret, friendship, level, m, bak, otindexes, countrybool, metbool, otbool); f1.Form1_Load(new object(), new EventArgs()); f1.Dispose(); });
                thread.SetApartmentState(ApartmentState.STA);

                // thread2 is basically my thread_finished_Eventhandler
                thread2 = new Thread(delegate() { thread.Join(); this.BeginInvoke((MethodInvoker)delegate { enableAll(); running = false; }); });
                thread.Start();
                thread2.Start();
            }
        }

        bool running = false;

        Thread thread = null;
        Thread thread2 = null;

        private void InitializeComponents()
        {
            f = new Form1();           
            CHK_AsEgg = f.getCHK_AsEgg();
            CB_GameOrigin = f.getCB_GameOrigin();
            CB_MetLocation = f.getCB_MetLocation();
            CB_Ball = f.getCB_Ball();
            CB_EncounterType = f.getCB_EncounterType();
            CB_EggLocation = f.getCB_EggLocation();
            TB_MetLevel = f.getTB_MetLevel();
            CAL_MetDate = f.getCAL_MetDate();
            CHK_Fateful = f.getCHK_Fateful();
            CAL_EggDate = f.getCAL_EggDate();
            GB_EggConditions = f.getGB_EggConditions();
            CB_Language = f.getCB_Language();
            CB_Country = f.getCB_Country();
            CB_SubRegion = f.getCB_SubRegion();
            CB_3DSReg = f.getCB_3DSReg();

            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.CHK_Unshiny = new System.Windows.Forms.CheckBox();
            this.CHK_Shiny = new System.Windows.Forms.CheckBox();
            this.CHK_Perfect_IVs = new System.Windows.Forms.CheckBox();
            this.CHK_ChangeOT = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CHK_DeleteNicknames = new System.Windows.Forms.CheckBox();
            this.CHK_Reroll = new System.Windows.Forms.CheckBox();
            this.CHK_Bak = new System.Windows.Forms.CheckBox();
            this.CHK_Met = new System.Windows.Forms.CheckBox();


            this.Label_EggDate = new System.Windows.Forms.Label();
            this.Label_EggLocation = new System.Windows.Forms.Label();
            this.Label_EncounterType = new System.Windows.Forms.Label();
            this.Label_MetDate = new System.Windows.Forms.Label();
            this.Label_MetLevel = new System.Windows.Forms.Label();
            this.Label_Ball = new System.Windows.Forms.Label();
            this.Label_MetLocation = new System.Windows.Forms.Label();
            this.Label_OriginGame = new System.Windows.Forms.Label();



            this.GB_Met = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CHK_Frienship = new System.Windows.Forms.CheckBox();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.B_Mass_Edit = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.MaskedTextBox();
            this.textBox6 = new System.Windows.Forms.MaskedTextBox();

            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CHK_Country = new System.Windows.Forms.CheckBox();
            this.Label_3DSRegion = new System.Windows.Forms.Label();
            this.Label_SubRegion = new System.Windows.Forms.Label();
            this.Label_Country = new System.Windows.Forms.Label();
            this.Label_Language = new System.Windows.Forms.Label();

            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();

            this.GB_EggConditions.SuspendLayout();
            this.GB_Met.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();

            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(86, 20);
            this.textBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "TID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "SID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "OT";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 370);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(403, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Drag and Drop in here:";
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Location = new System.Drawing.Point(13, 135);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(403, 215);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // CHK_Unshiny
            // 
            this.CHK_Unshiny.AutoSize = true;
            this.CHK_Unshiny.Location = new System.Drawing.Point(148, 44);
            this.CHK_Unshiny.Name = "CHK_Unshiny";
            this.CHK_Unshiny.Size = new System.Drawing.Size(118, 17);
            this.CHK_Unshiny.TabIndex = 15;
            this.CHK_Unshiny.Text = "Make them unshiny";
            this.CHK_Unshiny.UseVisualStyleBackColor = true;
            // 
            // CHK_Shiny
            // 
            this.CHK_Shiny.AutoSize = true;
            this.CHK_Shiny.Location = new System.Drawing.Point(148, 21);
            this.CHK_Shiny.Name = "CHK_Shiny";
            this.CHK_Shiny.Size = new System.Drawing.Size(106, 17);
            this.CHK_Shiny.TabIndex = 16;
            this.CHK_Shiny.Text = "Make them shiny";
            this.CHK_Shiny.UseVisualStyleBackColor = true;
            // 
            // CHK_Perfect_IVs
            // 
            this.CHK_Perfect_IVs.AutoSize = true;
            this.CHK_Perfect_IVs.Location = new System.Drawing.Point(148, 67);
            this.CHK_Perfect_IVs.Name = "CHK_Perfect_IVs";
            this.CHK_Perfect_IVs.Size = new System.Drawing.Size(104, 17);
            this.CHK_Perfect_IVs.TabIndex = 17;
            this.CHK_Perfect_IVs.Text = "Make perfectIVs";
            this.CHK_Perfect_IVs.UseVisualStyleBackColor = true;
            // 
            // CHK_ChangeOT
            // 
            this.CHK_ChangeOT.AutoSize = true;
            this.CHK_ChangeOT.Location = new System.Drawing.Point(610, 7);
            this.CHK_ChangeOT.Name = "CHK_ChangeOT";
            this.CHK_ChangeOT.Size = new System.Drawing.Size(96, 17);
            this.CHK_ChangeOT.TabIndex = 19;
            this.CHK_ChangeOT.Text = "Change OT to:";
            this.CHK_ChangeOT.UseVisualStyleBackColor = true;
            this.CHK_ChangeOT.CheckedChanged += new System.EventHandler(this.CB_ChangeOT_Changed);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(120, 33);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(86, 20);
            this.textBox2.TabIndex = 20;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(10, 74);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(86, 20);
            this.textBox3.TabIndex = 21;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(120, 74);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(86, 20);
            this.textBox4.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(142, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Last OT";
            // 
            // CHK_DeleteNicknames
            // 
            this.CHK_DeleteNicknames.AutoSize = true;
            this.CHK_DeleteNicknames.Location = new System.Drawing.Point(275, 37);
            this.CHK_DeleteNicknames.Name = "CHK_DeleteNicknames";
            this.CHK_DeleteNicknames.Size = new System.Drawing.Size(113, 17);
            this.CHK_DeleteNicknames.TabIndex = 24;
            this.CHK_DeleteNicknames.Text = "Delete Nicknames";
            this.CHK_DeleteNicknames.UseVisualStyleBackColor = true;
            // 
            // CHK_Reroll
            // 
            this.CHK_Reroll.AutoSize = true;
            this.CHK_Reroll.Location = new System.Drawing.Point(275, 60);
            this.CHK_Reroll.Name = "CHK_Reroll";
            this.CHK_Reroll.Size = new System.Drawing.Size(136, 17);
            this.CHK_Reroll.TabIndex = 25;
            this.CHK_Reroll.Text = "Reroll Encryption + PID";
            this.CHK_Reroll.UseVisualStyleBackColor = true;
            // 
            // CHK_Bak
            // 
            this.CHK_Bak.AutoSize = true;
            this.CHK_Bak.Location = new System.Drawing.Point(15, 21);
            this.CHK_Bak.Name = "CHK_Bak";
            this.CHK_Bak.Size = new System.Drawing.Size(105, 17);
            this.CHK_Bak.TabIndex = 27;
            this.CHK_Bak.Text = "Create .bak Files";
            this.CHK_Bak.UseVisualStyleBackColor = true;
            // 
            // CHK_Met
            // 
            this.CHK_Met.AutoSize = true;
            this.CHK_Met.Location = new System.Drawing.Point(610, 127);
            this.CHK_Met.Name = "CHK_Met";
            this.CHK_Met.Size = new System.Drawing.Size(15, 14);
            this.CHK_Met.TabIndex = 28;
            this.CHK_Met.UseVisualStyleBackColor = true;
            this.CHK_Met.CheckedChanged += new System.EventHandler(this.CB_Met_Changed);
            // 
            // CHK_AsEgg
            // 
            this.CHK_AsEgg.AutoSize = true;
            this.CHK_AsEgg.Location = new System.Drawing.Point(79, 172);
            this.CHK_AsEgg.Name = "CHK_AsEgg";
            this.CHK_AsEgg.Size = new System.Drawing.Size(60, 17);
            this.CHK_AsEgg.TabIndex = 41;
            this.CHK_AsEgg.Text = "As Egg";
            this.CHK_AsEgg.UseVisualStyleBackColor = true;
            // 
            // CHK_Fateful
            // 
            this.CHK_Fateful.AutoSize = true;
            this.CHK_Fateful.Location = new System.Drawing.Point(82, 127);
            this.CHK_Fateful.Name = "CHK_Fateful";
            this.CHK_Fateful.Size = new System.Drawing.Size(110, 17);
            this.CHK_Fateful.TabIndex = 40;
            this.CHK_Fateful.Text = "Fateful Encounter";
            this.CHK_Fateful.UseVisualStyleBackColor = true;
            // 
            // GB_EggConditions
            // 
            this.GB_EggConditions.Controls.Add(this.CB_EggLocation);
            this.GB_EggConditions.Controls.Add(this.CAL_EggDate);
            this.GB_EggConditions.Controls.Add(this.Label_EggDate);
            this.GB_EggConditions.Controls.Add(this.Label_EggLocation);
            this.GB_EggConditions.Enabled = false;
            this.GB_EggConditions.Location = new System.Drawing.Point(11, 194);
            this.GB_EggConditions.Name = "GB_EggConditions";
            this.GB_EggConditions.Size = new System.Drawing.Size(196, 67);
            this.GB_EggConditions.TabIndex = 42;
            this.GB_EggConditions.TabStop = false;
            this.GB_EggConditions.Text = "Egg Met Conditions";
            // 
            // CB_EggLocation
            // 
            this.CB_EggLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_EggLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_EggLocation.DropDownWidth = 150;
            this.CB_EggLocation.FormattingEnabled = true;
            this.CB_EggLocation.Location = new System.Drawing.Point(71, 19);
            this.CB_EggLocation.Name = "CB_EggLocation";
            this.CB_EggLocation.Size = new System.Drawing.Size(122, 21);
            this.CB_EggLocation.TabIndex = 11;
            // 
            // CAL_EggDate
            // 
            this.CAL_EggDate.CustomFormat = "MM/dd/yyyy";
            this.CAL_EggDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.CAL_EggDate.Location = new System.Drawing.Point(71, 40);
            this.CAL_EggDate.MaxDate = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            this.CAL_EggDate.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.CAL_EggDate.Name = "CAL_EggDate";
            this.CAL_EggDate.Size = new System.Drawing.Size(122, 20);
            this.CAL_EggDate.TabIndex = 11;
            this.CAL_EggDate.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            // 
            // Label_EggDate
            // 
            this.Label_EggDate.Location = new System.Drawing.Point(5, 44);
            this.Label_EggDate.Name = "Label_EggDate";
            this.Label_EggDate.Size = new System.Drawing.Size(63, 13);
            this.Label_EggDate.TabIndex = 8;
            this.Label_EggDate.Text = "Date:";
            this.Label_EggDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EggLocation
            // 
            this.Label_EggLocation.Location = new System.Drawing.Point(5, 24);
            this.Label_EggLocation.Name = "Label_EggLocation";
            this.Label_EggLocation.Size = new System.Drawing.Size(63, 13);
            this.Label_EggLocation.TabIndex = 6;
            this.Label_EggLocation.Text = "Location:";
            this.Label_EggLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EncounterType
            // 
            this.Label_EncounterType.Enabled = false;
            this.Label_EncounterType.Location = new System.Drawing.Point(2, 150);
            this.Label_EncounterType.Name = "Label_EncounterType";
            this.Label_EncounterType.Size = new System.Drawing.Size(77, 13);
            this.Label_EncounterType.TabIndex = 37;
            this.Label_EncounterType.Text = "Encounter:";
            this.Label_EncounterType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MetDate
            // 
            this.Label_MetDate.Location = new System.Drawing.Point(2, 105);
            this.Label_MetDate.Name = "Label_MetDate";
            this.Label_MetDate.Size = new System.Drawing.Size(77, 13);
            this.Label_MetDate.TabIndex = 35;
            this.Label_MetDate.Text = "Met Date:";
            this.Label_MetDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MetLevel
            // 
            this.Label_MetLevel.Location = new System.Drawing.Point(2, 85);
            this.Label_MetLevel.Name = "Label_MetLevel";
            this.Label_MetLevel.Size = new System.Drawing.Size(77, 13);
            this.Label_MetLevel.TabIndex = 33;
            this.Label_MetLevel.Text = "Met Level:";
            this.Label_MetLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Ball
            // 
            this.Label_Ball.Location = new System.Drawing.Point(2, 64);
            this.Label_Ball.Name = "Label_Ball";
            this.Label_Ball.Size = new System.Drawing.Size(77, 13);
            this.Label_Ball.TabIndex = 32;
            this.Label_Ball.Text = "Ball:";
            this.Label_Ball.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MetLocation
            // 
            this.Label_MetLocation.Location = new System.Drawing.Point(2, 42);
            this.Label_MetLocation.Name = "Label_MetLocation";
            this.Label_MetLocation.Size = new System.Drawing.Size(77, 13);
            this.Label_MetLocation.TabIndex = 30;
            this.Label_MetLocation.Text = "Met Location:";
            this.Label_MetLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_OriginGame
            // 
            this.Label_OriginGame.Location = new System.Drawing.Point(2, 23);
            this.Label_OriginGame.Name = "Label_OriginGame";
            this.Label_OriginGame.Size = new System.Drawing.Size(77, 13);
            this.Label_OriginGame.TabIndex = 29;
            this.Label_OriginGame.Text = "Origin Game:";
            this.Label_OriginGame.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_GameOrigin
            // 
            this.CB_GameOrigin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_GameOrigin.FormattingEnabled = true;
            this.CB_GameOrigin.Location = new System.Drawing.Point(82, 19);
            this.CB_GameOrigin.Name = "CB_GameOrigin";
            this.CB_GameOrigin.Size = new System.Drawing.Size(122, 21);
            this.CB_GameOrigin.TabIndex = 31;
            // 
            // CB_MetLocation
            // 
            this.CB_MetLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_MetLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_MetLocation.DropDownWidth = 150;
            this.CB_MetLocation.FormattingEnabled = true;
            this.CB_MetLocation.Location = new System.Drawing.Point(82, 39);
            this.CB_MetLocation.Name = "CB_MetLocation";
            this.CB_MetLocation.Size = new System.Drawing.Size(122, 21);
            this.CB_MetLocation.TabIndex = 43;
            // 
            // CB_Ball
            // 
            this.CB_Ball.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_Ball.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Ball.FormattingEnabled = true;
            this.CB_Ball.Location = new System.Drawing.Point(82, 60);
            this.CB_Ball.Name = "CB_Ball";
            this.CB_Ball.Size = new System.Drawing.Size(122, 21);
            this.CB_Ball.TabIndex = 34;
            // 
            // TB_MetLevel
            // 
            this.TB_MetLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_MetLevel.Location = new System.Drawing.Point(82, 82);
            this.TB_MetLevel.Mask = "000";
            this.TB_MetLevel.Name = "TB_MetLevel";
            this.TB_MetLevel.Size = new System.Drawing.Size(122, 20);
            this.TB_MetLevel.TabIndex = 36;
            // 
            // CAL_MetDate
            // 
            this.CAL_MetDate.CustomFormat = "MM/dd/yyyy";
            this.CAL_MetDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.CAL_MetDate.Location = new System.Drawing.Point(82, 102);
            this.CAL_MetDate.MaxDate = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            this.CAL_MetDate.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.CAL_MetDate.Name = "CAL_MetDate";
            this.CAL_MetDate.Size = new System.Drawing.Size(122, 20);
            this.CAL_MetDate.TabIndex = 38;
            this.CAL_MetDate.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            // 
            // CB_EncounterType
            // 
            this.CB_EncounterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_EncounterType.DropDownWidth = 140;
            this.CB_EncounterType.FormattingEnabled = true;
            this.CB_EncounterType.Location = new System.Drawing.Point(82, 147);
            this.CB_EncounterType.Name = "CB_EncounterType";
            this.CB_EncounterType.Size = new System.Drawing.Size(122, 21);
            this.CB_EncounterType.TabIndex = 39;
            // 
            // GB_Met
            // 
            this.GB_Met.Controls.Add(this.checkBox15);
            this.GB_Met.Controls.Add(this.checkBox14);
            this.GB_Met.Controls.Add(this.checkBox13);
            this.GB_Met.Controls.Add(this.checkBox12);
            this.GB_Met.Controls.Add(this.checkBox11);
            this.GB_Met.Controls.Add(this.checkBox10);
            this.GB_Met.Controls.Add(this.checkBox9);
            this.GB_Met.Controls.Add(this.checkBox8);
            this.GB_Met.Controls.Add(this.checkBox7);
            this.GB_Met.Controls.Add(this.checkBox6);
            this.GB_Met.Controls.Add(this.CB_GameOrigin);
            this.GB_Met.Controls.Add(this.CHK_AsEgg);
            this.GB_Met.Controls.Add(this.CB_EncounterType);
            this.GB_Met.Controls.Add(this.CHK_Fateful);
            this.GB_Met.Controls.Add(this.CAL_MetDate);
            this.GB_Met.Controls.Add(this.GB_EggConditions);
            this.GB_Met.Controls.Add(this.TB_MetLevel);
            this.GB_Met.Controls.Add(this.Label_EncounterType);
            this.GB_Met.Controls.Add(this.CB_Ball);
            this.GB_Met.Controls.Add(this.Label_MetDate);
            this.GB_Met.Controls.Add(this.CB_MetLocation);
            this.GB_Met.Controls.Add(this.Label_MetLevel);
            this.GB_Met.Controls.Add(this.Label_OriginGame);
            this.GB_Met.Controls.Add(this.Label_Ball);
            this.GB_Met.Controls.Add(this.Label_MetLocation);
            this.GB_Met.Location = new System.Drawing.Point(625, 131);
            this.GB_Met.Name = "GB_Met";
            this.GB_Met.Size = new System.Drawing.Size(231, 267);
            this.GB_Met.TabIndex = 44;
            this.GB_Met.TabStop = false;
            this.GB_Met.Text = "Change Met to";
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Checked = true;
            this.checkBox15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox15.Location = new System.Drawing.Point(210, 237);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(15, 14);
            this.checkBox15.TabIndex = 53;
            this.checkBox15.UseVisualStyleBackColor = true;
            this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBox15_CheckedChanged);
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Checked = true;
            this.checkBox14.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox14.Location = new System.Drawing.Point(210, 217);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(15, 14);
            this.checkBox14.TabIndex = 52;
            this.checkBox14.UseVisualStyleBackColor = true;
            this.checkBox14.CheckedChanged += new System.EventHandler(this.checkBox14_CheckedChanged);
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Checked = true;
            this.checkBox13.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox13.Location = new System.Drawing.Point(210, 173);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(15, 14);
            this.checkBox13.TabIndex = 51;
            this.checkBox13.UseVisualStyleBackColor = true;
            this.checkBox13.CheckedChanged += new System.EventHandler(this.checkBox13_CheckedChanged);
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Checked = true;
            this.checkBox12.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox12.Location = new System.Drawing.Point(210, 150);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(15, 14);
            this.checkBox12.TabIndex = 50;
            this.checkBox12.UseVisualStyleBackColor = true;
            this.checkBox12.CheckedChanged += new System.EventHandler(this.checkBox12_CheckedChanged);
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Checked = true;
            this.checkBox11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox11.Location = new System.Drawing.Point(210, 128);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(15, 14);
            this.checkBox11.TabIndex = 49;
            this.checkBox11.UseVisualStyleBackColor = true;
            this.checkBox11.CheckedChanged += new System.EventHandler(this.checkBox11_CheckedChanged);
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Checked = true;
            this.checkBox10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox10.Location = new System.Drawing.Point(210, 105);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(15, 14);
            this.checkBox10.TabIndex = 48;
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Checked = true;
            this.checkBox9.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox9.Location = new System.Drawing.Point(210, 85);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(15, 14);
            this.checkBox9.TabIndex = 47;
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Checked = true;
            this.checkBox8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox8.Location = new System.Drawing.Point(210, 63);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(15, 14);
            this.checkBox8.TabIndex = 46;
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Checked = true;
            this.checkBox7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox7.Location = new System.Drawing.Point(210, 43);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(15, 14);
            this.checkBox7.TabIndex = 45;
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.Location = new System.Drawing.Point(210, 23);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(15, 14);
            this.checkBox6.TabIndex = 44;
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox19);
            this.groupBox1.Controls.Add(this.checkBox18);
            this.groupBox1.Controls.Add(this.checkBox17);
            this.groupBox1.Controls.Add(this.checkBox16);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(626, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 100);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Change OT to";
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Checked = true;
            this.checkBox19.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox19.Location = new System.Drawing.Point(209, 77);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(15, 14);
            this.checkBox19.TabIndex = 56;
            this.checkBox19.UseVisualStyleBackColor = true;
            this.checkBox19.CheckedChanged += new System.EventHandler(this.checkBox19_CheckedChanged);
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Checked = true;
            this.checkBox18.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox18.Location = new System.Drawing.Point(209, 36);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(15, 14);
            this.checkBox18.TabIndex = 55;
            this.checkBox18.UseVisualStyleBackColor = true;
            this.checkBox18.CheckedChanged += new System.EventHandler(this.checkBox18_CheckedChanged);
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Checked = true;
            this.checkBox17.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox17.Location = new System.Drawing.Point(99, 77);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(15, 14);
            this.checkBox17.TabIndex = 54;
            this.checkBox17.UseVisualStyleBackColor = true;
            this.checkBox17.CheckedChanged += new System.EventHandler(this.checkBox17_CheckedChanged);
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Checked = true;
            this.checkBox16.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox16.Location = new System.Drawing.Point(99, 36);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(15, 14);
            this.checkBox16.TabIndex = 53;
            this.checkBox16.UseVisualStyleBackColor = true;
            this.checkBox16.CheckedChanged += new System.EventHandler(this.checkBox16_CheckedChanged);
            // 
            // CHK_Frienship
            // 
            this.CHK_Frienship.AutoSize = true;
            this.CHK_Frienship.Location = new System.Drawing.Point(9, 27);
            this.CHK_Frienship.Name = "CHK_Frienship";
            this.CHK_Frienship.Size = new System.Drawing.Size(71, 17);
            this.CHK_Frienship.TabIndex = 46;
            this.CHK_Frienship.Text = "Frienship:";
            this.CHK_Frienship.UseVisualStyleBackColor = true;
            // 
            // CHK_Level
            // 
            this.CHK_Level.AutoSize = true;
            this.CHK_Level.Location = new System.Drawing.Point(9, 62);
            this.CHK_Level.Name = "CHK_Level";
            this.CHK_Level.Size = new System.Drawing.Size(55, 17);
            this.CHK_Level.TabIndex = 47;
            this.CHK_Level.Text = "Level:";
            this.CHK_Level.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.CHK_Frienship);
            this.groupBox2.Controls.Add(this.CHK_Level);
            this.groupBox2.Controls.Add(this.CHK_Unshiny);
            this.groupBox2.Controls.Add(this.CHK_Shiny);
            this.groupBox2.Controls.Add(this.CHK_Perfect_IVs);
            this.groupBox2.Controls.Add(this.CHK_DeleteNicknames);
            this.groupBox2.Controls.Add(this.CHK_Reroll);
            this.groupBox2.Location = new System.Drawing.Point(164, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(418, 100);
            this.groupBox2.TabIndex = 48;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Set";
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.Location = new System.Drawing.Point(78, 59);
            this.textBox6.Mask = "000";
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(22, 20);
            this.textBox6.TabIndex = 51;
            this.textBox6.Text = "100";
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox5.Location = new System.Drawing.Point(78, 25);
            this.textBox5.Mask = "000";
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(22, 20);
            this.textBox5.TabIndex = 50;
            this.textBox5.Text = "255";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.CHK_Bak);
            this.groupBox3.Location = new System.Drawing.Point(13, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(131, 48);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            // 
            // B_Mass_Edit
            // 
            this.B_Mass_Edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Mass_Edit.Location = new System.Drawing.Point(13, 65);
            this.B_Mass_Edit.Name = "B_Mass_Edit";
            this.B_Mass_Edit.Size = new System.Drawing.Size(131, 47);
            this.B_Mass_Edit.TabIndex = 28;
            this.B_Mass_Edit.Text = "Mass Edit";
            this.B_Mass_Edit.UseVisualStyleBackColor = true;
            this.B_Mass_Edit.Click += new System.EventHandler(this.B_Mass_Edit_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox5);
            this.groupBox4.Controls.Add(this.checkBox4);
            this.groupBox4.Controls.Add(this.checkBox3);
            this.groupBox4.Controls.Add(this.checkBox2);
            this.groupBox4.Controls.Add(this.CB_Language);
            this.groupBox4.Controls.Add(this.CB_3DSReg);
            this.groupBox4.Controls.Add(this.CB_SubRegion);
            this.groupBox4.Controls.Add(this.CB_Country);
            this.groupBox4.Controls.Add(this.Label_3DSRegion);
            this.groupBox4.Controls.Add(this.Label_SubRegion);
            this.groupBox4.Controls.Add(this.Label_Country);
            this.groupBox4.Controls.Add(this.Label_Language);
            this.groupBox4.Location = new System.Drawing.Point(439, 131);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(165, 267);
            this.groupBox4.TabIndex = 50;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Change Country to";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Location = new System.Drawing.Point(134, 220);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(15, 14);
            this.checkBox5.TabIndex = 35;
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(134, 162);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(15, 14);
            this.checkBox4.TabIndex = 34;
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(134, 106);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(15, 14);
            this.checkBox3.TabIndex = 33;
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(135, 51);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 32;
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // CB_Language
            // 
            this.CB_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Language.FormattingEnabled = true;
            this.CB_Language.Location = new System.Drawing.Point(6, 48);
            this.CB_Language.Name = "CB_Language";
            this.CB_Language.Size = new System.Drawing.Size(122, 21);
            this.CB_Language.TabIndex = 25;
            // 
            // CB_3DSReg
            // 
            this.CB_3DSReg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_3DSReg.FormattingEnabled = true;
            this.CB_3DSReg.Location = new System.Drawing.Point(6, 218);
            this.CB_3DSReg.Name = "CB_3DSReg";
            this.CB_3DSReg.Size = new System.Drawing.Size(122, 21);
            this.CB_3DSReg.TabIndex = 31;
            // 
            // CB_SubRegion
            // 
            this.CB_SubRegion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_SubRegion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_SubRegion.DropDownWidth = 180;
            this.CB_SubRegion.FormattingEnabled = true;
            this.CB_SubRegion.Location = new System.Drawing.Point(6, 159);
            this.CB_SubRegion.Name = "CB_SubRegion";
            this.CB_SubRegion.Size = new System.Drawing.Size(122, 21);
            this.CB_SubRegion.TabIndex = 30;
            // 
            // CB_Country
            // 
            this.CB_Country.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_Country.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Country.DropDownWidth = 180;
            this.CB_Country.FormattingEnabled = true;
            this.CB_Country.Location = new System.Drawing.Point(6, 103);
            this.CB_Country.Name = "CB_Country";
            this.CB_Country.Size = new System.Drawing.Size(122, 21);
            this.CB_Country.TabIndex = 29;
            // 
            // Label_3DSRegion
            // 
            this.Label_3DSRegion.Location = new System.Drawing.Point(17, 201);
            this.Label_3DSRegion.Name = "Label_3DSRegion";
            this.Label_3DSRegion.Size = new System.Drawing.Size(77, 13);
            this.Label_3DSRegion.TabIndex = 28;
            this.Label_3DSRegion.Text = "3DS Region:";
            this.Label_3DSRegion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_SubRegion
            // 
            this.Label_SubRegion.Location = new System.Drawing.Point(17, 143);
            this.Label_SubRegion.Name = "Label_SubRegion";
            this.Label_SubRegion.Size = new System.Drawing.Size(77, 13);
            this.Label_SubRegion.TabIndex = 27;
            this.Label_SubRegion.Text = "Sub Region:";
            this.Label_SubRegion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Country
            // 
            this.Label_Country.Location = new System.Drawing.Point(3, 85);
            this.Label_Country.Name = "Label_Country";
            this.Label_Country.Size = new System.Drawing.Size(77, 13);
            this.Label_Country.TabIndex = 26;
            this.Label_Country.Text = "Country:";
            this.Label_Country.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Language
            // 
            this.Label_Language.Location = new System.Drawing.Point(17, 31);
            this.Label_Language.Name = "Label_Language";
            this.Label_Language.Size = new System.Drawing.Size(77, 13);
            this.Label_Language.TabIndex = 24;
            this.Label_Language.Text = "Language:";
            this.Label_Language.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CHK_Country
            // 
            this.CHK_Country.AutoSize = true;
            this.CHK_Country.Location = new System.Drawing.Point(422, 127);
            this.CHK_Country.Name = "CHK_Country";
            this.CHK_Country.Size = new System.Drawing.Size(15, 14);
            this.CHK_Country.TabIndex = 51;
            this.CHK_Country.UseVisualStyleBackColor = true;
            this.CHK_Country.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // OverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 405);
            this.Controls.Add(this.CHK_Country);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.B_Mass_Edit);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GB_Met);
            this.Controls.Add(this.CHK_Met);
            this.Controls.Add(this.CHK_ChangeOT);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "OverForm";
            this.Text = "Mass Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OverForm_FormClosed);
            this.GB_EggConditions.ResumeLayout(false);
            this.GB_Met.ResumeLayout(false);
            this.GB_Met.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void OverForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            if (thread2 != null)
            {
                if (thread2.IsAlive)
                {
                    thread2.Abort();
                }
            }
        }

       
    }
}
