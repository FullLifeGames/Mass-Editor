using System;
using System.Windows.Forms;

namespace PKHeX
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            RTB.Text = Mass_Editor.Properties.Resources.changelog;
        }
        private void B_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void B_Shortcuts_Click(object sender, EventArgs e)
        {
            if (B_Shortcuts.Text == "Shortcuts")
            {
                RTB.Text = Mass_Editor.Properties.Resources.shortcuts; // display shortcuts
                B_Shortcuts.Text = "Changelog";
            }
            else
            {
                RTB.Text = Mass_Editor.Properties.Resources.changelog; // display changelog
                B_Shortcuts.Text = "Shortcuts";
            }
        }
    }
}
