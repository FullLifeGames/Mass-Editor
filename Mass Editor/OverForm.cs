using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mass_Editor
{
    public partial class OverForm : Form
    {

        #region Global Variables: Always Visible!
        private Form1 f;
        private MemoryAmie ma;
        private RibbMedal badge;
        private bool running = false;
        private Thread thread = null;
        private Thread thread2 = null;
        private bool switchChecks = true;
        #endregion

        public OverForm()
        {
            // Using another Initialize Method to use objects from another Form
            //    InitializeComponent();
            InitializeComponents();
            string filename = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (filename.IndexOf("Mess") >= 0)
            {
                this.Text = "Mess Editor";
                Util.Alert("Illegal mode activated.", "Please behave.");
            }
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

        #region Enable & Disable
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
            CHK_Memories.Enabled = b;
            CHK_Badges.Enabled = b;
            groupBox6.Enabled = b;
            groupBox7.Enabled = b;
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
            CHK_Memories.Enabled = b;
            CHK_Badges.Enabled = b;
            groupBox6.Enabled = CHK_Badges.Checked;
            groupBox7.Enabled = b;
        }
        #endregion

        private bool filterMemoryBoxes(ComboBox cb)
        {
            return (cb.DataSource != null) && (((List<Util.cbItem>)cb.DataSource).Count > 1);
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
                bool[] otbool = { checkBox16.Checked, checkBox18.Checked, checkBox17.Checked, checkBox19.Checked, checkBox1.Checked, checkBox20.Checked };
                int[] otindexes = { CB_Language.SelectedIndex, CB_Country.SelectedIndex, CB_SubRegion.SelectedIndex, CB_3DSReg.SelectedIndex };
                string[] otgenders = { Label_OTGender.Text, Label_CTGender.Text };
                List<string> litems = new List<string>();
                foreach (ListViewItem l in listView1.Items)
                {
                    litems.Add(l.Text);
                }
                bool[] amienabled = { M_OT_Friendship.Enabled, M_OT_Affection.Enabled, CB_OTMemory.Enabled && CB_OTMemory.Visible, filterMemoryBoxes(CB_OTVar), CB_OTQual.Visible, CB_OTFeel.Visible,
                                    M_CT_Friendship.Enabled, M_CT_Affection.Enabled, CB_CTMemory.Enabled && CB_CTMemory.Visible, filterMemoryBoxes(CB_CTVar), CB_CTQual.Visible, CB_CTFeel.Visible,
                                    (CB_Country0.SelectedIndex != -1) && CB_Country0.Enabled, (CB_Country1.SelectedIndex != -1) && CB_Country1.Enabled, (CB_Country2.SelectedIndex != -1) && CB_Country2.Enabled, (CB_Country3.SelectedIndex != -1) && CB_Country3.Enabled, (CB_Country4.SelectedIndex != -1) && CB_Country4.Enabled, 
                                    (Region0.Items.Count > 1) && Region0.Enabled, (Region1.Items.Count > 1) && Region1.Enabled, (Region2.Items.Count > 1) && Region2.Enabled, (Region3.Items.Count > 1) && Region3.Enabled, (Region4.Items.Count > 1) && Region4.Enabled,
                                    CB_Handler.Enabled, M_Fullness.Enabled, M_Enjoyment.Enabled };
                int[] amiindex = { (M_OT_Friendship.Text=="")?0:int.Parse(M_OT_Friendship.Text), (M_OT_Affection.Text=="")?0:int.Parse(M_OT_Affection.Text), CB_OTMemory.SelectedIndex, CB_OTVar.SelectedIndex, CB_OTQual.SelectedIndex, CB_OTFeel.SelectedIndex,
                                    (M_CT_Friendship.Text=="")?0:int.Parse(M_CT_Friendship.Text), (M_CT_Affection.Text=="")?0:int.Parse(M_CT_Affection.Text), CB_CTMemory.SelectedIndex, CB_CTVar.SelectedIndex, CB_CTQual.SelectedIndex, CB_CTFeel.SelectedIndex,
                                    CB_Country0.SelectedIndex, CB_Country1.SelectedIndex, CB_Country2.SelectedIndex, CB_Country3.SelectedIndex, CB_Country4.SelectedIndex, 
                                    Region0.SelectedIndex, Region1.SelectedIndex, Region2.SelectedIndex, Region3.SelectedIndex, Region4.SelectedIndex,
                                    CB_Handler.SelectedIndex, (M_Fullness.Text=="")?0:int.Parse(M_Fullness.Text), (M_Enjoyment.Text=="")?0:int.Parse(M_Enjoyment.Text) };

                bool amilite = checkBox23.Checked;
                bool[] amilitebool = { checkBox21.Checked, checkBox22.Checked };
                int[] amiliteint = { (maskedTextBox2.Text == "") ? 0 : int.Parse(maskedTextBox2.Text), (maskedTextBox1.Text == "") ? 0 : int.Parse(maskedTextBox1.Text) };

                bool allintobox = CB_ToBox.Checked;

                Met m = new Met(CB_GameOrigin.SelectedIndex, CB_MetLocation.SelectedIndex, CB_Ball.SelectedIndex, TB_MetLevel.Text, CAL_MetDate.Value, CHK_Fateful.Checked, CB_EncounterType.Enabled, CB_EncounterType.SelectedIndex, CHK_AsEgg.Checked, CB_EggLocation.SelectedIndex, CAL_EggDate.Value);

                bool[] badgeChecks = getBadgeChecks();
                int[] badgeInts = { (TB_PastContest.Text == "") ? 0 : int.Parse(TB_PastContest.Text), (TB_PastBattle.Text == "") ? 0 : int.Parse(TB_PastBattle.Text), comboBox1.SelectedIndex, (int) numericUpDown1.Value };

                bool[] symbolChecks = { CHK_Circle.Checked, CHK_Triangle.Checked, CHK_Square.Checked, CHK_Heart.Checked, CHK_Star.Checked, CHK_Diamond.Checked };

                string[] contestStats = { TB_Cool.Text, TB_Beauty.Text, TB_Cute.Text, TB_Smart.Text, TB_Tough.Text, TB_Sheen.Text };

                string gender = Label_Gender.Text;

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
                if (CHK_PPMax.Checked)
                {
                    modes.Add(11);
                }
                if (CHK_Shiny.Checked)
                {
                    modes.Add(12);
                }
                if (CHK_Badges.Checked)
                {
                    modes.Add(13);
                }
                if (CHK_No_Pokerus.Checked)
                {
                    modes.Add(14);
                }
                if (CHK_Symbols.Checked)
                {
                    modes.Add(15);
                }
                if (CHK_Contest.Checked)
                {
                    modes.Add(16);
                }
                if (CHK_Gender.Checked)
                {
                    modes.Add(17);
                }
                if (CHK_EVS_0.Checked)
                {
                    modes.Add(18);
                }
                string filename = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                // thread for free UI
                thread = new Thread(delegate() { 
                    Form1 f1 = new Form1(litems, modes, this.progressBar1, ret, friendship, level, m, bak, otindexes, countrybool, metbool, otbool, amienabled, amiindex, otgenders, (filename.IndexOf("Mess") >= 0), amilite, amilitebool, amiliteint, allintobox, badgeChecks, badgeInts, symbolChecks, contestStats, gender); 
                    f1.Form1_Load(new object(), new EventArgs()); 
                    f1.Dispose(); 
                });

                thread.SetApartmentState(ApartmentState.STA);

                // thread2 is basically my thread_finished_Eventhandler
                thread2 = new Thread(delegate() { 
                    thread.Join(); 
                    this.BeginInvoke((MethodInvoker)delegate { 
                        enableAll(); running = false; 
                    }); 
                });

                thread.Start();
                thread2.Start();
            }
        }

        private bool[] getBadgeChecks()
        {
            bool[] cba = {
                                   Kalos1a_0.Checked,
								   Kalos1a_1.Checked,
								   Kalos1a_2.Checked,
								   Kalos1a_3.Checked,
								   Kalos1a_4.Checked,
								   Kalos1a_5.Checked,
								   Kalos1a_6.Checked,
								   Kalos1a_7.Checked,
                                   Kalos1b_0.Checked,
								   Kalos1b_1.Checked,
								   Kalos1b_2.Checked,
								   Kalos1b_3.Checked,
								   Kalos1b_4.Checked,
								   Kalos1b_5.Checked,
								   Kalos1b_6.Checked,
								   Kalos1b_7.Checked,
                                   Kalos2a_0.Checked,
								   Kalos2a_1.Checked,
								   Kalos2a_2.Checked,
								   Kalos2a_3.Checked,
								   Kalos2a_4.Checked,
								   Kalos2a_5.Checked,
								   Kalos2a_6.Checked,
								   Kalos2a_7.Checked,
                                   Kalos2b_0.Checked,
								   Kalos2b_1.Checked,
								   Kalos2b_2.Checked,
								   Kalos2b_3.Checked,
								   Kalos2b_4.Checked,
								   Kalos2b_5.Checked,
								   Kalos2b_6.Checked,
								   Kalos2b_7.Checked,
                                   Extra1_0.Checked,
								   Extra1_1.Checked,
								   Extra1_2.Checked,
								   Extra1_3.Checked,
								   Extra1_4.Checked,
                                   Extra1_7.Checked,
								   ORAS_0.Checked,
								   ORAS_1.Checked,
								   ORAS_2.Checked,
								   ORAS_3.Checked,
								   ORAS_4.Checked,
								   ORAS_5.Checked,
                                  TMedal3_4.Checked,
                                  TMedal3_5.Checked,
								  TMedal3_6.Checked,
								  TMedal3_7.Checked,
								  TMedal4_0.Checked,
                                  TMedal4_1.Checked,
								  TMedal4_2.Checked,
								  TMedal4_3.Checked,
                                  TMedal4_4.Checked,
								  TMedal4_5.Checked,
								  TMedal4_6.Checked,
                                  TMedal4_7.Checked,
                                  TMedal1_2.Checked,
								  TMedal1_3.Checked,
								  TMedal1_4.Checked,
								  TMedal1_5.Checked,
								  TMedal1_6.Checked,
								  TMedal1_7.Checked,
                                  TMedal2_0.Checked,
								  TMedal2_1.Checked,
								  TMedal2_2.Checked,
								  TMedal2_3.Checked,
								  TMedal2_4.Checked,
								  TMedal2_5.Checked,
                                  TMedal2_6.Checked,
								  TMedal2_7.Checked,
								  TMedal3_0.Checked,
								  TMedal3_1.Checked,
								  TMedal3_2.Checked,
								  TMedal3_3.Checked,
                                  CHK_Secret.Checked,
								CHK_D0.Checked,
								CHK_D1.Checked,
								CHK_D2.Checked,
								CHK_D3.Checked,
								CHK_D4.Checked,
								CHK_D5.Checked,
								};
            return cba;

        }

        #region Load & Close
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

        private void OverForm_Load(object sender, EventArgs e)
        {
            f.clickTRGender(Label_OTGender, e);
            f.clickTRGender(Label_CTGender, e);
            CHK_Badges.Checked = true;
            BTN_All.PerformClick();
            BTN_None.PerformClick();
            tabControl2.SelectedIndex = 1;
            BTN_All.PerformClick();
            BTN_None.PerformClick();
            tabControl2.SelectedIndex = 0;
            CHK_Badges.Checked = false;
        }
        #endregion

    }
}
