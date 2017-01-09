using PKHeX;
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
        private Main f;
        private MemoryAmie ma;
        private RibbonEditor badge;
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
            return (cb.DataSource != null) && (((List<ComboItem>)cb.DataSource).Count > 1);
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

                CheckBox[] badgeCheck = TLP_Ribbons.Controls.OfType<CheckBox>().ToArray();
                List<bool> badgeCheckList = new List<bool>();
                foreach(CheckBox box in badgeCheck)
                {
                    badgeCheckList.Add(box.Checked);
                }
                bool[] badgeChecks = badgeCheckList.ToArray();

                NumericUpDown[] nums = TLP_Ribbons.Controls.OfType<NumericUpDown>().ToArray();
                List<int> numsList = new List<int>();
                foreach (NumericUpDown num in nums)
                {
                    numsList.Add((int)num.Value);
                }
                int[] badgeInts = numsList.ToArray();

                // TODO: Change Symbols
                int[] symbolChecks = Main.pkm.Markings;

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
                    Main f1 = new Main(litems, modes, this.progressBar1, ret, friendship, level, m, bak, otindexes, countrybool, metbool, otbool, amienabled, amiindex, otgenders, (filename.IndexOf("Mess") >= 0), amilite, amilitebool, amiliteint, allintobox, badgeChecks, badgeInts, symbolChecks, contestStats, gender); 
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
           /* f.clickTRGender(Label_OTGender, e);
            f.clickTRGender(Label_CTGender, e);*/
            CHK_Badges.Checked = true;
      /*      B_All.PerformClick();
            B_None.PerformClick();
            
            B_All.PerformClick();
            B_None.PerformClick();*/
            
            CHK_Badges.Checked = false;
        }
        #endregion
        
    }
}
