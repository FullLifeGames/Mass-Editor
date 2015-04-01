﻿using System;
using System.Windows.Forms;

namespace Mass_Editor
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            RTB.Text = Properties.Resources.changelog;
        }
        private void B_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void B_Shortcuts_Click(object sender, EventArgs e)
        {
            if (B_Shortcuts.Text == "Shortcuts")
            {
                RTB.Text = Properties.Resources.shortcuts; // display shortcuts
                B_Shortcuts.Text = "Changelog";
            }
            else
            {
                RTB.Text = Properties.Resources.changelog; // display changelog
                B_Shortcuts.Text = "Shortcuts";
            }
        }
    }
}
