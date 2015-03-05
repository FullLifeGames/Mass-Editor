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
        MemoryAmie ma;

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
            textBox5.Enabled = b;
            textBox6.Enabled = b;
            listView1.Enabled = b;
            groupBox1.Enabled = b;
            groupBox2.Enabled = b;
            groupBox3.Enabled = b;
            GB_Met.Enabled = b;
            groupBox4.Enabled = b;
            CHK_Country.Enabled = b;
            groupBox5.Enabled = b;
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
            textBox5.Enabled = b;
            textBox6.Enabled = b;
            listView1.Enabled = b;
            groupBox1.Enabled = CHK_ChangeOT.Checked;
            groupBox2.Enabled = b;
            groupBox3.Enabled = b;
            GB_Met.Enabled = CHK_Met.Checked;
            groupBox4.Enabled = CHK_Country.Checked;
            CHK_Country.Enabled = b;
            groupBox5.Enabled = CHK_Memories.Checked;
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
                bool[] otbool = { checkBox16.Checked, checkBox18.Checked, checkBox17.Checked, checkBox19.Checked };
                int[] otindexes = { CB_Language.SelectedIndex, CB_Country.SelectedIndex, CB_SubRegion.SelectedIndex, CB_3DSReg.SelectedIndex };
                List<string> litems = new List<string>();
                foreach (ListViewItem l in listView1.Items)
                {
                    litems.Add(l.Text);
                }

                bool[] amienabled = { M_OT_Friendship.Enabled, M_OT_Affection.Enabled, CB_OTMemory.Enabled && CB_OTMemory.Visible, CB_OTVar.Enabled && CB_OTVar.Visible, CB_OTQual.Enabled && CB_OTQual.Visible, CB_OTFeel.Enabled && CB_OTFeel.Visible,
                                    M_CT_Friendship.Enabled, M_CT_Affection.Enabled, CB_CTMemory.Enabled && CB_CTMemory.Visible, CB_CTVar.Enabled && CB_CTVar.Visible, CB_CTQual.Enabled && CB_CTQual.Visible, CB_CTFeel.Enabled && CB_CTFeel.Visible,
                                    (CB_Country0.SelectedIndex != -1) && CB_Country0.Enabled, (CB_Country1.SelectedIndex != -1) && CB_Country1.Enabled, (CB_Country2.SelectedIndex != -1) && CB_Country2.Enabled, (CB_Country3.SelectedIndex != -1) && CB_Country3.Enabled, (CB_Country4.SelectedIndex != -1) && CB_Country4.Enabled, 
                                    (Region0.Items.Count > 1) && Region0.Enabled, (Region1.Items.Count > 1) && Region1.Enabled, (Region2.Items.Count > 1) && Region2.Enabled, (Region3.Items.Count > 1) && Region3.Enabled, (Region4.Items.Count > 1) && Region4.Enabled,
                                    CB_Handler.Enabled, M_Fullness.Enabled, M_Enjoyment.Enabled };
                int[] amiindex = { int.Parse(M_OT_Friendship.Text), int.Parse(M_OT_Affection.Text), CB_OTMemory.SelectedIndex, CB_OTVar.SelectedIndex, CB_OTQual.SelectedIndex, CB_OTFeel.SelectedIndex,
                                    int.Parse(M_CT_Friendship.Text), int.Parse(M_CT_Affection.Text), CB_CTMemory.SelectedIndex, CB_CTVar.SelectedIndex, CB_CTQual.SelectedIndex, CB_CTFeel.SelectedIndex,
                                    CB_Country0.SelectedIndex, CB_Country1.SelectedIndex, CB_Country2.SelectedIndex, CB_Country3.SelectedIndex, CB_Country4.SelectedIndex, 
                                    Region0.SelectedIndex, Region1.SelectedIndex, Region2.SelectedIndex, Region3.SelectedIndex, Region4.SelectedIndex,
                                    CB_Handler.SelectedIndex, int.Parse(M_Fullness.Text), int.Parse(M_Enjoyment.Text) };


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
                if (CHK_Memories.Checked)
                {
                    modes.Add(10);
                }

                // thread for free UI
                thread = new Thread(delegate() { Form1 f1 = new Form1(litems, modes, this.progressBar1, ret, friendship, level, m, bak, otindexes, countrybool, metbool, otbool, amienabled, amiindex); f1.Form1_Load(new object(), new EventArgs()); f1.Dispose(); });
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
            f.TB_OT.Text = "PKHeX";
            f.TB_OTt2.Text = "Mass Edit (Last OT)";
            ma = new MemoryAmie(f);
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
            Label_EggLocation = f.getLabel_EggLocation();
            Label_EggDate = f.getLabel_EggDate();
            tabControl1 = ma.tabControl1;
            M_OT_Friendship = ma.M_OT_Friendship;
            M_OT_Affection = ma.M_OT_Affection;
            CB_OTMemory = ma.CB_OTMemory;
            CB_OTVar = ma.CB_OTVar;
            CB_OTQual = ma.CB_OTQual;
            CB_OTFeel = ma.CB_OTFeel;

            Tab_CTMemory = ma.Tab_CTMemory;
            Tab_OTMemory = ma.Tab_OTMemory;
            Tab_Residence = ma.Tab_Residence;

            M_CT_Friendship = ma.M_CT_Friendship;
            M_CT_Affection = ma.M_CT_Affection;
            CB_CTMemory = ma.CB_CTMemory;
            CB_CTVar = ma.CB_CTVar;
            CB_CTQual = ma.CB_CTQual;
            CB_CTFeel = ma.CB_CTFeel;
            CB_Country0 = ma.CB_Country0;
            CB_Country1 = ma.CB_Country1;
            CB_Country2 = ma.CB_Country2;
            CB_Country3 = ma.CB_Country3;
            CB_Country4 = ma.CB_Country4;
            Region0 = ma.Region0;
            Region1 = ma.Region1;
            Region2 = ma.Region2;
            Region3 = ma.Region3;
            Region4 = ma.Region4;
            CB_Handler = ma.CB_Handler;
            M_Fullness = ma.M_Fullness;
            M_Enjoyment = ma.M_Enjoyment;
            GB_M_CT = ma.GB_M_CT;
            GB_M_OT = ma.GB_M_OT;
            GB_Residence = ma.GB_Residence;
            L_Arguments = ma.L_Arguments;
            L_Handler = ma.L_Handler;
            RTB_CT = ma.RTB_CT;
            RTB_OT = ma.RTB_OT;
            L_CT_Affection = ma.L_CT_Affection;
            L_Country = ma.L_Country;
            L_CT_Feeling = ma.L_CT_Feeling;
            L_CT_Friendship = ma.L_CT_Friendship;
            L_CT_Quality = ma.L_CT_Quality;
            L_CT_TextLine = ma.L_CT_TextLine;
            L_Enjoyment = ma.L_Enjoyment;
            L_Fullness = ma.L_Fullness;
            L_Geo0 = ma.L_Geo0;
            L_Geo1 = ma.L_Geo1;
            L_Geo2 = ma.L_Geo2;
            L_Geo3 = ma.L_Geo3;
            L_Geo4 = ma.L_Geo4;
            L_OT_Affection = ma.L_OT_Affection;
            L_OT_Feeling = ma.L_OT_Feeling;
            L_OT_Friendship = ma.L_OT_Friendship;
            L_OT_Quality = ma.L_OT_Quality;
            L_OT_TextLine = ma.L_OT_TextLine;
            L_Region = ma.L_Region;
            LOTV = ma.LOTV;
            LCTV = ma.LCTV;

            ComboBox[] mta = new ComboBox[]
            {
                Region0, Region1, Region2, Region3, Region4,
            };
            // doesn't work
            // mta[index].SelectedValue = 0;
            for (int i = 0; i < 5; i++)
            {
                mta[i].DataSource = new[] { new { Text = "", Value = 0 } };
                mta[i].DisplayMember = "Text";
                mta[i].ValueMember = "Value";
            }

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


      //      this.Label_EggDate = new System.Windows.Forms.Label();
      //      this.Label_EggLocation = new System.Windows.Forms.Label();
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
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
            this.CHK_Memories = new System.Windows.Forms.CheckBox();

            #region DesignCode
            this.GB_EggConditions.SuspendLayout();
            this.GB_Met.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Tab_OTMemory.SuspendLayout();
            this.GB_M_OT.SuspendLayout();
            this.Tab_CTMemory.SuspendLayout();
            this.GB_M_CT.SuspendLayout();
            this.Tab_Residence.SuspendLayout();
            this.GB_Residence.SuspendLayout();
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
   //         this.GB_EggConditions.Controls.Add(this.Label_EggDate);
   //         this.GB_EggConditions.Controls.Add(this.Label_EggLocation);
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
            this.Label_EggDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label_EggLocation
            // 
            this.Label_EggLocation.Location = new System.Drawing.Point(5, 24);
            this.Label_EggLocation.Name = "Label_EggLocation";
            this.Label_EggLocation.Size = new System.Drawing.Size(63, 13);
            this.Label_EggLocation.TabIndex = 6;
            this.Label_EggLocation.Text = "Location:";
            this.Label_EggLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.L_Arguments);
            this.groupBox5.Controls.Add(this.L_Handler);
            this.groupBox5.Controls.Add(this.tabControl1);
            this.groupBox5.Controls.Add(this.CB_Handler);
            this.groupBox5.Controls.Add(this.L_Enjoyment);
            this.groupBox5.Controls.Add(this.L_Fullness);
            this.groupBox5.Controls.Add(this.M_Fullness);
            this.groupBox5.Controls.Add(this.M_Enjoyment);
            this.groupBox5.Location = new System.Drawing.Point(878, 7);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(385, 391);
            this.groupBox5.TabIndex = 52;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Change MemoryAmie to";
            // 
            // L_Arguments
            // 
            this.L_Arguments.AutoSize = true;
            this.L_Arguments.Location = new System.Drawing.Point(337, 46);
            this.L_Arguments.Name = "L_Arguments";
            this.L_Arguments.Size = new System.Drawing.Size(33, 13);
            this.L_Arguments.TabIndex = 112;
            this.L_Arguments.Text = "(args)";
            this.L_Arguments.Visible = false;
            // 
            // L_Handler
            // 
            this.L_Handler.Location = new System.Drawing.Point(43, 284);
            this.L_Handler.Name = "L_Handler";
            this.L_Handler.Size = new System.Drawing.Size(120, 13);
            this.L_Handler.TabIndex = 111;
            this.L_Handler.Text = "Current Handler:";
            this.L_Handler.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(20, 49);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(355, 228);
            this.tabControl1.TabIndex = 109;
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
            // LOTV
            // 
            this.LOTV.AutoSize = true;
            this.LOTV.Location = new System.Drawing.Point(6, 65);
            this.LOTV.Name = "LOTV";
            this.LOTV.Size = new System.Drawing.Size(62, 13);
            this.LOTV.TabIndex = 83;
            this.LOTV.Text = "VARIABLE:";
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
            // M_OT_Friendship
            // 
            this.M_OT_Friendship.Location = new System.Drawing.Point(86, 16);
            this.M_OT_Friendship.Mask = "000";
            this.M_OT_Friendship.Name = "M_OT_Friendship";
            this.M_OT_Friendship.Size = new System.Drawing.Size(24, 20);
            this.M_OT_Friendship.TabIndex = 23;
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
            // L_OT_Quality
            // 
            this.L_OT_Quality.AutoSize = true;
            this.L_OT_Quality.Location = new System.Drawing.Point(6, 91);
            this.L_OT_Quality.Name = "L_OT_Quality";
            this.L_OT_Quality.Size = new System.Drawing.Size(49, 13);
            this.L_OT_Quality.TabIndex = 80;
            this.L_OT_Quality.Text = "Intensity:";
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
            // 
            // M_CT_Friendship
            // 
            this.M_CT_Friendship.Location = new System.Drawing.Point(86, 16);
            this.M_CT_Friendship.Mask = "000";
            this.M_CT_Friendship.Name = "M_CT_Friendship";
            this.M_CT_Friendship.Size = new System.Drawing.Size(24, 20);
            this.M_CT_Friendship.TabIndex = 15;
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
            // 
            // CB_Country3
            // 
            this.CB_Country3.DropDownWidth = 180;
            this.CB_Country3.FormattingEnabled = true;
            this.CB_Country3.Location = new System.Drawing.Point(82, 126);
            this.CB_Country3.Name = "CB_Country3";
            this.CB_Country3.Size = new System.Drawing.Size(102, 21);
            this.CB_Country3.TabIndex = 82;
            // 
            // CB_Country2
            // 
            this.CB_Country2.DropDownWidth = 180;
            this.CB_Country2.FormattingEnabled = true;
            this.CB_Country2.Location = new System.Drawing.Point(82, 93);
            this.CB_Country2.Name = "CB_Country2";
            this.CB_Country2.Size = new System.Drawing.Size(102, 21);
            this.CB_Country2.TabIndex = 81;
            // 
            // CB_Country1
            // 
            this.CB_Country1.DropDownWidth = 180;
            this.CB_Country1.FormattingEnabled = true;
            this.CB_Country1.Location = new System.Drawing.Point(82, 60);
            this.CB_Country1.Name = "CB_Country1";
            this.CB_Country1.Size = new System.Drawing.Size(102, 21);
            this.CB_Country1.TabIndex = 80;
            // 
            // CB_Country0
            // 
            this.CB_Country0.DropDownWidth = 180;
            this.CB_Country0.FormattingEnabled = true;
            this.CB_Country0.Location = new System.Drawing.Point(82, 27);
            this.CB_Country0.Name = "CB_Country0";
            this.CB_Country0.Size = new System.Drawing.Size(102, 21);
            this.CB_Country0.TabIndex = 79;
            // 
            // L_Geo4
            // 
            this.L_Geo4.Location = new System.Drawing.Point(-1, 162);
            this.L_Geo4.Name = "L_Geo4";
            this.L_Geo4.Size = new System.Drawing.Size(80, 13);
            this.L_Geo4.TabIndex = 78;
            this.L_Geo4.Text = "Past 4:";
            this.L_Geo4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Geo3
            // 
            this.L_Geo3.Location = new System.Drawing.Point(0, 130);
            this.L_Geo3.Name = "L_Geo3";
            this.L_Geo3.Size = new System.Drawing.Size(80, 13);
            this.L_Geo3.TabIndex = 77;
            this.L_Geo3.Text = "Past 3:";
            this.L_Geo3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Geo2
            // 
            this.L_Geo2.Location = new System.Drawing.Point(0, 97);
            this.L_Geo2.Name = "L_Geo2";
            this.L_Geo2.Size = new System.Drawing.Size(80, 13);
            this.L_Geo2.TabIndex = 76;
            this.L_Geo2.Text = "Past 2:";
            this.L_Geo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // L_Region
            // 
            this.L_Region.Location = new System.Drawing.Point(226, 12);
            this.L_Region.Name = "L_Region";
            this.L_Region.Size = new System.Drawing.Size(80, 13);
            this.L_Region.TabIndex = 73;
            this.L_Region.Text = "Region";
            this.L_Region.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Geo1
            // 
            this.L_Geo1.Location = new System.Drawing.Point(0, 64);
            this.L_Geo1.Name = "L_Geo1";
            this.L_Geo1.Size = new System.Drawing.Size(80, 13);
            this.L_Geo1.TabIndex = 69;
            this.L_Geo1.Text = "Past 1:";
            this.L_Geo1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Geo0
            // 
            this.L_Geo0.Location = new System.Drawing.Point(-1, 31);
            this.L_Geo0.Name = "L_Geo0";
            this.L_Geo0.Size = new System.Drawing.Size(80, 13);
            this.L_Geo0.TabIndex = 68;
            this.L_Geo0.Text = "Latest:";
            this.L_Geo0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_Handler
            // 
            this.CB_Handler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Handler.Enabled = true;
            this.CB_Handler.FormattingEnabled = true;
            this.CB_Handler.Location = new System.Drawing.Point(174, 281);
            this.CB_Handler.Name = "CB_Handler";
            this.CB_Handler.Size = new System.Drawing.Size(117, 21);
            this.CB_Handler.TabIndex = 110;

            int xmove = 80;
            // 
            // L_Enjoyment
            // 
            this.L_Enjoyment.Location = new System.Drawing.Point(93+xmove, 313);
            this.L_Enjoyment.Name = "L_Enjoyment";
            this.L_Enjoyment.Size = new System.Drawing.Size(70, 13);
            this.L_Enjoyment.TabIndex = 108;
            this.L_Enjoyment.Text = "Enjoyment:";
            this.L_Enjoyment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // L_Fullness
            // 
            this.L_Fullness.Location = new System.Drawing.Point(0 + xmove, 313);
            this.L_Fullness.Name = "L_Fullness";
            this.L_Fullness.Size = new System.Drawing.Size(60, 13);
            this.L_Fullness.TabIndex = 107;
            this.L_Fullness.Text = "Fullness:";
            this.L_Fullness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // M_Fullness
            // 
            this.M_Fullness.Location = new System.Drawing.Point(63 + xmove, 310);
            this.M_Fullness.Mask = "000";
            this.M_Fullness.Name = "M_Fullness";
            this.M_Fullness.Size = new System.Drawing.Size(24, 20);
            this.M_Fullness.TabIndex = 103;
            this.M_Fullness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // M_Enjoyment
            // 
            this.M_Enjoyment.Location = new System.Drawing.Point(166 + xmove, 310);
            this.M_Enjoyment.Mask = "000";
            this.M_Enjoyment.Name = "M_Enjoyment";
            this.M_Enjoyment.Size = new System.Drawing.Size(24, 20);
            this.M_Enjoyment.TabIndex = 104;
            this.M_Enjoyment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CHK_Memories
            // 
            this.CHK_Memories.AutoSize = true;
            this.CHK_Memories.Checked = false;
            this.CHK_Memories.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.CHK_Memories.Location = new System.Drawing.Point(862, 10);
            this.CHK_Memories.Name = "CHK_Memories";
            this.CHK_Memories.Size = new System.Drawing.Size(15, 14);
            this.CHK_Memories.TabIndex = 57;
            this.CHK_Memories.UseVisualStyleBackColor = true;
            this.CHK_Memories.CheckedChanged += new System.EventHandler(this.CHK_Memories_CheckedChanged);
            // 
            // OverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 405);
            this.Controls.Add(this.CHK_Memories);
            this.Controls.Add(this.groupBox5);
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
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.Tab_OTMemory.ResumeLayout(false);
            this.GB_M_OT.ResumeLayout(false);
            this.GB_M_OT.PerformLayout();
            this.Tab_CTMemory.ResumeLayout(false);
            this.GB_M_CT.ResumeLayout(false);
            this.GB_M_CT.PerformLayout();
            this.Tab_Residence.ResumeLayout(false);
            this.GB_Residence.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
            #endregion
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

        private void CHK_Memories_CheckedChanged(object sender, EventArgs e)
        {
            groupBox5.Enabled = CHK_Memories.Checked;
        }

       
    }
}
