using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private void CB_Met_Changed(object sender, System.EventArgs e)
        {
            GB_Met.Enabled = CHK_Met.Checked;
        }

        private void CB_ChangeOT_Changed(object sender, System.EventArgs e)
        {
            groupBox1.Enabled = CHK_ChangeOT.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox4.Enabled = CHK_Country.Checked;
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

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            CB_MetLocation.Enabled = checkBox7.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {           
            CB_GameOrigin.Enabled = checkBox6.Checked;
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

    }
}
