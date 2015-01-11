using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MirageXNA.Globals;
using MirageXNA.Network;
using MirageXNA.Core;
using Lidgren.Network;

namespace MirageXNA.Core
{

    class Database
    {
        string appPath = Application.StartupPath;

        /////////////////
        // COMPRESSION //
        /////////////////
        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
        public static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0) { memory.Write(buffer, 0, count); }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        ////////////
        // PLAYER //
        ////////////

        // Save Player //
        public static void savePlayer(Int32 Index)
        {
            DirectoryInfo dir = new DirectoryInfo("Data\\Accounts\\");
            using (FileStream stream = new FileStream(dir.FullName + Types.Players[Index].Name.ToLower() + ".bin", FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    // Player Information //
                    writer.Write(Types.Players[Index].Name);
                    writer.Write(Types.Players[Index].Password);
                    writer.Write(Types.Players[Index].Access);
                    writer.Write(Types.Players[Index].Sprite);

                    // Player Position // 
                    writer.Write(Types.Players[Index].Map);
                    writer.Write(Types.Players[Index].X);
                    writer.Write(Types.Players[Index].Y);
                    writer.Write(Types.Players[Index].Dir);

                    // Player Stats //
                    writer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Strength]);
                    writer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Defence]);
                    writer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Magic]);
                    writer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Speed]);

                    // Save Vitals
                    writer.Write(Types.Players[Index].Vitals[(int)Enumerations.Vitals.HP]);
                    writer.Write(Types.Players[Index].Vitals[(int)Enumerations.Vitals.MP]);
                    writer.Write(Types.Players[Index].Vitals[(int)Enumerations.Vitals.SP]);

                    writer.Write(Types.Players[Index].Level);
                    writer.Write(Types.Players[Index].Exp);
                    writer.Write(Types.Players[Index].Points);
                    writer.Close();

                }
            }
        }

        // Load Player //
        public static bool loadPlayer(Int32 Index, String Name)
        {
            DirectoryInfo dir = new DirectoryInfo("Data\\Accounts\\");
            string accountDir = dir.FullName + Name.ToLower() + ".bin";
            if (File.Exists(accountDir))
            {
                using (FileStream stream = new FileStream(accountDir, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        // Player Information //
                        Types.Players[Index].Name = reader.ReadString();
                        Types.Players[Index].Password = reader.ReadString();
                        Types.Players[Index].Access = reader.ReadByte();
                        Types.Players[Index].Sprite = reader.ReadInt32();

                        // Player Position //
                        Types.Players[Index].Map = reader.ReadInt32();
                        Types.Players[Index].X = reader.ReadInt32();
                        Types.Players[Index].Y = reader.ReadInt32();
                        Types.Players[Index].Dir = reader.ReadInt32();
                        
                        // Player Stats //
                        Types.Players[Index].Stats[(int)Enumerations.Stats.Strength] = reader.ReadInt32();
                        Types.Players[Index].Stats[(int)Enumerations.Stats.Defence] = reader.ReadInt32();
                        Types.Players[Index].Stats[(int)Enumerations.Stats.Magic] = reader.ReadInt32();
                        Types.Players[Index].Stats[(int)Enumerations.Stats.Speed] = reader.ReadInt32();

                        // Vitals
                        Types.Players[Index].Vitals[(int)Enumerations.Vitals.HP] = reader.ReadInt32();
                        Types.Players[Index].Vitals[(int)Enumerations.Vitals.MP] = reader.ReadInt32();
                        Types.Players[Index].Vitals[(int)Enumerations.Vitals.SP] = reader.ReadInt32();
                        
                        Types.Players[Index].Level = reader.ReadInt32();
                        Types.Players[Index].Exp = reader.ReadInt32();
                        Types.Players[Index].Points = reader.ReadInt32();
                        reader.Close();
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /////////
        // MAP //
        /////////
        public static void checkMaps()
        {
            for (int i = 1; i <= Constant.MAX_MAPS; i++)
            {
                DirectoryInfo dir = new DirectoryInfo("data\\Maps\\");
                if (!File.Exists(dir.FullName + i + ".bin"))
                {
                    clearMap(i);
                    saveMap(i);
                }
            }
        }
        public static void saveMap(Int32 MapNum)
        {
            int MaxX = Types.Map[MapNum].MaxX;
            int MaxY = Types.Map[MapNum].MaxY;

            DirectoryInfo dir = new DirectoryInfo("Data\\Maps\\");
            using (FileStream stream = new FileStream(dir.FullName + MapNum + ".bin", FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(Types.Map[MapNum].Name);
                    writer.Write(Types.Map[MapNum].Revision);
                    writer.Write(Types.Map[MapNum].Music);
                    writer.Write(Types.Map[MapNum].Moral);
                    writer.Write(Types.Map[MapNum].Weather);
                    writer.Write(Types.Map[MapNum].Tileset);
                    writer.Write(Types.Map[MapNum].Up);
                    writer.Write(Types.Map[MapNum].Down);
                    writer.Write(Types.Map[MapNum].Left);
                    writer.Write(Types.Map[MapNum].Right);
                    writer.Write(Types.Map[MapNum].BootMap);
                    writer.Write(Types.Map[MapNum].BootX);
                    writer.Write(Types.Map[MapNum].BootY);
                    writer.Write(Types.Map[MapNum].MaxX);
                    writer.Write(Types.Map[MapNum].MaxY);

                    for (int X = 0; X <= MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= MaxY - 1; Y++)
                        {
                            for (int I = 0; I <= 4; I++)
                            {
                                writer.Write(Types.Map[MapNum].Tile[X, Y].Layer[I].Tileset);
                                writer.Write(Types.Map[MapNum].Tile[X, Y].Layer[I].X);
                                writer.Write(Types.Map[MapNum].Tile[X, Y].Layer[I].Y);

                                writer.Write(Types.Map[MapNum].Tile[X, Y].Autotile[I]);

                                writer.Write(Types.Map[MapNum].Tile[X, Y].Type);
                                writer.Write(Types.Map[MapNum].Tile[X, Y].Data1);
                                writer.Write(Types.Map[MapNum].Tile[X, Y].Data2);
                                writer.Write(Types.Map[MapNum].Tile[X, Y].Data3);
                                writer.Write(Types.Map[MapNum].Tile[X, Y].DirBlock);
                            }
                        }
                    }

                    for (int X = 0; X <= MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= MaxY - 1; Y++)
                        {
                            writer.Write(Types.Map[MapNum].SoundID[X, Y]);
                        }
                    }

                    writer.Close();

                }
            }
        }
        public static void loadMap(Int32 MapNum)
        {

            DirectoryInfo dir = new DirectoryInfo("Data\\Maps\\");
            using (FileStream stream = new FileStream(dir.FullName +  MapNum + ".bin", FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Types.Map[MapNum].Name = reader.ReadString();
                    Types.Map[MapNum].Revision = reader.ReadInt32();
                    Types.Map[MapNum].Music = reader.ReadByte();
                    Types.Map[MapNum].Moral = reader.ReadByte();
                    Types.Map[MapNum].Weather = reader.ReadByte();
                    Types.Map[MapNum].Tileset = reader.ReadInt32();
                    Types.Map[MapNum].Up = reader.ReadInt16();
                    Types.Map[MapNum].Down = reader.ReadInt16();
                    Types.Map[MapNum].Left = reader.ReadInt16();
                    Types.Map[MapNum].Right = reader.ReadInt16();
                    Types.Map[MapNum].BootMap = reader.ReadInt16();
                    Types.Map[MapNum].BootX = reader.ReadByte();
                    Types.Map[MapNum].BootY = reader.ReadByte();
                    Types.Map[MapNum].MaxX = reader.ReadByte();
                    Types.Map[MapNum].MaxY = reader.ReadByte();

                    byte MaxX = Types.Map[MapNum].MaxX;
                    byte MaxY = Types.Map[MapNum].MaxY;

                    resizeMap(MapNum, MaxX, MaxY);

                    for (int X = 0; X <= MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= MaxY - 1; Y++)
                        {
                            for (int I = 0; I <= 4; I++)
                            {
                                Types.Map[MapNum].Tile[X, Y].Layer[I].Tileset = reader.ReadByte();
                                Types.Map[MapNum].Tile[X, Y].Layer[I].X = reader.ReadByte();
                                Types.Map[MapNum].Tile[X, Y].Layer[I].Y = reader.ReadByte();
                                Types.Map[MapNum].Tile[X, Y].Autotile[I] = reader.ReadByte();
                                Types.Map[MapNum].Tile[X, Y].Type = reader.ReadByte();
                                Types.Map[MapNum].Tile[X, Y].Data1 = reader.ReadInt32();
                                Types.Map[MapNum].Tile[X, Y].Data2 = reader.ReadInt32();
                                Types.Map[MapNum].Tile[X, Y].Data3 = reader.ReadInt32();
                                Types.Map[MapNum].Tile[X, Y].DirBlock = reader.ReadByte();
                            }
                        }
                    }

                    for (int X = 0; X <= MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= MaxY - 1; Y++)
                        {
                            Types.Map[MapNum].SoundID[X, Y] = reader.ReadInt16();
                        }
                    }

                    reader.Close();
                }
            }
        }
        public static void resizeMap(Int32 MapNum, byte MaxX, byte MaxY)
        {
            Types.Map[MapNum].Tileset = 1;
            Types.Map[MapNum].Name = "Map";
            Types.Map[MapNum].MaxX = MaxX;
            Types.Map[MapNum].MaxY = MaxY;

            // Restructure the Array!
            Types.Map[MapNum].Tile = new Types.Tile_Struct[Types.Map[MapNum].MaxX, Types.Map[MapNum].MaxY];
            Types.Map[MapNum].SoundID = new Int16[Types.Map[MapNum].MaxX, Types.Map[MapNum].MaxY];

            // Resize all Layers
            for (int x = 0; x <= Types.Map[MapNum].MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map[MapNum].MaxY - 1; y++)
                {
                    Array.Resize<Types.TileData_Struct>(ref Types.Map[MapNum].Tile[x, y].Layer, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map[MapNum].MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map[MapNum].MaxY - 1; y++)
                {
                    Array.Resize<byte>(ref Types.Map[MapNum].Tile[x, y].Autotile, 5);
                }
            }

            Array.Clear(Types.Map[MapNum].SoundID, 0, Types.Map[MapNum].SoundID.Length);

            // Clear Map Cache //
            Types.MapCache[MapNum].Data = null;
        }
        public static void clearMap(Int32 MapNum)
        {
            Types.Map[MapNum].Tileset = 1;
            Types.Map[MapNum].Name = "Map";
            Types.Map[MapNum].MaxX = 50;
            Types.Map[MapNum].MaxY = 50;

            // Restructure the Array!
            Types.Map[MapNum].Tile = new Types.Tile_Struct[Types.Map[MapNum].MaxX, Types.Map[MapNum].MaxY];
            Types.Map[MapNum].SoundID = new Int16[Types.Map[MapNum].MaxX, Types.Map[MapNum].MaxY];

            // Resize all Layers
            for (int x = 0; x <= Types.Map[MapNum].MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map[MapNum].MaxY - 1; y++)
                {
                    Array.Resize<Types.TileData_Struct>(ref Types.Map[MapNum].Tile[x, y].Layer, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map[MapNum].MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map[MapNum].MaxY - 1; y++)
                {
                    Array.Resize<byte>(ref Types.Map[MapNum].Tile[x, y].Autotile, 5);
                }
            }

            Array.Clear(Types.Map[MapNum].SoundID, 0, Types.Map[MapNum].SoundID.Length);

            // Clear Map Cache //
            Types.MapCache[MapNum].Data = null;
        }
        public static void loadMaps()
        {
            checkMaps();
            for (int LoopI = 1; LoopI <= Constant.MAX_MAPS; LoopI++)
            {
                loadMap(LoopI);
                cacheMap(LoopI);
                Application.DoEvents();
            }

        }
        public static void cacheMap(Int32 mapNum)
        {
            int X; int Y;
            int MaxX = Types.Map[mapNum].MaxX;
            int MaxY = Types.Map[mapNum].MaxY;

            // ****** PreAllocate Buffer ******
            int mapSize = Marshal.SizeOf(Types.Map[mapNum]);
            int tileSize = Marshal.SizeOf(Types.Map[mapNum].Tile[0,0]);
            int nLength = mapSize + ((tileSize * MaxX) * MaxY);

            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(nLength);

            // Must Preallocate //
            TempBuffer.Write(mapNum);

            // ****** Map Info ******
            TempBuffer.Write(Types.Map[mapNum].Name);
            TempBuffer.Write(Types.Map[mapNum].Music);
            TempBuffer.Write(Types.Map[mapNum].Revision);
            TempBuffer.Write(Types.Map[mapNum].Moral);
            TempBuffer.Write(Types.Map[mapNum].Weather);
            TempBuffer.Write(Types.Map[mapNum].Tileset);
            TempBuffer.Write(Types.Map[mapNum].Up);
            TempBuffer.Write(Types.Map[mapNum].Down);
            TempBuffer.Write(Types.Map[mapNum].Left);
            TempBuffer.Write(Types.Map[mapNum].Right);
            TempBuffer.Write(Types.Map[mapNum].BootMap);
            TempBuffer.Write(Types.Map[mapNum].BootX);
            TempBuffer.Write(Types.Map[mapNum].BootY);
            TempBuffer.Write(Types.Map[mapNum].MaxX);
            TempBuffer.Write(Types.Map[mapNum].MaxY);

            // Faster Maybe? //
            Types.Tile_Struct[,] tempTile = Types.Map[mapNum].Tile;

            // ****** Tiles ******
            for (X = 0; X <= MaxX - 1; X++)
            {
                for (Y = 0; Y <= MaxY - 1; Y++)
                {
                    for (int I = 0; I <= 4; I++)
                    {
                        TempBuffer.Write(tempTile[X, Y].Layer[I].X);
                        TempBuffer.Write(tempTile[X, Y].Layer[I].Y);
                        TempBuffer.Write(tempTile[X, Y].Layer[I].Tileset);
                        TempBuffer.Write(tempTile[X, Y].Autotile[I]);
                    }
                    TempBuffer.Write(tempTile[X, Y].Type);
                    TempBuffer.Write(tempTile[X, Y].Data1);
                    TempBuffer.Write(tempTile[X, Y].Data2);
                    TempBuffer.Write(tempTile[X, Y].Data3);
                    TempBuffer.Write(tempTile[X, Y].DirBlock);
                    Application.DoEvents();
                }
            }

            // ****** Send Map SoundID ******
            for (X = 0; X <= MaxX - 1; X++)
            {
                for (Y = 0; Y <= MaxY - 1; Y++)
                {
                    TempBuffer.Write(Types.Map[mapNum].SoundID[X, Y]);
                }
            }

            // ****** Compress Using 7Zip.dll ******
            //TempBuffer.CompressBuffer();

            Types.MapCache[mapNum].Data = TempBuffer;
        }

        ///////////////////
        ////// CLASS //////
        ///////////////////
        public static void CreateClasses()
        {
            DirectoryInfo dir = new DirectoryInfo("Data\\Class.ini");
            IniFile ini = new IniFile(dir.FullName);

            // Class Defaults
            for (int I = 0; I <= Constant.MAX_CLASSES - 1; I++)
            {
                // Class Name
                ini.Write("CLASS" + I, "Name", "Mage");

                // Sprites
                ini.Write("CLASS" + I, "MaleSprite", "1,2,3");
                ini.Write("CLASS" + I, "FemaleSprite", "1,2,3");

                // Statistics
                ini.Write("CLASS" + I, "Strength", "5");
                ini.Write("CLASS" + I, "Defence", "3");
                ini.Write("CLASS" + I, "Magic", "2");
                ini.Write("CLASS" + I, "Speed", "0");
            }
        }

        public static void LoadClasses()
        {
            DirectoryInfo dir = new DirectoryInfo("Data\\Class.ini");
            IniFile ini = new IniFile(dir.FullName);

            if (!dir.Exists)
                CreateClasses();

            for (int I = 0; I <= Constant.MAX_CLASSES - 1; I++)
            {
                Types.Class[I].Name = ini.Read("CLASS" + I, "Name");

                // Seperate Sprites
                String tmpString = ini.Read("CLASS" + I, "MaleSprite");
                String[] tmpArray = tmpString.Split(',');
                Array.Resize<int>(ref Types.Class[I].MaleSprite, tmpArray.Length);
                for (int N = 0; N <= tmpArray.Length - 1; N++)
                {
                    Types.Class[I].MaleSprite[N] = Convert.ToInt32(tmpArray[N]);
                }

                // Seperate Sprites
                tmpString = ini.Read("CLASS" + I, "FemaleSprite");
                tmpArray = tmpString.Split(',');
                Array.Resize<int>(ref Types.Class[I].FemaleSprite, tmpArray.Length);
                for (int N = 0; N <= tmpArray.Length - 1; N++)
                {
                    Types.Class[I].FemaleSprite[N] = Convert.ToInt32(tmpArray[N]);
                }

                // Statistics
                Array.Resize<int>(ref Types.Class[I].Stats, (int)Enumerations.Stats.statCount);
                Types.Class[I].Stats[(int)Enumerations.Stats.Strength] = Convert.ToInt32(ini.Read("CLASS" + I, "Strength"));
                Types.Class[I].Stats[(int)Enumerations.Stats.Defence] = Convert.ToInt32(ini.Read("CLASS" + I, "Defence"));
                Types.Class[I].Stats[(int)Enumerations.Stats.Magic] = Convert.ToInt32(ini.Read("CLASS" + I, "Magic"));
                Types.Class[I].Stats[(int)Enumerations.Stats.Speed] = Convert.ToInt32(ini.Read("CLASS" + I, "Speed"));
            }
        }

        /////////////////
        ////// NPC //////
        /////////////////

        // Load All Npcs Available //
        public static void loadAllNpcs()
        {
            for (int i = 1; i <= Constant.MAX_MAP_NPCS; i++)
            {
                clearNpc(i);
                loadNpc(i);
            }
        }

        // Load Npc //
        public static void loadNpc(Int32 Index)
        {
            // Get Directory Path //
            DirectoryInfo dir = new DirectoryInfo("Data\\Npcs\\");

            string accountDir = dir.FullName + Index + ".bin";
            if (File.Exists(accountDir))
            {
                using (FileStream stream = new FileStream(accountDir, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        // Npc Information //
                        Types.Npc[Index].Name = reader.ReadString();
                        Types.Npc[Index].Sprite = reader.ReadInt32();
                        Types.Npc[Index].Range = reader.ReadInt32();
                        Types.Npc[Index].Behaviour = reader.ReadInt32();
                        Types.Npc[Index].SpawnSecs = reader.ReadInt32();
                        Types.Npc[Index].Health = reader.ReadInt32();
                        Types.Npc[Index].Exp = reader.ReadInt32();

                        // Read Stat Array //
                        for (int statNum = 0; statNum <= (int)Enumerations.Stats.statCount - 1; statNum++)
                        {
                            Types.Npc[Index].Stats[statNum] = reader.ReadInt32();
                        }
                        reader.Close();
                    }
                }
            }
            else
            {
                using (FileStream stream = new FileStream(accountDir, FileMode.Create))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        // Npc Information //
                        writer.Write(Types.Npc[Index].Name);
                        writer.Write(Types.Npc[Index].Sprite);
                        writer.Write(Types.Npc[Index].Range);
                        writer.Write(Types.Npc[Index].Behaviour);
                        writer.Write(Types.Npc[Index].SpawnSecs);
                        writer.Write(Types.Npc[Index].Health);
                        writer.Write(Types.Npc[Index].Exp);

                        // Write Stat Array //
                        for (int statNum = 0; statNum <= (int)Enumerations.Stats.statCount - 1; statNum++)
                        {
                            writer.Write(Types.Npc[Index].Stats[statNum]);
                        }
                        writer.Close();
                    }
                }
            }
        }

        // Clear Npc //
        public static void clearNpc(int Index)
        {
            // Clear //
            Types.Npc[Index].Name = String.Empty;
            Types.Npc[Index].Sprite = 0;
            Types.Npc[Index].Behaviour = 0;
            Types.Npc[Index].SpawnSecs = 0;

            Types.Npc[Index].Stats = new int[(int)Enumerations.Stats.statCount];
            for (int statNum = 0; statNum <= (int)Enumerations.Stats.statCount - 1; statNum++)
            {
                Types.Npc[Index].Stats[statNum] = 0;
            }
        }
    }

}
