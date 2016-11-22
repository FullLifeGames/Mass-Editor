﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PKHeX.Properties;

namespace PKHeX
{
    public partial class KChart : Form
    {
        private readonly string[] species = Main.GameStrings.specieslist;
        private readonly string[] abilities = Main.GameStrings.abilitylist;

        private readonly bool alolanOnly = Main.SAV.Generation == 7 && DialogResult.Yes == Util.Prompt(MessageBoxButtons.YesNo, "Alolan Dex only?");
        private static int[] baseForm;
        private static int[] formVal;
        public KChart()
        {
            InitializeComponent();

            Array.Resize(ref species, Main.SAV.Personal.TableLength);

            var AltForms = Main.SAV.Personal.getFormList(species, Main.SAV.MaxSpeciesID);
            species = Main.SAV.Personal.getPersonalEntryList(AltForms, species, Main.SAV.MaxSpeciesID, out baseForm, out formVal);

            DGV.Rows.Clear();
            for (int i = 1; i < species.Length; i++)
                popEntry(i);

            DGV.Sort(DGV.Columns[0], ListSortDirection.Ascending);
        }

        private void popEntry(int index)
        {
            var p = Main.SAV.Personal[index];

            int s = index > Main.SAV.MaxSpeciesID ? baseForm[index] : index;
            var f = index <= Main.SAV.MaxSpeciesID ? 0 : formVal[index];
            bool alolan = s > 721 || Legal.PastGenAlolanNatives.Contains(s);

            if (alolanOnly && !alolan)
                return;

            var row = new DataGridViewRow();
            row.CreateCells(DGV);

            int r = 0;
            row.Cells[r++].Value = s.ToString("000") + (f > 0 ? "-"+f.ToString("00") :"");
            row.Cells[r++].Value = PKX.getSprite(s, f, 0, 0, false, false, Main.SAV.Generation);
            row.Cells[r++].Value = species[index];
            row.Cells[r++].Value = s > 721 || Legal.PastGenAlolanNatives.Contains(s);
            row.Cells[r].Style.BackColor = mapColor((int)((p.BST - 175) / 3f));
            row.Cells[r++].Value = p.BST.ToString("000");
            row.Cells[r++].Value = (Image)Mass_Editor.Properties.Resources.ResourceManager.GetObject("type_icon_" + p.Types[0].ToString("00"));
            row.Cells[r++].Value = p.Types[0] == p.Types[1] ? Mass_Editor.Properties.Resources.slotTrans : (Image)Mass_Editor.Properties.Resources.ResourceManager.GetObject("type_icon_" + p.Types[1].ToString("00"));
            row.Cells[r].Style.BackColor = mapColor(p.HP);
            row.Cells[r++].Value = p.HP.ToString("000");
            row.Cells[r].Style.BackColor = mapColor(p.ATK);
            row.Cells[r++].Value = p.ATK.ToString("000");
            row.Cells[r].Style.BackColor = mapColor(p.DEF);
            row.Cells[r++].Value = p.DEF.ToString("000");
            row.Cells[r].Style.BackColor = mapColor(p.SPA);
            row.Cells[r++].Value = p.SPA.ToString("000");
            row.Cells[r].Style.BackColor = mapColor(p.SPD);
            row.Cells[r++].Value = p.SPD.ToString("000");
            row.Cells[r].Style.BackColor = mapColor(p.SPE);
            row.Cells[r++].Value = p.SPE.ToString("000");
            row.Cells[r++].Value = abilities[p.Abilities[0]];
            row.Cells[r++].Value = abilities[p.Abilities[1]];
            row.Cells[r++].Value = abilities[p.Abilities[2]];
            DGV.Rows.Add(row);
        }
        private static Color mapColor(int v)
        {
            const float maxval = 180; // shift the green cap down
            float x = 100f * v / maxval;
            if (x > 100)
                x = 100;
            double red = 255f * (x > 50 ? 1 - 2 * (x - 50) / 100.0 : 1.0);
            double green = 255f * (x > 50 ? 1.0 : 2 * x / 100.0);

            return Blend(Color.FromArgb((int)red, (int)green, 0), Color.White, 0.4);
        }
        public static Color Blend(Color color, Color backColor, double amount)
        {
            byte r = (byte)(color.R * amount + backColor.R * (1 - amount));
            byte g = (byte)(color.G * amount + backColor.G * (1 - amount));
            byte b = (byte)(color.B * amount + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }
    }
}
