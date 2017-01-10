﻿using PKHeX.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PKHeX.Saves.Substructures;
using Mass_Editor;

namespace PKHeX
{
    public partial class Main : Form
    {

        private bool box_load = false;

        private int picturebox = 0;
        private int box = 0;
        private int gennedboxfile = 0;
        private string gennedfolder = "";

        private List<int> modes;
        private ProgressBar pbr;
        private string friendship;
        private string level;
        private Mass_Editor.Met m;
        private bool bak;
        private List<string> litems;
        private string[] ret;
        private int[] otindexes;
        private bool[] country;
        private bool[] met;
        private bool[] ot;
        private bool[] amienabled;
        private int[] amiindex;
        private string[] otgenders;
        private bool amilite;
        private bool[] amilitebool;
        private int[] amiliteint;
        private bool allintobox;
        private bool[] badgeChecks;
        private int[] badgeInts;
        private int[] symbolChecks;
        private string[] contestStats;
        private string gender;

        public Main()
        {
            #region Initialize Form
            new Thread(() => new SplashScreen().ShowDialog()).Start();
            DragInfo.slotPkmSource = SAV.BlankPKM.EncryptedPartyData;
            InitializeComponent();

            // Check for Updates
            L_UpdateAvailable.Click += (sender, e) => Process.Start(ThreadPath);
            new Thread(() =>
            {
                string data = Util.getStringFromURL(VersionPath);
                if (data == null)
                    return;
                try
                {
                    DateTime upd = DateTime.ParseExact(data, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    DateTime cur = DateTime.ParseExact(Mass_Editor.Properties.Resources.ProgramVersion, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (upd <= cur)
                        return;
                    
                    string message = $"New Update Available! {upd.ToString("d")}";
                    if (InvokeRequired)
                        try { Invoke((MethodInvoker) delegate { L_UpdateAvailable.Visible = true; L_UpdateAvailable.Text = message; }); }
                        catch { L_UpdateAvailable.Visible = true; L_UpdateAvailable.Text = message; }
                    else { L_UpdateAvailable.Visible = true; L_UpdateAvailable.Text = message; }
                }
                catch { }
            }).Start();

            CB_ExtraBytes.SelectedIndex = 0;
            getFieldsfromPKM = populateFieldsPK7;
            getPKMfromFields = preparePK7;

            // Set up form properties and arrays.
            SlotPictureBoxes = new[] {
                                    bpkx1, bpkx2, bpkx3, bpkx4, bpkx5, bpkx6,
                                    bpkx7, bpkx8, bpkx9, bpkx10,bpkx11,bpkx12,
                                    bpkx13,bpkx14,bpkx15,bpkx16,bpkx17,bpkx18,
                                    bpkx19,bpkx20,bpkx21,bpkx22,bpkx23,bpkx24,
                                    bpkx25,bpkx26,bpkx27,bpkx28,bpkx29,bpkx30,

                                    ppkx1, ppkx2, ppkx3, ppkx4, ppkx5, ppkx6,
                                    bbpkx1,bbpkx2,bbpkx3,bbpkx4,bbpkx5,bbpkx6,

                                    dcpkx1, dcpkx2, gtspkx, fusedpkx,subepkx1,subepkx2,subepkx3,
                                };
            relearnPB = new[] { PB_WarnRelearn1, PB_WarnRelearn2, PB_WarnRelearn3, PB_WarnRelearn4 };
            movePB = new[] { PB_WarnMove1, PB_WarnMove2, PB_WarnMove3, PB_WarnMove4 };
            Label_Species.ResetForeColor();

            // Set up Language Selection
            foreach (var cbItem in main_langlist)
                CB_MainLanguage.Items.Add(cbItem);

            // ToolTips for Drag&Drop
            new ToolTip().SetToolTip(dragout, "PKM QuickSave");

            // Box Drag & Drop
            foreach (PictureBox pb in SlotPictureBoxes)
            {
                pb.AllowDrop = true; // The PictureBoxes have their own drag&drop event handlers (pbBoxSlot)
                pb.GiveFeedback += (sender, e) => { e.UseDefaultCursors = false; };
            }
            dragout.GiveFeedback += (sender, e) => { e.UseDefaultCursors = false; };
            GiveFeedback += (sender, e) => { e.UseDefaultCursors = false; };
            foreach (TabPage tab in tabMain.TabPages)
            {
                tab.AllowDrop = true;
                tab.DragDrop += tabMain_DragDrop;
                tab.DragEnter += tabMain_DragEnter;
            }
            foreach (TabPage tab in tabBoxMulti.TabPages)
            {
                tab.AllowDrop = true;
                tab.DragDrop += tabMain_DragDrop;
                tab.DragEnter += tabMain_DragEnter;
            }

            GB_OT.Click += clickGT;
            GB_nOT.Click += clickGT;
            GB_Daycare.Click += switchDaycare;
            GB_CurrentMoves.Click += clickMoves;
            GB_RelearnMoves.Click += clickMoves;

            TB_Nickname.Font = PKX.getPKXFont(11);
            TB_OT.Font = (Font)TB_Nickname.Font.Clone();
            TB_OTt2.Font = (Font)TB_Nickname.Font.Clone();

            Menu_Modify.DropDown.Closing += (sender, e) =>
            {
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    e.Cancel = true;
            };
            Menu_Options.DropDown.Closing += (sender, e) =>
            {
                if (!Menu_Unicode.Selected)
                    return;
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    e.Cancel = true;
            };

            // Box to Tabs D&D
            dragout.AllowDrop = true;

            FLP_SAVtools.Scroll += Util.PanelScroll;

            // Load WC6 folder to legality
            refreshWC6DB();
            // Load WC7 folder to legality
            refreshWC7DB();

            #endregion
            #region Localize & Populate Fields
            string[] args = Environment.GetCommandLineArgs();
            string filename = args.Length > 0 ? Path.GetFileNameWithoutExtension(args[0])?.ToLower() : "";
            HaX = filename?.IndexOf("hax", StringComparison.Ordinal) >= 0;

            bool showChangelog = false;
            // Load User Settings
            {
                unicode = Menu_Unicode.Checked = Mass_Editor.Properties.Settings.Default.Unicode;
                updateUnicode();
                SaveFile.SetUpdateDex = Menu_ModifyDex.Checked = Mass_Editor.Properties.Settings.Default.SetUpdateDex;
                SaveFile.SetUpdatePKM = Menu_ModifyPKM.Checked = Mass_Editor.Properties.Settings.Default.SetUpdatePKM;

                // Select Language
                string l = Mass_Editor.Properties.Settings.Default.Language;
                int lang = Array.IndexOf(GameInfo.lang_val, l);
                if (lang < 0) Array.IndexOf(GameInfo.lang_val, "en");
                CB_MainLanguage.SelectedIndex = lang < 0 ? 1 : lang;

                // Version Check
                if (Mass_Editor.Properties.Settings.Default.Version.Length > 0) // already run on system
                {
                    int lastrev; int.TryParse(Mass_Editor.Properties.Settings.Default.Version, out lastrev);
                    int currrev; int.TryParse(Mass_Editor.Properties.Resources.ProgramVersion, out currrev);

                    showChangelog = lastrev < currrev;
                }
                Mass_Editor.Properties.Settings.Default.Version = Mass_Editor.Properties.Resources.ProgramVersion;
                Mass_Editor.Properties.Settings.Default.Save();
            }

            InitializeFields();
            formInitialized = true;
            
            #endregion
            #region Load Initial File(s)
            if (args.Length > 1) // Load the arguments
            {
                foreach (string arg in args.Skip(1).Where(a => a.Length > 4))
                    openQuick(arg, force: true);
            }
            if (!SAV.Exportable) // No SAV loaded from exe args
            {
                string path = null;
                try
                {
                    path = SaveUtil.detectSaveFile();
                }
                catch (Exception ex)
                {
                    // Todo: translate this
                    ErrorWindow.ShowErrorDialog("An error occurred while attempting to auto-load your save file.", ex, true);
                }
                
                if (path != null && File.Exists(path))
                    openQuick(path, force: true);
                else
                {
                    openSAV(SAV, null);
                    SAV.Edited = false; // Prevents form close warning from showing until changes are made
                }                    
            }

            // Splash Screen closes on its own.
            BringToFront();
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = FormWindowState.Normal;
            if (HaX) Util.Alert("Illegal mode activated.", "Please behave.");
            
            if (showChangelog)
                new About().ShowDialog();
            #endregion
            this.Hide();
            this.Tab_Main.Controls.Clear();
        }

        public Main(OverForm form)
        {
            #region Initialize Form
            DragInfo.slotPkmSource = SAV.BlankPKM.EncryptedPartyData;
            InitializeComponent();

            // Check for Updates
           
            CB_ExtraBytes.SelectedIndex = 0;
            getFieldsfromPKM = populateFieldsPK7;
            getPKMfromFields = preparePK7;

            // Set up form properties and arrays.
            SlotPictureBoxes = new[] {
                                    bpkx1, bpkx2, bpkx3, bpkx4, bpkx5, bpkx6,
                                    bpkx7, bpkx8, bpkx9, bpkx10,bpkx11,bpkx12,
                                    bpkx13,bpkx14,bpkx15,bpkx16,bpkx17,bpkx18,
                                    bpkx19,bpkx20,bpkx21,bpkx22,bpkx23,bpkx24,
                                    bpkx25,bpkx26,bpkx27,bpkx28,bpkx29,bpkx30,

                                    ppkx1, ppkx2, ppkx3, ppkx4, ppkx5, ppkx6,
                                    bbpkx1,bbpkx2,bbpkx3,bbpkx4,bbpkx5,bbpkx6,

                                    dcpkx1, dcpkx2, gtspkx, fusedpkx,subepkx1,subepkx2,subepkx3,
                                };
            relearnPB = new[] { PB_WarnRelearn1, PB_WarnRelearn2, PB_WarnRelearn3, PB_WarnRelearn4 };
            movePB = new[] { PB_WarnMove1, PB_WarnMove2, PB_WarnMove3, PB_WarnMove4 };
            Label_Species.ResetForeColor();

            // Set up Language Selection
            foreach (var cbItem in main_langlist)
                CB_MainLanguage.Items.Add(cbItem);

          
            
            FLP_SAVtools.Scroll += Util.PanelScroll;

            // Load WC6 folder to legality
            refreshWC6DB();
            // Load WC7 folder to legality
            refreshWC7DB();

            #endregion
            #region Localize & Populate Fields
            string[] args = Environment.GetCommandLineArgs();
            string filename = args.Length > 0 ? Path.GetFileNameWithoutExtension(args[0])?.ToLower() : "";
            HaX = filename?.IndexOf("hax", StringComparison.Ordinal) >= 0;

            bool showChangelog = false;
            // Load User Settings
            {
                unicode = Menu_Unicode.Checked = Mass_Editor.Properties.Settings.Default.Unicode;
                updateUnicode();
                SaveFile.SetUpdateDex = Menu_ModifyDex.Checked = Mass_Editor.Properties.Settings.Default.SetUpdateDex;
                SaveFile.SetUpdatePKM = Menu_ModifyPKM.Checked = Mass_Editor.Properties.Settings.Default.SetUpdatePKM;

                // Select Language
                string l = Mass_Editor.Properties.Settings.Default.Language;
                int lang = Array.IndexOf(GameInfo.lang_val, l);
                if (lang < 0) Array.IndexOf(GameInfo.lang_val, "en");
                CB_MainLanguage.SelectedIndex = lang < 0 ? 1 : lang;

                // Version Check
                if (Mass_Editor.Properties.Settings.Default.Version.Length > 0) // already run on system
                {
                    int lastrev; int.TryParse(Mass_Editor.Properties.Settings.Default.Version, out lastrev);
                    int currrev; int.TryParse(Mass_Editor.Properties.Resources.ProgramVersion, out currrev);

                    showChangelog = lastrev < currrev;
                }
                Mass_Editor.Properties.Settings.Default.Version = Mass_Editor.Properties.Resources.ProgramVersion;
                Mass_Editor.Properties.Settings.Default.Save();
            }

            InitializeFields();
            formInitialized = true;

            #endregion
            #region Load Initial File(s)
            if (args.Length > 1) // Load the arguments
            {
                foreach (string arg in args.Skip(1).Where(a => a.Length > 4))
                    openQuick(arg, force: true);
            }
            if (!SAV.Exportable) // No SAV loaded from exe args
            {
                string path = null;
                try
                {
                    path = SaveUtil.detectSaveFile();
                }
                catch (Exception ex)
                {
                    // Todo: translate this
                }

                if (path != null && File.Exists(path))
                    openQuick(path, force: true);
                else
                {
                    openSAV(SAV, null);
                    SAV.Edited = false; // Prevents form close warning from showing until changes are made
                }
            }

            // Splash Screen closes on its own.
          //  BringToFront();
            WindowState = FormWindowState.Minimized;
          //  Show();
            WindowState = FormWindowState.Normal;
             #endregion
            this.Hide();
            this.Tab_Main.Controls.Clear();
        }

        public Main(List<string> litems, List<int> modes, ProgressBar progressBar, string[] ret, string friendship, string level, Mass_Editor.Met m, bool bak, int[] otindexes, bool[] country, bool[] met, bool[] ot, bool[] amienabled, int[] amiindex, string[] otgenders, bool hax, bool amilite, bool[] amilitebool, int[] amiliteint, bool allintobox, bool[] badgeChecks, int[] badgeInts, int[] symbolChecks, string[] contestStats, string gender)
        {
            this.litems = litems;
            this.modes = modes;
            this.pbr = progressBar;
            this.ret = ret;
            this.friendship = friendship;
            this.level = level;
            this.m = m;
            this.bak = bak;
            this.otindexes = otindexes;
            this.country = country;
            this.met = met;
            this.ot = ot;
            this.amienabled = amienabled;
            this.amiindex = amiindex;
            this.otgenders = otgenders;
            this.amilite = amilite;
            this.amilitebool = amilitebool;
            this.amiliteint = amiliteint;
            this.allintobox = allintobox;
            this.badgeChecks = badgeChecks;
            this.badgeInts = badgeInts;
            this.symbolChecks = symbolChecks;
            this.contestStats = contestStats;
            this.gender = gender;

            #region Initialize Form
           
            DragInfo.slotPkmSource = SAV.BlankPKM.EncryptedPartyData;
            InitializeComponent();

            CB_ExtraBytes.SelectedIndex = 0;
            getFieldsfromPKM = populateFieldsPK7;
            getPKMfromFields = preparePK7;

            // Set up form properties and arrays.
            SlotPictureBoxes = new[] {
                                    bpkx1, bpkx2, bpkx3, bpkx4, bpkx5, bpkx6,
                                    bpkx7, bpkx8, bpkx9, bpkx10,bpkx11,bpkx12,
                                    bpkx13,bpkx14,bpkx15,bpkx16,bpkx17,bpkx18,
                                    bpkx19,bpkx20,bpkx21,bpkx22,bpkx23,bpkx24,
                                    bpkx25,bpkx26,bpkx27,bpkx28,bpkx29,bpkx30,

                                    ppkx1, ppkx2, ppkx3, ppkx4, ppkx5, ppkx6,
                                    bbpkx1,bbpkx2,bbpkx3,bbpkx4,bbpkx5,bbpkx6,

                                    dcpkx1, dcpkx2, gtspkx, fusedpkx,subepkx1,subepkx2,subepkx3,
                                };
            relearnPB = new[] { PB_WarnRelearn1, PB_WarnRelearn2, PB_WarnRelearn3, PB_WarnRelearn4 };
            movePB = new[] { PB_WarnMove1, PB_WarnMove2, PB_WarnMove3, PB_WarnMove4 };
            Label_Species.ResetForeColor();

            // Set up Language Selection
            foreach (var cbItem in main_langlist)
                CB_MainLanguage.Items.Add(cbItem);

            // ToolTips for Drag&Drop
            new ToolTip().SetToolTip(dragout, "PKM QuickSave");

            // Box Drag & Drop
            foreach (PictureBox pb in SlotPictureBoxes)
            {
                pb.AllowDrop = true; // The PictureBoxes have their own drag&drop event handlers (pbBoxSlot)
                pb.GiveFeedback += (sender, e) => { e.UseDefaultCursors = false; };
            }
            dragout.GiveFeedback += (sender, e) => { e.UseDefaultCursors = false; };
            GiveFeedback += (sender, e) => { e.UseDefaultCursors = false; };
            foreach (TabPage tab in tabMain.TabPages)
            {
                tab.AllowDrop = true;
                tab.DragDrop += tabMain_DragDrop;
                tab.DragEnter += tabMain_DragEnter;
            }
            foreach (TabPage tab in tabBoxMulti.TabPages)
            {
                tab.AllowDrop = true;
                tab.DragDrop += tabMain_DragDrop;
                tab.DragEnter += tabMain_DragEnter;
            }

            GB_OT.Click += clickGT;
            GB_nOT.Click += clickGT;
            GB_Daycare.Click += switchDaycare;
            GB_CurrentMoves.Click += clickMoves;
            GB_RelearnMoves.Click += clickMoves;

            TB_Nickname.Font = PKX.getPKXFont(11);
            TB_OT.Font = (Font)TB_Nickname.Font.Clone();
            TB_OTt2.Font = (Font)TB_Nickname.Font.Clone();

            Menu_Modify.DropDown.Closing += (sender, e) =>
            {
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    e.Cancel = true;
            };
            Menu_Options.DropDown.Closing += (sender, e) =>
            {
                if (!Menu_Unicode.Selected)
                    return;
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    e.Cancel = true;
            };

            // Box to Tabs D&D
            dragout.AllowDrop = true;

            FLP_SAVtools.Scroll += Util.PanelScroll;

            // Load WC6 folder to legality
            refreshWC6DB();
            // Load WC7 folder to legality
            refreshWC7DB();

            #endregion
            #region Localize & Populate Fields
            string[] args = Environment.GetCommandLineArgs();
            string filename = args.Length > 0 ? Path.GetFileNameWithoutExtension(args[0])?.ToLower() : "";
            HaX = filename?.IndexOf("hax", StringComparison.Ordinal) >= 0;

            bool showChangelog = false;
            // Load User Settings
            {
                unicode = Menu_Unicode.Checked = Mass_Editor.Properties.Settings.Default.Unicode;
                updateUnicode();
                SaveFile.SetUpdateDex = Menu_ModifyDex.Checked = Mass_Editor.Properties.Settings.Default.SetUpdateDex;
                SaveFile.SetUpdatePKM = Menu_ModifyPKM.Checked = Mass_Editor.Properties.Settings.Default.SetUpdatePKM;

                // Select Language
                string l = Mass_Editor.Properties.Settings.Default.Language;
                int lang = Array.IndexOf(GameInfo.lang_val, l);
                if (lang < 0) Array.IndexOf(GameInfo.lang_val, "en");
                CB_MainLanguage.SelectedIndex = lang < 0 ? 1 : lang;

                // Version Check
                if (Mass_Editor.Properties.Settings.Default.Version.Length > 0) // already run on system
                {
                    int lastrev; int.TryParse(Mass_Editor.Properties.Settings.Default.Version, out lastrev);
                    int currrev; int.TryParse(Mass_Editor.Properties.Resources.ProgramVersion, out currrev);

                    showChangelog = lastrev < currrev;
                }
                Mass_Editor.Properties.Settings.Default.Version = Mass_Editor.Properties.Resources.ProgramVersion;
                Mass_Editor.Properties.Settings.Default.Save();
            }

            InitializeFields();
            formInitialized = true;

            #endregion
            #region Load Initial File(s)
            if (args.Length > 1) // Load the arguments
            {
                foreach (string arg in args.Skip(1).Where(a => a.Length > 4))
                    openQuick(arg, force: true);
            }
            if (!SAV.Exportable) // No SAV loaded from exe args
            {
                string path = null;
                try
                {
                    path = SaveUtil.detectSaveFile();
                }
                catch (Exception ex)
                {
                    // Todo: translate this
                    ErrorWindow.ShowErrorDialog("An error occurred while attempting to auto-load your save file.", ex, true);
                }

                if (path != null && File.Exists(path))
                    openQuick(path, force: true);
                else
                {
                    openSAV(SAV, null);
                    SAV.Edited = false; // Prevents form close warning from showing until changes are made
                }
            }

            // Splash Screen closes on its own.
     //       BringToFront();
            WindowState = FormWindowState.Minimized;
        //    Show();
            WindowState = FormWindowState.Normal;
            /*      if (HaX) Util.Alert("Illegal mode activated.", "Please behave.");

                  if (showChangelog)
                      new About().ShowDialog();*/
            this.Hide();
            #endregion
        }

                
        #region Important Variables
        // TODO Problem
        public static SaveFile SAV = new SAV7 { Game = (int)GameVersion.SN, OT = "PKHeX", TID = 12345, SID = 54321, Language = 2, Country = 49, SubRegion = 7, ConsoleRegion = 1 }; // Save File
        public static PKM pkm = SAV.BlankPKM; // Tab Pokemon Data Storage
        //private LegalityAnalysis Legality = new LegalityAnalysis(pkm);
        public static GameInfo.GameStrings GameStrings;

        public static string curlanguage = "en";
        public static string[] gendersymbols = { "♂", "♀", "-" };
        public static bool unicode;

        public static volatile bool formInitialized, fieldsInitialized, fieldsLoaded;
        private static int colorizedbox = -1;
        private static Image colorizedcolor;
        private static int colorizedslot;
        public static bool HaX;
        private static readonly Image mixedHighlight = null;
        private static readonly string[] main_langlist =
            {
                "日本語", // JPN
                "English", // ENG
                "Français", // FRE
                "Italiano", // ITA
                "Deutsch", // GER
                "Español", // SPA
                "한국어", // KOR
                "中文", // CHN
                "Português", // Portuguese
            };
        private static GameVersion origintrack;
        private readonly PictureBox[] SlotPictureBoxes, movePB, relearnPB;
        private readonly ToolTip Tip1 = new ToolTip(), Tip2 = new ToolTip(), Tip3 = new ToolTip(), NatureTip = new ToolTip();
        #endregion

        #region Path Variables

        public static string WorkingDirectory => Util.IsClickonceDeployed ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PKHeX") : Environment.CurrentDirectory;
        public static string DatabasePath => Path.Combine(WorkingDirectory, "pkmdb");
        public static string MGDatabasePath => Path.Combine(WorkingDirectory, "mgdb");
        private static string BackupPath => Path.Combine(WorkingDirectory, "bak");
        private static string ThreadPath => @"https://projectpokemon.org/forums/showthread.php?36986";
        private static string VersionPath => @"https://raw.githubusercontent.com/kwsch/PKHeX/master/PKHeX/Resources/text/version.txt";
        
        #endregion

        #region //// MAIN MENU FUNCTIONS ////
        // Main Menu Strip UI Functions
        private void mainMenuOpen(object sender, EventArgs e)
        {
            string pkx = pkm.Extension;
            string ekx = 'e' + pkx.Substring(1, pkx.Length-1);

            string supported = "*.pkm;";
            for (int i = 3; i <= SAV.Generation; i++)
            {
                supported += $"*.pk{i}";
                if (i != pkm.Format)
                    supported += ";";
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = $"Decrypted PKM File|{supported}" +
                         $"|Encrypted PKM File|*.{ekx}" +
                         "|Binary File|*.bin" +
                         "|All Files|*.*",
                RestoreDirectory = true,
                FilterIndex = 4,
                FileName = "main",
            };

            // Reset file dialog path if it no longer exists
            if (!Directory.Exists(ofd.InitialDirectory))
                ofd.InitialDirectory = WorkingDirectory;

            // Detect main
            string path = SaveUtil.detectSaveFile();
            if (path != null)
            { ofd.InitialDirectory = Path.GetDirectoryName(path); }
            else if (File.Exists(Path.Combine(ofd.InitialDirectory, "main")))
            { }
            else if (!Directory.Exists(ofd.InitialDirectory))
            { ofd.RestoreDirectory = false; ofd.FilterIndex = 1; ofd.FileName = ""; }

            if (ofd.ShowDialog() == DialogResult.OK) 
                openQuick(ofd.FileName);
        }
        private void mainMenuSave(object sender, EventArgs e)
        {
            if (!verifiedPKM()) return;
            PKM pk = preparePKM();
            string pkx = pk.Extension;
            string ekx = 'e' + pkx.Substring(1, pkx.Length - 1);
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = $"Decrypted PKM File|*.{pkx}" +
                         (SAV.Generation > 2 ? "" : $"|Encrypted PKM File|*.{ekx}") +
                         "|Binary File|*.bin" +
                         "|All Files|*.*",
                DefaultExt = pkx,
                FileName = Util.CleanFileName(pk.FileName)
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            string path = sfd.FileName;
            string ext = Path.GetExtension(path);

            if (File.Exists(path))
            {
                // File already exists, save a .bak
                byte[] backupfile = File.ReadAllBytes(path);
                File.WriteAllBytes(path + ".bak", backupfile);
            }

            if (new[] {".ekx", "."+ekx, ".bin"}.Contains(ext))
                File.WriteAllBytes(path, pk.EncryptedPartyData);
            else if (new[] { "."+pkx }.Contains(ext))
                File.WriteAllBytes(path, pk.DecryptedBoxData);
            else
            {
                Util.Error($"Foreign File Extension: {ext}", "Exporting as encrypted.");
                File.WriteAllBytes(path, pkm.EncryptedPartyData);
            }
        }
        private void mainMenuExit(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control) // Hotkey Triggered
                if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Quit PKHeX?")) return;
            Close();
        }
        private void mainMenuAbout(object sender, EventArgs e)
        {
            // Open a new form with the About details.
            new About().ShowDialog();
        }
        // Sub Menu Options
        private void mainMenuBoxReport(object sender, EventArgs e)
        {
            var z = Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.GetType() == typeof(frmReport)) as frmReport;
            if (z != null)
            { Util.CenterToForm(z, this); z.BringToFront(); return; }
            
            frmReport ReportForm = new frmReport();
            ReportForm.Show();
            ReportForm.PopulateData(SAV.BoxData);
        }
        private void mainMenuDatabase(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                var c = Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.GetType() == typeof(KChart)) as KChart;
                if (c != null)
                { Util.CenterToForm(c, this); c.BringToFront(); }
                else
                    new KChart().Show();
                return;
            }

            var z = Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.GetType() == typeof(SAV_Database)) as SAV_Database;
            if (z != null)
            { Util.CenterToForm(z, this); z.BringToFront(); return; }

            if (Directory.Exists(DatabasePath))
                new SAV_Database(this).Show();
            else
                Util.Alert("PKHeX's database was not found.",
                    $"Please dump all boxes from a save file, then ensure the '{DatabasePath}' folder exists.");
        }
        private void mainMenuMysteryDM(object sender, EventArgs e)
        {
            var z = Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.GetType() == typeof(SAV_MysteryGiftDB)) as SAV_MysteryGiftDB;
            if (z != null)
            { Util.CenterToForm(z, this); z.BringToFront(); return; }

            new SAV_MysteryGiftDB(this).Show();
            
        }
        private void mainMenuUnicode(object sender, EventArgs e)
        {
            Mass_Editor.Properties.Settings.Default.Unicode = unicode = Menu_Unicode.Checked;
            Mass_Editor.Properties.Settings.Default.Save();
            updateUnicode();
        }
        private void mainMenuModifyDex(object sender, EventArgs e)
        {
            Mass_Editor.Properties.Settings.Default.SetUpdateDex = SaveFile.SetUpdateDex = Menu_ModifyDex.Checked;
            Mass_Editor.Properties.Settings.Default.Save();
        }
        private void mainMenuModifyPKM(object sender, EventArgs e)
        {
            Mass_Editor.Properties.Settings.Default.SetUpdatePKM = SaveFile.SetUpdatePKM = Menu_ModifyPKM.Checked;
            Mass_Editor.Properties.Settings.Default.Save();
        }
        private void mainMenuBoxLoad(object sender, EventArgs e)
        {
            string path = "";
            if (Directory.Exists(DatabasePath))
            {
                DialogResult ld = Util.Prompt(MessageBoxButtons.YesNo, "Load from PKHeX's database?");
                if (ld == DialogResult.Yes)
                    path = DatabasePath;
                else if (ld == DialogResult.No)
                {
                    // open folder dialog
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    if (fbd.ShowDialog() == DialogResult.OK)
                        path = fbd.SelectedPath;
                }
                else return;
            }
            else
            {
                // open folder dialog
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                    path = fbd.SelectedPath;
            }
            loadBoxesFromDB(path);
        }
        private void mainMenuBoxDump(object sender, EventArgs e)
        {
            string path;
            bool dumptoboxes = false;
            // Dump all of box content to files.
            DialogResult ld = Util.Prompt(MessageBoxButtons.YesNo, "Save to PKHeX's database?");
            if (ld == DialogResult.Yes)
            {
                path = DatabasePath;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            else if (ld == DialogResult.No)
            {
                dumptoboxes = DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Save each box separately?");

                // open folder dialog
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                path = fbd.SelectedPath;
            }
            else return;

            dumpBoxesToDB(path, dumptoboxes);
        }
        private void manMenuBatchEditor(object sender, EventArgs e)
        {
            new BatchEditor().ShowDialog();
            setPKXBoxes(); // refresh
            updateBoxViewers();
        }
        // Misc Options
        private void clickShowdownImportPKM(object sender, EventArgs e)
        {
            if (!formInitialized)
                return;
            if (!Clipboard.ContainsText())
            { Util.Alert("Clipboard does not contain text."); return; }

            // Get Simulator Data
            ShowdownSet Set = new ShowdownSet(Clipboard.GetText());

            if (Set.Species < 0)
            { Util.Alert("Set data not found in clipboard."); return; }

            if (Set.Nickname != null && Set.Nickname.Length > SAV.NickLength)
                Set.Nickname = Set.Nickname.Substring(0, SAV.NickLength);

            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Import this set?", Set.getText())) 
            { return; }

            if (Set.InvalidLines.Any())
                Util.Alert("Invalid lines detected:", string.Join(Environment.NewLine, Set.InvalidLines));

            // Set Species & Nickname
            CB_Species.SelectedValue = Set.Species;
            CHK_Nicknamed.Checked = Set.Nickname != null;
            if (Set.Nickname != null)
                TB_Nickname.Text = Set.Nickname;
            if (Set.Gender != null)
            {
                int Gender = PKX.getGender(Set.Gender);
                Label_Gender.Text = gendersymbols[Gender];
                Label_Gender.ForeColor = Gender == 2 ? Label_Species.ForeColor : (Gender == 1 ? Color.Red : Color.Blue);
            }

            // Set Form
            string[] formStrings = PKX.getFormList(Set.Species,
                Util.getTypesList("en"),
                Util.getFormsList("en"), gendersymbols, SAV.Generation);
            int form = 0;
            for (int i = 0; i < formStrings.Length; i++)
                if (formStrings[i].Contains(Set.Form ?? ""))
                { form = i; break; }
            CB_Form.SelectedIndex = form;

            // Set Ability
            int[] abilities = SAV.Personal.getAbilities(Set.Species, form);
            int ability = Array.IndexOf(abilities, Set.Ability);
            if (ability < 0) ability = 0;
            CB_Ability.SelectedIndex = ability;
            ComboBox[] m = {CB_Move1, CB_Move2, CB_Move3, CB_Move4};
            for (int i = 0; i < 4; i++) m[i].SelectedValue = Set.Moves[i];

            // Set Item and Nature
            CB_HeldItem.SelectedValue = Set.Item < 0 ? 0 : Set.Item;
            CB_Nature.SelectedValue = Set.Nature < 0 ? 0 : Set.Nature;

            // Set IVs
            TB_HPIV.Text = Set.IVs[0].ToString();
            TB_ATKIV.Text = Set.IVs[1].ToString();
            TB_DEFIV.Text = Set.IVs[2].ToString();
            TB_SPAIV.Text = Set.IVs[4].ToString();
            TB_SPDIV.Text = Set.IVs[5].ToString();
            TB_SPEIV.Text = Set.IVs[3].ToString();

            // Set EVs
            TB_HPEV.Text = Set.EVs[0].ToString();
            TB_ATKEV.Text = Set.EVs[1].ToString();
            TB_DEFEV.Text = Set.EVs[2].ToString();
            TB_SPAEV.Text = Set.EVs[4].ToString();
            TB_SPDEV.Text = Set.EVs[5].ToString();
            TB_SPEEV.Text = Set.EVs[3].ToString();

            // Set Level and Friendship
            TB_Level.Text = Set.Level.ToString();
            TB_Friendship.Text = Set.Friendship.ToString();

            // Reset IV/EVs
            BTN_RerollPID.PerformClick();
            BTN_RerollEC.PerformClick();
            ComboBox[] p = {CB_PPu1, CB_PPu2, CB_PPu3, CB_PPu4};
            for (int i = 0; i < 4; i++)
                p[i].SelectedIndex = m[i].SelectedIndex != 0 ? 3 : 0; // max PP
            
            if (Set.Shiny) BTN_Shinytize.PerformClick();
            pkm = preparePKM();
            updateLegality();
        }
        private void clickShowdownExportPKM(object sender, EventArgs e)
        {
            if (!formInitialized)
                return;
            if (!verifiedPKM())
            { Util.Alert("Fix data before exporting."); return; }

            Clipboard.SetText(preparePKM().ShowdownText);
            Util.Alert("Exported Showdown Set to Clipboard:", Clipboard.GetText());
        }
        private void clickShowdownExportParty(object sender, EventArgs e)
        {
            if (SAV.PartyData.Length <= 0) return;
            try
            {
                Clipboard.SetText(
                    SAV.PartyData.Aggregate("", (current, pk) => current + pk.ShowdownText
                            + Environment.NewLine + Environment.NewLine).Trim());
                Util.Alert("Showdown Team (Party) set to Clipboard.");
            }
            catch { }
        }
        private void clickShowdownExportBattleBox(object sender, EventArgs e)
        {
            if (SAV.BattleBoxData.Length <= 0) return;
            try
            {
                Clipboard.SetText(
                    SAV.BattleBoxData.Aggregate("", (current, pk) => current + pk.ShowdownText
                            + Environment.NewLine + Environment.NewLine).Trim());
                Util.Alert("Showdown Team (Battle Box) set to Clipboard.");
            }
            catch { }
        }
        private void clickOpenTempFolder(object sender, EventArgs e)
        {
            string path = Util.GetTempFolder();
            if (Directory.Exists(Path.Combine(path, "root")))
                Process.Start("explorer.exe", Path.Combine(path, "root"));
            else if (Directory.Exists(path))
                Process.Start("explorer.exe", path);
            else
                Util.Alert("Can't find the temporary file.", "Make sure the Cyber Gadget software is paused.");
        }
        private void clickOpenCacheFolder(object sender, EventArgs e)
        {
            string path = Util.GetCacheFolder();
            if (Directory.Exists(path))
                Process.Start("explorer.exe", path);
            else
                Util.Alert("Can't find the cache folder.");
        }
        private void clickOpenSDFFolder(object sender, EventArgs e)
        {
            string path = Path.GetPathRoot(Util.get3DSLocation());
            if (path != null && Directory.Exists(path = Path.Combine(path, "filer", "UserSaveData")))
                Process.Start("explorer.exe", path);
            else
                Util.Alert("Can't find the SaveDataFiler folder.");
        }
        private void clickOpenSDBFolder(object sender, EventArgs e)
        {
            string path3DS = Path.GetPathRoot(Util.get3DSLocation());
            string path;
            if (path3DS != null && Directory.Exists(path = Path.Combine(path3DS, "SaveDataBackup")))
                Process.Start("explorer.exe", path);
            else
                Util.Alert("Can't find the SaveDataBackup folder.");
        }

        // Main Menu Subfunctions
        private void openQuick(string path, bool force = false)
        {
            // detect if it is a folder (load into boxes or not)
            if (Directory.Exists(path))
            { loadBoxesFromDB(path); return; }

            string ext = Path.GetExtension(path);
            FileInfo fi = new FileInfo(path);
            if (fi.Length > 0x10009C && fi.Length != 0x380000)
                Util.Error("Input file is too large.", path);
            else
            {
                byte[] input; try { input = File.ReadAllBytes(path); }
                catch (Exception e) { Util.Error("Unable to load file.  It could be in use by another program.\nPath: " + path, e); return; }

                try { openFile(input, path, ext); }
                catch (Exception e) { Util.Error("Unable to load file.\nPath: " + path, e); }
            }
        }
        private void openFile(byte[] input, string path, string ext)
        {
            SaveFile sav; MysteryGift tg; PKM temp; string c;
            #region Powersaves Read-Only Conversion
            if (input.Length == 0x10009C) // Resize to 1MB
            {
                Array.Copy(input, 0x9C, input, 0, 0x100000);
                Array.Resize(ref input, 0x100000);
            }
            // Verify the Data Input Size is Proper
            if (input.Length == 0x100000)
            {
                if (openXOR(input, path)) // Check if we can load the save via xorpad
                    return; // only if a save is loaded we abort
                if (BitConverter.ToUInt64(input, 0x10) != 0) // encrypted save
                { Util.Error("PKHeX only edits decrypted save files." + Environment.NewLine + "This save file is not decrypted.", path); return; }
                
                DialogResult sdr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Press Yes to load the sav at 0x3000", "Press No for the one at 0x82000");
                if (sdr == DialogResult.Cancel)
                    return;
                int savshift = sdr == DialogResult.Yes ? 0 : 0x7F000;
                byte[] psdata = input.Skip(0x5400 + savshift).Take(SaveUtil.SIZE_G6ORAS).ToArray();

                if (BitConverter.ToUInt32(psdata, SaveUtil.SIZE_G6ORAS - 0x1F0) == SaveUtil.BEEF)
                    Array.Resize(ref psdata, SaveUtil.SIZE_G6ORAS); // set to ORAS size
                else if (BitConverter.ToUInt32(psdata, SaveUtil.SIZE_G6XY - 0x1F0) == SaveUtil.BEEF)
                    Array.Resize(ref psdata, SaveUtil.SIZE_G6XY); // set to X/Y size
                else if (BitConverter.ToUInt32(psdata, SaveUtil.SIZE_G7SM - 0x1F0) == SaveUtil.BEEF)
                    Array.Resize(ref psdata, SaveUtil.SIZE_G7SM); // set to S/M size
                else
                { Util.Error("The data file is not a valid save file", path); return; }

                openSAV(SaveUtil.getVariantSAV(psdata), path);
            }
            #endregion
            #region SAV/PKM
            else if ((sav = SaveUtil.getVariantSAV(input)) != null)
            {
                openSAV(sav, path);
            }
            else if ((temp = PKMConverter.getPKMfromBytes(input)) != null)
            {
                PKM pk = PKMConverter.convertToFormat(temp, SAV.PKMType, out c);
                if (pk == null)
                    Util.Alert("Conversion failed.", c);
                else if (SAV.Generation < 3 && ((pk as PK1)?.Japanese ?? ((PK2)pk).Japanese) != SAV.Japanese)
                {
                    string a_lang = SAV.Japanese ? "an International" : "a Japanese";
                    string pk_type = pk.GetType().Name;
                    Util.Alert($"Cannot load {a_lang} {pk_type} in {a_lang} save file.");
                }
                else 
                    populateFields(pk);
                Console.WriteLine(c);
            }
            #endregion
            #region PC/Box Data
            else if (BitConverter.ToUInt16(input, 4) == 0 && BitConverter.ToUInt32(input, 8) > 0 && PKX.getIsPKM(input.Length / SAV.BoxSlotCount / SAV.BoxCount) || PKX.getIsPKM(input.Length / SAV.BoxSlotCount))
            {
                if (SAV.setPCBin(input))
                    box_load = true;
                else if (SAV.setBoxBin(input, CB_BoxSelect.SelectedIndex))
                    box_load = true;
                else
                {
                    Util.Alert("Binary is not compatible with save file.", "Current SAV Generation: " + SAV.Generation);
                    return;
                }
                setPKXBoxes();
                updateBoxViewers();
            }
            #endregion
            #region Battle Video
            else if (BattleVideo.getIsValid(input))
            {
                BattleVideo b = BattleVideo.getVariantBattleVideo(input);
                if (SAV.Generation != b.Generation)
                { Util.Alert($"Cannot load a Gen{b.Generation} Battle Video to a different generation save file."); return; }

                if (Util.Prompt(MessageBoxButtons.YesNo, $"Load Battle Video Pokémon data to {CB_BoxSelect.Text}?", "The box will be overwritten.") != DialogResult.Yes)
                    return;

                bool? noSetb = getPKMSetOverride();
                PKM[] data = b.BattlePKMs;
                int offset = SAV.getBoxOffset(CB_BoxSelect.SelectedIndex);
                for (int i = 0; i < 24; i++)
                    SAV.setStoredSlot(data[i], offset + i*SAV.SIZE_STORED, noSetb);

                setPKXBoxes();
                updateBoxViewers();
            }
            #endregion
            #region Mystery Gift (Templates)
            else if ((tg = MysteryGift.getMysteryGift(input, ext)) != null)
            {
                if (!tg.IsPokémon)
                { Util.Alert("Mystery Gift is not a Pokémon.", path); return; }

                temp = tg.convertToPKM(SAV);
                PKM pk = PKMConverter.convertToFormat(temp, SAV.PKMType, out c);

                if (pk == null)
                    Util.Alert("Conversion failed.", c);
                else
                    populateFields(pk);
                Console.WriteLine(c);
            }
            #endregion
            else
                Util.Error("Attempted to load an unsupported file type/size.",
                    $"File Loaded:{Environment.NewLine}{path}",
                    $"File Size:{Environment.NewLine}{input.Length} bytes (0x{input.Length.ToString("X4")})");
        }
        private bool openXOR(byte[] input, string path)
        {
            // Detection of stored Decryption XORpads:
            if (ModifierKeys == Keys.Control) return false; // no xorpad compatible
            byte[] savID = input.Take(0x10).ToArray();
            string exepath = Application.StartupPath;
            string xorpath = exepath.Clone().ToString();
            string[] XORpads = Directory.GetFiles(xorpath);

            int loop = 0;

            while (xorpath == exepath && loop++ == 0)
            {
                foreach (byte[] data in from file in XORpads let fi = new FileInfo(file) where (fi.Name.ToLower().Contains("xorpad") || fi.Name.ToLower().Contains("key")) && (fi.Length == 0x10009C || fi.Length == 0x100000) select File.ReadAllBytes(file))
                {
                    // Fix xorpad alignment
                    byte[] xorpad = data;
                    if (xorpad.Length == 0x10009C) // Trim off Powersaves' header
                        xorpad = xorpad.Skip(0x9C).ToArray(); // returns 0x100000

                    if (!xorpad.Take(0x10).SequenceEqual(savID)) continue;

                    // Set up Decrypted File
                    byte[] decryptedPS = input.Skip(0x5400).Take(SaveUtil.SIZE_G6ORAS).ToArray();

                    // xor through and decrypt
                    for (int z = 0; z < decryptedPS.Length; z++)
                        decryptedPS[z] ^= xorpad[0x5400 + z];

                    // Weakly check the validity of the decrypted content
                    if (BitConverter.ToUInt32(decryptedPS, SaveUtil.SIZE_G6ORAS - 0x1F0) == SaveUtil.BEEF)
                        Array.Resize(ref decryptedPS, SaveUtil.SIZE_G6ORAS); // set to ORAS size
                    else if (BitConverter.ToUInt32(decryptedPS, SaveUtil.SIZE_G6XY - 0x1F0) == SaveUtil.BEEF)
                        Array.Resize(ref decryptedPS, SaveUtil.SIZE_G6XY); // set to X/Y size
                    else if (BitConverter.ToUInt32(decryptedPS, SaveUtil.SIZE_G7SM - 0x1F0) == SaveUtil.BEEF)
                        Array.Resize(ref decryptedPS, SaveUtil.SIZE_G7SM); // set to S/M size
                    else
                        continue;

                    // Save file is now decrypted!
                    // Trigger Loading of the decrypted save file.
                    openSAV(SaveUtil.getVariantSAV(decryptedPS), path);
                    return true;
                }
                // End file check loop, check the input path for xorpads too if it isn't the same as the EXE (quite common).
                xorpath = Path.GetDirectoryName(path); // try again in the next folder up
            }
            return false; // no xorpad compatible
        }
        private void openSAV(SaveFile sav, string path)
        {
            if (sav == null || sav.Version == GameVersion.Invalid)
            { Util.Error("Invalid save file loaded. Aborting.", path); return; }

            if (!string.IsNullOrEmpty(path)) // If path is null, this is the default save
            {
                if (sav.RequiresMemeCrypto && !MemeCrypto.CanUseMemeCrypto())
                {
                    Util.Error("Your platform does not support the required cryptography components.", "In order to be able to save your changes, you must either upgrade to a newer version of Windows or disable FIPS compliance mode.");
                    // Don't abort loading; user can still view save and fix checksum on another platform.
                }
            }            

            // Finish setting up the save file.
            if (sav.IndeterminateGame && sav.Generation == 3)
            {
                // Hacky cheats invalidated the Game Code value.
                var drGame = Util.Prompt(MessageBoxButtons.YesNoCancel,
                    "Unknown Gen3 Game Detected. Select Origins:",
                    "Yes: Ruby / Sapphire" + Environment.NewLine +
                    "No: Emerald" + Environment.NewLine +
                    "Cancel: FireRed / LeafGreen");

                switch (drGame) // Reset save file info
                {
                    case DialogResult.Yes: sav = new SAV3(sav.BAK, GameVersion.RS); break;
                    case DialogResult.No: sav = new SAV3(sav.BAK, GameVersion.E); break;
                    case DialogResult.Cancel: sav = new SAV3(sav.BAK, GameVersion.FRLG); break;
                    default: return;
                }
            }
            if (sav.IndeterminateLanguage)
            {
                // Japanese Save files are different. Get isJapanese
                var drJP = Util.Prompt(MessageBoxButtons.YesNoCancel, $"{sav.Version} Save File detected. Select language...", 
                    "Yes: International" + Environment.NewLine + "No: Japanese");
                if (drJP == DialogResult.Cancel)
                    return;

                sav.Japanese = drJP == DialogResult.No;
            }
            if (sav.IndeterminateSubVersion && sav.Version == GameVersion.FRLG)
            {
                var drFRLG = Util.Prompt(MessageBoxButtons.YesNoCancel, $"{sav.Version} detected. Select version...",
                    "Yes: FireRed" + Environment.NewLine + "No: LeafGreen");
                if (drFRLG == DialogResult.Cancel)
                    return;

                sav.Personal = drFRLG == DialogResult.Yes ? PersonalTable.FR : PersonalTable.LG;
            }

            // clean fields
            PKM pk = preparePKM();
            populateFields(SAV.BlankPKM);
            SAV = sav;

            if (path != null) // Actual save file
            {
                SAV.FilePath = Path.GetDirectoryName(path);
                SAV.FileName = Path.GetExtension(path) == ".bak"
                    ? Path.GetFileName(path).Split(new[] { " [" }, StringSplitOptions.None)[0]
                    : Path.GetFileName(path);
                Text = $"PKH{(HaX ? "a" : "e")}X - " + $"SAV{SAV.Generation}: {Path.GetFileNameWithoutExtension(Util.CleanFileName(SAV.BAKName))}"; // more descriptive

                // If backup folder exists, save a backup.
                string backupName = Path.Combine(BackupPath, Util.CleanFileName(SAV.BAKName));
                if (SAV.Exportable && Directory.Exists(BackupPath) && !File.Exists(backupName))
                    File.WriteAllBytes(backupName, SAV.BAK);

                GB_SAVtools.Visible = true;
            }
            else // Blank save file
            {
                SAV.FilePath = null;
                SAV.FileName = "Blank Save File";
                Text = $"PKH{(HaX ? "a" : "e")}X - " + $"SAV{SAV.Generation}: {SAV.FileName} [{SAV.OT} ({SAV.Version})]";

                GB_SAVtools.Visible = false;
            }
            Menu_ExportSAV.Enabled = B_VerifyCHK.Enabled = SAV.Exportable;

            // Close subforms that are save dependent
            Type[] f = { typeof(SAV_BoxViewer), typeof(f2_Text) };
            foreach (var form in Application.OpenForms.Cast<Form>().Where(form => f.Contains(form.GetType())).ToArray())
                form.Close();

            setBoxNames();   // Display the Box Names
            if (SAV.HasBox)
            {
                int startBox = path == null ? 0 : SAV.CurrentBox; // FF if BattleBox
                if (startBox > SAV.BoxCount - 1) { tabBoxMulti.SelectedIndex = 1; CB_BoxSelect.SelectedIndex = 0; }
                else { tabBoxMulti.SelectedIndex = 0; CB_BoxSelect.SelectedIndex = startBox; }
            }
            setPKXBoxes();   // Reload all of the PKX Windows

            bool WindowTranslationRequired = false;

            // Hide content if not present in game.
            GB_SUBE.Visible = SAV.HasSUBE;
            PB_Locked.Visible = SAV.HasBattleBox && SAV.BattleBoxLocked;

            if (!SAV.HasBox && tabBoxMulti.TabPages.Contains(Tab_Box))
                tabBoxMulti.TabPages.Remove(Tab_Box);
            else if (SAV.HasBox && !tabBoxMulti.TabPages.Contains(Tab_Box))
            {
                tabBoxMulti.TabPages.Insert(0, Tab_Box);
                WindowTranslationRequired = true;
            }
            Menu_LoadBoxes.Enabled = Menu_DumpBoxes.Enabled = Menu_Report.Enabled = Menu_Modify.Enabled = B_SaveBoxBin.Enabled = SAV.HasBox;

            int BoxTab = tabBoxMulti.TabPages.IndexOf(Tab_Box);
            int PartyTab = tabBoxMulti.TabPages.IndexOf(Tab_PartyBattle);

            if (!SAV.HasParty && tabBoxMulti.TabPages.Contains(Tab_PartyBattle))
                tabBoxMulti.TabPages.Remove(Tab_PartyBattle);
            else if (SAV.HasParty && !tabBoxMulti.TabPages.Contains(Tab_PartyBattle))
            {
                int index = BoxTab;
                if (index < 0)
                    index = -1;
                tabBoxMulti.TabPages.Insert(index + 1, Tab_PartyBattle);
                WindowTranslationRequired = true;
            }

            if (!SAV.HasDaycare && tabBoxMulti.TabPages.Contains(Tab_Other))
                tabBoxMulti.TabPages.Remove(Tab_Other);
            else if (SAV.HasDaycare && !tabBoxMulti.TabPages.Contains(Tab_Other))
            {
                int index = PartyTab;
                if (index < 0)
                    index = BoxTab;
                if (index < 0)
                    index = -1;
                tabBoxMulti.TabPages.Insert(index + 1, Tab_Other);
                WindowTranslationRequired = true;
            }

            if (path != null) // Actual save file
            {
                PAN_BattleBox.Visible = L_BattleBox.Visible = L_ReadOnlyPBB.Visible = SAV.HasBattleBox;
                GB_Daycare.Visible = SAV.HasDaycare;
                GB_Fused.Visible = SAV.HasFused;
                GB_GTS.Visible = SAV.HasGTS;
                B_OpenSecretBase.Enabled = SAV.HasSecretBase;
                B_OpenPokepuffs.Enabled = SAV.HasPuff;
                B_OpenPokeBeans.Enabled = SAV.Generation == 7;
                B_OUTPasserby.Enabled = SAV.HasPSS;
                B_OpenBoxLayout.Enabled = SAV.HasBoxWallpapers;
                B_OpenWondercards.Enabled = SAV.HasWondercards;
                B_OpenSuperTraining.Enabled = SAV.HasSuperTrain;
                B_OpenHallofFame.Enabled = SAV.HasHoF;
                B_OpenOPowers.Enabled = SAV.HasOPower;
                B_OpenPokedex.Enabled = SAV.HasPokeDex;
                B_OpenBerryField.Enabled = SAV.HasBerryField && SAV.XY;
                B_OpenPokeblocks.Enabled = SAV.HasPokeBlock;
                B_JPEG.Visible = SAV.HasJPEG;
                B_OpenEventFlags.Enabled = SAV.HasEvents;
                B_OpenLinkInfo.Enabled = SAV.HasLink;
                B_CGearSkin.Enabled = SAV.Generation == 5;

                B_OpenTrainerInfo.Visible = B_OpenItemPouch.Visible = SAV.HasParty; // Box RS
            }
            GB_SAVtools.Visible = FLP_SAVtools.Controls.Cast<Control>().Any(c => c.Enabled);
            foreach (Control c in FLP_SAVtools.Controls.Cast<Control>())
                c.Visible = c.Enabled;


            // Generational Interface
            byte[] extraBytes = new byte[1];
            Tip1.RemoveAll(); Tip2.RemoveAll(); Tip3.RemoveAll(); // TSV/PSV

            FLP_Country.Visible = FLP_SubRegion.Visible = FLP_3DSRegion.Visible = SAV.Generation >= 6;
            Label_EncryptionConstant.Visible = BTN_RerollEC.Visible = TB_EC.Visible = SAV.Generation >= 6;
            GB_nOT.Visible = GB_RelearnMoves.Visible = BTN_Medals.Visible = BTN_History.Visible = SAV.Generation >= 6;
            PB_Legal.Visible = PB_WarnMove1.Visible = PB_WarnMove2.Visible = PB_WarnMove3.Visible = PB_WarnMove4.Visible = SAV.Generation >= 6;

            PB_MarkPentagon.Visible = SAV.Generation >= 6;
            PB_MarkAlola.Visible = SAV.Generation >= 7;
            TB_Secure1.Visible = TB_Secure2.Visible = L_GameSync.Visible = L_Secure1.Visible = L_Secure2.Visible = SAV.Exportable && SAV.Generation >= 6;
            TB_GameSync.Visible = SAV.Exportable && SAV.Generation == 6;

            FLP_NSparkle.Visible = L_NSparkle.Visible = CHK_NSparkle.Visible = SAV.Generation == 5;

            CB_Form.Visible = Label_Form.Visible = CHK_AsEgg.Visible = GB_EggConditions.Visible = PB_Mark5.Visible = PB_Mark6.Visible = SAV.Generation >= 4;

            DEV_Ability.Enabled = DEV_Ability.Visible = SAV.Generation > 3 && HaX;
            CB_Ability.Visible = !DEV_Ability.Enabled && SAV.Generation >= 3;
            FLP_Nature.Visible = SAV.Generation >= 3;
            FLP_Ability.Visible = SAV.Generation >= 3;
            FLP_Language.Visible = SAV.Generation >= 3;
            GB_ExtraBytes.Visible = GB_ExtraBytes.Enabled = SAV.Generation >= 3;
            GB_Markings.Visible = SAV.Generation >= 3;
            BTN_Ribbons.Visible = SAV.Generation >= 3;
            CB_HPType.Enabled = CB_Form.Enabled = SAV.Generation >= 3;
            BTN_RerollPID.Visible = Label_PID.Visible = TB_PID.Visible = Label_SID.Visible = TB_SID.Visible = SAV.Generation >= 3;

            FLP_FriendshipForm.Visible = SAV.Generation >= 2;
            FLP_HeldItem.Visible = SAV.Generation >= 2;
            CHK_IsEgg.Visible = Label_Gender.Visible = SAV.Generation >= 2;
            FLP_PKRS.Visible = FLP_EggPKRSRight.Visible = SAV.Generation >= 2;
            Label_OTGender.Visible = SAV.Generation >= 2;

            if (SAV.Generation == 1)
                Label_IsShiny.Visible = false;

            if (SAV.Version == GameVersion.BATREV)
            {
                L_SaveSlot.Visible = CB_SaveSlot.Visible = true;
                CB_SaveSlot.Items.Clear();
                CB_SaveSlot.DisplayMember = "Text"; CB_SaveSlot.ValueMember = "Value";
                CB_SaveSlot.DataSource = new BindingSource(((SAV4BR) SAV).SaveSlots.Select(i => new ComboItem
                {
                    Text = ((SAV4BR) SAV).SaveNames[i],
                    Value = i
                }).ToList(), null);
                CB_SaveSlot.SelectedValue = ((SAV4BR)SAV).CurrentSlot;
            }
            else
                L_SaveSlot.Visible = CB_SaveSlot.Visible = false;
            
            FLP_Purification.Visible = FLP_ShadowID.Visible = SAV.Version == GameVersion.COLO || SAV.Version == GameVersion.XD;
            NUD_ShadowID.Maximum = SAV.MaxShadowID;

            // HaX override, needs to be after DEV_Ability enabled assignment.
            TB_AbilityNumber.Visible = SAV.Generation >= 6 && DEV_Ability.Enabled;

            // Met Tab
            FLP_MetDate.Visible = SAV.Generation >= 4;
            FLP_Fateful.Visible = FLP_Ball.Visible = FLP_OriginGame.Visible = SAV.Generation >= 3;
            FLP_MetLocation.Visible = FLP_MetLevel.Visible = SAV.Generation >= 2;
            FLP_TimeOfDay.Visible = SAV.Generation == 2;

            // Stats
            FLP_StatsTotal.Visible = SAV.Generation >= 3;
            FLP_Characteristic.Visible = SAV.Generation >= 3;
            FLP_HPType.Visible = SAV.Generation >= 2;

            PAN_Contest.Visible = SAV.Generation >= 3;

            // Second daycare slot
            SlotPictureBoxes[43].Visible = SAV.Generation >= 2;

            if (sav.Generation == 1)
            {
                FLP_SpD.Visible = false;
                Label_SPA.Visible = false;
                Label_SPC.Visible = true;
                TB_HPIV.Enabled = false;
                MaskedTextBox[] evControls = { TB_SPAEV, TB_HPEV, TB_ATKEV, TB_DEFEV, TB_SPEEV, TB_SPDEV };
                foreach (var ctrl in evControls)
                {
                    ctrl.Mask = "00000";
                    ctrl.Size = Stat_HP.Size;
                }
            }
            else if (sav.Generation == 2)
            {
                FLP_SpD.Visible = true;
                Label_SPA.Visible = true;
                Label_SPC.Visible = false;
                TB_SPDEV.Enabled = TB_SPDIV.Enabled = false;
                TB_HPIV.Enabled = false;
                MaskedTextBox[] evControls = { TB_SPAEV, TB_HPEV, TB_ATKEV, TB_DEFEV, TB_SPEEV, TB_SPDEV };
                foreach (var ctrl in evControls)
                {
                    ctrl.Mask = "00000";
                    ctrl.Size = Stat_HP.Size;
                }
            }
            else
            {
                FLP_SpD.Visible = true;
                Label_SPA.Visible = true;
                Label_SPC.Visible = false;
                TB_SPDEV.Enabled = TB_SPDIV.Enabled = true;
                TB_HPIV.Enabled = true;
                MaskedTextBox[] evControls = { TB_SPAEV, TB_HPEV, TB_ATKEV, TB_DEFEV, TB_SPEEV, TB_SPDEV };
                foreach (var ctrl in evControls)
                {
                    ctrl.Mask = "000";
                    ctrl.Size = TB_ExtraByte.Size;
                }
            }

            // Recenter PKM SubEditors
            FLP_PKMEditors.Location = new Point((Tab_OTMisc.Width - FLP_PKMEditors.Width) / 2, FLP_PKMEditors.Location.Y);

            bool init = fieldsInitialized;
            fieldsInitialized = fieldsLoaded = false;

            switch (SAV.Generation)
            {
                case 1:
                    getFieldsfromPKM = populateFieldsPK1;
                    getPKMfromFields = preparePK1;
                    extraBytes = new byte[] {};
                    break;
                case 2:
                    getFieldsfromPKM = populateFieldsPK2;
                    getPKMfromFields = preparePK2;
                    extraBytes = new byte[] { };
                    break;
                case 3:
                    if (SAV.Version == GameVersion.COLO)
                    {
                        getFieldsfromPKM = populateFieldsCK3;
                        getPKMfromFields = prepareCK3;
                        extraBytes = CK3.ExtraBytes;
                        break;
                    }
                    if (SAV.Version == GameVersion.XD)
                    {
                        getFieldsfromPKM = populateFieldsXK3;
                        getPKMfromFields = prepareXK3;
                        extraBytes = XK3.ExtraBytes;
                        break;
                    }
                    getFieldsfromPKM = populateFieldsPK3;
                    getPKMfromFields = preparePK3;
                    extraBytes = PK3.ExtraBytes;
                    break;
                case 4:
                    if (SAV.Version == GameVersion.BATREV)
                    {
                        getFieldsfromPKM = populateFieldsBK4;
                        getPKMfromFields = prepareBK4;
                    }
                    else
                    {
                        getFieldsfromPKM = populateFieldsPK4;
                        getPKMfromFields = preparePK4;
                    }
                    extraBytes = PK4.ExtraBytes;
                    break;
                case 5:
                    getFieldsfromPKM = populateFieldsPK5;
                    getPKMfromFields = preparePK5;
                    extraBytes = PK5.ExtraBytes;
                    break;
                case 6:
                    getFieldsfromPKM = populateFieldsPK6;
                    getPKMfromFields = preparePK6;
                    extraBytes = PK6.ExtraBytes;
                    TB_GameSync.Enabled = (SAV.GameSyncID ?? 0) != 0;
                    TB_GameSync.Text = SAV.GameSyncID?.ToString("X16");
                    TB_Secure1.Text = SAV.Secure1?.ToString("X16");
                    TB_Secure2.Text = SAV.Secure2?.ToString("X16");
                    break;
                case 7:
                    getFieldsfromPKM = populateFieldsPK7;
                    getPKMfromFields = preparePK7;
                    extraBytes = PK7.ExtraBytes;
                    TB_GameSync.Enabled = (SAV.GameSyncID ?? 0) != 0;
                    TB_GameSync.Text = SAV.GameSyncID?.ToString("X16");
                    TB_Secure1.Text = SAV.Secure1?.ToString("X16");
                    TB_Secure2.Text = SAV.Secure2?.ToString("X16");
                    break;
            }
            pkm = pkm.GetType() != SAV.PKMType ? SAV.BlankPKM : pk;
            if (pkm.Format < 3)
                pkm = SAV.BlankPKM;
            populateFilteredDataSources();
            populateFields(pkm);
            fieldsInitialized |= init;

            // SAV Specific Limits
            TB_OT.MaxLength = SAV.OTLength;
            TB_OTt2.MaxLength = SAV.OTLength;
            TB_Nickname.MaxLength = SAV.NickLength;

            // Hide Unused Tabs
            if (SAV.Generation == 1 && tabMain.TabPages.Contains(Tab_Met))
                tabMain.TabPages.Remove(Tab_Met);
            else if (SAV.Generation != 1 && !tabMain.TabPages.Contains(Tab_Met))
            {
                tabMain.TabPages.Insert(1, Tab_Met);
                WindowTranslationRequired = true;
            }

            // Common HaX Interface
            CHK_HackedStats.Enabled = CHK_HackedStats.Visible = MT_Level.Enabled = MT_Level.Visible = MT_Form.Enabled = MT_Form.Visible = HaX;
            TB_Level.Visible = !HaX;

            // Load Extra Byte List
            if (GB_ExtraBytes.Enabled)
            {
                CB_ExtraBytes.Items.Clear();
                foreach (byte b in extraBytes)
                    CB_ExtraBytes.Items.Add("0x" + b.ToString("X2"));
                CB_ExtraBytes.SelectedIndex = 0;
            }

            // pk2 save files do not have an Origin Game stored. Prompt the met location list to update.
            if (SAV.Generation == 2)
                updateOriginGame(null, null);

            // Refresh PK* conversion info
            PKMConverter.updateConfig(SAV.SubRegion, SAV.Country, SAV.ConsoleRegion, SAV.OT, SAV.Gender);

            if (WindowTranslationRequired) // force update -- re-added controls may be untranslated
                Util.TranslateInterface(this, curlanguage);
            
            // No changes made yet
            UndoStack.Clear(); Menu_Undo.Enabled = false;
            RedoStack.Clear(); Menu_Redo.Enabled = false;

            // Indicate audibly the save is loaded
            SystemSounds.Beep.Play();
        }
        private static void refreshWC6DB()
        {
            List<MysteryGift> wc6db = new List<MysteryGift>();
            byte[] wc6bin = Mass_Editor.Properties.Resources.wc6;
            for (int i = 0; i < wc6bin.Length; i += WC6.Size)
            {
                byte[] data = new byte[WC6.Size];
                Array.Copy(wc6bin, i, data, 0, WC6.Size);
                wc6db.Add(new WC6(data));
            }
            byte[] wc6full = Mass_Editor.Properties.Resources.wc6full;
            for (int i = 0; i < wc6full.Length; i += WC6.SizeFull)
            {
                byte[] data = new byte[WC6.SizeFull];
                Array.Copy(wc6full, i, data, 0, WC6.SizeFull);
                wc6db.Add(new WC6(data));
            }

            if (Directory.Exists(MGDatabasePath))
                wc6db.AddRange(from file in Directory.GetFiles(MGDatabasePath, "*", SearchOption.AllDirectories)
                    let fi = new FileInfo(file)
                    where new[] {".wc6full", ".wc6"}.Contains(fi.Extension) && new[] {WC6.Size, WC6.SizeFull}.Contains((int)fi.Length)
                    select new WC6(File.ReadAllBytes(file)));

            Legal.MGDB_G6 = wc6db.Distinct().ToArray();
        }
        private static void refreshWC7DB()
        {
            List<MysteryGift> wc7db = new List<MysteryGift>();
            byte[] wc7bin = Mass_Editor.Properties.Resources.wc7;
            for (int i = 0; i < wc7bin.Length; i += WC7.Size)
            {
                byte[] data = new byte[WC7.Size];
                Array.Copy(wc7bin, i, data, 0, WC7.Size);
                wc7db.Add(new WC7(data));
            }
            byte[] wc7full = Mass_Editor.Properties.Resources.wc7full;
            for (int i = 0; i < wc7full.Length; i += WC7.SizeFull)
            {
                byte[] data = new byte[WC7.SizeFull];
                Array.Copy(wc7full, i, data, 0, WC7.SizeFull);
                wc7db.Add(new WC7(data));
            }

            if (Directory.Exists(MGDatabasePath))
                wc7db.AddRange(from file in Directory.GetFiles(MGDatabasePath, "*", SearchOption.AllDirectories)
                               let fi = new FileInfo(file)
                               where new[] { ".wc7full", ".wc7" }.Contains(fi.Extension) && new[] { WC7.Size, WC7.SizeFull }.Contains((int)fi.Length)
                               select new WC7(File.ReadAllBytes(file)));

            Legal.MGDB_G7 = wc7db.Distinct().ToArray();
        }

        // Language Translation
        private void changeMainLanguage(object sender, EventArgs e)
        {
            PKM pk = SAV.getPKM((fieldsInitialized ? preparePKM() : pkm).Data);
            bool alreadyInit = fieldsInitialized;
            fieldsInitialized = false;
            Menu_Options.DropDown.Close();
            InitializeStrings();
            InitializeLanguage();
            string ProgramTitle = Text;
            Util.TranslateInterface(this, curlanguage); // Translate the UI to language.
            Text = ProgramTitle;
            // Recenter PKM SubEditors
            FLP_PKMEditors.Location = new Point((Tab_OTMisc.Width - FLP_PKMEditors.Width)/2, FLP_PKMEditors.Location.Y);
            populateFields(pk); // put data back in form
            fieldsInitialized |= alreadyInit;

            // Set the culture (makes it easy to pass language to other forms)
            Mass_Editor.Properties.Settings.Default.Language = curlanguage;
            Mass_Editor.Properties.Settings.Default.Save();
            Thread.CurrentThread.CurrentCulture = new CultureInfo(curlanguage.Substring(0, 2));
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
        private void InitializeStrings()
        {
            if (CB_MainLanguage.SelectedIndex < 8)
                curlanguage = GameInfo.lang_val[CB_MainLanguage.SelectedIndex];
            
            string l = curlanguage;
            GameStrings = GameInfo.getStrings(l);

            // Force an update to the met locations
            origintrack = GameVersion.Unknown;

            // Update Legality Analysis strings
            LegalityAnalysis.movelist = GameStrings.movelist;

            if (fieldsInitialized)
                updateIVs(null, null); // Prompt an update for the characteristics
        }
        #endregion

        #region //// PKX WINDOW FUNCTIONS ////
        private void InitializeFields()
        {
            // Now that the ComboBoxes are ready, load the data.
            fieldsInitialized = true;
            pkm.RefreshChecksum();

            // Load Data
            populateFields(pkm);
            {
                CB_Species.SelectedValue = 493;
                CB_Move1.SelectedValue = 1;
                TB_OT.Text = "PKHeX";
                TB_TID.Text = 12345.ToString();
                TB_SID.Text = 54321.ToString();
                CB_GameOrigin.SelectedIndex = 0;
                int curlang = Array.IndexOf(GameInfo.lang_val, curlanguage);
                CB_Language.SelectedIndex = curlang > CB_Language.Items.Count - 1 ? 1 : curlang;
                CB_BoxSelect.SelectedIndex = 0;
                CB_Ball.SelectedIndex = 0;
                CB_Country.SelectedIndex = 0;
                CAL_MetDate.Value = CAL_EggDate.Value = DateTime.Today;
            }
        }
        private void InitializeLanguage()
        {
            ComboBox[] cbs =
            {
                CB_Country, CB_SubRegion, CB_3DSReg, CB_Language, CB_Ball, CB_HeldItem, CB_Species, DEV_Ability,
                CB_Nature, CB_EncounterType, CB_GameOrigin, CB_HPType
            };
            foreach (var cb in cbs) { cb.DisplayMember = "Text"; cb.ValueMember = "Value"; }

            // Set the various ComboBox DataSources up with their allowed entries
            setCountrySubRegion(CB_Country, "countries");
            CB_3DSReg.DataSource = Util.getUnsortedCBList("regions3ds");

            GameInfo.InitializeDataSources(GameStrings);

            CB_EncounterType.DataSource = Util.getCBList(GameStrings.encountertypelist, new[] { 0 }, Legal.Gen4EncounterTypes);
            CB_HPType.DataSource = Util.getCBList(GameStrings.types.Skip(1).Take(16).ToArray(), null);
            CB_Nature.DataSource = new BindingSource(GameInfo.NatureDataSource, null);

            populateFilteredDataSources();
        }
        private void populateFilteredDataSources()
        {
            GameInfo.setItemDataSource(HaX, SAV.MaxItemID, SAV.HeldItems, SAV.Generation, SAV.Version, GameStrings);
            if (SAV.Generation > 1)
                CB_HeldItem.DataSource = new BindingSource(GameInfo.ItemDataSource.Where(i => i.Value <= SAV.MaxItemID).ToList(), null);

            var languages = Util.getUnsortedCBList("languages");
            if (SAV.Generation < 7)
                languages = languages.Where(l => l.Value <= 8).ToList(); // Korean
            CB_Language.DataSource = languages;

            CB_Ball.DataSource = new BindingSource(GameInfo.BallDataSource.Where(b => b.Value <= SAV.MaxBallID).ToList(), null);
            CB_Species.DataSource = new BindingSource(GameInfo.SpeciesDataSource.Where(s => s.Value <= SAV.MaxSpeciesID).ToList(), null);
            DEV_Ability.DataSource = new BindingSource(GameInfo.AbilityDataSource.Where(a => a.Value <= SAV.MaxAbilityID).ToList(), null);
            CB_GameOrigin.DataSource = new BindingSource(GameInfo.VersionDataSource.Where(g => g.Value <= SAV.MaxGameID || SAV.Generation >= 3 && g.Value == 15).ToList(), null);

            // Set the Move ComboBoxes too..
            var moves = (HaX ? GameInfo.HaXMoveDataSource : GameInfo.MoveDataSource).Where(m => m.Value <= SAV.MaxMoveID).ToList(); // Filter Z-Moves if appropriate
            foreach (ComboBox cb in new[] { CB_Move1, CB_Move2, CB_Move3, CB_Move4, CB_RelearnMove1, CB_RelearnMove2, CB_RelearnMove3, CB_RelearnMove4 })
            {
                cb.DisplayMember = "Text"; cb.ValueMember = "Value";
                cb.DataSource = new BindingSource(moves, null);
            }
        }
        private Action getFieldsfromPKM;
        private Func<PKM> getPKMfromFields;
        public void populateFields(PKM pk, bool focus = true)
        {
            if (pk == null) { Util.Error("Attempted to load a null file."); return; }
            
            if ((pk.Format >= 3 && pk.Format > SAV.Generation) // pk3-7, can't go backwards
                || (pk.Format <= 2 && SAV.Generation > 2 && SAV.Generation < 7)) // pk1-2, can't go 3-6
            { Util.Alert($"Can't load Gen{pk.Format} to Gen{SAV.Generation} games."); return; }

            bool oldInit = fieldsInitialized;
            fieldsInitialized = fieldsLoaded = false;
            if (focus)
                Tab_Main.Focus();

            pkm = pk.Clone();

            if (fieldsInitialized & !pkm.ChecksumValid)
                Util.Alert("PKX File has an invalid checksum.");

            if (pkm.Format != SAV.Generation) // past gen format
            {
                string c;
                pkm = PKMConverter.convertToFormat(pkm, SAV.PKMType, out c);
                if (pk.Format != pkm.Format && focus) // converted
                    Util.Alert("Converted File.");
            }

            try { getFieldsfromPKM(); }
            catch { fieldsInitialized = oldInit; throw; }

            CB_EncounterType.Visible = Label_EncounterType.Visible = pkm.Gen4 && SAV.Generation < 7;
            fieldsInitialized = oldInit;
            updateIVs(null, null);
            updatePKRSInfected(null, null);
            updatePKRSCured(null, null);

            if (HaX)
            {
                MT_Level.Text = pkm.Stat_Level.ToString();
                MT_Form.Text = pkm.AltForm.ToString();
                if (pkm.Stat_HPMax != 0) // stats present
                {
                    Stat_HP.Text = pkm.Stat_HPCurrent.ToString();
                    Stat_ATK.Text = pkm.Stat_ATK.ToString();
                    Stat_DEF.Text = pkm.Stat_DEF.ToString();
                    Stat_SPA.Text = pkm.Stat_SPA.ToString();
                    Stat_SPD.Text = pkm.Stat_SPD.ToString();
                    Stat_SPE.Text = pkm.Stat_SPE.ToString();
                }
            }
            fieldsLoaded = true;

            Label_HatchCounter.Visible = CHK_IsEgg.Checked && SAV.Generation > 1;
            Label_Friendship.Visible = !CHK_IsEgg.Checked && SAV.Generation > 1;

            // Set the Preview Box
            dragout.Image = pk.Sprite;
            setMarkings();
            updateLegality();
        }

        // General Use Functions shared by other Forms // 
        internal static void setCountrySubRegion(ComboBox CB, string type)
        {
            int index = CB.SelectedIndex;
            // fix for Korean / Chinese being swapped
            string cl = curlanguage + "";
            cl = cl == "zh" ? "ko" : cl == "ko" ? "zh" : cl;

            CB.DataSource = Util.getCBList(type, cl);

            if (index > 0 && index < CB.Items.Count && fieldsInitialized)
                CB.SelectedIndex = index;
        }
        private void setForms()
        {
            int species = Util.getIndex(CB_Species);
            if (SAV.Generation < 4 && species != 201)
            {
                Label_Form.Visible = CB_Form.Visible = CB_Form.Enabled = false;
                return;
            }

            bool hasForms = SAV.Personal[species].HasFormes || new[] { 201, 664, 665, 414 }.Contains(species);
            CB_Form.Enabled = CB_Form.Visible = Label_Form.Visible = hasForms;
            
            if (HaX && SAV.Generation >= 4)
                Label_Form.Visible = true;

            if (!hasForms)
                return;

            var ds = PKX.getFormList(species, GameStrings.types, GameStrings.forms, gendersymbols, SAV.Generation).ToList();
            if (ds.Count == 1 && string.IsNullOrEmpty(ds[0])) // empty (Alolan Totems)
                CB_Form.Enabled = CB_Form.Visible = Label_Form.Visible = false;
            else CB_Form.DataSource = ds;
        }
        private void setAbilityList()
        {
            if (SAV.Generation < 3) // no abilities
                return;

            int formnum = 0;
            int species = Util.getIndex(CB_Species);
            if (SAV.Generation > 3) // has forms
                formnum = CB_Form.SelectedIndex;

            int[] abils = SAV.Personal.getAbilities(species, formnum);
            if (abils[1] == 0 && SAV.Generation != 3)
                abils[1] = abils[0];
            string[] abilIdentifier = {" (1)", " (2)", " (H)"};
            List<string> ability_list = abils.Where(a => a != 0).Select((t, i) => GameStrings.abilitylist[t] + abilIdentifier[i]).ToList();
            if (!ability_list.Any())
                ability_list.Add(GameStrings.abilitylist[0] + abilIdentifier[0]);

            int abil = CB_Ability.SelectedIndex;
            CB_Ability.DataSource = ability_list;
            CB_Ability.SelectedIndex = abil < 0 || abil >= CB_Ability.Items.Count ? 0 : abil;
        }
        // PKX Data Calculation Functions //
        private void setIsShiny(object sender)
        {
            if (sender == TB_PID)
                pkm.PID = Util.getHEXval(TB_PID.Text);
            else if (sender == TB_TID)
                pkm.TID = (int)Util.ToUInt32(TB_TID.Text);
            else if (sender == TB_SID)
                pkm.SID = (int)Util.ToUInt32(TB_SID.Text);

            bool isShiny = pkm.IsShiny;

            // Set the Controls
            BTN_Shinytize.Visible = BTN_Shinytize.Enabled = !isShiny && SAV.Generation > 1;
            Label_IsShiny.Visible = isShiny && SAV.Generation > 1;

            // Refresh Markings (for Shiny Star if applicable)
            setMarkings();
        }
        private void setMarkings()
        {
            PictureBox[] pba = { PB_Mark1, PB_Mark2, PB_Mark3, PB_Mark4, PB_Mark5, PB_Mark6 };
            for (int i = 0; i < pba.Length; i++)
                pba[i].Image = Util.ChangeOpacity(pba[i].InitialImage, pkm.Markings[i] != 0 ? 1 : 0.1);

            PB_MarkShiny.Image = Util.ChangeOpacity(PB_MarkShiny.InitialImage, !BTN_Shinytize.Enabled ? 1 : 0.1);
            PB_MarkCured.Image = Util.ChangeOpacity(PB_MarkCured.InitialImage, CHK_Cured.Checked ? 1 : 0.1);
            
            PB_MarkPentagon.Image = Util.ChangeOpacity(PB_MarkPentagon.InitialImage, pkm.Gen6 ? 1 : 0.1);

            // Gen7 Markings
            if (pkm.Format != 7)
                return;

            PB_MarkAlola.Image = Util.ChangeOpacity(PB_MarkAlola.InitialImage, pkm.Gen7 ? 1 : 0.1);
            for (int i = 0; i < pba.Length; i++)
            {
                switch (pkm.Markings[i])
                {
                    case 1:
                        pba[i].Image = Util.ChangeAllColorTo(pba[i].Image, Color.FromArgb(000, 191, 255));
                        break;
                    case 2:
                        pba[i].Image = Util.ChangeAllColorTo(pba[i].Image, Color.FromArgb(255, 117, 179));
                        break;
                }
            }
        }
        // Clicked Label Shortcuts //
        private void clickQR(object sender, EventArgs e)
        {
            if (SAV.Generation <= 3)
            {
                Util.Alert("QR feature not available for loaded game.");
                return;
            }
            if (ModifierKeys == Keys.Alt)
            {
                // Fetch data from QR code...
                byte[] ekx = QR.getQRData();

                if (ekx == null) return;
                
                PKM pk = PKMConverter.getPKMfromBytes(ekx);
                if (pk == null) { Util.Alert("Decoded data not a valid PKM.", $"QR Data Size: {ekx.Length}"); }
                else
                {
                    if (!pk.Valid || pk.Species <= 0)
                    { Util.Alert("Invalid data detected."); return; }

                    string c; PKM pkz = PKMConverter.convertToFormat(pk, SAV.PKMType, out c);
                    if (pkz == null)
                    { Util.Alert(c); return; }

                    populateFields(pkz);
                }
            }
            else
            {
                if (!verifiedPKM()) return;
                PKM pkx = preparePKM();
                byte[] ekx = pkx.EncryptedBoxData;

                const string server = "http://loadcode.projectpokemon.org/b1s1.html#"; // Rehosted with permission from LC/MS -- massive thanks!
                Image qr;
                switch (pkx.Format)
                {
                    case 7:
                        qr = QR7.GenerateQRCode7((PK7) pkx);
                        break;
                    default:
                        qr = QR.getQRImage(ekx, pkx.Format == 6 ? server : "null/#");
                        break;
                }

                if (qr == null) return;

                string[] r = pkx.QRText;
                const string refURL = "PKHeX @ ProjectPokemon.org";
                new QR(qr, dragout.Image, r[0], r[1], r[2], $"{refURL} ({pkx.GetType().Name})", pkx).ShowDialog();
            }
        }
        private void clickFriendship(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control) // prompt to reset
                TB_Friendship.Text = pkm.CurrentFriendship.ToString();
            else
                TB_Friendship.Text = TB_Friendship.Text == "255" ? SAV.Personal[pkm.Species].BaseFriendship.ToString() : "255";
        }
        private void clickGender(object sender, EventArgs e)
        {
            if (SAV.Generation == 2)
                return;

            // Get Gender Threshold
            int gt = SAV.Personal[Util.getIndex(CB_Species)].Gender;

            if (gt == 255 || gt == 0 || gt == 254) // Single gender/genderless
                return;

            if (gt >= 255) return; 
            // If not a single gender(less) species: (should be <254 but whatever, 255 never happens)

            int newGender = PKX.getGender(Label_Gender.Text) ^ 1;
            if (SAV.Generation <= 4)
            {
                pkm.Species = Util.getIndex(CB_Species);
                pkm.Version = Util.getIndex(CB_GameOrigin);
                pkm.Nature = Util.getIndex(CB_Nature);
                pkm.AltForm = CB_Form.SelectedIndex;

                pkm.setPIDGender(newGender);
                TB_PID.Text = pkm.PID.ToString("X8");
            }
            pkm.Gender = newGender;
            Label_Gender.Text = gendersymbols[pkm.Gender];
            Label_Gender.ForeColor = pkm.Gender == 2 ? Label_Species.ForeColor : (pkm.Gender == 1 ? Color.Red : Color.Blue);


            if (PKX.getGender(CB_Form.Text) < 2) // Gendered Forms
                CB_Form.SelectedIndex = PKX.getGender(Label_Gender.Text);

            getQuickFiller(dragout);
        }
        private void clickPPUps(object sender, EventArgs e)
        {
            CB_PPu1.SelectedIndex = ModifierKeys != Keys.Control && Util.getIndex(CB_Move1) > 0 ? 3 : 0;
            CB_PPu2.SelectedIndex = ModifierKeys != Keys.Control && Util.getIndex(CB_Move2) > 0 ? 3 : 0;
            CB_PPu3.SelectedIndex = ModifierKeys != Keys.Control && Util.getIndex(CB_Move3) > 0 ? 3 : 0;
            CB_PPu4.SelectedIndex = ModifierKeys != Keys.Control && Util.getIndex(CB_Move4) > 0 ? 3 : 0;
        }
        private void clickMarking(object sender, EventArgs e)
        {
            PictureBox[] pba = { PB_Mark1, PB_Mark2, PB_Mark3, PB_Mark4, PB_Mark5, PB_Mark6 };
            int index = Array.IndexOf(pba, sender);

            // Handling Gens 3-6
            int[] markings = pkm.Markings;
            switch (pkm.Format)
            {
                case 3:
                case 4:
                case 5:
                case 6: // on/off
                    markings[index] ^= 1; // toggle
                    pkm.Markings = markings;
                    break;
                case 7: // 0 (none) | 1 (blue) | 2 (pink)
                    markings[index] = (markings[index]+1)%3; // cycle
                    pkm.Markings = markings;
                    break;
                default:
                    return;
            }
            setMarkings();
        }
        private void clickStatLabel(object sender, MouseEventArgs e)
        {
            if (!(ModifierKeys == Keys.Control || ModifierKeys == Keys.Alt))
                return;

            if (sender == Label_SPC)
                sender = Label_SPA;
            int index = Array.IndexOf(new[] { Label_HP, Label_ATK, Label_DEF, Label_SPA, Label_SPD, Label_SPE }, sender);

            if (ModifierKeys == Keys.Alt) // EV
            {
                var mt = new[] {TB_HPEV, TB_ATKEV, TB_DEFEV, TB_SPAEV, TB_SPDEV, TB_SPEEV}[index];
                if (e.Button == MouseButtons.Left) // max
                    mt.Text = SAV.Generation >= 3
                        ? Math.Min(Math.Max(510 - Util.ToInt32(TB_EVTotal.Text) + Util.ToInt32(mt.Text), 0), 252).ToString()
                        : ushort.MaxValue.ToString();
                else // min
                    mt.Text = 0.ToString();
            }
            else
            new[] {TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPAIV, TB_SPDIV, TB_SPEIV}[index].Text =
                (e.Button == MouseButtons.Left ? SAV.MaxIV : 0).ToString();
        }
        private void clickIV(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control)
                if (SAV.Generation < 7)
                    ((MaskedTextBox) sender).Text = SAV.MaxIV.ToString();
                else
                {
                    var index = Array.IndexOf(new[] {TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPAIV, TB_SPDIV, TB_SPEIV}, sender);
                    switch (index)
                    {
                        case 0:
                            pkm.HT_HP ^= true;
                            break;
                        case 1:
                            pkm.HT_ATK ^= true;
                            break;
                        case 2:
                            pkm.HT_DEF ^= true;
                            break;
                        case 3:
                            pkm.HT_SPA ^= true;
                            break;
                        case 4:
                            pkm.HT_SPD ^= true;
                            break;
                        case 5:
                            pkm.HT_SPE ^= true;
                            break;
                    }
                    updateIVs(sender, e);
                }
            else if (ModifierKeys == Keys.Alt)
                ((MaskedTextBox) sender).Text = 0.ToString();
        }
        private void clickEV(object sender, EventArgs e)
        {
            MaskedTextBox mt = (MaskedTextBox)sender;
            if (ModifierKeys == Keys.Control) // EV
                mt.Text = SAV.Generation >= 3
                    ? Math.Min(Math.Max(510 - Util.ToInt32(TB_EVTotal.Text) + Util.ToInt32(mt.Text), 0), 252).ToString()
                    : ushort.MaxValue.ToString();
            else if (ModifierKeys == Keys.Alt)
                mt.Text = 0.ToString();
        }
        private void clickOT(object sender, EventArgs e)
        {
            if (!SAV.Exportable)
                return;

            // Get Save Information
            TB_OT.Text = SAV.OT;
            Label_OTGender.Text = gendersymbols[SAV.Gender % 2];
            Label_OTGender.ForeColor = SAV.Gender == 1 ? Color.Red : Color.Blue;
            TB_TID.Text = SAV.TID.ToString("00000");
            TB_SID.Text = SAV.SID.ToString("00000");

            if (SAV.Game >= 0)
                CB_GameOrigin.SelectedValue = SAV.Game;
            if (SAV.Language >= 0)
                CB_Language.SelectedValue = SAV.Language;
            if (SAV.HasGeolocation)
            {
                CB_SubRegion.SelectedValue = SAV.SubRegion;
                CB_Country.SelectedValue = SAV.Country;
                CB_3DSReg.SelectedValue = SAV.ConsoleRegion;
            }
            updateNickname(null, null);
        }
        private void clickCT(object sender, EventArgs e)
        {
            if (TB_OTt2.Text.Length > 0)
                Label_CTGender.Text = gendersymbols[SAV.Gender % 2];
        }
        private void clickGT(object sender, EventArgs e)
        {
            if (!GB_nOT.Visible)
                return;
            if (sender == GB_OT)
            {
                pkm.CurrentHandler = 0;
                GB_OT.BackgroundImage = mixedHighlight;
                GB_nOT.BackgroundImage = null;
            }
            else if (TB_OTt2.Text.Length > 0)
            {
                pkm.CurrentHandler = 1;
                GB_OT.BackgroundImage = null;
                GB_nOT.BackgroundImage = mixedHighlight;
            }
            TB_Friendship.Text = pkm.CurrentFriendship.ToString();
        }
        public void clickTRGender(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (!string.IsNullOrWhiteSpace(lbl?.Text)) // set gender label (toggle M/F)
            {
                int gender = PKX.getGender(lbl.Text) ^ 1;
                lbl.Text = gendersymbols[gender];
                lbl.ForeColor = gender == 1 ? Color.Red : Color.Blue;
            }
        }
        private void clickMoves(object sender, EventArgs e)
        {
      /*      updateLegality();
            if (sender == GB_CurrentMoves)
            {
                bool random = ModifierKeys == Keys.Control;
                int[] m = Legality.getSuggestedMoves(tm: random, tutor: random);
                if (m == null)
                { Util.Alert("Suggestions are not enabled for this PKM format."); return; }

                if (random)
                    Util.Shuffle(m);
                if (m.Length > 4)
                    m = m.Skip(m.Length - 4).ToArray();
                Array.Resize(ref m, 4);

                if (pkm.Moves.SequenceEqual(m))
                    return;

                string r = string.Join(Environment.NewLine, m.Select(v => v >= GameStrings.movelist.Length ? "ERROR" : GameStrings.movelist[v]));
                if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Apply suggested current moves?", r))
                    return;

                CB_Move1.SelectedValue = m[0];
                CB_Move2.SelectedValue = m[1];
                CB_Move3.SelectedValue = m[2];
                CB_Move4.SelectedValue = m[3];
            }
            else if (sender == GB_RelearnMoves)
            {
                int[] m = Legality.getSuggestedRelearn();

                if (pkm.RelearnMoves.SequenceEqual(m))
                    return;

                string r = string.Join(Environment.NewLine, m.Select(v => v >= GameStrings.movelist.Length ? "ERROR" : GameStrings.movelist[v]));
                if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Apply suggested relearn moves?", r))
                    return;

                CB_RelearnMove1.SelectedValue = m[0];
                CB_RelearnMove2.SelectedValue = m[1];
                CB_RelearnMove3.SelectedValue = m[2];
                CB_RelearnMove4.SelectedValue = m[3];
            }

            updateLegality();*/
        }
        // Prompted Updates of PKX Functions // 
        private bool changingFields;
        private void updateEXPLevel(object sender, EventArgs e)
        {
            if (changingFields || !fieldsInitialized) return;

            changingFields = true;
            if (sender == TB_EXP)
            {
                // Change the Level
                uint EXP = Util.ToUInt32(TB_EXP.Text);
                int Species = Util.getIndex(CB_Species);
                int Level = PKX.getLevel(Species, EXP);
                if (Level == 100)
                    EXP = PKX.getEXP(100, Species);

                TB_Level.Text = Level.ToString();
                if (!MT_Level.Visible)
                    TB_EXP.Text = EXP.ToString();
                else
                    MT_Level.Text = Level.ToString();
            }
            else
            {
                // Change the XP
                int Level = Util.ToInt32((MT_Level.Visible ? MT_Level : TB_Level).Text);
                if (Level > 100) TB_Level.Text = "100";
                if (Level > byte.MaxValue) MT_Level.Text = "255";

                TB_EXP.Text = PKX.getEXP(Level, Util.getIndex(CB_Species)).ToString();
            }
            changingFields = false;
            if (fieldsLoaded) // store values back
            {
                pkm.EXP = Util.ToUInt32(TB_EXP.Text);
                pkm.Stat_Level = Util.ToInt32((MT_Level.Visible ? MT_Level : TB_Level).Text);
            }
            updateStats();
            updateLegality();
        }
        private void updateHPType(object sender, EventArgs e)
        {
            if (changingFields || !fieldsInitialized) return;
            changingFields = true;
            int[] ivs =
            {
                Util.ToInt32(TB_HPIV.Text), Util.ToInt32(TB_ATKIV.Text), Util.ToInt32(TB_DEFIV.Text), 
                Util.ToInt32(TB_SPAIV.Text), Util.ToInt32(TB_SPDIV.Text), Util.ToInt32(TB_SPEIV.Text) 
            };

            // Change IVs to match the new Hidden Power
            int[] newIVs = PKX.setHPIVs(Util.getIndex(CB_HPType), ivs);
            TB_HPIV.Text = newIVs[0].ToString();
            TB_ATKIV.Text = newIVs[1].ToString();
            TB_DEFIV.Text = newIVs[2].ToString();
            TB_SPAIV.Text = newIVs[3].ToString();
            TB_SPDIV.Text = newIVs[4].ToString();
            TB_SPEIV.Text = newIVs[5].ToString();

            // Refresh View
            changingFields = false;
            updateIVs(null, null);
        }
        private void updateIVs(object sender, EventArgs e)
        {
            if (changingFields || !fieldsInitialized) return;
            if (sender != null && Util.ToInt32(((MaskedTextBox) sender).Text) > SAV.MaxIV)
                ((MaskedTextBox) sender).Text = SAV.MaxIV.ToString("00");

            changingFields = true;

            // Update IVs
            pkm.IV_HP = Util.ToInt32(TB_HPIV.Text);
            pkm.IV_ATK = Util.ToInt32(TB_ATKIV.Text);
            pkm.IV_DEF = Util.ToInt32(TB_DEFIV.Text);
            pkm.IV_SPE = Util.ToInt32(TB_SPEIV.Text);
            pkm.IV_SPA = Util.ToInt32(TB_SPAIV.Text);
            pkm.IV_SPD = Util.ToInt32(TB_SPDIV.Text);

            var IV_Boxes = new[] {TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPAIV, TB_SPDIV, TB_SPEIV};
            var HT_Vals = new[] {pkm.HT_HP, pkm.HT_ATK, pkm.HT_DEF, pkm.HT_SPA, pkm.HT_SPD, pkm.HT_SPE};
            for (int i = 0; i < IV_Boxes.Length; i++)
                if (HT_Vals[i])
                    IV_Boxes[i].BackColor = Color.LightGreen;
                else
                    IV_Boxes[i].ResetBackColor();

            if (SAV.Generation < 3)
            {
                TB_HPIV.Text = pkm.IV_HP.ToString();
                TB_SPDIV.Text = TB_SPAIV.Text;
                if (SAV.Generation == 2)
                {
                    Label_Gender.Text = gendersymbols[pkm.Gender];
                    Label_Gender.ForeColor = pkm.Gender == 2
                        ? Label_Species.ForeColor
                        : (pkm.Gender == 1 ? Color.Red : Color.Blue);
                    if (pkm.Species == 201 && e != null) // Unown
                        CB_Form.SelectedIndex = pkm.AltForm;
                    setIsShiny(null);
                    getQuickFiller(dragout);
                }
            }
                    
            CB_HPType.SelectedValue = pkm.HPType;
            changingFields = false;

            // Potential Reading
            L_Potential.Text = (!unicode
                ? new[] {"★☆☆☆", "★★☆☆", "★★★☆", "★★★★"}
                : new[] {"+", "++", "+++", "++++"}
                )[pkm.PotentialRating];

            TB_IVTotal.Text = pkm.IVs.Sum().ToString();

            int characteristic = pkm.Characteristic;
            L_Characteristic.Visible = Label_CharacteristicPrefix.Visible = characteristic > -1;
            if (characteristic > -1)
                L_Characteristic.Text = GameStrings.characteristics[pkm.Characteristic];
            updateStats();
        }
        private void updateEVs(object sender, EventArgs e)
        {
            if (sender is MaskedTextBox)
            {
                MaskedTextBox m = (MaskedTextBox)sender;
                if (Util.ToInt32(m.Text) > SAV.MaxEV)
                { m.Text = SAV.MaxEV.ToString(); return; } // recursive on text set
            }

            changingFields = true;
            if (sender == TB_HPEV) pkm.EV_HP = Util.ToInt32(TB_HPEV.Text);
            else if (sender == TB_ATKEV) pkm.EV_ATK = Util.ToInt32(TB_ATKEV.Text);
            else if (sender == TB_DEFEV) pkm.EV_DEF = Util.ToInt32(TB_DEFEV.Text);
            else if (sender == TB_SPEEV) pkm.EV_SPE = Util.ToInt32(TB_SPEEV.Text);
            else if (sender == TB_SPAEV) pkm.EV_SPA = Util.ToInt32(TB_SPAEV.Text);
            else if (sender == TB_SPDEV) pkm.EV_SPD = Util.ToInt32(TB_SPDEV.Text);

            if (SAV.Generation < 3)
                TB_SPDEV.Text = TB_SPAEV.Text;

            int evtotal = pkm.EVs.Sum();

            if (evtotal > 510) // Background turns Red
                TB_EVTotal.BackColor = Color.Red;
            else if (evtotal == 510) // Maximum EVs
                TB_EVTotal.BackColor = Color.Honeydew;
            else if (evtotal == 508) // Fishy EVs
                TB_EVTotal.BackColor = Color.LightYellow;
            else TB_EVTotal.BackColor = TB_IVTotal.BackColor;

            TB_EVTotal.Text = evtotal.ToString();
            changingFields = false;
            updateStats();
        }
        private void updateRandomIVs(object sender, EventArgs e)
        {
            changingFields = true;
            if (ModifierKeys == Keys.Control || ModifierKeys == Keys.Shift)
            {
                // Max IVs
                TB_HPIV.Text = SAV.MaxIV.ToString();
                TB_ATKIV.Text = SAV.MaxIV.ToString();
                TB_DEFIV.Text = SAV.MaxIV.ToString();
                TB_SPAIV.Text = SAV.MaxIV.ToString();
                TB_SPDIV.Text = SAV.MaxIV.ToString();
                TB_SPEIV.Text = SAV.MaxIV.ToString();
            }
            else
            {
                TB_HPIV.Text = (Util.rnd32() % SAV.MaxIV).ToString();
                TB_ATKIV.Text = (Util.rnd32() % SAV.MaxIV).ToString();
                TB_DEFIV.Text = (Util.rnd32() % SAV.MaxIV).ToString();
                TB_SPAIV.Text = (Util.rnd32() % SAV.MaxIV).ToString();
                TB_SPDIV.Text = (Util.rnd32() % SAV.MaxIV).ToString();
                TB_SPEIV.Text = (Util.rnd32() % SAV.MaxIV).ToString();
            }
            changingFields = false;
            updateIVs(null, e);
        }
        private void updateRandomEVs(object sender, EventArgs e)
        {
            changingFields = true;
            if (ModifierKeys == Keys.Control || ModifierKeys == Keys.Shift)
            {
                // Max EVs
                TB_HPEV.Text = 0.ToString();
                TB_ATKEV.Text = 0.ToString();
                TB_DEFEV.Text = 0.ToString();
                TB_SPAEV.Text = 0.ToString();
                TB_SPDEV.Text = 0.ToString();
                TB_SPEEV.Text = 0.ToString();
            }
            else
            {
                var evs = PKX.getRandomEVs(SAV.Generation);
                TB_HPEV.Text = evs[0].ToString();
                TB_ATKEV.Text = evs[1].ToString();
                TB_DEFEV.Text = evs[2].ToString();
                TB_SPAEV.Text = evs[3].ToString();
                TB_SPDEV.Text = evs[4].ToString();
                TB_SPEEV.Text = evs[5].ToString();
            }
            changingFields = false;
            updateEVs(null, null);
        }
        private void updateRandomPID(object sender, EventArgs e)
        {
            if (fieldsLoaded)
                pkm.PID = Util.getHEXval(TB_PID.Text);

            if (sender == Label_Gender)
                pkm.setPIDGender(pkm.Gender);
            else if (sender == CB_Nature && pkm.Nature != Util.getIndex(CB_Nature))
                pkm.setPIDNature(Util.getIndex(CB_Nature));
            else if (sender == BTN_RerollPID)
                pkm.setPIDGender(pkm.Gender);
            else if (sender == CB_Ability && CB_Ability.SelectedIndex != pkm.PIDAbility && pkm.PIDAbility > -1)
                pkm.PID = PKX.getRandomPID(pkm.Species, pkm.Gender, pkm.Version, pkm.Nature, pkm.Format, (uint)(CB_Ability.SelectedIndex * 0x10001));

            TB_PID.Text = pkm.PID.ToString("X8");
            getQuickFiller(dragout);
            if (pkm.GenNumber < 6 && SAV.Generation >= 6)
                TB_EC.Text = TB_PID.Text;
        }
        private void updateRandomEC(object sender, EventArgs e)
        {
            pkm.Version = Util.getIndex(CB_GameOrigin);
            if (pkm.GenNumber < 6)
            {
                TB_EC.Text = TB_PID.Text;
                Util.Alert("EC should match PID.");
            }
            
            int wIndex = Array.IndexOf(Legal.WurmpleEvolutions, Util.getIndex(CB_Species));
            if (wIndex < 0)
            {
                TB_EC.Text = Util.rnd32().ToString("X8");
            }
            else
            {
                uint EC;
                do { EC = Util.rnd32(); } while ((EC >> 16)%10/5 != wIndex/2);
                TB_EC.Text = EC.ToString("X8");
            }
        }
        private void updateHackedStats(object sender, EventArgs e)
        {
            Stat_HP.Enabled =
                Stat_ATK.Enabled =
                Stat_DEF.Enabled =
                Stat_SPA.Enabled =
                Stat_SPD.Enabled =
                Stat_SPE.Enabled = CHK_HackedStats.Checked;
        }
        private void updateHackedStatText(object sender, EventArgs e)
        {
            if (!CHK_HackedStats.Checked || !(sender is TextBox))
                return;

            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text))
                ((TextBox)sender).Text = "0";

            if (Convert.ToUInt32(text) > ushort.MaxValue)
                ((TextBox)sender).Text = "65535";
        }
        private void update255_MTB(object sender, EventArgs e)
        {
            if (Util.ToInt32(((MaskedTextBox) sender).Text) > byte.MaxValue)
                    ((MaskedTextBox) sender).Text = "255";
        }
        private void updateForm(object sender, EventArgs e)
        {
            if (CB_Form == sender && fieldsLoaded)
                pkm.AltForm = CB_Form.SelectedIndex;
            updateStats();
            // Repopulate Abilities if Species Form has different abilities
            setAbilityList();

            // Gender Forms
            if (Util.getIndex(CB_Species) == 201 && fieldsLoaded)
            {
                if (SAV.Generation == 3)
                {
                    pkm.setPIDUnown3(CB_Form.SelectedIndex);
                    TB_PID.Text = pkm.PID.ToString("X8");
                }
                else if (SAV.Generation == 2)
                {
                    int desiredForm = CB_Form.SelectedIndex;
                    while (pkm.AltForm != desiredForm)
                        updateRandomIVs(null, null);
                }
            }
            else if (PKX.getGender(CB_Form.Text) < 2)
                Label_Gender.Text = CB_Form.Text;

            if (changingFields) 
                return;
            changingFields = true;
            MT_Form.Text = CB_Form.SelectedIndex.ToString();
            changingFields = false;
        }
        private void updateHaXForm(object sender, EventArgs e)
        {
            if (changingFields)
                return;
            changingFields = true;
            int form = pkm.AltForm = Util.ToInt32(MT_Form.Text);
            CB_Form.SelectedIndex = CB_Form.Items.Count > form ? form : -1;
            changingFields = false;
        }
        private void updatePP(object sender, EventArgs e)
        {
            ComboBox[] cbs = {CB_Move1, CB_Move2, CB_Move3, CB_Move4};
            ComboBox[] pps = {CB_PPu1, CB_PPu2, CB_PPu3, CB_PPu4};
            MaskedTextBox[] tbs = {TB_PP1, TB_PP2, TB_PP3, TB_PP4};
            int index = Array.IndexOf(cbs, sender);
            if (index < 0)
                index = Array.IndexOf(pps, sender);
            if (index < 0)
                return;

            int move = Util.getIndex(cbs[index]);
            int pp = pps[index].SelectedIndex;
            if (move == 0 && pp != 0)
            {
                pps[index].SelectedIndex = 0;
                return; // recursively triggers
            }
            tbs[index].Text = pkm.getMovePP(move, pp).ToString();
        }
        private void updatePKRSstrain(object sender, EventArgs e)
        {
            // Change the PKRS Days to the legal bounds.
            int currentDuration = CB_PKRSDays.SelectedIndex;
            CB_PKRSDays.Items.Clear();
            foreach (int day in Enumerable.Range(0, CB_PKRSStrain.SelectedIndex % 4 + 2)) CB_PKRSDays.Items.Add(day);

            // Set the days back if they're legal, else set it to 1. (0 always passes).
            CB_PKRSDays.SelectedIndex = currentDuration < CB_PKRSDays.Items.Count ? currentDuration : 1;

            if (CB_PKRSStrain.SelectedIndex != 0) return;
            
            // Never Infected
            CB_PKRSDays.SelectedIndex = 0;
            CHK_Cured.Checked = false;
            CHK_Infected.Checked = false;
        }
        private void updatePKRSdays(object sender, EventArgs e)
        {
            if (CB_PKRSDays.SelectedIndex != 0) return;

            // If no days are selected
            if (CB_PKRSStrain.SelectedIndex == 0)
                CHK_Cured.Checked = CHK_Infected.Checked = false; // No Strain = Never Cured / Infected, triggers Strain update
            else CHK_Cured.Checked = true; // Any Strain = Cured
        }
        private void updatePKRSCured(object sender, EventArgs e)
        {
            if (!fieldsInitialized) return;
            // Cured PokeRus is toggled
            if (CHK_Cured.Checked)
            {
                // Has Had PokeRus
                Label_PKRSdays.Visible = CB_PKRSDays.Visible = false;
                CB_PKRSDays.SelectedIndex = 0;

                Label_PKRS.Visible = CB_PKRSStrain.Visible = true;
                CHK_Infected.Checked = true;

                // If we're cured we have to have a strain infection.
                if (CB_PKRSStrain.SelectedIndex == 0)
                    CB_PKRSStrain.SelectedIndex = 1;
            }
            else if (!CHK_Infected.Checked)
            {
                // Not Infected, Disable the other
                Label_PKRS.Visible = CB_PKRSStrain.Visible = false;
                CB_PKRSStrain.SelectedIndex = 0;
            }
            else
            {
                // Still Infected for a duration
                Label_PKRSdays.Visible = CB_PKRSDays.Visible = true;
                CB_PKRSDays.SelectedValue = 1;
            }
            // if not cured yet, days > 0
            if (!CHK_Cured.Checked && CHK_Infected.Checked && CB_PKRSDays.SelectedIndex == 0)
                CB_PKRSDays.SelectedIndex++;

            setMarkings();
        }
        private void updatePKRSInfected(object sender, EventArgs e)
        {
            if (!fieldsInitialized) return;
            if (CHK_Cured.Checked && !CHK_Infected.Checked) { CHK_Cured.Checked = false; return; }
            if (CHK_Cured.Checked) return;
            Label_PKRS.Visible = CB_PKRSStrain.Visible = CHK_Infected.Checked;
            if (!CHK_Infected.Checked) { CB_PKRSStrain.SelectedIndex = 0; CB_PKRSDays.SelectedIndex = 0; Label_PKRSdays.Visible = CB_PKRSDays.Visible = false; }
            else if (CB_PKRSStrain.SelectedIndex == 0) { CB_PKRSStrain.SelectedIndex = 1; Label_PKRSdays.Visible = CB_PKRSDays.Visible = true; updatePKRSCured(sender, e);}

            // if not cured yet, days > 0
            if (CHK_Infected.Checked && CB_PKRSDays.SelectedIndex == 0) CB_PKRSDays.SelectedIndex++;
        }
        private void updateCountry(object sender, EventArgs e)
        {
            if (Util.getIndex(sender as ComboBox) > 0)
                setCountrySubRegion(CB_SubRegion, "sr_" + Util.getIndex(sender as ComboBox).ToString("000"));
        }
        private void updateSpecies(object sender, EventArgs e)
        {
            // Get Species dependent information
            setAbilityList();
            setForms();
            updateForm(null, null);
            
            if (!fieldsLoaded)
                return;

            pkm.Species = Util.getIndex(CB_Species);
            // Recalculate EXP for Given Level
            uint EXP = PKX.getEXP(pkm.CurrentLevel, pkm.Species);
            TB_EXP.Text = EXP.ToString();

            // Check for Gender Changes
            // Get Gender Threshold
            int gt = SAV.Personal[pkm.Species].Gender;
            int cg = Array.IndexOf(gendersymbols, Label_Gender.Text);
            int Gender;

            if (gt == 255)      // Genderless
                Gender = 2;
            else if (gt == 254) // Female Only
                Gender = 1;
            else if (gt == 0)  // Male Only
                Gender = 0;
            else if (cg == 2 || Util.getIndex(CB_GameOrigin) < 24)
                Gender = (Util.getHEXval(TB_PID.Text) & 0xFF) <= gt ? 1 : 0;
            else
                Gender = cg;
            
            Label_Gender.Text = gendersymbols[Gender];
            Label_Gender.ForeColor = Gender == 2 ? Label_Species.ForeColor : (Gender == 1 ? Color.Red : Color.Blue);

            // If species changes and no nickname, set the new name == speciesName.
            if (!CHK_Nicknamed.Checked)
                updateNickname(sender, e);

            updateLegality();
        }
        private void updateOriginGame(object sender, EventArgs e)
        {
            GameVersion Version = (GameVersion)Util.getIndex(CB_GameOrigin);

            // check if differs
            GameVersion newTrack = GameUtil.getMetLocationVersionGroup(Version);
            if (newTrack != origintrack)
            {
                var met_list = GameInfo.getLocationList(Version, SAV.Generation, egg:false);
                CB_MetLocation.DisplayMember = "Text";
                CB_MetLocation.ValueMember = "Value";
                CB_MetLocation.DataSource = new BindingSource(met_list, null);
                CB_MetLocation.SelectedIndex = 0; // transporter or pal park for past gen pkm

                var egg_list = GameInfo.getLocationList(Version, SAV.Generation, egg:true);
                CB_EggLocation.DisplayMember = "Text";
                CB_EggLocation.ValueMember = "Value";
                CB_EggLocation.DataSource = new BindingSource(egg_list, null);
                CB_EggLocation.SelectedIndex = CHK_AsEgg.Checked ? 1 : 0; // daycare : none

                origintrack = newTrack;

                // Stretch C/XD met location dropdowns
                int width = CB_EggLocation.DropDownWidth;
                if (Version == GameVersion.CXD && SAV.Generation == 3)
                    width = 2*width;
                CB_MetLocation.DropDownWidth = width;
            }

            // Visibility logic for Gen 4 encounter type; only show for Gen 4 Pokemon.
            if (SAV.Generation >= 4)
            {
                bool g4 = Version >= GameVersion.HG && Version <= GameVersion.Pt;
                if ((int) Version == 9) // invalid
                    g4 = false;
                CB_EncounterType.Visible = Label_EncounterType.Visible = g4 && SAV.Generation < 7;
                if (!g4)
                    CB_EncounterType.SelectedValue = 0;
            }

            if (!fieldsLoaded)
                return;
            pkm.Version = (int)Version;
            setMarkings(); // Set/Remove KB marking
            updateLegality();
        }
        private void updateExtraByteValue(object sender, EventArgs e)
        {
            if (CB_ExtraBytes.Items.Count == 0)
                return;
            // Changed Extra Byte's Value
            if (Util.ToInt32(((MaskedTextBox) sender).Text) > byte.MaxValue)
                ((MaskedTextBox) sender).Text = "255";

            int value = Util.ToInt32(TB_ExtraByte.Text);
            int offset = Convert.ToInt32(CB_ExtraBytes.Text, 16);
            pkm.Data[offset] = (byte)value;
        }
        private void updateExtraByteIndex(object sender, EventArgs e)
        {
            if (CB_ExtraBytes.Items.Count == 0)
                return;
            // Byte changed, need to refresh the Text box for the byte's value.
            TB_ExtraByte.Text = pkm.Data[Convert.ToInt32(CB_ExtraBytes.Text, 16)].ToString();
        }
        private void updateNatureModification(object sender, EventArgs e)
        {
            if (sender != CB_Nature) return;
            int nature = Util.getIndex(CB_Nature);
            int incr = nature / 5;
            int decr = nature % 5;

            Label[] labarray = { Label_ATK, Label_DEF, Label_SPE, Label_SPA, Label_SPD };
            // Reset Label Colors
            foreach (Label label in labarray)
                label.ResetForeColor();

            // Set Colored StatLabels only if Nature isn't Neutral
            NatureTip.SetToolTip(CB_Nature,
                incr != decr
                    ? $"+{labarray[incr].Text} / -{labarray[decr].Text}".Replace(":", "")
                    : "-/-");
        }
        private void updateNickname(object sender, EventArgs e)
        {
            if (fieldsInitialized && ModifierKeys == Keys.Control && sender != null) // Import Showdown
            { clickShowdownImportPKM(sender, e); return; }
            if (fieldsInitialized && ModifierKeys == Keys.Alt && sender != null) // Export Showdown
            { clickShowdownExportPKM(sender, e); return; }

            if (sender == CB_Language || sender == CHK_Nicknamed)
            {
                int lang = Util.getIndex(CB_Language);
                switch (lang)
                {
                    case 9:
                    case 10:
                        TB_Nickname.Visible = CHK_Nicknamed.Checked;
                        break;
                    default:
                        TB_Nickname.Visible = true;
                        break;
                }
            }

            if (!fieldsInitialized || CHK_Nicknamed.Checked)
                return;

            // Fetch Current Species and set it as Nickname Text
            int species = Util.getIndex(CB_Species);
            if (species < 1 || species > SAV.MaxSpeciesID)
                TB_Nickname.Text = "";
            else
            {
                // get language
                int lang = Util.getIndex(CB_Language);
                if (CHK_IsEgg.Checked) species = 0; // Set species to 0 to get the egg name.
                string nick = PKX.getSpeciesName(CHK_IsEgg.Checked ? 0 : species, lang);

                if (SAV.Generation < 5) // All caps GenIV and previous
                    nick = nick.ToUpper();
                if (SAV.Generation < 3)
                    nick = nick.Replace(" ", "");
                TB_Nickname.Text = nick;
                if (SAV.Generation == 1)
                    ((PK1)pkm).setNotNicknamed();
                if (SAV.Generation == 2)
                    ((PK2)pkm).setNotNicknamed();
            }
        }
        private void updateNicknameClick(object sender, MouseEventArgs e)
        {
            TextBox tb = !(sender is TextBox) ? TB_Nickname : (TextBox) sender;
            // Special Character Form
            if (ModifierKeys != Keys.Control)
                return;

            var z = Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.GetType() == typeof(f2_Text)) as f2_Text;
            if (z != null)
            { Util.CenterToForm(z, this); z.BringToFront(); return; }
            new f2_Text(tb).Show();
        }
        private void updateNotOT(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TB_OTt2.Text))
            {
                clickGT(GB_OT, null); // Switch CT over to OT.
                Label_CTGender.Text = "";
                TB_Friendship.Text = pkm.CurrentFriendship.ToString();
            }
            else if (string.IsNullOrWhiteSpace(Label_CTGender.Text))
                Label_CTGender.Text = gendersymbols[0];
        }
        private void updateIsEgg(object sender, EventArgs e)
        {
            // Display hatch counter if it is an egg, Display Friendship if it is not.
            Label_HatchCounter.Visible = CHK_IsEgg.Checked && SAV.Generation > 1;
            Label_Friendship.Visible = !CHK_IsEgg.Checked && SAV.Generation > 1;

            if (!fieldsLoaded)
                return;

            pkm.IsEgg = CHK_IsEgg.Checked;
            if (CHK_IsEgg.Checked)
            {
                TB_Friendship.Text = "1";

                // If we are an egg, it won't have a met location.
                CHK_AsEgg.Checked = true;
                GB_EggConditions.Enabled = true;

                CAL_MetDate.Value = new DateTime(2000, 01, 01);

                // if egg wasn't originally obtained by OT => Link Trade, else => None
                bool isTraded = SAV.OT != TB_OT.Text || SAV.TID != Util.ToInt32(TB_TID.Text) || SAV.SID != Util.ToInt32(TB_SID.Text);
                CB_MetLocation.SelectedIndex = isTraded ? 2 : 0;

                if (!CHK_Nicknamed.Checked)
                {
                    TB_Nickname.Text = PKX.getSpeciesName(0, Util.getIndex(CB_Language));
                    CHK_Nicknamed.Checked = true;
                }
            }
            else // Not Egg
            {
                if (!CHK_Nicknamed.Checked)
                    updateNickname(null, null);

                TB_Friendship.Text = SAV.Personal[Util.getIndex(CB_Species)].BaseFriendship.ToString();

                if (CB_EggLocation.SelectedIndex == 0)
                {
                    CAL_EggDate.Value = new DateTime(2000, 01, 01);
                    CHK_AsEgg.Checked = false;
                    GB_EggConditions.Enabled = false;
                }

                if (TB_Nickname.Text == PKX.getSpeciesName(0, Util.getIndex(CB_Language)))
                    CHK_Nicknamed.Checked = false;
            }

            updateNickname(null, null);
            getQuickFiller(dragout);
        }
        private void updateMetAsEgg(object sender, EventArgs e)
        {
            GB_EggConditions.Enabled = CHK_AsEgg.Checked;
            if (CHK_AsEgg.Checked)
            {
                if (!fieldsLoaded)
                    return;

                CAL_EggDate.Value = DateTime.Now;
                CB_EggLocation.SelectedIndex = 1;
                return;
            }
            // Remove egg met data
            CHK_IsEgg.Checked = false;
            CAL_EggDate.Value = new DateTime(2000, 01, 01);
            CB_EggLocation.SelectedValue = 0;

            updateLegality();
        }
        private void updateShinyPID(object sender, EventArgs e)
        {
            pkm.TID = Util.ToInt32(TB_TID.Text);
            pkm.SID = Util.ToInt32(TB_SID.Text);
            pkm.PID = Util.getHEXval(TB_PID.Text);
            pkm.Nature = Util.getIndex(CB_Nature);
            pkm.Gender = PKX.getGender(Label_Gender.Text);
            pkm.AltForm = CB_Form.SelectedIndex;
            pkm.Version = Util.getIndex(CB_GameOrigin);

            if (pkm.Format > 2)
                pkm.setShinyPID();
            else
            {
                TB_ATKIV.Text = "15";
                TB_DEFIV.Text = "10";
                TB_SPEIV.Text = "10";
                TB_SPAIV.Text = "10";
                updateIVs(null, null);
            }
            TB_PID.Text = pkm.PID.ToString("X8");

            if (pkm.GenNumber < 6 && TB_EC.Visible)
                TB_EC.Text = TB_PID.Text;

            getQuickFiller(dragout);
            updateLegality();
        }
        private void updateTSV(object sender, EventArgs e)
        {
            if (SAV.Generation < 6)
                return;

            var TSV = pkm.TSV.ToString("0000");
            string IDstr = "TSV: " + TSV;
            if (SAV.Generation > 6)
                IDstr += Environment.NewLine + "G7TID: " + pkm.TrainerID7.ToString("000000");

            Tip1.SetToolTip(TB_TID, IDstr);
            Tip2.SetToolTip(TB_SID, IDstr);

            pkm.PID = Util.getHEXval(TB_PID.Text);
            var PSV = pkm.PSV;
            Tip3.SetToolTip(TB_PID, "PSV: " + PSV.ToString("0000"));
        }
        private void update_ID(object sender, EventArgs e)
        {
            // Trim out nonhex characters
            TB_PID.Text = Util.getHEXval(TB_PID.Text).ToString("X8");
            TB_EC.Text = Util.getHEXval(TB_EC.Text).ToString("X8");

            // Max TID/SID is 65535
            if (Util.ToUInt32(TB_TID.Text) > ushort.MaxValue) TB_TID.Text = "65535";
            if (Util.ToUInt32(TB_SID.Text) > ushort.MaxValue) TB_SID.Text = "65535";

            setIsShiny(sender);
            getQuickFiller(dragout);
            updateIVs(null, null);   // If the EC is changed, EC%6 (Characteristic) might be changed. 
            TB_PID.Select(60, 0);   // position cursor at end of field
            if (SAV.Generation <= 4 && fieldsLoaded)
            {
                fieldsLoaded = false;
                pkm.PID = Util.getHEXval(TB_PID.Text);
                CB_Nature.SelectedValue = pkm.Nature;
                Label_Gender.Text = gendersymbols[pkm.Gender];
                Label_Gender.ForeColor = pkm.Gender == 2 ? Label_Species.ForeColor : (pkm.Gender == 1 ? Color.Red : Color.Blue);
                fieldsLoaded = true;
            }
        }
        private void updateShadowID(object sender, EventArgs e)
        {
            if (!fieldsLoaded)
                return;
            FLP_Purification.Visible = NUD_ShadowID.Value > 0;
        }
        private void updatePurification(object sender, EventArgs e)
        {
            if (!fieldsLoaded)
                return;
            fieldsLoaded = false;
            CHK_Shadow.Checked = NUD_Purification.Value == 0;
            fieldsLoaded = true;
        }
        private void updateShadowCHK(object sender, EventArgs e)
        {
            if (!fieldsLoaded)
                return;
            fieldsLoaded = false;
            NUD_Purification.Value = CHK_Shadow.Checked ? NUD_Purification.Maximum : 0;
            fieldsLoaded = true;
        }
        private void validateComboBox(object sender)
        {
            if (!formInitialized)
                return;
            ComboBox cb = sender as ComboBox;
            if (cb == null) 
                return;
            
            cb.SelectionLength = 0;
            if (cb.Text == "")
            { cb.SelectedIndex = 0; return; }
            if (cb.SelectedValue == null)
                cb.BackColor = Color.DarkSalmon;
            else
                cb.ResetBackColor();
        }
        private void validateComboBox(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(sender is ComboBox))
                return;

            validateComboBox(sender);

            if (fieldsLoaded)
                getQuickFiller(dragout);
        }
        private void validateComboBox2(object sender, EventArgs e)
        {
            if (!fieldsInitialized)
                return;
            validateComboBox(sender, null);
            if (fieldsLoaded)
            {
                if (sender == CB_Ability && SAV.Generation >= 6)
                    TB_AbilityNumber.Text = (1 << CB_Ability.SelectedIndex).ToString();
                if (sender == CB_Ability && SAV.Generation <= 5 && CB_Ability.SelectedIndex < 2) // not hidden
                    updateRandomPID(sender, e);
                if (sender == CB_Nature && SAV.Generation <= 4)
                {
                    pkm.Nature = CB_Nature.SelectedIndex;
                    updateRandomPID(sender, e);
                }
            }
            updateNatureModification(sender, null);
            updateIVs(null, null); // updating Nature will trigger stats to update as well
        }
        private void validateMove(object sender, EventArgs e)
        {
            if (!fieldsInitialized)
                return;
            validateComboBox(sender);
            if (!fieldsLoaded)
                return;

            if (new[] { CB_Move1, CB_Move2, CB_Move3, CB_Move4 }.Contains(sender)) // Move
                updatePP(sender, e);

            // Legality
            pkm.Moves = new[] {CB_Move1, CB_Move2, CB_Move3, CB_Move4}.Select(Util.getIndex).ToArray();
            pkm.RelearnMoves = new[] {CB_RelearnMove1, CB_RelearnMove2, CB_RelearnMove3, CB_RelearnMove4}.Select(Util.getIndex).ToArray();
            updateLegality(skipMoveRepop:true);
        }
        private void validateMovePaint(object sender, DrawItemEventArgs e)
        {
    /*        if (e.Index < 0) return;

            var i = (ComboItem)(sender as ComboBox).Items[e.Index];
            var moves = Legality.AllSuggestedMovesAndRelearn;
            bool vm = moves != null && moves.Contains(i.Value) && !HaX;

            bool current = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Brush tBrush = current ? SystemBrushes.HighlightText : new SolidBrush(e.ForeColor);
            Brush brush = current ? SystemBrushes.Highlight : vm ? Brushes.PaleGreen : new SolidBrush(e.BackColor);

            e.Graphics.FillRectangle(brush, e.Bounds);
            e.Graphics.DrawString(i.Text, e.Font, tBrush, e.Bounds, StringFormat.GenericDefault);
            if (current) return;
            tBrush.Dispose();
            if (!vm)
                brush.Dispose();*/
        }
        private void validateLocation(object sender, EventArgs e)
        {
            validateComboBox(sender);
            if (!fieldsLoaded)
                return;

            pkm.Met_Location = Util.getIndex(CB_MetLocation);
            pkm.Egg_Location = Util.getIndex(CB_EggLocation);
            updateLegality();
        }
        private void removedropCB(object sender, KeyEventArgs e)
        {
            ((ComboBox)sender).DroppedDown = false;
        }
        private void showLegality(PKM pk, bool tabs, bool verbose, bool skipMoveRepop = false)
        {
            LegalityAnalysis la = new LegalityAnalysis(pk);
            if (!la.Parsed)
            {
                Util.Alert($"Checking legality of PK{pk.Format} files that originated from Gen{pk.GenNumber} is not supported.");
                return;
            }
            if (tabs)
                updateLegality(la, skipMoveRepop);
            Util.Alert(verbose ? la.VerboseReport : la.Report);
        }
        private void updateLegality(LegalityAnalysis la = null, bool skipMoveRepop = false)
        {
        /*    if (!fieldsLoaded)
                return;
            Legality = la ?? new LegalityAnalysis(pkm);
            if (!Legality.Parsed || HaX)
            {
                PB_Legal.Visible = false;
                return;
            }
            PB_Legal.Visible = true;

            PB_Legal.Image = Legality.Valid ? Mass_Editor.Properties.Resources.valid : Mass_Editor.Properties.Resources.warn;

            // Refresh Move Legality
            for (int i = 0; i < 4; i++)
                movePB[i].Visible = !Legality.vMoves[i].Valid && !HaX;
            
            for (int i = 0; i < 4; i++)
                relearnPB[i].Visible = !Legality.vRelearn[i].Valid && !HaX;

            if (skipMoveRepop)
                return;
            // Resort moves
            bool tmp = fieldsLoaded;
            fieldsLoaded = false;
            var cb = new[] {CB_Move1, CB_Move2, CB_Move3, CB_Move4 };
            var moves = Legality.AllSuggestedMovesAndRelearn;
            var moveList = GameInfo.MoveDataSource.OrderByDescending(m => moves.Contains(m.Value)).ToList();
            foreach (ComboBox c in cb)
            {
                var index = c.SelectedValue;
                c.DataSource = new BindingSource(moveList, null);
                c.SelectedValue = index;
            }
            fieldsLoaded |= tmp;*/
        }
        private void updateStats()
        {
            // Generate the stats.
            pkm.setStats(pkm.getStats(SAV.Personal.getFormeEntry(pkm.Species, pkm.AltForm)));

            Stat_HP.Text = pkm.Stat_HPCurrent.ToString();
            Stat_ATK.Text = pkm.Stat_ATK.ToString();
            Stat_DEF.Text = pkm.Stat_DEF.ToString();
            Stat_SPA.Text = pkm.Stat_SPA.ToString();
            Stat_SPD.Text = pkm.Stat_SPD.ToString();
            Stat_SPE.Text = pkm.Stat_SPE.ToString();

            // Recolor the Stat Labels based on boosted stats.
            {
                int incr = pkm.Nature / 5;
                int decr = pkm.Nature % 5;

                Label[] labarray = { Label_ATK, Label_DEF, Label_SPE, Label_SPA, Label_SPD };
                // Reset Label Colors
                foreach (Label label in labarray)
                    label.ResetForeColor();

                // Set Colored StatLabels only if Nature isn't Neutral
                if (incr == decr) return;
                labarray[incr].ForeColor = Color.Red;
                labarray[decr].ForeColor = Color.Blue;
            }
        }
        private void updateUnicode()
        {
            if (!unicode)
            {
                gendersymbols = new[] { "M", "F", "-" };
                BTN_Shinytize.Text = "*";
                TB_Nickname.Font = TB_OT.Font = TB_OTt2.Font = Label_TID.Font;
            }
            else
            {
                gendersymbols = new[] { "♂", "♀", "-" };
                BTN_Shinytize.Text = "☆";
                TB_Nickname.Font = TB_OT.Font = TB_OTt2.Font = PKX.getPKXFont(11);
            }
            // Switch active gender labels to new if they are active.
            if (PKX.getGender(Label_Gender.Text) < 2)
                Label_Gender.Text = gendersymbols[PKX.getGender(Label_Gender.Text)];
            if (PKX.getGender(Label_OTGender.Text) < 2)
                Label_OTGender.Text = gendersymbols[PKX.getGender(Label_OTGender.Text)];
            if (PKX.getGender(Label_CTGender.Text) < 2)
                Label_CTGender.Text = gendersymbols[PKX.getGender(Label_CTGender.Text)];
        }
        // Secondary Windows for Ribbons/Amie/Memories
        private void openRibbons(object sender, EventArgs e)
        {
            new RibbonEditor().ShowDialog();
        }
        private void openMedals(object sender, EventArgs e)
        {
            new SuperTrainingEditor().ShowDialog();
        }
        private void openHistory(object sender, EventArgs e)
        {
            // Write back current values
            pkm.HT_Name = TB_OTt2.Text;
            pkm.OT_Name = TB_OT.Text;
            pkm.IsEgg = CHK_IsEgg.Checked;
            pkm.CurrentFriendship = Util.ToInt32(TB_Friendship.Text);
            new MemoryAmie().ShowDialog();
            TB_Friendship.Text = pkm.CurrentFriendship.ToString();
        }
        // Open/Save Array Manipulation //
        public bool verifiedPKM()
        {
            if (ModifierKeys == (Keys.Control | Keys.Shift | Keys.Alt))
                return true; // Override
            // Make sure the PKX Fields are filled out properly (color check)
            ComboBox[] cba = {
                                 CB_Species, CB_Nature, CB_HeldItem, CB_Ability, // Main Tab
                                 CB_MetLocation, CB_EggLocation, CB_Ball,   // Met Tab
                                 CB_Move1, CB_Move2, CB_Move3, CB_Move4,    // Moves
                                 CB_RelearnMove1, CB_RelearnMove2, CB_RelearnMove3, CB_RelearnMove4 // Moves
                             };

            ComboBox cb = cba.FirstOrDefault(c => c.BackColor == Color.DarkSalmon);
            if (cb != null)
            {
                Control c = cb.Parent;
                while (!(c is TabPage))
                    c = c.Parent;
                tabMain.SelectedTab = c as TabPage;
            }
            else if (SAV.Generation >= 3 && Convert.ToUInt32(TB_EVTotal.Text) > 510 && !CHK_HackedStats.Checked)
                tabMain.SelectedTab = Tab_Stats;
            else if (Util.getIndex(CB_Species) == 0)
                tabMain.SelectedTab = Tab_Main;
            else
                return true;

            SystemSounds.Exclamation.Play();
            return false;
        }
        public static string[] verifyPKMtoSAV(PKM pk)
        {
            // Check if PKM properties are outside of the valid range
            List<string> errata = new List<string>();
            if (SAV.Generation > 1)
            {
                ushort held = (ushort)pk.HeldItem;

                if (held > GameStrings.itemlist.Length)
                    errata.Add($"Item Index beyond range: {held}");
                else if (held > SAV.MaxItemID)
                    errata.Add($"Game can't obtain item: {GameStrings.itemlist[held]}");
                else if (!pk.CanHoldItem(SAV.HeldItems))
                    errata.Add($"Game can't hold item: {GameStrings.itemlist[held]}");
            }

            if (pk.Species > GameStrings.specieslist.Length)
                errata.Add($"Species Index beyond range: {pk.HeldItem}");
            else if (SAV.MaxSpeciesID < pk.Species)
                errata.Add($"Game can't obtain species: {GameStrings.specieslist[pk.Species]}");

            if (pk.Moves.Any(m => m > GameStrings.movelist.Length))
                errata.Add($"Item Index beyond range: {string.Join(", ", pk.Moves.Where(m => m > GameStrings.movelist.Length).Select(m => m.ToString()))}");
            else if (pk.Moves.Any(m => m > SAV.MaxMoveID))
                errata.Add($"Game can't have move: {string.Join(", ", pk.Moves.Where(m => m > SAV.MaxMoveID).Select(m => GameStrings.movelist[m]))}");

            if (pk.Ability > GameStrings.abilitylist.Length)
                errata.Add($"Ability Index beyond range: {pk.Ability}");
            else if (pk.Ability > SAV.MaxAbilityID)
                errata.Add($"Game can't have ability: {GameStrings.abilitylist[pk.Ability]}");

            return errata.ToArray();
        }
        public PKM preparePKM(bool click = true)
        {
            if (click)
                tabMain.Select(); // hack to make sure comboboxes are set (users scrolling through and immediately setting causes this)

            PKM pk = getPKMfromFields();
            return pk?.Clone();
        }
        // Drag & Drop Events
        private void tabMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == (DragDropEffects.Copy | DragDropEffects.Link)) // external file
                e.Effect = DragDropEffects.Copy;
            else if (e.Data != null) // within
                e.Effect = DragDropEffects.Move;
        }
        private void tabMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            openQuick(files[0]);
            e.Effect = DragDropEffects.Copy;

            Cursor = DefaultCursor;
        }
        // Decrypted Export
        private void dragout_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (ModifierKeys == Keys.Alt || ModifierKeys == Keys.Shift))
                clickQR(sender, e);
            if (e.Button == MouseButtons.Right)
                return;
            if (!verifiedPKM())
                return;

            // Create Temp File to Drag
            PKM pkx = preparePKM();
            bool encrypt = ModifierKeys == Keys.Control;
            string filename = $"{Path.GetFileNameWithoutExtension(pkx.FileName)}{(encrypt ? ".ek" + pkx.Format : "."+pkx.Extension) }";
            byte[] dragdata = encrypt ? pkx.EncryptedBoxData : pkx.DecryptedBoxData;
            // Make file
            string newfile = Path.Combine(Path.GetTempPath(), Util.CleanFileName(filename));
            try
            {
                File.WriteAllBytes(newfile, dragdata);
                PictureBox pb = (PictureBox)sender;
                Cursor = DragInfo.Cursor = new Cursor(((Bitmap)pb.Image).GetHicon());
                DoDragDrop(new DataObject(DataFormats.FileDrop, new[] { newfile }), DragDropEffects.Move);
            }
            catch (Exception x)
            { Util.Error("Drag & Drop Error", x); }
            Cursor = DragInfo.Cursor = DefaultCursor;
            File.Delete(newfile);
        }
        private void dragout_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        // Dragout Display
        private void dragoutHover(object sender, EventArgs e)
        {
            dragout.BackgroundImage = Util.getIndex(CB_Species) > 0 ? Mass_Editor.Properties.Resources.slotSet : Mass_Editor.Properties.Resources.slotDel;
        }
        private void dragoutLeave(object sender, EventArgs e)
        {
            dragout.BackgroundImage = Mass_Editor.Properties.Resources.slotTrans;
        }
        private void dragoutDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            openQuick(files[0]);
            e.Effect = DragDropEffects.Copy;

            Cursor = DefaultCursor;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SAV.Edited)
            {
                if (Util.Prompt(MessageBoxButtons.YesNo, "Any unsaved changes will be lost.  Are you sure you want to close PKHeX?") != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region //// SAVE FILE FUNCTIONS ////
        private void clickVerifyCHK(object sender, EventArgs e)
        {
            if (SAV.Edited) { Util.Alert("Save has been edited. Cannot integrity check."); return; }

            if (SAV.ChecksumsValid) { Util.Alert("Checksums are valid."); return; }
            if (DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Export Checksum Info to Clipboard?"))
                Clipboard.SetText(SAV.ChecksumInfo);
        }
        private void clickExportSAVBAK(object sender, EventArgs e)
        {
            if (!SAV.Exportable)
                return;
            SaveFileDialog sfd = new SaveFileDialog
            { FileName = Util.CleanFileName(SAV.BAKName) };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            string path = sfd.FileName;
            File.WriteAllBytes(path, SAV.BAK);
            Util.Alert("Saved Backup of current SAV to:", path);

            if (Directory.Exists(BackupPath)) return;
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo,
                $"PKHeX can perform automatic backups if you create a folder with the name \"{BackupPath}\" in the same folder as PKHeX's executable.",
                "Would you like to create the backup folder now and save backup of current save?")) return;

            try { Directory.CreateDirectory(BackupPath); Util.Alert("Backup folder created!", 
                $"If you wish to no longer automatically back up save files, delete the \"{BackupPath}\" folder."); }
            catch(Exception ex) { Util.Error($"Unable to create backup folder @ {BackupPath}", ex); }
        }
        private void clickExportSAV(object sender, EventArgs e)
        {
            if (!Menu_ExportSAV.Enabled)
                return;

            // Chunk Error Checking
            string err = SAV.MiscSaveChecks();
            if (err.Length > 0 && Util.Prompt(MessageBoxButtons.YesNo, err, "Continue saving?") != DialogResult.Yes)
                return;

            SaveFileDialog main = new SaveFileDialog
            {
                Filter = SAV.Filter, 
                FileName = SAV.FileName,
                RestoreDirectory = true
            };
            if (Directory.Exists(SAV.FilePath))
                main.InitialDirectory = SAV.FilePath;

            // Export
            if (main.ShowDialog() != DialogResult.OK) return;

            if (SAV.HasBox)
                SAV.CurrentBox = CB_BoxSelect.SelectedIndex;

            bool dsv = Path.GetExtension(main.FileName)?.ToLower() == ".dsv";
            File.WriteAllBytes(main.FileName, SAV.Write(dsv));
            SAV.Edited = false;
            Util.Alert("SAV exported to:", main.FileName);
        }

        // Box/SAV Functions //
        private void clickBoxRight(object sender, EventArgs e)
        {
            CB_BoxSelect.SelectedIndex = (CB_BoxSelect.SelectedIndex + 1) % SAV.BoxCount;
        }
        private void clickBoxLeft(object sender, EventArgs e)
        {
            CB_BoxSelect.SelectedIndex = (CB_BoxSelect.SelectedIndex + SAV.BoxCount-1) % SAV.BoxCount;
        }
        private void clickBoxSort(object sender, EventArgs e)
        {
            if (tabBoxMulti.SelectedTab != Tab_Box)
                return;
            if (!SAV.HasBox)
                return;

            string modified;
            bool all = false;
            if (ModifierKeys == (Keys.Alt | Keys.Shift) && DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Clear ALL Boxes?!"))
            {
                SAV.resetBoxes();
                modified = "Boxes cleared!";
                all = true;
            }
            else if (ModifierKeys == Keys.Alt && DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Clear Current Box?"))
            {
                SAV.resetBoxes(CB_BoxSelect.SelectedIndex, CB_BoxSelect.SelectedIndex + 1);
                modified = "Current Box cleared!";
            }
            else if (ModifierKeys == (Keys.Control | Keys.Shift) && DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Sort ALL Boxes?!"))
            {
                SAV.sortBoxes();
                modified = "Boxes sorted!";
                all = true;
            }
            else if (ModifierKeys == Keys.Control && DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Sort Current Box?"))
            {
                SAV.sortBoxes(CB_BoxSelect.SelectedIndex, CB_BoxSelect.SelectedIndex + 1);
                modified = "Current Box sorted!";
            }
            else
                return;

            setPKXBoxes();
            updateBoxViewers(all);
            Util.Alert(modified);
        }
        private void clickBoxDouble(object sender, MouseEventArgs e)
        {
            if (tabBoxMulti.SelectedTab == Tab_SAV)
            {
                clickSaveFileName(sender, e);
                return;
            }
            if (tabBoxMulti.SelectedTab != Tab_Box)
                return;
            if (!SAV.HasBox)
                return;
            if (ModifierKeys != Keys.Shift)
            {
                var z = Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.GetType() == typeof(SAV_BoxViewer)) as SAV_BoxViewer;
                if (z != null)
                { Util.CenterToForm(z, this); z.BringToFront(); return; }
            }
            new SAV_BoxViewer(this).Show();
        }
        public int swapBoxesViewer(int viewBox)
        {
            int mainBox = CB_BoxSelect.SelectedIndex;
            CB_BoxSelect.SelectedIndex = viewBox;
            return mainBox;
        }
        public void notifyBoxViewerRefresh()
        {
            var views = Application.OpenForms.OfType<SAV_BoxViewer>();
            foreach (var v in views.Where(v => DragInfo.WasDragParticipant(v, v.CurrentBox) == false))
                v.setPKXBoxes();
            if (DragInfo.WasDragParticipant(this, CB_BoxSelect.SelectedIndex) == false)
                setPKXBoxes();
        }
        private void updateBoxViewers(bool all = false)
        {
            var views = Application.OpenForms.OfType<SAV_BoxViewer>();
            foreach (var v in views.Where(v => v.CurrentBox == CB_BoxSelect.SelectedIndex || all))
                v.setPKXBoxes();
        }

        private void clickSlot(object sender, EventArgs e)
        {
            switch (ModifierKeys)
            {
                case Keys.Control | Keys.Alt: clickClone(sender, e); break;
                case Keys.Control: clickView(sender, e); break;
                case Keys.Shift: clickSet(sender, e); break;
                case Keys.Alt: clickDelete(sender, e); break;
            }
        }
        private void clickView(object sender, EventArgs e)
        {
            int slot = getSlot(sender);

            if (SlotPictureBoxes[slot].Image == null)
            { SystemSounds.Exclamation.Play(); return; }
            int offset = getPKXOffset(slot);
            if (offset < 0)
            {
                Util.Error($"Slot read error @ slot {slot}.");
                return;
            }
            // Load the PKX file
            PKM pk = 30 <= slot && slot < 36 ? SAV.getPartySlot(offset) : SAV.getStoredSlot(offset);
            if (pk.Valid && pk.Species != 0)
            {
                try { populateFields(pk); }
                catch { }
                // Visual to display what slot is currently loaded.
                getSlotColor(slot, Mass_Editor.Properties.Resources.slotView);
            }
            else
                SystemSounds.Exclamation.Play();
        }
        private void clickSet(object sender, EventArgs e)
        {
            if (!verifiedPKM()) return;
            int slot = getSlot(sender);
            if (slot == 30 && (CB_Species.SelectedIndex == 0 || CHK_IsEgg.Checked))
            { Util.Alert("Can't have empty/egg first slot."); return; }

            int offset = getPKXOffset(slot);
            if (offset < 0)
            {
                Util.Error($"Slot read error @ slot {slot}.");
                return;
            }
            PKM pk = preparePKM();

            string[] errata = verifyPKMtoSAV(pk);
            if (errata.Length > 0 && DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, string.Join(Environment.NewLine, errata), "Continue?"))
                return;

            if (slot >= 30 && slot < 36) // Party
            {
                // If slot isn't overwriting existing PKM, make it write to the lowest empty PKM slot
                if (SAV.PartyCount < slot + 1 - 30)
                { slot = SAV.PartyCount + 30; offset = getPKXOffset(slot); }
                SAV.setPartySlot(pk, offset);
                setParty();
                getSlotColor(slot, Mass_Editor.Properties.Resources.slotSet);
            }
            else if (slot < 30 || HaX && slot >= 36 && slot < 42)
            {
                if (slot < 30)
                {
                    UndoStack.Push(new SlotChange
                    {
                        Box = CB_BoxSelect.SelectedIndex,
                        Slot = slot,
                        Offset = offset,
                        OriginalData = SAV.getStoredSlot(offset)
                    });
                    Menu_Undo.Enabled = true;
                }

                SAV.setStoredSlot(pk, offset);
                getQuickFiller(SlotPictureBoxes[slot], pk);
                getSlotColor(slot, Mass_Editor.Properties.Resources.slotSet);
            }

            updateBoxViewers();

            RedoStack.Clear(); Menu_Redo.Enabled = false;
        }
        private void clickDelete(object sender, EventArgs e)
        {
            int slot = getSlot(sender);
            if (slot == 30 && SAV.PartyCount == 1 && !HaX) { Util.Alert("Can't delete first slot."); return; }

            int offset = getPKXOffset(slot);
            if (offset < 0)
            {
                Util.Error($"Slot read error @ slot {slot}.");
                return;
            }
            if (slot >= 30 && slot < 36) // Party
            {
                SAV.deletePartySlot(slot-30);
                setParty();
                getSlotColor(slot, Mass_Editor.Properties.Resources.slotDel);
                return;
            }
            if (slot < 30 || HaX && slot >= 36 && slot < 42)
            {
                if (slot < 30)
                {
                    UndoStack.Push(new SlotChange
                    {
                        Box = CB_BoxSelect.SelectedIndex,
                        Slot = slot,
                        Offset = offset,
                        OriginalData = SAV.getStoredSlot(offset)
                    });
                    Menu_Undo.Enabled = true;
                }
                SAV.setStoredSlot(SAV.BlankPKM, getPKXOffset(slot));
            }
            else return;

            getQuickFiller(SlotPictureBoxes[slot], SAV.BlankPKM);
            getSlotColor(slot, Mass_Editor.Properties.Resources.slotDel);
            updateBoxViewers();

            RedoStack.Clear(); Menu_Redo.Enabled = false;
        }
        private readonly Stack<SlotChange> UndoStack = new Stack<SlotChange>();
        private readonly Stack<SlotChange> RedoStack = new Stack<SlotChange>();
        private void clickUndo(object sender, EventArgs e)
        {
            if (!UndoStack.Any())
                return;

            SlotChange change = UndoStack.Pop();
            if (change.Slot >= 30)
                return;

            RedoStack.Push(new SlotChange
            {
                Slot = change.Slot,
                Box = change.Box,
                Offset = change.Offset,
                OriginalData = SAV.getStoredSlot(change.Offset)
            });
            undoSlotChange(change);
        }
        private void clickRedo(object sender, EventArgs e)
        {
            if (!RedoStack.Any())
                return;

            SlotChange change = RedoStack.Pop();
            if (change.Slot >= 30)
                return;

            UndoStack.Push(new SlotChange
            {
                Slot = change.Slot,
                Box = change.Box,
                Offset = change.Offset,
                OriginalData = SAV.getStoredSlot(change.Offset)
            });
            undoSlotChange(change);
        }
        private void undoSlotChange(SlotChange change)
        {
            int slot = change.Slot;
            int offset = change.Offset;
            PKM pk = (PKM)change.OriginalData;

            if (CB_BoxSelect.SelectedIndex != change.Box)
                CB_BoxSelect.SelectedIndex = change.Box;
            SAV.setStoredSlot(pk, offset);
            getQuickFiller(SlotPictureBoxes[slot], pk);
            getSlotColor(slot, Mass_Editor.Properties.Resources.slotSet);

            Menu_Undo.Enabled = UndoStack.Any();
            Menu_Redo.Enabled = RedoStack.Any();

            SystemSounds.Asterisk.Play();
        }
        private void clickClone(object sender, EventArgs e)
        {
            if (getSlot(sender) > 30) return; // only perform action if cloning to boxes
            if (!verifiedPKM()) return; // don't copy garbage to the box

            if (Util.Prompt(MessageBoxButtons.YesNo, $"Clone Pokemon from Editing Tabs to all slots in {CB_BoxSelect.Text}?") != DialogResult.Yes)
                return;

            PKM pk = preparePKM();

            for (int i = 0; i < 30; i++) // set to every slot in box
            {
                SAV.setStoredSlot(pk, getPKXOffset(i));
                getQuickFiller(SlotPictureBoxes[i], pk);
            }
            updateBoxViewers();
        }
        private void clickLegality(object sender, EventArgs e)
        {
            int slot = getSlot(sender);
            PKM pk;
            if (slot >= 0)
                pk = SAV.getStoredSlot(getPKXOffset(slot));
            else if (verifiedPKM())
                pk = preparePKM();
            else
                return;

            if (pk.Species == 0 || !pk.ChecksumValid)
            { SystemSounds.Asterisk.Play(); return; }
            showLegality(pk, slot < 0, ModifierKeys == Keys.Control);
        }
        private void updateSaveSlot(object sender, EventArgs e)
        {
            if (SAV.Version != GameVersion.BATREV)
                return;
            ((SAV4BR)SAV).CurrentSlot = Util.getIndex(CB_SaveSlot);
            setPKXBoxes();
        }
        private void updateEggRNGSeed(object sender, EventArgs e)
        {
            if (TB_RNGSeed.Text.Length == 0)
            {
                // Reset to 0
                TB_RNGSeed.Text = 0.ToString("X"+SAV.DaycareSeedSize);
                return; // recursively triggers this method, no need to continue
            }

            string filterText = Util.getOnlyHex(TB_RNGSeed.Text);
            if (filterText.Length != TB_RNGSeed.Text.Length)
            {
                Util.Alert("Expected HEX (0-9, A-F).", "Received: " + Environment.NewLine + TB_RNGSeed.Text);
                // Reset to Stored Value
                var seed = SAV.getDaycareRNGSeed(SAV.DaycareIndex);
                if (seed != null)
                    TB_RNGSeed.Text = seed;
                return; // recursively triggers this method, no need to continue
            }

            // Write final value back to the save
            var value = filterText.PadLeft(SAV.DaycareSeedSize, '0');
            if (value != SAV.getDaycareRNGSeed(SAV.DaycareIndex))
            {
                SAV.setDaycareRNGSeed(SAV.DaycareIndex, value);
                SAV.Edited = true;
            }            
        }
        private void updateU64(object sender, EventArgs e)
        {
            if (!fieldsLoaded)
                return;

            TextBox tb = sender as TextBox;
            if (tb?.Text.Length == 0)
            {
                // Reset to 0
                tb.Text = 0.ToString("X16");
                return; // recursively triggers this method, no need to continue
            }

            // Currently saved Value
            ulong? oldval = 0;
            if (SAV.Generation == 6)
            {
                if (tb == TB_GameSync)
                    oldval = SAV.GameSyncID;
                else if (tb == TB_Secure1)
                    oldval = SAV.Secure1;
                else if (tb == TB_Secure2)
                    oldval = SAV.Secure2;
            }

            string filterText = Util.getOnlyHex(tb.Text);

            if (filterText.Length != tb.Text.Length)
            {
                Util.Alert("Expected HEX (0-9, A-F).", "Received: " + Environment.NewLine + tb.Text);
                // Reset to Stored Value
                tb.Text = (oldval ?? 0).ToString("X16");
                return; // recursively triggers this method, no need to continue
            }

            // Write final value back to the save
            ulong? newval = Convert.ToUInt64(filterText, 16);
            if (newval == oldval) return;

            if (SAV.Generation >= 6)
            {
                if (tb == TB_GameSync)
                    SAV.GameSyncID = newval;
                else if (tb == TB_Secure1)
                    SAV.Secure1 = newval;
                else if (tb == TB_Secure2)
                    SAV.Secure2 = newval;
                SAV.Edited = true;
            }
        }
        private void updateIsNicknamed(object sender, EventArgs e)
        {
            if (!fieldsLoaded)
                return;

            if (!CHK_Nicknamed.Checked)
            {
                int species = Util.getIndex(CB_Species);
                if (species < 1 || species > SAV.MaxSpeciesID)
                    return;
                int lang = Util.getIndex(CB_Language);
                if (CHK_IsEgg.Checked) species = 0; // Set species to 0 to get the egg name.
                string nick = PKX.getSpeciesName(CHK_IsEgg.Checked ? 0 : species, lang);

                if (SAV.Generation < 5) // All caps GenIV and previous
                    nick = nick.ToUpper();
                if (SAV.Generation < 3)
                    nick = nick.Replace(" ", "");
                if (TB_Nickname.Text != nick)
                {
                    CHK_Nicknamed.Checked = true;
                    pkm.Nickname = TB_Nickname.Text;
                }
            }
        }

        // Generic Subfunctions //
        private void setParty()
        {
            PKM[] party = SAV.PartyData;
            PKM[] battle = SAV.BattleBoxData;
            // Refresh slots
            if (SAV.HasParty)
            {
                for (int i = 0; i < party.Length; i++)
                    getQuickFiller(SlotPictureBoxes[i + 30], party[i]);
                for (int i = party.Length; i < 6; i++)
                    SlotPictureBoxes[i + 30].Image = null;
            }
            if (SAV.HasBattleBox)
            {
                for (int i = 0; i < battle.Length; i++)
                    getQuickFiller(SlotPictureBoxes[i + 36], battle[i]);
                for (int i = battle.Length; i < 6; i++)
                    SlotPictureBoxes[i + 36].Image = null;
            }
        }
        private int getPKXOffset(int slot)
        {
            if (slot < 30) // Box Slot
                return SAV.getBoxOffset(CB_BoxSelect.SelectedIndex) + slot * SAV.SIZE_STORED;
            slot -= 30;
            if (slot < 6) // Party Slot
                return SAV.getPartyOffset(slot);
            slot -= 6;
            if (slot < 6) // Battle Box Slot
                return SAV.BattleBox + slot * SAV.SIZE_STORED;
            slot -= 6;
            if (slot < 2) // Daycare
                return SAV.getDaycareSlotOffset(SAV.DaycareIndex, slot);
            slot -= 2;
            if (slot == 0) // GTS
                return SAV.GTS;
            slot -= 1;
            if (slot == 0) // Fused
                return SAV.Fused;
            slot -= 1;
            if (slot < 3) // SUBE
                return SAV.SUBE + slot * (SAV.SIZE_STORED + 4);
            return -1;
        }
        private int getSlot(object sender)
        {
            sender = ((sender as ToolStripItem)?.Owner as ContextMenuStrip)?.SourceControl ?? sender as PictureBox;
            return Array.IndexOf(SlotPictureBoxes, sender);
        }
        public void setPKXBoxes()
        {
            if (SAV.HasBox)
            {
                int boxoffset = SAV.getBoxOffset(CB_BoxSelect.SelectedIndex);
                int boxbgval = SAV.getBoxWallpaper(CB_BoxSelect.SelectedIndex);
                PAN_Box.BackgroundImage = BoxWallpaper.getWallpaper(SAV, boxbgval);
                for (int i = 0; i < 30; i++)
                {
                    if (i < SAV.BoxSlotCount)
                    {
                        getSlotFiller(boxoffset + SAV.SIZE_STORED * i, SlotPictureBoxes[i]);
                    }
                    else
                    {
                        SlotPictureBoxes[i].Image = null;
                        SlotPictureBoxes[i].Visible = false;
                    }
                }
            }

            // Reload Party
            if (SAV.HasParty)
            {
                for (int i = 0; i < 6; i++)
                    getSlotFiller(SAV.getPartyOffset(i), SlotPictureBoxes[i + 30]);
            }

            // Reload Battle Box
            if (SAV.HasBattleBox)
            {
                for (int i = 0; i < 6; i++)
                    getSlotFiller(SAV.BattleBox + SAV.SIZE_STORED * i, SlotPictureBoxes[i + 36]);
            }

            // Reload Daycare
            if (SAV.HasDaycare)
            {
                Label[] L_SlotOccupied = { L_DC1, L_DC2 };
                TextBox[] TB_SlotEXP = { TB_Daycare1XP, TB_Daycare2XP };
                Label[] L_SlotEXP = { L_XP1, L_XP2 };

                for (int i = 0; i < 2; i++)
                {
                    getSlotFiller(SAV.getDaycareSlotOffset(SAV.DaycareIndex, i), SlotPictureBoxes[i + 42]);
                    uint? exp = SAV.getDaycareEXP(SAV.DaycareIndex, i);
                    TB_SlotEXP[i].Visible = L_SlotEXP[i].Visible = exp != null;
                    TB_SlotEXP[i].Text = exp.ToString();
                    bool? occ = SAV.getDaycareOccupied(SAV.DaycareIndex, i);
                    L_SlotOccupied[i].Visible = occ != null;
                    if (occ == true)   // If Occupied
                        L_SlotOccupied[i].Text = $"{i + 1}: ✓";
                    else
                    {
                        L_SlotOccupied[i].Text = $"{i + 1}: ✘";
                        SlotPictureBoxes[i + 42].Image = Util.ChangeOpacity(SlotPictureBoxes[i + 42].Image, 0.6);
                    }
                }
                bool? egg = SAV.getDaycareHasEgg(SAV.DaycareIndex);
                DayCare_HasEgg.Visible = egg != null;
                DayCare_HasEgg.Checked = egg == true;

                var seed = SAV.getDaycareRNGSeed(SAV.DaycareIndex);
                L_DaycareSeed.Visible = TB_RNGSeed.Visible = seed != null;
                if (seed != null)
                {
                    TB_RNGSeed.MaxLength = SAV.DaycareSeedSize;
                    TB_RNGSeed.Text = seed;
                }
            }

            // GTS
            if (SAV.HasGTS)
            getSlotFiller(SAV.GTS, SlotPictureBoxes[44]);

            // Fused
            if (SAV.HasFused)
            getSlotFiller(SAV.Fused, SlotPictureBoxes[45]);

            // SUBE
            if (SAV.HasSUBE)
            for (int i = 0; i < 3; i++)
            {
                int offset = SAV.SUBE + i * (SAV.SIZE_STORED + 4);
                if (BitConverter.ToUInt64(SAV.Data, offset) != 0)
                    getSlotFiller(offset, SlotPictureBoxes[46 + i]);
                else SlotPictureBoxes[46 + i].Image = null;
            }

            // Recoloring of a storage box slot (to not show for other storage boxes)
            if (colorizedslot <= SAV.BoxCount)
                SlotPictureBoxes[colorizedslot].BackgroundImage = colorizedbox == CB_BoxSelect.SelectedIndex ? colorizedcolor : null;
        }
        private void setBoxNames()
        {
            if (!SAV.HasBox)
                return;
            // Build ComboBox Dropdown Items
            try
            {
                CB_BoxSelect.Items.Clear();
                for (int i = 0; i < SAV.BoxCount; i++)
                    CB_BoxSelect.Items.Add(SAV.getBoxName(i));
            }
            catch
            {
                CB_BoxSelect.Items.Clear();
                for (int i = 1; i <= SAV.BoxCount; i++)
                    CB_BoxSelect.Items.Add($"BOX {i}");
            }
            if (SAV.CurrentBox < CB_BoxSelect.Items.Count)
                CB_BoxSelect.SelectedIndex = SAV.CurrentBox; // restore selected box
        }
        private void getQuickFiller(PictureBox pb, PKM pk = null)
        {
            if (!fieldsInitialized) return;
            pk = pk ?? preparePKM(false); // don't perform control loss click

            if (pb == dragout) mnuLQR.Enabled = pk.Species != 0; // Species
            pb.Image = pk.Species != 0 ? pk.Sprite : null;
            if (pb.BackColor == Color.Red)
                pb.BackColor = Color.Transparent;
        }
        private void getSlotFiller(int offset, PictureBox pb)
        {
            if (SAV.getData(offset, SAV.SIZE_STORED).SequenceEqual(new byte[SAV.SIZE_STORED]))
            {
                // 00s present in slot.
                pb.Image = null;
                pb.BackColor = Color.Transparent;
                pb.Visible = true;
                return;
            }
            PKM p = SAV.getStoredSlot(offset);
            if (!p.Valid) // Invalid
            {
                // Bad Egg present in slot.
                pb.Image = null;
                pb.BackColor = Color.Red;
                pb.Visible = true;
                return;
            }
            // Something stored in slot. Only display if species is valid.
            pb.Image = p.Species == 0 ? null : p.Sprite;
            pb.BackColor = Color.Transparent;
            pb.Visible = true;
        }
        private void getSlotColor(int slot, Image color)
        {
            foreach (PictureBox t in SlotPictureBoxes)
                t.BackgroundImage = null;

            if (slot < 30)
                colorizedbox = CB_BoxSelect.SelectedIndex;

            SlotPictureBoxes[slot].BackgroundImage = color;
            colorizedcolor = color;
            colorizedslot = slot;
        }
        private void getBox(object sender, EventArgs e)
        {
            if (SAV.CurrentBox != CB_BoxSelect.SelectedIndex)
            {
                SAV.CurrentBox = CB_BoxSelect.SelectedIndex;
                SAV.Edited = true; // Dumb
            }
            setPKXBoxes();
        }
        private void switchDaycare(object sender, EventArgs e)
        {
            if (!SAV.HasTwoDaycares) return;
            if (DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Would you like to switch the view to the other Daycare?",
                $"Currently viewing daycare {SAV.DaycareIndex + 1}."))
                // If ORAS, alter the daycare offset via toggle.
                SAV.DaycareIndex ^= 1;

            // Refresh Boxes
            setPKXBoxes();
        }
        private void dumpBoxesToDB(string path, bool individualBoxFolders)
        {
            PKM[] boxdata = SAV.BoxData;
            if (boxdata == null) { Util.Error("Null argument when dumping boxes."); return; } 
            for (int i = 0; i < boxdata.Length; i++)
            {
                PKM pk = boxdata[i];
                if (pk.Species == 0 || !pk.Valid)
                    continue;
                string fileName = Util.CleanFileName(pk.FileName);
                string boxfolder = "";
                if (individualBoxFolders)
                {
                    boxfolder = SAV.getBoxName(i/SAV.BoxSlotCount);
                    Directory.CreateDirectory(Path.Combine(path, boxfolder));
                }
                if (!File.Exists(Path.Combine(Path.Combine(path, boxfolder), fileName)))
                    File.WriteAllBytes(Path.Combine(Path.Combine(path, boxfolder), fileName), pk.DecryptedBoxData);
            }
        }
        private void loadBoxesFromDB(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            if (!SAV.HasBox) return;
            
            DialogResult dr = Util.Prompt(MessageBoxButtons.YesNoCancel, "Clear subsequent boxes when importing data?", "If you only want to overwrite for new data, press no.");
            if (dr == DialogResult.Cancel)
                return;
            if (dr == DialogResult.Yes)
                SAV.resetBoxes(CB_BoxSelect.SelectedIndex);

            bool? noSetb = getPKMSetOverride();

            int ctr = CB_BoxSelect.SelectedIndex*SAV.BoxSlotCount;
            int pastctr = 0;
            string[] filepaths = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);

            foreach (byte[] data in from file in filepaths where PKX.getIsPKM(new FileInfo(file).Length) select File.ReadAllBytes(file))
            {
                string c;
                PKM temp = PKMConverter.getPKMfromBytes(data);
                PKM pk = PKMConverter.convertToFormat(temp, SAV.PKMType, out c);

                if (pk != null) // Write to save
                {
                    if (verifyPKMtoSAV(pk).Length > 0)
                        continue;
                    SAV.setStoredSlot(pk, SAV.getBoxOffset(ctr/SAV.BoxSlotCount) + ctr%SAV.BoxSlotCount * SAV.SIZE_STORED, noSetb);
                    if (pk.Format != temp.Format) // Transferred
                        pastctr++;
                    if (++ctr == SAV.BoxCount*SAV.BoxSlotCount) // Boxes full!
                        break; 
                }
                Console.WriteLine(c);
            }
            ctr -= SAV.BoxSlotCount * CB_BoxSelect.SelectedIndex; // actual imported count
            if (ctr <= 0)
                return; 

            setPKXBoxes();
            updateBoxViewers();
            string result = $"Loaded {ctr} files to boxes.";
            if (pastctr > 0)
                Util.Alert(result, $"Conversion successful for {pastctr} past generation files.");
            else
                Util.Alert(result);
        }
        private void B_SaveBoxBin_Click(object sender, EventArgs e)
        {
            if (!SAV.HasBox)
            { Util.Alert("Save file does not have boxes to dump!"); return; }

            DialogResult dr = Util.Prompt(MessageBoxButtons.YesNoCancel, 
                "Yes: Export All Boxes" + Environment.NewLine + 
                $"No: Export {CB_BoxSelect.Text} (Box {CB_BoxSelect.SelectedIndex + 1})" + Environment.NewLine + 
                "Cancel: Abort");

            if (dr == DialogResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog {Filter = "Box Data|*.bin", FileName = "pcdata.bin"};
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                File.WriteAllBytes(sfd.FileName, SAV.getPCBin());
            }
            else if (dr == DialogResult.No)
            {
                SaveFileDialog sfd = new SaveFileDialog {Filter = "Box Data|*.bin", FileName = $"boxdata {CB_BoxSelect.Text}.bin"};
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                File.WriteAllBytes(sfd.FileName, SAV.getBoxBin(CB_BoxSelect.SelectedIndex));
            }
        }
        private bool? getPKMSetOverride()
        {
            var yn = Menu_ModifyPKM.Checked ? "Yes" : "No";
            DialogResult noSet = Util.Prompt(MessageBoxButtons.YesNoCancel, 
                "Loading overrides:",
                    "Yes - Modify .pk* when set to SAV" + Environment.NewLine +
                    "No - Don't modify .pk*" + Environment.NewLine +
                    $"Cancel - Use current settings ({yn})");
            return noSet == DialogResult.Yes ? true : (noSet == DialogResult.No ? (bool?)false : null);
        }

        // Subfunction Save Buttons //
        private void B_OpenWondercards_Click(object sender, EventArgs e)
        {
            new SAV_Wondercard(sender as MysteryGift).ShowDialog();
        }
        private void B_OpenBoxLayout_Click(object sender, EventArgs e)
        {
            new SAV_BoxLayout(CB_BoxSelect.SelectedIndex).ShowDialog();
            setBoxNames(); // fix box names
            setPKXBoxes(); // refresh box background
            updateBoxViewers(all:true); // update subviewers
        }
        private void B_OpenTrainerInfo_Click(object sender, EventArgs e)
        {
            if (SAV.Generation < 6)
                new SAV_SimpleTrainer().ShowDialog();
            else if (SAV.Generation == 6)
                new SAV_Trainer().ShowDialog();
            else if (SAV.Generation == 7)
                new SAV_Trainer7().ShowDialog();
            // Refresh conversion info
            PKMConverter.updateConfig(SAV.SubRegion, SAV.Country, SAV.ConsoleRegion, SAV.OT, SAV.Gender);
        }
        private void B_OpenPokepuffs_Click(object sender, EventArgs e)
        {
            new SAV_Pokepuff().ShowDialog();
        }
        private void B_OpenPokeBeans_Click(object sender, EventArgs e)
        {
            new SAV_Pokebean().ShowDialog();
        }
        private void B_OpenItemPouch_Click(object sender, EventArgs e)
        {
            new SAV_Inventory().ShowDialog();
        }
        private void B_OpenBerryField_Click(object sender, EventArgs e)
        {
            new SAV_BerryFieldXY().ShowDialog();
        }
        private void B_OpenPokeblocks_Click(object sender, EventArgs e)
        {
            new SAV_PokeBlockORAS().ShowDialog();
        }
        private void B_OpenEventFlags_Click(object sender, EventArgs e)
        {
            new SAV_EventFlags().ShowDialog();
        }
        private void B_OpenSuperTraining_Click(object sender, EventArgs e)
        {
            new SAV_SuperTrain().ShowDialog();
        }
        private void B_OpenOPowers_Click(object sender, EventArgs e)
        {
            if (SAV.Generation != 6)
                return;
            if (SAV.ORAS)
            {
                DialogResult dr = Util.Prompt(MessageBoxButtons.YesNo, "No editing support for ORAS :(", "Max O-Powers with a working code?");
                if (dr != DialogResult.Yes) return;
                new byte[] 
                { 
                    0x00, 0x01, 0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x01, 0x00,
                    0x01, 0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01,
                    0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x01, 0x01,
                    0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01,
                    0x01, 0x00, 0x00, 0x00,
                }.CopyTo(SAV.Data, ((SAV6) SAV).OPower);
            }
            else if (SAV.XY)
                new SAV_OPower().ShowDialog();
        }
        private void B_OpenPokedex_Click(object sender, EventArgs e)
        {
            if (SAV.ORAS)
                new SAV_PokedexORAS().ShowDialog();
            else if (SAV.XY)
                new SAV_PokedexXY().ShowDialog();
            else if (SAV.RBY || SAV.GSC)
                new SAV_SimplePokedex().ShowDialog();
            else if (SAV.SM)
                new SAV_PokedexSM().ShowDialog();
        }
        private void B_OUTPasserby_Click(object sender, EventArgs e)
        {
            if (SAV.Generation != 6)
                return;
            if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, "Export Passerby Info to Clipboard?"))
                return;
            string result = "PSS List" + Environment.NewLine;
            string[] headers = { "PSS Data - Friends", "PSS Data - Acquaintances", "PSS Data - Passerby", };
            int offset = ((SAV6) SAV).PSS;
            for (int g = 0; g < 3; g++)
            {
                result += Environment.NewLine
                    + "----" + Environment.NewLine
                    + headers[g] + Environment.NewLine
                    + "----" + Environment.NewLine;
                // uint count = BitConverter.ToUInt32(savefile, offset + 0x4E20);
                int r_offset = offset;

                for (int i = 0; i < 100; i++)
                {
                    ulong unkn = BitConverter.ToUInt64(SAV.Data, r_offset);
                    if (unkn == 0) break; // No data present here
                    if (i > 0) result += Environment.NewLine + Environment.NewLine;

                    string otname = Util.TrimFromZero(Encoding.Unicode.GetString(SAV.Data, r_offset + 8, 0x1A));
                    string message = Util.TrimFromZero(Encoding.Unicode.GetString(SAV.Data, r_offset + 0x22, 0x22));

                    // Trim terminated

                    // uint unk1 = BitConverter.ToUInt32(savefile, r_offset + 0x44);
                    // ulong unk2 = BitConverter.ToUInt64(savefile, r_offset + 0x48);
                    // uint unk3 = BitConverter.ToUInt32(savefile, r_offset + 0x50);
                    // uint unk4 = BitConverter.ToUInt16(savefile, r_offset + 0x54);
                    byte region = SAV.Data[r_offset + 0x56];
                    byte country = SAV.Data[r_offset + 0x57];
                    byte game = SAV.Data[r_offset + 0x5A];
                    // ulong outfit = BitConverter.ToUInt64(savefile, r_offset + 0x5C);
                    int favpkm = BitConverter.ToUInt16(SAV.Data, r_offset + 0x9C) & 0x7FF;
                    string gamename;
                    try { gamename = GameStrings.gamelist[game]; }
                    catch { gamename = "UNKNOWN GAME"; }

                    string[] cr = PKX.getCountryRegionText(country, region, curlanguage);
                    result +=
                        "OT: " + otname + Environment.NewLine +
                        "Message: " + message + Environment.NewLine +
                        "Game: " + gamename + Environment.NewLine +
                        "Country: " + cr[0] + Environment.NewLine +
                        "Region: " + cr[1] + Environment.NewLine +
                        "Favorite: " + GameStrings.specieslist[favpkm];

                    r_offset += 0xC8; // Advance to next entry
                }
                offset += 0x5000; // Advance to next block
            }
            Clipboard.SetText(result);
        }
        private void B_OUTHallofFame_Click(object sender, EventArgs e)
        {
            new SAV_HallOfFame().ShowDialog();
        }
        private void B_OpenSecretBase_Click(object sender, EventArgs e)
        {
            new SAV_SecretBase().ShowDialog();
        }
        private void B_LinkInfo_Click(object sender, EventArgs e)
        {
            new SAV_Link6().ShowDialog();
        }
        private void B_CGearSkin_Click(object sender, EventArgs e)
        {
            new SAV_CGearSkin().ShowDialog();
        }
        private void B_JPEG_Click(object sender, EventArgs e)
        {
            byte[] jpeg = SAV.JPEGData;
            if (SAV.JPEGData == null)
            { Util.Alert("No PGL picture data found in the save file!"); return; }
            string filename = SAV.JPEGTitle + "'s picture";
            SaveFileDialog sfd = new SaveFileDialog {FileName = filename, Filter = "JPEG|*.jpeg"};
            if (sfd.ShowDialog() != DialogResult.OK) return;
            File.WriteAllBytes(sfd.FileName, jpeg);
        }
        // Save Folder Related
        private void clickSaveFileName(object sender, EventArgs e)
        {
            string path = SaveUtil.detectSaveFile();
            if (path == null || !File.Exists(path)) return;
            if (Util.Prompt(MessageBoxButtons.YesNo, "Open save file from the following location?", path) == DialogResult.Yes)
                openQuick(path); // load save
        }

        // Drag and drop related functions
        private void pbBoxSlot_MouseClick(object sender, MouseEventArgs e)
        {
            if (DragInfo.slotDragDropInProgress)
                return;
            
            clickSlot(sender, e);
        }
        private void pbBoxSlot_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                DragInfo.slotLeftMouseIsDown = false;
            if (e.Button == MouseButtons.Right)
                DragInfo.slotRightMouseIsDown = false;
        }
        private void pbBoxSlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                DragInfo.slotLeftMouseIsDown = true;
            if (e.Button == MouseButtons.Right)
                DragInfo.slotRightMouseIsDown = true;
        }
        private void pbBoxSlot_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragInfo.slotDragDropInProgress)
                return;

            if (DragInfo.slotLeftMouseIsDown)
            {
                // The goal is to create a temporary PKX file for the underlying Pokemon
                // and use that file to perform a drag drop operation.

                // Abort if there is no Pokemon in the given slot.
                PictureBox pb = (PictureBox)sender;
                if (pb.Image == null)
                    return;

                // Set flag to prevent re-entering.
                DragInfo.slotDragDropInProgress = true;

                DragInfo.slotSource = this;
                DragInfo.slotSourceSlotNumber = getSlot(pb);
                int offset = getPKXOffset(DragInfo.slotSourceSlotNumber);

                // Prepare Data
                DragInfo.slotPkmSource = SAV.getData(offset, SAV.SIZE_STORED);
                DragInfo.slotSourceOffset = offset;
                DragInfo.slotSourceBoxNumber = DragInfo.slotSourceSlotNumber >= 30  ? -1 : CB_BoxSelect.SelectedIndex;

                // Make a new file name based off the PID
                bool encrypt = ModifierKeys == Keys.Control;
                byte[] dragdata = SAV.decryptPKM(DragInfo.slotPkmSource);
                Array.Resize(ref dragdata, SAV.SIZE_STORED);
                PKM pkx = SAV.getPKM(dragdata);
                string filename = $"{Path.GetFileNameWithoutExtension(pkx.FileName)}{(encrypt ? ".ek" + pkx.Format : "." + pkx.Extension) }";

                // Make File
                string newfile = Path.Combine(Path.GetTempPath(), Util.CleanFileName(filename));
                try
                {
                    File.WriteAllBytes(newfile, encrypt ? pkx.EncryptedBoxData : pkx.DecryptedBoxData);
                    var img = (Bitmap)pb.Image;
                    DragInfo.Cursor = Cursor.Current = new Cursor(img.GetHicon());
                    pb.Image = null;
                    pb.BackgroundImage = Mass_Editor.Properties.Resources.slotDrag;
                    // Thread Blocks on DoDragDrop
                    DragInfo.CurrentPath = newfile;
                    DragDropEffects result = pb.DoDragDrop(new DataObject(DataFormats.FileDrop, new[] { newfile }), DragDropEffects.Move);
                    if (!DragInfo.SourceValid || result != DragDropEffects.Link) // not dropped to another box slot, restore img
                        pb.Image = img;
                    else // refresh image
                        getQuickFiller(pb, SAV.getStoredSlot(DragInfo.slotSourceOffset));
                    pb.BackgroundImage = null;
                    
                    if (DragInfo.slotDestinationBoxNumber == DragInfo.slotSourceBoxNumber && DragInfo.slotDestinationSlotNumber > -1)
                        SlotPictureBoxes[DragInfo.slotDestinationSlotNumber].Image = img;

                    if (result == DragDropEffects.Copy) // viewed in tabs, apply 'view' highlight
                        getSlotColor(DragInfo.slotSourceSlotNumber, Mass_Editor.Properties.Resources.slotView);
                }
                catch (Exception x)
                {
                    Util.Error("Drag & Drop Error", x);
                }
                notifyBoxViewerRefresh();
                DragInfo.Reset();
                Cursor = DefaultCursor;

                // Browser apps need time to load data since the file isn't moved to a location on the user's local storage.
                // Tested 10ms -> too quick, 100ms was fine. 500ms should be safe?
                new Thread(() =>
                {
                    Thread.Sleep(500);
                    if (File.Exists(newfile) && DragInfo.CurrentPath == null)
                        File.Delete(newfile);
                }).Start();
            }
        }
        private void pbBoxSlot_DragDrop(object sender, DragEventArgs e)
        {
            DragInfo.slotDestination = this;
            DragInfo.slotDestinationSlotNumber = getSlot(sender);
            DragInfo.slotDestinationOffset = getPKXOffset(DragInfo.slotDestinationSlotNumber);
            DragInfo.slotDestinationBoxNumber = CB_BoxSelect.SelectedIndex;

            // Check for In-Dropped files (PKX,SAV,ETC)
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (Directory.Exists(files[0])) { loadBoxesFromDB(files[0]); return; }
            if (DragInfo.slotSourceOffset < 0) // file
            {
                if (files.Length <= 0)
                    return;
                string file = files[0];
                FileInfo fi = new FileInfo(file);
                if (!PKX.getIsPKM(fi.Length) && !MysteryGift.getIsMysteryGift(fi.Length))
                { openQuick(file); return; }

                byte[] data = File.ReadAllBytes(file);
                MysteryGift mg = MysteryGift.getMysteryGift(data, fi.Extension);
                PKM temp = mg != null ? mg.convertToPKM(SAV) : PKMConverter.getPKMfromBytes(data);
                string c;

                PKM pk = PKMConverter.convertToFormat(temp, SAV.PKMType, out c);
                if (pk == null)
                { Util.Error(c); Console.WriteLine(c); return; }

                string[] errata = verifyPKMtoSAV(pk);
                if (errata.Length > 0)
                {
                    string concat = string.Join(Environment.NewLine, errata);
                    if (DialogResult.Yes != Util.Prompt(MessageBoxButtons.YesNo, concat, "Continue?"))
                    { Console.WriteLine(c); Console.WriteLine(concat); return; }
                }

                SAV.setStoredSlot(pk, DragInfo.slotDestinationOffset);
                getQuickFiller(SlotPictureBoxes[DragInfo.slotDestinationSlotNumber], pk);
                getSlotColor(DragInfo.slotDestinationSlotNumber, Mass_Editor.Properties.Resources.slotSet);
                Console.WriteLine(c);
            }
            else
            {
                PKM pkz = SAV.getStoredSlot(DragInfo.slotSourceOffset);
                if (!DragInfo.SourceValid) { } // not overwritable, do nothing
                else if (ModifierKeys == Keys.Alt && DragInfo.DestinationValid) // overwrite delete old slot
                {
                    // Clear from slot
                    if (DragInfo.SameBox)
                        getQuickFiller(SlotPictureBoxes[DragInfo.slotSourceSlotNumber], SAV.BlankPKM); // picturebox

                    SAV.setStoredSlot(SAV.BlankPKM, DragInfo.slotSourceOffset);
                }
                else if (ModifierKeys != Keys.Control && DragInfo.DestinationValid)
                {
                    // Load data from destination
                    PKM pk = ((PictureBox) sender).Image != null
                        ? SAV.getStoredSlot(DragInfo.slotDestinationOffset)
                        : SAV.BlankPKM;

                    // Set destination pokemon image to source picture box
                    if (DragInfo.SameBox)
                        getQuickFiller(SlotPictureBoxes[DragInfo.slotSourceSlotNumber], pk);

                    // Set destination pokemon data to source slot
                    SAV.setStoredSlot(pk, DragInfo.slotSourceOffset);
                }
                else if (DragInfo.SameBox)
                    getQuickFiller(SlotPictureBoxes[DragInfo.slotSourceSlotNumber], pkz);

                // Copy from temp to destination slot.
                SAV.setStoredSlot(pkz, DragInfo.slotDestinationOffset);
                getQuickFiller(SlotPictureBoxes[DragInfo.slotDestinationSlotNumber], pkz);

                e.Effect = DragDropEffects.Link;
                Cursor = DefaultCursor;
            }
            if (DragInfo.slotSource == null) // another instance or file
            {
                notifyBoxViewerRefresh();
                DragInfo.Reset();
            }
        }
        private void pbBoxSlot_DragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == (DragDropEffects.Copy | DragDropEffects.Link)) // external file
                e.Effect = DragDropEffects.Copy;
            else if (e.Data != null) // within
                e.Effect = DragDropEffects.Move;

            if (DragInfo.slotDragDropInProgress)
                Cursor = DragInfo.Cursor;
        }
        private void pbBoxSlot_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Cancel || e.Action == DragAction.Drop)
            {
                DragInfo.slotLeftMouseIsDown = false;
                DragInfo.slotRightMouseIsDown = false;
                DragInfo.slotDragDropInProgress = false;
            }
        }

        public static class DragInfo
        {
            public static bool slotLeftMouseIsDown;
            public static bool slotRightMouseIsDown;
            public static bool slotDragDropInProgress;

            public static byte[] slotPkmSource;
            public static byte[] slotPkmDestination;

            public static object slotSource;
            public static int slotSourceOffset = -1;
            public static int slotSourceSlotNumber = -1;
            public static int slotSourceBoxNumber = -1;

            public static object slotDestination;
            public static int slotDestinationOffset = -1;
            public static int slotDestinationSlotNumber = -1;
            public static int slotDestinationBoxNumber = -1;

            public static Cursor Cursor;
            public static string CurrentPath;

            public static bool SameBox => slotSourceBoxNumber > -1 && slotSourceBoxNumber == slotDestinationBoxNumber;
            public static bool SourceValid => slotSourceBoxNumber > -1;
            public static bool DestinationValid => slotDestinationBoxNumber > -1;
            public static void Reset()
            {
                slotLeftMouseIsDown = false;
                slotRightMouseIsDown = false;
                slotDragDropInProgress = false;

                slotPkmSource = null;
                slotSourceOffset = slotSourceSlotNumber = slotSourceBoxNumber = -1;
                slotPkmDestination = null;
                slotDestinationOffset = slotSourceBoxNumber = slotDestinationBoxNumber = -1;

                Cursor = null;
                CurrentPath = null;

                slotSource = null;
                slotDestination = null;
            }

            public static bool? WasDragParticipant(object form, int index)
            {
                if (slotDestinationBoxNumber != index)
                    return null; // form was not watching box
                return slotSource == form || slotDestination == form; // form already updated?
            }
        }

        #endregion

        public void Form1_Load(object sender, EventArgs e)
        {

            if (allintobox)
            {
                if (this.litems.Count > 0)
                {
                    gennedfolder = this.litems[0];
                }
            }

            foreach (string l in this.litems)
            {
                if (Directory.Exists(l))
                {
                    foreach (string s in Directory.GetDirectories(l))
                    {
                        rekurGo(s);
                    }
                    foreach (string s in Directory.GetFiles(l))
                    {
                        ChangeIt(s);
                    }
                }
                else
                {
                    ChangeIt(l);
                }
                pbr.BeginInvoke((MethodInvoker)delegate
                {
                    pbr.Value++;
                    pbr.Refresh();
                });
            }

            if (allintobox)
            {

                if (this.litems.Count > 0)
                {

                    FileInfo f = new FileInfo(this.litems[0]);

                    if (box == 0)
                    {
                        File.WriteAllBytes(f.Directory.FullName + "\\GeneratedBoxFile" + gennedboxfile + ".bin", SAV.getBoxBin(CB_BoxSelect.SelectedIndex));
                    }
                    else
                    {
                        File.WriteAllBytes(f.Directory.FullName + "\\GeneratedBoxFile" + gennedboxfile + ".bin", SAV.getPCBin());
                    }

                }

            }


            this.Close();
            this.Dispose();
        }

        private void rekurGo(string s)
        {
            if (Directory.Exists(s))
            {
                foreach (string sf in Directory.GetDirectories(s))
                {
                    rekurGo(sf);
                }
                foreach (string sf in Directory.GetFiles(s))
                {
                    rekurGo(sf);
                }
            }
            else
            {
                ChangeIt(s);
            }
        }

        private void ChangeIt(string s)
        {
            object sender = null;
            EventArgs e = null;
            mainMenuOpen(s);
            if (box_load)
            {
                if (allintobox)
                {
                    Util.Error("Illegal Argument Exception", "\"All into Box / PC Data\" is not compatible with other Box / PC Data", "Ignoring \"" + s + "\"");
                    return;
                }

                bool onebox = true;

                for (int i = 0; i < CB_BoxSelect.Items.Count; i++)
                {
                    CB_BoxSelect.SelectedIndex = i;
                    PictureBox[] pba = {
                                    bpkx1, bpkx2, bpkx3, bpkx4, bpkx5, bpkx6,
                                    bpkx7, bpkx8, bpkx9, bpkx10,bpkx11,bpkx12,
                                    bpkx13,bpkx14,bpkx15,bpkx16,bpkx17,bpkx18,
                                    bpkx19,bpkx20,bpkx21,bpkx22,bpkx23,bpkx24,
                                    bpkx25,bpkx26,bpkx27,bpkx28,bpkx29,bpkx30
                                };
                    foreach (PictureBox pictureBox in pba)
                    {
                        bool worked = clickView(pictureBox);
                        if (worked)
                        {
                            if (i > 0)
                            {
                                onebox = false;
                            }
                            try
                            {
                                massEdit(sender, e);
                            }
                            catch (ArgumentException ae)
                            {
                                Util.Error("Illegal Argument Exception", "Incompatible Selections: " + ae.Message);
                                return;
                            }
                            clickSet(pictureBox, e);
                        }
                    }
                }

                PictureBox[] pbb = { ppkx1, ppkx2, ppkx3, ppkx4, ppkx5, ppkx6,
                                    bbpkx1,bbpkx2,bbpkx3,bbpkx4,bbpkx5,bbpkx6,
                                    dcpkx1, dcpkx2, gtspkx, fusedpkx,subepkx1,subepkx2,subepkx3 };

                foreach (PictureBox pictureBox in pbb)
                {
                    bool worked = clickView(pictureBox);
                    if (worked)
                    {
                        onebox = false;
                        try
                        {
                            massEdit(sender, e);
                        }
                        catch (ArgumentException ae)
                        {
                            Util.Error("Illegal Argument Exception", "Incompatible Selections: " + ae.Message);
                            return;
                        }
                        clickSet(pictureBox, e);
                    }
                }

                if (bak)
                {
                    // File already exists, save a .bak
                    byte[] backupfile = File.ReadAllBytes(s);
                    File.WriteAllBytes(s + ".bak", backupfile);
                }

                CB_BoxSelect.SelectedIndex = 0;

                if (onebox)
                {
                    File.WriteAllBytes(s, SAV.getBoxBin(CB_BoxSelect.SelectedIndex));
                }
                else
                {
                    File.WriteAllBytes(s, SAV.getPCBin());
                }

            }
            else
            {
                try
                {
                    massEdit(sender, e);
                }
                catch (ArgumentException ae)
                {
                    Util.Error("Illegal Argument Exception", "Incompatible Selections: " + ae.Message);
                    return;
                }

                if (allintobox)
                {

                    PictureBox[] pba = {
                                    bpkx1, bpkx2, bpkx3, bpkx4, bpkx5, bpkx6,
                                    bpkx7, bpkx8, bpkx9, bpkx10,bpkx11,bpkx12,
                                    bpkx13,bpkx14,bpkx15,bpkx16,bpkx17,bpkx18,
                                    bpkx19,bpkx20,bpkx21,bpkx22,bpkx23,bpkx24,
                                    bpkx25,bpkx26,bpkx27,bpkx28,bpkx29,bpkx30
                                };

                    if (picturebox == 30)
                    {
                        if (box == 30)
                        {

                            FileInfo f = new FileInfo(gennedfolder);

                            File.WriteAllBytes(f.Directory.FullName + "\\GeneratedBoxFile" + gennedboxfile + ".bin", SAV.getPCBin());

                            gennedboxfile++;

                            box = 0;
                            picturebox = 0;

                            for (int i = 0; i < CB_BoxSelect.Items.Count; i++)
                            {
                                CB_BoxSelect.SelectedIndex = i;
                                foreach (PictureBox pictureBox in pba)
                                {
                                    clickDelete(pictureBox, e);
                                }
                            }

                        }
                        else
                        {
                            picturebox = 0;
                            box++;
                        }
                        CB_BoxSelect.SelectedIndex = box;
                    }

                    clickSet(pba[picturebox], e);

                    picturebox++;

                }
                else
                {
                    mainMenuSave(s);
                }

            }

        }

        private void mainMenuSave(string path)
        {
            PKM pk = preparePKM();
            string pkx = pk.Extension;
            string ekx = 'e' + pkx.Substring(1, pkx.Length - 1);
            string ext = Path.GetExtension(path);

            if (bak && File.Exists(path))
            {
                // File already exists, save a .bak
                byte[] backupfile = File.ReadAllBytes(path);
                File.WriteAllBytes(path + ".bak", backupfile);
            }

            if (new[] { ".ekx", "." + ekx, ".bin" }.Contains(ext))
                File.WriteAllBytes(path, pk.EncryptedPartyData);
            else if (new[] { "." + pkx }.Contains(ext))
                File.WriteAllBytes(path, pk.DecryptedBoxData);
            else
            {
                File.WriteAllBytes(path, pkm.EncryptedPartyData);
            }            
        }

        private void mainMenuOpen(string s)
        {
            string path = s;
            box_load = false;
            openQuick(path);
        }

        private bool clickView(object sender)
        {
            int slot = getSlot(sender);

            if (SlotPictureBoxes[slot].Image == null)
            { return false; }
            int offset = getPKXOffset(slot);
            if (offset < 0)
            {
                return false;
            }
            // Load the PKX file
            PKM pk = 30 <= slot && slot < 36 ? SAV.getPartySlot(offset) : SAV.getStoredSlot(offset);
            if (pk.Valid && pk.Species != 0)
            {
                try { populateFields(pk); }
                catch { }
                // Visual to display what slot is currently loaded.
                getSlotColor(slot, Mass_Editor.Properties.Resources.slotView);
                return true;
            }
            else
                return false;
        }

        private void massEdit(object sender, EventArgs e)
        {
            foreach (int mode in modes)
            {
                switch (mode)
                {
                    case 1:
                        this.updateRandomPID(sender, e);
                        break;
                    case 2:
                        if (ot[0])
                            TB_TID.Text = ret[0];
                        if (ot[1])
                            TB_SID.Text = ret[1];
                        if (ot[2])
                            TB_OT.Text = ret[2];
                        if (ot[3])
                            TB_OTt2.Text = ret[3];
                        if (ot[4])
                            Label_OTGender.Text = otgenders[0];
                        if (ot[5])
                            Label_CTGender.Text = otgenders[1];
                        break;
                    case 3:
                        if (CHK_Nicknamed.Checked)
                        {
                            CHK_Nicknamed.Checked = false;
                        }
                        ComboItem cb = (ComboItem) CB_Species.SelectedItem;
                        TB_Nickname.Text = cb.Text;
                        break;
                    case 4:
                        TB_HPIV.Text = "31";
                        TB_ATKIV.Text = "31";
                        TB_DEFIV.Text = "31";
                        TB_SPAIV.Text = "31";
                        TB_SPDIV.Text = "31";
                        TB_SPEIV.Text = "31";
                        break;
                    case 5:
                        this.updateRandomPID(sender, e);
                        this.updateRandomEC(sender, e);
                        break;
                    case 6:
                        TB_Friendship.Text = friendship;
                        break;
                    case 7:
                        TB_Level.Text = level;
                        break;
                    case 8:
                        if (met[0])
                            CB_GameOrigin.SelectedIndex = m.p1;
                        if (met[1])
                        {
                            if (CB_GameOrigin.SelectedIndex == m.p1)
                            {
                                CB_MetLocation.SelectedIndex = m.p2;
                            }
                            else
                            {
                                string before = ((ComboItem)CB_GameOrigin.SelectedItem).Text;
                                CB_GameOrigin.SelectedIndex = m.p1;
                                string after = ((ComboItem)CB_GameOrigin.SelectedItem).Text;
                                throw new ArgumentException(before + " | " + after);
                            }
                        }
                        if (met[2])
                            CB_Ball.SelectedIndex = m.p3;
                        if (met[3])
                            TB_MetLevel.Text = m.p4;
                        if (met[4])
                            CAL_MetDate.Value = m.dateTime1;
                        if (met[5])
                            CHK_Fateful.Checked = m.p5;
                        if (m.p6)
                        {
                            if (met[6])
                                CB_EncounterType.SelectedIndex = m.p7;
                        }
                        if (met[7])
                            CHK_AsEgg.Checked = m.p8;
                        if (CHK_AsEgg.Checked)
                        {
                            if (met[8])
                                CB_EggLocation.SelectedIndex = m.p9;
                            if (met[9])
                                CAL_EggDate.Value = m.dateTime2;
                        }
                        break;
                    case 9:
                        if (country[0])
                            CB_Language.SelectedIndex = otindexes[0];
                        if (country[1])
                            CB_Country.SelectedIndex = otindexes[1];
                        if (country[2])
                        {
                            if (CB_Country.SelectedIndex == otindexes[1])
                            {
                                CB_SubRegion.SelectedIndex = otindexes[2];
                            }
                            else
                            {
                                string before = ((ComboItem)CB_Country.SelectedItem).Text;
                                CB_Country.SelectedIndex = otindexes[1];
                                string after = ((ComboItem)CB_Country.SelectedItem).Text;
                                throw new ArgumentException(before + " | " + after);
                            }
                        }
                        if (country[3])
                            CB_3DSReg.SelectedIndex = otindexes[3];
                        break;
                    case 10:
                        MemoryAmie ma = new MemoryAmie(this, amienabled, amiindex, amilite, amilitebool, amiliteint);
                        ma.MemoryAmie_Load(ma, null);
                        break;
                    case 11:
                        CB_PPu1.SelectedIndex = 3;
                        CB_PPu2.SelectedIndex = 3;
                        CB_PPu3.SelectedIndex = 3;
                        CB_PPu4.SelectedIndex = 3;
                        break;
                    case 12:
                        this.updateShinyPID(sender, e);
                        break;
                    case 13:
                        RibbonEditor rb = new RibbonEditor(this, badgeChecks, badgeInts);
                        rb.RibbonEditor_Load(rb, null);
                        break;
                    case 14:
                        CHK_Cured.Checked = false;
                        CHK_Infected.Checked = false;
                        break;
                    case 15:
                        PictureBox[] cba = { PB_Mark1, PB_Mark2, PB_Mark3, PB_Mark4, PB_Mark5, PB_Mark6 };
                        for (int i = 0; i < cba.Length; i++)
                        {
                            // Handling Gens 3-6
                            int[] markings = pkm.Markings;
                            switch (pkm.Format)
                            {
                                case 3:
                                case 4:
                                case 5:
                                case 6: // on/off
                                    markings[i] = symbolChecks[i]; // toggle
                                    pkm.Markings = markings;
                                    break;
                                case 7: // 0 (none) | 1 (blue) | 2 (pink)
                                    markings[i] = symbolChecks[i]; // cycle
                                    pkm.Markings = markings;
                                    break;
                                default:
                                    return;
                            }
                            setMarkings();
                        }
                        break;
                    case 16:
                        MaskedTextBox[] TB_Contests = { TB_Cool, TB_Beauty, TB_Cute, TB_Smart, TB_Tough, TB_Sheen };
                        for (int i = 0; i < TB_Contests.Length; i++)
                        {
                            TB_Contests[i].Text = contestStats[i];
                        }
                        break;
                    case 17:
                        int gt = SAV.Personal[Util.getIndex(CB_Species)].Gender;

                        if (SAV.Generation == 2)
                            return;
                        
                        // Exclude Pokémon with no gender or just one
                        if (!(gt == 255 || gt == 0 || gt == 254))
                        {
                            if (Label_Gender.Text != gender)
                            {
                                clickGender(Label_Gender, null);
                            }
                        }

                        break;
                    case 18:
                        TB_ATKEV.Text = "0";
                        TB_DEFEV.Text = "0";
                        TB_SPAEV.Text = "0";
                        TB_SPDEV.Text = "0";
                        TB_HPEV.Text = "0";
                        TB_SPEEV.Text = "0";
                        break;
                }
            }
        }
        }
}
