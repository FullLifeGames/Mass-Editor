﻿using System;
using System.Linq;
using System.Text;

namespace PKHeX
{
    public sealed class SAV7 : SaveFile
    {
        // Save Data Attributes
        public override string BAKName => $"{FileName} [{OT} ({Version}) - {LastSavedTime}].bak";
        public override string Filter => "Main SAV|*.*";
        public override string Extension => "";
        public SAV7(byte[] data = null)
        {
            Data = data == null ? new byte[SaveUtil.SIZE_G7SM] : (byte[])data.Clone();
            BAK = (byte[])Data.Clone();
            Exportable = !Data.SequenceEqual(new byte[Data.Length]);

            // Load Info
            getBlockInfo();
            getSAVOffsets();
            
            HeldItems = Legal.HeldItems_SM;
            Personal = PersonalTable.SM;
            if (!Exportable)
                resetBoxes();

            var demo = new byte[0x4C4].SequenceEqual(Data.Skip(PCLayout).Take(0x4C4));
            if (demo)
            {
                PokeDex = -1; // Disabled
            }
        }

        // Configuration
        public override SaveFile Clone() { return new SAV7(Data); }
        
        public override int SIZE_STORED => PKX.SIZE_6STORED;
        public override int SIZE_PARTY => PKX.SIZE_6PARTY;
        public override PKM BlankPKM => new PK7();
        public override Type PKMType => typeof(PK7);

        public override int BoxCount => 32;
        public override int MaxEV => 252;
        public override int Generation => 7;
        protected override int GiftCountMax => 48;
        protected override int GiftFlagMax => 0x100 * 8;
        protected override int EventFlagMax => 3968;
        protected override int EventConstMax => (EventFlag - EventConst) / 2;
        public override int OTLength => 12;
        public override int NickLength => 12;

        public override int MaxMoveID => 720;
        public override int MaxSpeciesID => Legal.MaxSpeciesID_7;
        public override int MaxItemID => 920;
        public override int MaxAbilityID => 232;
        public override int MaxBallID => 0x1A; // 26
        public override int MaxGameID => 31; // MN

        public int QRSaveData;

        // Feature Overrides
        public override bool HasGeolocation => true;

        // Blocks & Offsets
        private int BlockInfoOffset;
        private BlockInfo[] Blocks;
        private void getBlockInfo()
        {
            BlockInfoOffset = Data.Length - 0x200 + 0x10;
            if (BitConverter.ToUInt32(Data, BlockInfoOffset) != SaveUtil.BEEF)
                BlockInfoOffset -= 0x200; // No savegames have more than 0x3D blocks, maybe in the future?
            int count = (Data.Length - BlockInfoOffset - 0x8) / 8;
            BlockInfoOffset += 4;

            Blocks = new BlockInfo[count];
            int CurrentPosition = 0;
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = new BlockInfo
                {
                    Offset = CurrentPosition,
                    Length = BitConverter.ToInt32(Data, BlockInfoOffset + 0 + 8 * i),
                    ID = BitConverter.ToUInt16(Data, BlockInfoOffset + 4 + 8 * i),
                    Checksum = BitConverter.ToUInt16(Data, BlockInfoOffset + 6 + 8 * i)
                };

                // Expand out to nearest 0x200
                CurrentPosition += Blocks[i].Length % 0x200 == 0 ? Blocks[i].Length : 0x200 - Blocks[i].Length % 0x200 + Blocks[i].Length;

                if ((Blocks[i].ID != 0) || i == 0) continue;
                count = i;
                break;
            }
            // Fix Final Array Lengths
            Array.Resize(ref Blocks, count);
        }
        protected override void setChecksums()
        {
            // Check for invalid block lengths
            if (Blocks.Length < 3) // arbitrary...
            {
                Console.WriteLine("Not enough blocks ({0}), aborting setChecksums", Blocks.Length);
                return;
            }
            // Apply checksums
            for (int i = 0; i < Blocks.Length; i++)
            {
                byte[] array = new byte[Blocks[i].Length];
                Array.Copy(Data, Blocks[i].Offset, array, 0, array.Length);
                BitConverter.GetBytes(SaveUtil.check16(array, Blocks[i].ID)).CopyTo(Data, BlockInfoOffset + 6 + i * 8);
            }
            
            Data = SaveUtil.Resign7(Data);
        }
        public override bool ChecksumsValid
        {
            get
            {
                for (int i = 0; i < Blocks.Length; i++)
                {
                    byte[] array = new byte[Blocks[i].Length];
                    Array.Copy(Data, Blocks[i].Offset, array, 0, array.Length);
                    if (SaveUtil.check16(array, Blocks[i].ID) != BitConverter.ToUInt16(Data, BlockInfoOffset + 6 + i * 8))
                        return false;
                }
                return true;
            }
        }
        public override string ChecksumInfo
        {
            get
            {
                int invalid = 0;
                string rv = "";
                for (int i = 0; i < Blocks.Length; i++)
                {
                    byte[] array = new byte[Blocks[i].Length];
                    Array.Copy(Data, Blocks[i].Offset, array, 0, array.Length);
                    if (SaveUtil.check16(array, Blocks[i].ID) == BitConverter.ToUInt16(Data, BlockInfoOffset + 6 + i * 8))
                        continue;

                    invalid++;
                    rv += $"Invalid: {i.ToString("X2")} @ Region {Blocks[i].Offset.ToString("X5") + Environment.NewLine}";
                }
                // Return Outputs
                rv += $"SAV: {Blocks.Length - invalid}/{Blocks.Length + Environment.NewLine}";
                return rv;
            }
        }
        public override ulong? Secure1
        {
            get { return BitConverter.ToUInt64(Data, BlockInfoOffset - 0x14); }
            set { BitConverter.GetBytes(value ?? 0).CopyTo(Data, BlockInfoOffset - 0x14); }
        }
        public override ulong? Secure2
        {
            get { return BitConverter.ToUInt64(Data, BlockInfoOffset - 0xC); }
            set { BitConverter.GetBytes(value ?? 0).CopyTo(Data, BlockInfoOffset - 0xC); }
        }

        private void getSAVOffsets()
        {
            if (SM)
            {
                /* 00 */ Item           = 0x00000;  // [DE0]    MyItem
                /* 01 */ Trainer1       = 0x00E00;  // [07C]    Situation
                /* 02 */            //  = 0x01000;  // [014]    RandomGroup
                /* 03 */ TrainerCard    = 0x01200;  // [0C0]    MyStatus
                /* 04 */ Party          = 0x01400;  // [61C]    PokePartySave
                /* 05 */ EventConst     = 0x01C00;  // [E00]    EventWork
                /* 06 */ PokeDex        = 0x02A00;  // [F78]    ZukanData
                /* 07 */ GTS            = 0x03A00;  // [228]    GtsData
                /* 08 */ Fused          = 0x03E00;  // [104]    UnionPokemon 
                /* 09 */ Misc           = 0x04000;  // [200]    Misc
                /* 10 */ Trainer2       = 0x04200;  // [020]    FieldMenu
                /* 11 */            //  = 0x04400;  // [004]    ConfigSave
                /* 12 */ AdventureInfo  = 0x04600;  // [058]    GameTime
                /* 13 */ PCLayout       = 0x04800;  // [5E6]    BOX
                /* 14 */ Box            = 0x04E00;  // [36600]  BoxPokemon
                /* 15 */ Resort         = 0x3B400;  // [572C]   ResortSave
                /* 16 */ PlayTime       = 0x40C00;  // [008]    PlayTime
                /* 17 */ Overworld      = 0x40E00;  // [1080]   FieldMoveModelSave
                /* 18 */            //  = 0x42000;  // [1A08]   Fashion
                /* 19 */            //  = 0x43C00;  // [6408]   JoinFestaPersonalSave
                /* 20 */            //  = 0x4A200;  // [6408]   JoinFestaPersonalSave
                /* 21 */ JoinFestaData  = 0x50800;  // [3998]   JoinFestaDataSave
                /* 22 */            //  = 0x54200;  // [100]    BerrySpot
                /* 23 */            //  = 0x54400;  // [100]    FishingSpot
                /* 24 */            //  = 0x54600;  // [10528]  LiveMatchData
                /* 25 */            //  = 0x64C00;  // [204]    BattleSpotData
                /* 26 */            //  = 0x65000;  // [B60]    PokeFinderSave
                /* 27 */ WondercardFlags = 0x65C00; // [3F50]   MysteryGiftSave
                /* 28 */            //  = 0x69C00;  // [358]    Record
                /* 29 */            //  = 0x6A000;  // [728]    Data Block
                /* 30 */            //  = 0x6A800;  // [200]    GameSyncSave
                /* 31 */            //  = 0x6AA00;  // [718]    PokeDiarySave
                /* 32 */            //  = 0x6B200;  // [1FC]    BattleInstSave
                /* 33 */ Daycare        = 0x6B400;  // [200]    Sodateya
                /* 34 */            //  = 0x6B600;  // [120]    WeatherSave
                /* 35 */ QRSaveData     = 0x6B800;  // [1C8]    QRReaderSaveData
                /* 36 */            //  = 0x6BA00;  // [200]    TurtleSalmonSave

                EventFlag = EventConst + 0x7D0;

                OFS_PouchHeldItem =     Item + 0; // 430 (Case 0)
                OFS_PouchKeyItem =      Item + 0x6B8; // 184 (Case 4)
                OFS_PouchTMHM =         Item + 0x998; // 108 (Case 2)
                OFS_PouchMedicine =     Item + 0xB48; // 64 (Case 1)
                OFS_PouchBerry =        Item + 0xC48; // 72 (Case 3)
                OFS_PouchZCrystals =    Item + 0xD68; // 30 (Case 5)

                PokeDexLanguageFlags =  PokeDex + 0x550;
                WondercardData = WondercardFlags + 0x100;

                PCBackgrounds =         PCLayout + 0x5C0;
                LastViewedBox =         PCLayout + 0x5E3;
                PCFlags =               PCLayout + 0x5E0;
            }
            else // Empty input
            {
                Party = 0x0;
                Box = Party + SIZE_PARTY * 6 + 0x1000;
            }
        }

        // Private Only
        private int Item { get; set; } = int.MinValue;
        private int AdventureInfo { get; set; } = int.MinValue;
        private int Trainer2 { get; set; } = int.MinValue;
        private int Misc { get; set; } = int.MinValue;
        private int LastViewedBox { get; set; } = int.MinValue;
        private int WondercardFlags { get; set; } = int.MinValue;
        private int PlayTime { get; set; } = int.MinValue;
        private int ItemInfo { get; set; } = int.MinValue;
        private int Overworld { get; set; } = int.MinValue;
        private int JoinFestaData { get; set; } = int.MinValue;

        // Accessible as SAV7
        public int TrainerCard { get; private set; } = 0x14000;
        public int Resort { get; set; }
        public int PCFlags { get; private set; } = int.MinValue;
        public int PSSStats { get; private set; } = int.MinValue;
        public int MaisonStats { get; private set; } = int.MinValue;
        public int PCBackgrounds { get; private set; } = int.MinValue;
        public int Contest { get; private set; } = int.MinValue;
        public int Accessories { get; private set; } = int.MinValue;
        public int PokeDexLanguageFlags { get; private set; } = int.MinValue;

        private const int ResortCount = 93;
        public PKM[] ResortPKM
        {
            get
            {
                PKM[] data = new PKM[ResortCount];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = getPKM(getData(Resort + 0x12 + i * SIZE_STORED, SIZE_STORED));
                    data[i].Identifier = $"Resort Slot {i}";
                }
                return data;
            }
            set
            {
                if (value?.Length != ResortCount)
                    throw new ArgumentException();

                for (int i = 0; i < value.Length; i++)
                    setStoredSlot(value[i], Resort + 0x12 + i*SIZE_STORED);
            }
        }

        public override GameVersion Version
        {
            get
            {
                switch (Game)
                {
                    case 30: return GameVersion.SN;
                    case 31: return GameVersion.MN;
                }
                return GameVersion.Unknown;
            }
        }
        
        // Player Information
        public override ushort TID
        {
            get { return BitConverter.ToUInt16(Data, TrainerCard + 0); }
            set { BitConverter.GetBytes(value).CopyTo(Data, TrainerCard + 0); }
        }
        public override ushort SID
        {
            get { return BitConverter.ToUInt16(Data, TrainerCard + 2); }
            set { BitConverter.GetBytes(value).CopyTo(Data, TrainerCard + 2); }
        }
        public override int Game
        {
            get { return Data[TrainerCard + 4]; }
            set { Data[TrainerCard + 4] = (byte)value; }
        }
        public override int Gender
        {
            get { return Data[TrainerCard + 5]; }
            set { Data[TrainerCard + 5] = (byte)value; }
        }
        public override ulong? GameSyncID
        {
            get { return BitConverter.ToUInt64(Data, TrainerCard + 0x18); }
            set { BitConverter.GetBytes(value ?? 0).CopyTo(Data, TrainerCard + 0x18); }
        }
        public override int SubRegion
        {
            get { return Data[TrainerCard + 0x2E]; }
            set { Data[TrainerCard + 0x2E] = (byte)value; }
        }
        public override int Country
        {
            get { return Data[TrainerCard + 0x2F]; }
            set { Data[TrainerCard + 0x2F] = (byte)value; }
        }
        public override int ConsoleRegion
        {
            get { return Data[TrainerCard + 0x34]; }
            set { Data[TrainerCard + 0x34] = (byte)value; }
        }
        public override int Language
        {
            get { return Data[TrainerCard + 0x35]; }
            set { Data[TrainerCard + 0x35] = (byte)value; }
        }
        public override string OT
        {
            get { return Util.TrimFromZero(Encoding.Unicode.GetString(Data, TrainerCard + 0x38, 0x1A)); }
            set { Encoding.Unicode.GetBytes(value.PadRight(13, '\0')).CopyTo(Data, TrainerCard + 0x38); }
        }
        public int M
        {
            get { return BitConverter.ToUInt16(Data, Trainer1 + 0x00); } // could be anywhere 0x0-0x7
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, Trainer1 + 0x00); }
        }
        public float X
        {
            get { return BitConverter.ToSingle(Data, Trainer1 + 0x08); }
            set
            {
                BitConverter.GetBytes(value).CopyTo(Data, Trainer1 + 0x08);
                BitConverter.GetBytes(value).CopyTo(Data, Overworld + 0x08);
            }
        }
        public float Z
        {
            get { return BitConverter.ToSingle(Data, Trainer1 + 0x10); }
            set
            {
                BitConverter.GetBytes(value).CopyTo(Data, Trainer1 + 0x10);
                BitConverter.GetBytes(value).CopyTo(Data, Overworld + 0x10);
            }
        }
        public float Y
        {
            get { return (int)BitConverter.ToSingle(Data, Trainer1 + 0x18); }
            set
            {
                BitConverter.GetBytes(value).CopyTo(Data, Trainer1 + 0x18);
                BitConverter.GetBytes(value).CopyTo(Data, Overworld + 0x18);
            }
        }
        public float R
        {
            get { return (int)BitConverter.ToSingle(Data, Trainer1 + 0x20); }
            set
            {
                BitConverter.GetBytes(value).CopyTo(Data, Trainer1 + 0x20);
                BitConverter.GetBytes(value).CopyTo(Data, Overworld + 0x20);
            }
        }

        public override uint Money
        {
            get { return BitConverter.ToUInt32(Data, Misc + 0x4); }
            set
            {
                if (value > 9999999) value = 9999999;
                BitConverter.GetBytes(value).CopyTo(Data, Misc + 0x4);
            }
        }
        public uint BP
        {
            get { return BitConverter.ToUInt32(Data, Misc + 0x11C); }
            set
            {
                if (value > 9999) value = 9999;
                BitConverter.GetBytes(value).CopyTo(Data, Misc + 0x11C);
            }
        }
        public uint FestaCoins
        {
            get { return BitConverter.ToUInt32(Data, JoinFestaData + 0x50C); }
            set
            {
                if (value > 9999999) value = 9999999;
                BitConverter.GetBytes(value).CopyTo(Data, JoinFestaData + 0x50C);

                if (TotalFestaCoins < value)
                    TotalFestaCoins = value;
            }
        }
        private uint TotalFestaCoins
        {
            get { return BitConverter.ToUInt32(Data, JoinFestaData + 0x510); }
            set
            {
                if (value > 9999999) value = 9999999;
                BitConverter.GetBytes(value).CopyTo(Data, JoinFestaData + 0x510);
            }
        }

        public override int PlayedHours
        { 
            get { return BitConverter.ToUInt16(Data, PlayTime); } 
            set { BitConverter.GetBytes((ushort)value).CopyTo(Data, PlayTime); } 
        }
        public override int PlayedMinutes
        {
            get { return Data[PlayTime + 2]; }
            set { Data[PlayTime + 2] = (byte)value; } 
        }
        public override int PlayedSeconds
        {
            get { return Data[PlayTime + 3]; }
            set { Data[PlayTime + 3] = (byte)value; }
        }
        public uint LastSaved { get { return BitConverter.ToUInt32(Data, PlayTime + 0x4); } set { BitConverter.GetBytes(value).CopyTo(Data, PlayTime + 0x4); } }
        public int LastSavedYear { get { return (int)(LastSaved & 0xFFF); } set { LastSaved = LastSaved & 0xFFFFF000 | (uint)value; } }
        public int LastSavedMonth { get { return (int)(LastSaved >> 12 & 0xF); } set { LastSaved = LastSaved & 0xFFFF0FFF | ((uint)value & 0xF) << 12; } }
        public int LastSavedDay { get { return (int)(LastSaved >> 16 & 0x1F); } set { LastSaved = LastSaved & 0xFFE0FFFF | ((uint)value & 0x1F) << 16; } }
        public int LastSavedHour { get { return (int)(LastSaved >> 21 & 0x1F); } set { LastSaved = LastSaved & 0xFC1FFFFF | ((uint)value & 0x1F) << 21; } }
        public int LastSavedMinute { get { return (int)(LastSaved >> 26 & 0x3F); } set { LastSaved = LastSaved & 0x03FFFFFF | ((uint)value & 0x3F) << 26; } }
        public string LastSavedTime => $"{LastSavedYear.ToString("0000")}{LastSavedMonth.ToString("00")}{LastSavedDay.ToString("00")}{LastSavedHour.ToString("00")}{LastSavedMinute.ToString("00")}";

        public int ResumeYear { get { return BitConverter.ToInt32(Data, AdventureInfo + 0x4); } set { BitConverter.GetBytes(value).CopyTo(Data,AdventureInfo + 0x4); } }
        public int ResumeMonth { get { return Data[AdventureInfo + 0x8]; } set { Data[AdventureInfo + 0x8] = (byte)value; } }
        public int ResumeDay { get { return Data[AdventureInfo + 0x9]; } set { Data[AdventureInfo + 0x9] = (byte)value; } }
        public int ResumeHour { get { return Data[AdventureInfo + 0xB]; } set { Data[AdventureInfo + 0xB] = (byte)value; } }
        public int ResumeMinute { get { return Data[AdventureInfo + 0xC]; } set { Data[AdventureInfo + 0xC] = (byte)value; } }
        public int ResumeSeconds { get { return Data[AdventureInfo + 0xD]; } set { Data[AdventureInfo + 0xD] = (byte)value; } }
        public override int SecondsToStart { get { return BitConverter.ToInt32(Data, AdventureInfo + 0x28); } set { BitConverter.GetBytes(value).CopyTo(Data, AdventureInfo + 0x28); } }
        public override int SecondsToFame { get { return BitConverter.ToInt32(Data, AdventureInfo + 0x30); } set { BitConverter.GetBytes(value).CopyTo(Data, AdventureInfo + 0x30); } }
        
        public ulong AlolaTime { get { return BitConverter.ToUInt64(Data, AdventureInfo + 0x48); } set { BitConverter.GetBytes(value).CopyTo(Data, AdventureInfo+0x48);} }

        // Inventory
        public override InventoryPouch[] Inventory
        {
            get
            {
                InventoryPouch[] pouch =
                {
                    new InventoryPouch(InventoryType.Medicine, Legal.Pouch_Medicine_SM, 999, OFS_PouchMedicine),
                    new InventoryPouch(InventoryType.Items, Legal.Pouch_Items_SM, 999, OFS_PouchHeldItem),
                    new InventoryPouch(InventoryType.TMHMs, Legal.Pouch_TMHM_SM, 1, OFS_PouchTMHM),
                    new InventoryPouch(InventoryType.Berries, Legal.Pouch_Berries_SM, 999, OFS_PouchBerry),
                    new InventoryPouch(InventoryType.KeyItems, Legal.Pouch_Key_SM, 1, OFS_PouchKeyItem),
                    new InventoryPouch(InventoryType.ZCrystals, Legal.Pouch_ZCrystal_SM, 1, OFS_PouchZCrystals),
                };
                foreach (var p in pouch)
                    p.getPouch7(ref Data);
                return pouch;
            }
            set
            {
                foreach (var p in value)
                    p.setPouch7(ref Data);
            }
        }

        // Resort Save
        public int GetPokebeanCount(int bean_id)
        {
            if (bean_id < 0 || bean_id > 14)
                throw new ArgumentException("Invalid bean id!");
            return Data[Resort + 0x564C + bean_id];
        }

        public void SetPokebeanCount(int bean_id, int count)
        {
            if (bean_id < 0 || bean_id > 14)
                throw new ArgumentException("Invalid bean id!");
            if (count < 0)
                count = 0;
            if (count > 255)
                count = 255;
            Data[Resort + 0x564C + bean_id] = (byte) count;
        }

        // Storage
        public override int CurrentBox { get { return Data[LastViewedBox]; } set { Data[LastViewedBox] = (byte)value; } }
        public override int getPartyOffset(int slot)
        {
            return Party + SIZE_PARTY * slot;
        }
        public override int getBoxOffset(int box)
        {
            return Box + SIZE_STORED*box*30;
        }
        protected override int getBoxWallpaperOffset(int box)
        {
            int ofs = PCBackgrounds > 0 && PCBackgrounds < Data.Length ? PCBackgrounds : -1;
            if (ofs > -1)
                return ofs + box;
            return ofs;
        }
        public override void setBoxWallpaper(int box, int value)
        {
            if (PCBackgrounds < 0)
                return;
            int ofs = PCBackgrounds > 0 && PCBackgrounds < Data.Length ? PCBackgrounds : 0;
            Data[ofs + box] = (byte)value;
        }
        public override string getBoxName(int box)
        {
            if (PCLayout < 0)
                return "B" + (box + 1);
            return Util.TrimFromZero(Encoding.Unicode.GetString(Data, PCLayout + 0x22*box, 0x22));
        }
        public override void setBoxName(int box, string val)
        {
            Encoding.Unicode.GetBytes(val.PadRight(0x11, '\0')).CopyTo(Data, PCLayout + 0x22*box);
            Edited = true;
        }
        public override PKM getPKM(byte[] data)
        {
            return new PK7(data);
        }
        protected override void setPKM(PKM pkm)
        {
            PK7 pk7 = pkm as PK7;
            // Apply to this Save File
            int CT = pk7.CurrentHandler;
            DateTime Date = DateTime.Now;
            pk7.Trade(OT, TID, SID, Country, SubRegion, Gender, false, Date.Day, Date.Month, Date.Year);
            if (CT != pk7.CurrentHandler) // Logic updated Friendship
            {
                // Copy over the Friendship Value only under certain circumstances
                if (pk7.Moves.Contains(216)) // Return
                    pk7.CurrentFriendship = pk7.OppositeFriendship;
                else if (pk7.Moves.Contains(218)) // Frustration
                    pkm.CurrentFriendship = pk7.OppositeFriendship;
                else if (pk7.CurrentHandler == 1) // OT->HT, needs new Friendship/Affection
                    pk7.TradeFriendshipAffection(OT);
            }
            pkm.RefreshChecksum();
        }
        protected override void setDex(PKM pkm)
        {
            if (PokeDex < 0 || Version == GameVersion.Unknown) // sanity
                return;
            if (pkm.Species == 0 || pkm.Species > MaxSpeciesID) // out of range
                return;
            if (pkm.IsEgg) // do not add
                return;

            int bit = pkm.Species - 1;
            int bd = bit >> 3; // div8
            int bm = bit & 7; // mod8
            int gender = pkm.Gender % 2; // genderless -> male
            int shiny = pkm.IsShiny ? 1 : 0;
            if (pkm.Species == 351) // castform
                shiny = 0;
            int shift = gender | (shiny << 1);
            if (pkm.Species == 327) // Spinda
            {
                if ((Data[PokeDex + 0x84] & (1 << (shift + 4))) != 0) // Already 2
                {
                    BitConverter.GetBytes(pkm.EncryptionConstant).CopyTo(Data, PokeDex + 0x8E8 + shift * 4);
                    // Data[PokeDex + 0x84] |= (byte)(1 << (shift + 4)); // 2 -- pointless
                    Data[PokeDex + 0x84] |= (byte)(1 << shift); // 1
                }
                else if ((Data[PokeDex + 0x84] & (1 << shift)) == 0) // Not yet 1
                {
                    Data[PokeDex + 0x84] |= (byte)(1 << shift); // 1
                }
            }
            int ofs = PokeDex // Raw Offset
                      + 0x08 // Magic + Flags
                      + 0x80; // Misc Data (1024 bits)
            // Set the Owned Flag
            Data[ofs + bd] |= (byte)(1 << bm);

            // Starting with Gen7, form bits are stored in the same region as the species flags.

            int formstart = pkm.AltForm;
            int formend = pkm.AltForm;
            int fs, fe;
            bool reset = sanitizeFormsToIterate(pkm.Species, out fs, out fe, formstart);
            if (reset)
            {
                formstart = fs;
                formend = fe;
            }

            for (int form = formstart; form <= formend; form++)
            {
                int bitIndex = bit;
                if (form > 0) // Override the bit to overwrite
                {
                    int fc = Personal[pkm.Species].FormeCount;
                    if (fc > 1) // actually has forms
                    {
                        int f = SaveUtil.getDexFormIndexSM(pkm.Species, fc, MaxSpeciesID - 1);
                        if (f >= 0) // bit index valid
                            bitIndex = f + form;
                    }
                }
                setDexFlags(bitIndex, gender, shiny, pkm.Species - 1);
            }

            // Set the Language
            int lang = pkm.Language;
            const int langCount = 9;
            if (lang <= 10 && lang != 6 && lang != 0) // valid language
            {
                if (lang >= 7)
                    lang--;
                lang--; // 0-8 languages
                if (lang < 0) lang = 1;
                int lbit = bit * langCount + lang;
                if (lbit >> 3 < 920) // Sanity check for max length of region
                    Data[PokeDexLanguageFlags + (lbit >> 3)] |= (byte)(1 << (lbit & 7));
            }
        }
        private static bool sanitizeFormsToIterate(int species, out int formStart, out int formEnd, int formIn)
        {
            // 004AA370 in Moon
            // Simplified in terms of usage -- only overrides to give all the battle forms for a pkm
            formStart = 0;
            formEnd = 0;
            switch (species)
            {
                case 351: // Castform
                    formStart = 0;
                    formEnd = 3;
                    return true;
                case 421: // Cherrim
                case 555: // Darmanitan
                case 648: // Meloetta
                case 746: // Wishiwashi
                case 778: // Mimikyu
                    formStart = 0;
                    formEnd = 1;
                    return true;

                case 774: // Minior
                    // Cores forms are after Meteor forms, so the game iterator would give all meteor forms (NO!)
                    // So the game so the game chooses to only award entries for Core forms after they appear in battle.
                    return formIn > 6; // resets to 0/0 if an invalid request is made (non-form entry)
                    
                case 718:
                    if (formIn == 3) // complete
                        return true; // 0/0
                    if (formIn != 2) // give
                        return false;

                    // Apparently form 2 is invalid (50% core-ability), set to 10%'s form
                    formStart = 1; 
                    formEnd = 1;
                    return true;
                default:
                    return false;
            }
        }
        private void setDexFlags(int index, int gender, int shiny, int baseSpecies)
        {
            const int brSize = 0x8C;
            int shift = gender | (shiny << 1);
            int ofs = PokeDex // Raw Offset
                      + 0x08 // Magic + Flags
                      + 0x80 // Misc Data (1024 bits)
                      + 0x68; // Owned Flags

            int bd = index >> 3; // div8
            int bm = index & 7; // mod8
            int bd1 = baseSpecies >> 3;
            int bm1 = baseSpecies & 7;
            // Set the [Species/Gender/Shiny] Seen Flag
            int brSeen = shift * brSize;
            Data[ofs + brSeen + bd] |= (byte)(1 << bm);

            // Check Displayed Status for base form
            bool Displayed = false;
            for (int i = 0; i < 4; i++)
            {
                int brDisplayed = (4 + i) * brSize;
                Displayed |= (Data[ofs + brDisplayed + bd1] & (byte)(1 << bm1)) != 0;
            }

            // If form is not base form, check form too
            if (!Displayed && baseSpecies != index)
            {
                for (int i = 0; i < 4; i++)
                {
                    int brDisplayed = (4 + i) * brSize;
                    Displayed |= (Data[ofs + brDisplayed + bd] & (byte)(1 << bm)) != 0;
                }
            }
            if (Displayed)
                return;

            // Set the Display flag if none are set
            Data[ofs + (4 + shift) * brSize + bd] |= (byte)(1 << bm);
        }
        public override byte[] decryptPKM(byte[] data)
        {
            return PKX.decryptArray(data);
        }
        public override int PartyCount
        {
            get { return Data[Party + 6 * SIZE_PARTY]; }
            protected set { Data[Party + 6 * SIZE_PARTY] = (byte)value; }
        }
        public override int BoxesUnlocked { get { return Data[PCFlags + 1]; } set { Data[PCFlags + 1] = (byte)value; } }

        public override int DaycareSeedSize => 32; // 128 bits
        public override int getDaycareSlotOffset(int loc, int slot)
        {
            if (loc != 0)
                return -1;
            if (Daycare < 0)
                return -1;
            return Daycare + 1 + slot * (SIZE_STORED + 1);
        }
        public override bool? getDaycareOccupied(int loc, int slot)
        {
            if (loc != 0)
                return null;
            if (Daycare < 0)
                return null;

            return Data[Daycare + (SIZE_STORED + 1) * slot] != 0;
        }
        public override string getDaycareRNGSeed(int loc)
        {
            if (loc != 0)
                return null;
            if (Daycare < 0)
                return null;

            var data = Data.Skip(Daycare + 0x1DC).Take(DaycareSeedSize / 2).Reverse().ToArray();
            return BitConverter.ToString(data).Replace("-", "");
        }
        public override bool? getDaycareHasEgg(int loc)
        {
            if (loc != 0)
                return null;
            if (Daycare < 0)
                return null;

            return Data[Daycare + 0x1D8] == 1;
        }
        public override void setDaycareOccupied(int loc, int slot, bool occupied)
        {
            if (loc != 0)
                return;
            if (Daycare < 0)
                return;
            
            Data[Daycare + (SIZE_STORED + 1) * slot] = (byte)(occupied ? 1 : 0);
        }
        public override void setDaycareRNGSeed(int loc, string seed)
        {
            if (loc != 0)
                return;
            if (Daycare < 0)
                return;
            if (seed == null)
                return;
            if (seed.Length > DaycareSeedSize)
                return;

            Enumerable.Range(0, seed.Length)
                 .Where(x => x % 2 == 0)
                 .Reverse()
                 .Select(x => Convert.ToByte(seed.Substring(x, 2), 16))
                 .Reverse().ToArray().CopyTo(Data, Daycare + 0x1DC);
        }
        public override void setDaycareHasEgg(int loc, bool hasEgg)
        {
            if (loc != 0)
                return;
            if (Daycare < 0)
                return;

            Data[Daycare + 0x1D8] = (byte)(hasEgg ? 1 : 0);
        }

        // Mystery Gift
        protected override bool[] MysteryGiftReceivedFlags
        {
            get
            {
                if (WondercardData < 0 || WondercardFlags < 0)
                    return null;

                bool[] r = new bool[(WondercardData-WondercardFlags)*8];
                for (int i = 0; i < r.Length; i++)
                    r[i] = (Data[WondercardFlags + (i>>3)] >> (i&7) & 0x1) == 1;
                return r;
            }
            set
            {
                if (WondercardData < 0 || WondercardFlags < 0)
                    return;
                if ((WondercardData - WondercardFlags)*8 != value?.Length)
                    return;

                byte[] data = new byte[value.Length/8];
                for (int i = 0; i < value.Length; i++)
                    if (value[i])
                        data[i>>3] |= (byte)(1 << (i&7));

                data.CopyTo(Data, WondercardFlags);
                Edited = true;
            }
        }
        protected override MysteryGift[] MysteryGiftCards
        {
            get
            {
                if (WondercardData < 0)
                    return null;
                MysteryGift[] cards = new MysteryGift[GiftCountMax];
                for (int i = 0; i < cards.Length; i++)
                    cards[i] = getWC7(i);

                return cards;
            }
            set
            {
                if (value == null)
                    return;
                if (value.Length > GiftCountMax)
                    Array.Resize(ref value, GiftCountMax);
                
                for (int i = 0; i < value.Length; i++)
                    setWC7(value[i], i);
                for (int i = value.Length; i < GiftCountMax; i++)
                    setWC7(new WC7(), i);
            }
        }

        private WC7 getWC7(int index)
        {
            if (WondercardData < 0)
                return null;
            if (index < 0 || index > GiftCountMax)
                return null;

            return new WC7(Data.Skip(WondercardData + index * WC7.Size).Take(WC7.Size).ToArray());
        }
        private void setWC7(MysteryGift wc7, int index)
        {
            if (WondercardData < 0)
                return;
            if (index < 0 || index > GiftCountMax)
                return;

            wc7.Data.CopyTo(Data, WondercardData + index * WC7.Size);

            for (int i = 0; i < GiftCountMax; i++)
                if (BitConverter.ToUInt16(Data, WondercardData + i * WC7.Size) == 0)
                    for (int j = i + 1; j < GiftCountMax - i; j++) // Shift everything down
                        Array.Copy(Data, WondercardData + j * WC7.Size, Data, WondercardData + (j - 1) * WC7.Size, WC7.Size);

            Edited = true;
        }

        // Writeback Validity
        public override string MiscSaveChecks()
        {
            var r = new StringBuilder();

            // MemeCrypto check
            if (RequiresMemeCrypto && !MemeCrypto.CanUseMemeCrypto())
            {
                r.AppendLine("Platform does not support required cryptography providers.");
                r.AppendLine("Checksum will be broken until the file is saved using an OS without FIPS compliance enabled or a newer OS.");
                r.AppendLine();
            }

            // FFFF checks
            byte[] FFFF = Enumerable.Repeat((byte)0xFF, 0x200).ToArray();
            for (int i = 0; i < Data.Length / 0x200; i++)
            {
                if (!FFFF.SequenceEqual(Data.Skip(i * 0x200).Take(0x200))) continue;
                r.AppendLine($"0x200 chunk @ 0x{(i * 0x200).ToString("X5")} is FF'd.");
                r.AppendLine("Cyber will screw up (as of August 31st 2014).");
                r.AppendLine();

                // Check to see if it is in the Pokedex
                if (i * 0x200 > PokeDex && i * 0x200 < PokeDex + 0x900)
                {
                    r.Append("Problem lies in the Pokedex. ");
                    if (i * 0x200 == PokeDex + 0x400)
                        r.Append("Remove a language flag for a species < 585, ie Petilil");
                }
                break;
            }
            return r.ToString();
        }
        public override string MiscSaveInfo()
        {
            return Blocks.Aggregate("", (current, b) => current +
                $"{b.ID.ToString("00")}: {b.Offset.ToString("X5")}-{(b.Offset + b.Length).ToString("X5")}, {b.Length.ToString("X5")}{Environment.NewLine}");
        }

        public override bool RequiresMemeCrypto
        {
            get
            {
                return true;
            }
        }
    }
}
