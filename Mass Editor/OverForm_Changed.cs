using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mass_Editor
{
    partial class OverForm
    {

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                if (int.Parse(textBox6.Text) > 100)
                {
                    textBox6.Text = "100";
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                if (int.Parse(textBox5.Text) > 255)
                {
                    textBox5.Text = "255";
                }
            }
        }
        
        private void Label_Gender_Click(object sender, EventArgs e)
        {
            string[] gender = { "♂", "♀" };
            if (Label_Gender.Text == gender[0])
            {
                Label_Gender.Text = gender[1];
            }
            else
            {
                Label_Gender.Text = gender[0];
            }
        }

        private void CB_ChangeOT_Changed(object sender, System.EventArgs e)
        {
            groupBox1.Enabled = CHK_ChangeOT.Checked;
        }

        private void CB_Met_Changed(object sender, System.EventArgs e)
        {
            GB_Met.Enabled = CHK_Met.Checked;
        }

        private void CHK_Country_CheckedChanged(object sender, EventArgs e)
        {
            groupBox4.Enabled = CHK_Country.Checked;
        }

        private void CHK_Memories_CheckedChanged(object sender, EventArgs e)
        {
            groupBox5.Enabled = CHK_Memories.Checked;
        }

        private void CHK_Reroll_CheckedChanged(object sender, EventArgs e)
        {
        /*    if (CHK_Reroll.Checked)
            {
                if (CHK_Unshiny.Checked)
                {
                    CHK_Unshiny.Checked = !CHK_Unshiny.Checked;
                }

                if (CHK_Shiny.Checked)
                {
                    CHK_Shiny.Checked = !CHK_Shiny.Checked;
                }
            }*/
        }

        private void CHK_Shiny_CheckedChanged(object sender, EventArgs e)
        {
            if (CHK_Shiny.Checked)
            {
                if (CHK_Unshiny.Checked)
                {
                    CHK_Unshiny.Checked = !CHK_Unshiny.Checked;
                }

       /*         if (CHK_Reroll.Checked)
                {
                    CHK_Reroll.Checked = !CHK_Reroll.Checked;
                }*/
            }
        }

        private void CHK_Unshiny_CheckedChanged(object sender, EventArgs e)
        {
            if (CHK_Unshiny.Checked)
            {
                if (CHK_Shiny.Checked)
                {
                    CHK_Shiny.Checked = !CHK_Shiny.Checked;
                }

  /*              if (CHK_Reroll.Checked)
                {
                    CHK_Reroll.Checked = !CHK_Reroll.Checked;
                }*/
            }
        }

        private void CHK_Badges_CheckedChanged(object sender, EventArgs e)
        {
            groupBox6.Enabled = CHK_Badges.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Label_OTGender.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CB_Language.Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CB_Country.Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CB_SubRegion.Enabled = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            CB_3DSReg.Enabled = checkBox5.Checked;
        }
        
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {           
            CB_GameOrigin.Enabled = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            CB_MetLocation.Enabled = checkBox7.Checked;
        }
  
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            CB_Ball.Enabled = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            TB_MetLevel.Enabled = checkBox9.Checked;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            CAL_MetDate.Enabled = checkBox10.Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            CHK_Fateful.Enabled = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            CB_EncounterType.Enabled = checkBox12.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            CHK_AsEgg.Enabled = checkBox13.Checked;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            CB_EggLocation.Enabled = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            CAL_EggDate.Enabled = checkBox15.Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox16.Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = checkBox17.Checked;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = checkBox18.Checked;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = checkBox19.Checked;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            Label_CTGender.Enabled = checkBox20.Checked;
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            maskedTextBox2.Enabled = checkBox21.Checked;
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            maskedTextBox1.Enabled = checkBox22.Checked;
        }       

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (switchChecks)
            {
                switchChecks = false;
                checkBox24.Checked = !checkBox23.Checked;
                switchChecks = true;
            }
            checkBox21.Enabled = checkBox23.Checked;
            checkBox22.Enabled = checkBox23.Checked;
            maskedTextBox1.Enabled = checkBox22.Checked;
            maskedTextBox2.Enabled = checkBox21.Checked;
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (switchChecks)
            {
                switchChecks = false;
                checkBox23.Checked = !checkBox24.Checked;
                switchChecks = true;
            }
            tabControl1.Enabled = checkBox24.Checked;
        }

        private void CHK_Symbols_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] cba = { CHK_Circle, CHK_Triangle, CHK_Square, CHK_Heart, CHK_Star, CHK_Diamond };

            foreach (CheckBox c in cba)
            {
                c.Enabled = CHK_Symbols.Checked;
            }
            Label_Diamond.Enabled = CHK_Symbols.Checked;
        }

        private void update255_MTB(object sender, EventArgs e)
        {
            MaskedTextBox mtb = sender as MaskedTextBox;
            try
            {
                if (Util.ToInt32((sender as MaskedTextBox).Text) > 255)
                    (sender as MaskedTextBox).Text = "255";
            }
            catch { mtb.Text = "0"; }
        }

        private void CHK_Contest_CheckedChanged(object sender, EventArgs e)
        {
            bool b = CHK_Contest.Checked;
            TB_Beauty.Enabled = b;
            TB_Cool.Enabled = b;
            Label_Beauty.Enabled = b;
            Label_Cool.Enabled = b;
            TB_Smart.Enabled = b;
            TB_Cute.Enabled = b;
            Label_Smart.Enabled = b;
            Label_Cute.Enabled = b;
            TB_Sheen.Enabled = b;
            TB_Tough.Enabled = b;
            Label_Sheen.Enabled = b;
            Label_Tough.Enabled = b;
        }

        private void CHK_Gender_CheckedChanged(object sender, EventArgs e)
        {
            Label_Gender.Enabled = CHK_Gender.Checked;
        }

    }
}
