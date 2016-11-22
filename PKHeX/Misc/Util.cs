﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Mass_Editor
{
    public class Util
    {
        // Image Layering/Blending Utility
        internal static Bitmap LayerImage(Image baseLayer, Image overLayer, int x, int y, double trans)
        {
            Bitmap overlayImage = (Bitmap)overLayer;
            Bitmap newImage = (Bitmap)baseLayer;
            if (baseLayer == null) return overlayImage;
            for (int i = 0; i < (overlayImage.Width * overlayImage.Height); i++)
            {
                Color newColor = overlayImage.GetPixel(i % (overlayImage.Width), i / (overlayImage.Width));
                Color oldColor = newImage.GetPixel(i % (overlayImage.Width) + x, i / (overlayImage.Width) + y);
                newColor = Color.FromArgb((int)(newColor.A * trans), newColor.R, newColor.G, newColor.B); // Apply transparency change
                // if (newColor.A != 0) // If Pixel isn't transparent, we'll overwrite the color.
                {
                    // if (newColor.A < 100) 
                    newColor = AlphaBlend(newColor, oldColor);
                    newImage.SetPixel(
                        i % (overlayImage.Width) + x,
                        i / (overlayImage.Width) + y,
                        newColor);
                }
            }
            return newImage;
        }
        internal static Bitmap ChangeOpacity(Image img, double trans)
        {
            if (img == null) return null;
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix {Matrix33 = (float) trans};
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics
            return bmp;
        }
        internal static Color AlphaBlend(Color ForeGround, Color BackGround)
        {
            if (ForeGround.A == 0)
                return BackGround;
            if (BackGround.A == 0)
                return ForeGround;
            if (ForeGround.A == 255)
                return ForeGround;
            int Alpha = Convert.ToInt32(ForeGround.A);
            int B = Alpha * ForeGround.B + (255 - Alpha) * BackGround.B >> 8;
            int G = Alpha * ForeGround.G + (255 - Alpha) * BackGround.G >> 8;
            int R = Alpha * ForeGround.R + (255 - Alpha) * BackGround.R >> 8;
            int A = ForeGround.A;
            if (BackGround.A == 255)
                A = 255;
            if (A > 255)
                A = 255;
            if (R > 255)
                R = 255;
            if (G > 255)
                G = 255;
            if (B > 255)
                B = 255;
            return Color.FromArgb(Math.Abs(A), Math.Abs(R), Math.Abs(G), Math.Abs(B));
        }

        // Strings and Paths
        internal static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Union(directory.GetDirectories().Select(GetNewestFile))
                .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
                .FirstOrDefault();
        }
        internal static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
               .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        internal static string GetTempFolder()
        {
            return Path.Combine(Path.GetTempPath(), "3DSSE");
        }
        internal static string GetCacheFolder()
        {
            return Path.Combine(GetBackupLocation(), "cache");
        }
        internal static string GetRegistryValue(string key)
        {
            Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey key3 = currentUser.OpenSubKey(GetRegistryBase());
            if (key3 == null)
                return null;

            string str = key3.GetValue(key) as string;
            key3.Close();
            currentUser.Close();
            return str;
        }
        internal static string GetRegistryBase()
        {
            return @"SOFTWARE\CYBER Gadget\3DSSaveEditor";
        }
        internal static string GetBackupLocation()
        {
            string registryValue = GetRegistryValue("Location");
            if (!string.IsNullOrEmpty(registryValue))
            {
                Directory.CreateDirectory(registryValue);
                return registryValue;
            }
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + "3DSSaveBank";
            Directory.CreateDirectory(path);
            return path;
        }
        internal static string get3DSLocation()
        {
            try
            {
                string path_3DS = null;
                string[] DriveList = Environment.GetLogicalDrives();
                for (int i = 1; i < DriveList.Length; i++) // Skip first drive (some users still have floppy drives and would chew up time!)
                {
                    string potentialPath = DriveList[i] + Path.DirectorySeparatorChar + "Nintendo 3DS";
                    if (!Directory.Exists(potentialPath)) continue;

                    path_3DS = potentialPath; break;
                }
                return path_3DS;
            }
            catch { return null; }
        }
        internal static string GetSDFLocation()
        {
            try
            {
                // Start by checking if the 3DS file path exists or not.
                string path_SDF = null;
                string[] DriveList = Environment.GetLogicalDrives();
                for (int i = 1; i < DriveList.Length; i++) // Skip first drive (some users still have floppy drives and would chew up time!)
                {
                    string potentialPath_SDF = NormalizePath(Path.Combine(DriveList[i], "filer" + Path.DirectorySeparatorChar + "UserSaveData"));
                    if (!Directory.Exists(potentialPath_SDF)) continue;

                    path_SDF = potentialPath_SDF; break;
                }
                if (path_SDF == null)
                    return null;
                // 3DS data found in SD card reader. Let's get the title folder location!
                string[] folders = Directory.GetDirectories(path_SDF, "*", SearchOption.TopDirectoryOnly);
                Array.Sort(folders); // Don't need Modified Date, sort by path names just in case.

                // Loop through all the folders in the Nintendo 3DS folder to see if any of them contain 'title'.
                for (int i = folders.Length - 1; i >= 0; i--)
                {
                    if (File.Exists(Path.Combine(folders[i], "000011c4" + Path.DirectorySeparatorChar + "main"))) return Path.Combine(folders[i], "000011c4"); // OR
                    if (File.Exists(Path.Combine(folders[i], "000011c5" + Path.DirectorySeparatorChar + "main"))) return Path.Combine(folders[i], "000011c5"); // AS
                    if (File.Exists(Path.Combine(folders[i], "0000055d" + Path.DirectorySeparatorChar + "main"))) return Path.Combine(folders[i], "0000055d"); // X
                    if (File.Exists(Path.Combine(folders[i], "0000055e" + Path.DirectorySeparatorChar + "main"))) return Path.Combine(folders[i], "0000055e"); // Y
                }
                return null; // Fallthrough
            }
            catch { return null; }
        }
        internal static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
        internal static string TrimFromZero(string input)
        {
            int index = input.IndexOf('\0');
            return index < 0 ? input : input.Substring(0, index);
        }
        internal static string[] getStringList(string f, string l)
        {
            object txt = Mass_Editor.Properties.Resources.ResourceManager.GetObject("text_" + f + "_" + l); // Fetch File, \n to list.
            List<string> rawlist = ((string)txt).Split(new[] { '\n' }).ToList();

            string[] stringdata = new string[rawlist.Count];
            for (int i = 0; i < rawlist.Count; i++)
                stringdata[i] = rawlist[i].Trim();

            return stringdata;
        }
        internal static string[] getSimpleStringList(string f)
        {
            object txt = Mass_Editor.Properties.Resources.ResourceManager.GetObject(f); // Fetch File, \n to list.
            List<string> rawlist = ((string)txt).Split(new[] { '\n' }).ToList();

            string[] stringdata = new string[rawlist.Count];
            for (int i = 0; i < rawlist.Count; i++)
                stringdata[i] = rawlist[i].Trim();

            return stringdata;
        }
        internal static string[] getNulledStringArray(string[] SimpleStringList)
        {
            try
            {
                string[] newlist = new string[ToInt32(SimpleStringList[SimpleStringList.Length - 1].Split(',')[0]) + 1];
                for (int i = 1; i < SimpleStringList.Length; i++)
                    newlist[ToInt32(SimpleStringList[i].Split(',')[0])] = SimpleStringList[i].Split(',')[1];
                return newlist;
            }
            catch { return null; }
        }

        // Randomization
        internal static Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));
        }

        // Data Retrieval
        internal static int ToInt32(TextBox tb)
        {
            string value = tb.Text;
            return ToInt32(value);
        }
        internal static uint ToUInt32(TextBox tb)
        {
            string value = tb.Text;
            return ToUInt32(value);
        }
        internal static int ToInt32(MaskedTextBox tb)
        {
            string value = tb.Text;
            return ToInt32(value);
        }
        internal static uint ToUInt32(MaskedTextBox tb)
        {
            string value = tb.Text;
            return ToUInt32(value);
        }
        internal static int ToInt32(String value)
        {
            value = value.Replace(" ", "");
            if (String.IsNullOrEmpty(value))
                return 0;
            try
            {
                value = value.TrimEnd(new[] { '_' });
                return Int32.Parse(value);
            }
            catch { return 0; }
        }
        internal static uint ToUInt32(String value)
        {
            value = value.Replace(" ", "");
            if (String.IsNullOrEmpty(value))
                return 0;
            try
            {
                value = value.TrimEnd(new[] { '_' });
                return UInt32.Parse(value);
            }
            catch { return 0; }
        }
        internal static uint getHEXval(TextBox tb)
        {
            if (tb.Text == null)
                return 0;
            string str = getOnlyHex(tb.Text);
            return UInt32.Parse(str, NumberStyles.HexNumber);
        }
        internal static int getIndex(ComboBox cb)
        {
            int val;
            if (cb.SelectedValue == null) return 0;

            try { val = int.Parse(cb.SelectedValue.ToString()); }
            catch { val = cb.SelectedIndex; if (val < 0) val = 0; }

            return val;
        }
        internal static string getOnlyHex(string str)
        {
            if (str == null) return "0";

            string s = "";

            foreach (char c in str)
            {
                // filter for hex
                if ((c < 0x0047 && c > 0x002F) || (c < 0x0067 && c > 0x0060))
                    s += c;
                else
                    System.Media.SystemSounds.Beep.Play();
            }
            if (s.Length == 0)
                s = "0";
            return s;
        }

        // Data Manipulation
        internal static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(rand.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        // Form Translation
        internal static void TranslateInterface(Control form, string lang, MenuStrip menu = null)
        {
            string FORM_NAME = form.Name;
            Control.ControlCollection Controls = form.Controls;
            // debug(Controls);
            // Fetch a File
            // Check to see if a the translation file exists in the same folder as the executable
            string externalLangPath = Application.StartupPath + Path.DirectorySeparatorChar + "lang_" + lang + ".txt";
            string[] rawlist;
            if (File.Exists(externalLangPath))
                rawlist = File.ReadAllLines(externalLangPath);
            else
            {
                object txt = Mass_Editor.Properties.Resources.ResourceManager.GetObject("lang_" + lang);
                if (txt == null) return; // Translation file does not exist as a resource; abort this function and don't translate UI.
                rawlist = ((string)txt).Split(new[] { "\n" }, StringSplitOptions.None);
                rawlist = rawlist.Select(i => i.Trim()).ToArray(); // Remove trailing spaces
            }

            string[] stringdata = new string[rawlist.Length];
            int itemsToRename = 0;
            for (int i = 0; i < rawlist.Length; i++)
            {
                // Find our starting point
                if (!rawlist[i].Contains("! " + FORM_NAME)) continue;

                // Allow renaming of the Window Title
                string[] WindowName = Regex.Split(rawlist[i], " = ");
                if (WindowName.Length > 1) form.Text = WindowName[1];
                // Copy our Control Names and Text to a new array for later processing.
                for (int j = i + 1; j < rawlist.Length; j++)
                {
                    if (rawlist[j].Length == 0) continue; // Skip Over Empty Lines, errhandled
                    if (rawlist[j][0].ToString() == "-") continue; // Keep translating if line is a comment line
                    if (rawlist[j][0].ToString() == "!") // Stop if we have reached the end of translation
                        goto rename;
                    stringdata[itemsToRename] = rawlist[j]; // Add the entry to process later.
                    itemsToRename++;
                }
            }
            return; // Not Found

            // Now that we have our items to rename in: Control = Text format, let's execute the changes!
        rename:
            for (int i = 0; i < itemsToRename; i++)
            {
                string[] SplitString = Regex.Split(stringdata[i], " = ");
                if (SplitString.Length < 2)
                    continue; // Error in Input, errhandled
                string ctrl = SplitString[0]; // Control to change the text of...
                string text = SplitString[1]; // Text to set Control.Text to...
                Control[] controllist = Controls.Find(ctrl, true);
                if (controllist.Length == 0) // If Control isn't found...
                    try
                    {
                        // Menu Items can't be found with Controls.Find as they aren't Controls
                        ToolStripDropDownItem TSI = (ToolStripDropDownItem)menu.Items[ctrl];
                        if (TSI == null) continue;

                        // We'll rename the main and child in a row.
                        string[] ToolItems = Regex.Split(SplitString[1], " ; ");
                        TSI.Text = ToolItems[0]; // Set parent's text first
                        if (TSI.DropDownItems.Count != ToolItems.Length - 1)
                            continue; // Error in Input, errhandled
                        for (int ti = 1; ti <= TSI.DropDownItems.Count; ti++)
                            TSI.DropDownItems[ti - 1].Text = ToolItems[ti]; // Set child text
                        // If not found, it is not something to rename and is thus skipped.
                    }
                    catch { }
                else // Set the input control's text.
                    controllist[0].Text = text;
            }
        }

        // Message Displays
        internal static DialogResult Error(params string[] lines)
        {
            System.Media.SystemSounds.Exclamation.Play();
            string msg = String.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        internal static DialogResult Alert(params string[] lines)
        {
            System.Media.SystemSounds.Asterisk.Play();
            string msg = String.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        internal static DialogResult Prompt(MessageBoxButtons btn, params string[] lines)
        {
            System.Media.SystemSounds.Question.Play();
            string msg = String.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Prompt", btn, MessageBoxIcon.Asterisk);
        }

        // DataSource Providing
        public class cbItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
        }
        internal static List<cbItem> getCBList(string textfile, string lang)
        {
            // Set up
            string[] inputCSV = getSimpleStringList(textfile);

            // Get Language we're fetching for
            int index = Array.IndexOf(new[] { "ja", "en", "fr", "de", "it", "es", "ko", "zh", }, lang);

            // Set up our Temporary Storage
            string[] unsortedList = new string[inputCSV.Length - 1];
            int[] indexes = new int[inputCSV.Length - 1];

            // Gather our data from the input file
            for (int i = 1; i < inputCSV.Length; i++)
            {
                string[] countryData = inputCSV[i].Split(',');
                indexes[i - 1] = Convert.ToInt32(countryData[0]);
                unsortedList[i - 1] = countryData[index + 1];
            }

            // Sort our input data
            string[] sortedList = new string[inputCSV.Length - 1];
            Array.Copy(unsortedList, sortedList, unsortedList.Length);
            Array.Sort(sortedList);

            // Arrange the input data based on original number
            return sortedList.Select(s => new cbItem
            {
                Text = s, 
                Value = indexes[Array.IndexOf(unsortedList, s)]
            }).ToList();
        }
        internal static List<cbItem> getCBList(string[] inStrings, params int[][] allowed)
        {
            List<cbItem> cbList = new List<cbItem>();
            if (allowed == null)
                allowed = new[] { Enumerable.Range(0, inStrings.Length).ToArray() };

            foreach (int[] list in allowed)
            {
                // Sort the Rest based on String Name
                string[] unsortedChoices = new string[list.Length];
                for (int i = 0; i < list.Length; i++)
                    unsortedChoices[i] = inStrings[list[i]];

                string[] sortedChoices = new string[unsortedChoices.Length];
                Array.Copy(unsortedChoices, sortedChoices, unsortedChoices.Length);
                Array.Sort(sortedChoices);

                // Add the rest of the items
                cbList.AddRange(sortedChoices.Select(t => new cbItem
                {
                    Text = t, 
                    Value = list[Array.IndexOf(unsortedChoices, t)]
                }));
            }
            return cbList;
        }
        internal static List<cbItem> getOffsetCBList(List<cbItem> cbList, string[] inStrings, int offset, int[] allowed)
        {
            if (allowed == null)
                allowed = Enumerable.Range(0, inStrings.Length).ToArray();

            int[] list = (int[])allowed.Clone();
            for (int i = 0; i < list.Length; i++)
                list[i] -= offset;

            {
                // Sort the Rest based on String Name
                string[] unsortedChoices = new string[allowed.Length];
                for (int i = 0; i < allowed.Length; i++)
                    unsortedChoices[i] = inStrings[list[i]];

                string[] sortedChoices = new string[unsortedChoices.Length];
                Array.Copy(unsortedChoices, sortedChoices, unsortedChoices.Length);
                Array.Sort(sortedChoices);

                // Add the rest of the items
                cbList.AddRange(sortedChoices.Select(s => new cbItem
                {
                    Text = s, Value = allowed[Array.IndexOf(unsortedChoices, s)]
                }));
            }
            return cbList;
        }
        internal static List<cbItem> getVariedCBList(List<cbItem> cbList, string[] inStrings, int[] stringNum, int[] stringVal)
        {
            // Set up
            List<cbItem> newlist = new List<cbItem>();

            for (int i = 4; i > 1; i--) // add 4,3,2
            {
                // First 3 Balls are always first
                cbItem ncbi = new cbItem
                {
                    Text = inStrings[i], 
                    Value = i
                };
                newlist.Add(ncbi);
            }

            // Sort the Rest based on String Name
            string[] ballnames = new string[stringNum.Length];
            for (int i = 0; i < stringNum.Length; i++)
                ballnames[i] = inStrings[stringNum[i]];

            string[] sortedballs = new string[stringNum.Length];
            Array.Copy(ballnames, sortedballs, ballnames.Length);
            Array.Sort(sortedballs);

            // Add the rest of the balls
            newlist.AddRange(sortedballs.Select(t => new cbItem
            {
                Text = t, 
                Value = stringVal[Array.IndexOf(ballnames, t)]
            }));
            return newlist;
        }
        internal static List<cbItem> getUnsortedCBList(string textfile)
        {
            // Set up
            List<cbItem> cbList = new List<cbItem>();
            string[] inputCSV = getSimpleStringList(textfile);

            // Gather our data from the input file
            for (int i = 1; i < inputCSV.Length; i++)
            {
                string[] inputData = inputCSV[i].Split(',');
                cbItem ncbi = new cbItem 
                {
                    Text = inputData[1], 
                    Value = Convert.ToInt32(inputData[0])
                };
                cbList.Add(ncbi);
            }
            return cbList;
        }

     /*   // QR Utility
        internal static byte[] getQRData()
        {
            // Fetch data from QR code...
            string address;
            try { address = Clipboard.GetText(); }
            catch { Alert("No text (url) in clipboard."); return null; }
            try { if (address.Length < 4 || address.Substring(0, 3) != "htt") { Alert("Clipboard text is not a valid URL:", address); return null; } }
            catch { Alert("Clipboard text is not a valid URL:", address); return null; }
            string webURL = "http://api.qrserver.com/v1/read-qr-code/?fileurl=" + System.Web.HttpUtility.UrlEncode(address);
            try
            {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(webURL);
                System.Net.HttpWebResponse httpWebReponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                var reader = new StreamReader(httpWebReponse.GetResponseStream());
                string data = reader.ReadToEnd();
                if (data.Contains("could not find")) { Alert("Reader could not find QR data in the image."); return null; }
                if (data.Contains("filetype not supported")) { Alert("Input URL is not valid. Double check that it is an image (jpg/png).", address); return null; }
                // Quickly convert the json response to a data string
                string pkstr = data.Substring(data.IndexOf("#", StringComparison.Ordinal) + 1); // Trim intro
                pkstr = pkstr.Substring(0, pkstr.IndexOf("\",\"error\":null}]}]", StringComparison.Ordinal)); // Trim outro
                if (pkstr.Contains("nQR-Code:")) pkstr = pkstr.Substring(0, pkstr.IndexOf("nQR-Code:", StringComparison.Ordinal)); //  Remove multiple QR codes in same image
                pkstr = pkstr.Replace("\\", ""); // Rectify response

                try { return Convert.FromBase64String(pkstr); }
                catch { Alert("QR string to Data failed.", pkstr); return null; }
            }
            catch { Alert("Unable to connect to the internet to decode QR code."); return null;}
        }
        internal static Image getQRImage(byte[] data, string server)
        {
            string qrdata = Convert.ToBase64String(data);
            string message = server + qrdata;
            string webURL = "http://chart.apis.google.com/chart?chs=365x365&cht=qr&chl=" + System.Web.HttpUtility.UrlEncode(message);

            try
            {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(webURL);
                System.Net.HttpWebResponse httpWebReponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                Stream stream = httpWebReponse.GetResponseStream();
                if (stream != null) return Image.FromStream(stream);
            }
            catch
            {
                if (Prompt(MessageBoxButtons.YesNo, 
                        "Unable to connect to the internet to receive QR code.",
                        "Copy QR URL to Clipboard?")
                        != DialogResult.Yes) return null;
                try { Clipboard.SetText(webURL); }
                catch { Alert("Failed to set text to Clipboard"); }
            }
            return null;
        }*/
    }
}