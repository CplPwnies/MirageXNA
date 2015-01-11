///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using MirageXNA.Engine;

namespace MirageXNA.Global
{
    public static class General
    {

        ////////////////////////
        // GERNERAL FUNCTIONS //
        ////////////////////////

        // Calculate FPS //
        public static void calculateFPS(GameTime gameTime)
        {
            Static.frameCount++;
            Static.totalTime += gameTime.ElapsedGameTime.Milliseconds;
            if (Static.totalTime > 1000)
            {
                Static.fps = Static.frameCount;
                Static.frameCount = 0;
                Static.totalTime = 0;
            }
        }

        // Change Sprite //
        public static void changeSprite()
        {
            int SpriteCount = 10;

            if (Static.rSprite >= SpriteCount)
            {
                Static.rSprite = 1;
            }
            else
            {
                Static.rSprite = Static.rSprite + 1;
            }

        }

        ///////////////
        // SYSTEM IO //
        ///////////////
        public static void resizeMap(byte MapX, byte MaxY)
        {
            Types.Map.Tileset = 1;
            Types.Map.Name = "Map";
            Types.Map.MaxX = MapX;
            Types.Map.MaxY = MaxY;

            // Set Static Var //
            Static._MaxX = Types.Map.MaxX;
            Static._MaxY = Types.Map.MaxY;

            // Restructure the Array!
            Types.Map.Tile = new Types.Tile_Struct[Types.Map.MaxX, Types.Map.MaxY];
            Types.Map.SoundID = new Int16[Types.Map.MaxX, Types.Map.MaxY];
            Autotiles.Autotile = new Autotiles.Autotile_Struct[Types.Map.MaxX, Types.Map.MaxY];

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    Array.Resize<Types.TileData_Struct>(ref Types.Map.Tile[x, y].Layer, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    Array.Resize<byte>(ref Types.Map.Tile[x, y].Autotile, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    Array.Resize<Autotiles.QuarterTile_Struct>(ref Autotiles.Autotile[x, y].Layer, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    for (int layer = 0; layer <= 4; layer++)
                    {
                        Array.Resize<Autotiles.Destination_Struct>(ref Autotiles.Autotile[x, y].Layer[layer].QuarterTile, 5);
                        Array.Resize<int>(ref Autotiles.Autotile[x, y].Layer[layer].sX, 5);
                        Array.Resize<int>(ref Autotiles.Autotile[x, y].Layer[layer].sY, 5);
                    }
                }
            }

            Array.Clear(Types.Map.SoundID, 0, Types.Map.SoundID.Length);
        }

        public static void clearPlayer(int Index)
        {
            Types.Players[Index].Name = String.Empty;
            Types.Players[Index].Sprite = 0;
            Types.Players[Index].Map = 0;
            Types.Players[Index].X = 0;
            Types.Players[Index].Y = 0;
            Types.Players[Index].Dir = 0;
            Types.Players[Index].Level = 0;
            Types.Players[Index].Exp = 0;
            Types.Players[Index].Points = 0;

            Types.Players[Index].Stats = new int[(int)Enumerations.Stats.statCount];
            Types.Players[Index].Stats[(int)Enumerations.Stats.Strength] = 0;
            Types.Players[Index].Stats[(int)Enumerations.Stats.Defence] = 0;
            Types.Players[Index].Stats[(int)Enumerations.Stats.Magic] = 0;
            Types.Players[Index].Stats[(int)Enumerations.Stats.Speed] = 0;

            Types.Players[Index].Vitals = new int[(int)Enumerations.Vitals.vitalCount];
            Types.Players[Index].Vitals[(int)Enumerations.Vitals.HP] = 0;
            Types.Players[Index].Vitals[(int)Enumerations.Vitals.MP] = 0;
            Types.Players[Index].Vitals[(int)Enumerations.Vitals.SP] = 0;

            Types.Players[Index].MaxVitals = new int[(int)Enumerations.Vitals.vitalCount];
            Types.Players[Index].MaxVitals[(int)Enumerations.Vitals.HP] = 0;
            Types.Players[Index].MaxVitals[(int)Enumerations.Vitals.MP] = 0;
            Types.Players[Index].MaxVitals[(int)Enumerations.Vitals.SP] = 0;
        }

        public static void clearMapNpc(int mapNpcNum)
        {
            Types.MapNpcs[mapNpcNum].Active = false;
            Types.MapNpcs[mapNpcNum].Dir = 0;
            Types.MapNpcs[mapNpcNum].Map = 0;
            Types.MapNpcs[mapNpcNum].Moving = 0;
            Types.MapNpcs[mapNpcNum].Num = 0;
            Types.MapNpcs[mapNpcNum].OffsetX = 0;
            Types.MapNpcs[mapNpcNum].OffsetY = 0;
            Types.MapNpcs[mapNpcNum].Sprite = 0;
            Types.MapNpcs[mapNpcNum].Step = 0;
            Types.MapNpcs[mapNpcNum].Target = 0;
            Types.MapNpcs[mapNpcNum].TargetType = 0;
            Types.MapNpcs[mapNpcNum].X = 0;
            Types.MapNpcs[mapNpcNum].Y = 0;
        }
        public static void clearMapNpcs()
        {
            for (int LoopI = 0; LoopI <= Constant.MAX_MAP_NPCS - 1; LoopI++)
            {
                Types.MapNpcs[LoopI].Active = false;
                Types.MapNpcs[LoopI].Dir = 0;
                Types.MapNpcs[LoopI].Map = 0;
                Types.MapNpcs[LoopI].Moving = 0;
                Types.MapNpcs[LoopI].Num = 0;
                Types.MapNpcs[LoopI].OffsetX = 0;
                Types.MapNpcs[LoopI].OffsetY = 0;
                Types.MapNpcs[LoopI].Sprite = 0;
                Types.MapNpcs[LoopI].Step = 0;
                Types.MapNpcs[LoopI].Target = 0;
                Types.MapNpcs[LoopI].TargetType = 0;
                Types.MapNpcs[LoopI].X = 0;
                Types.MapNpcs[LoopI].Y = 0;
            }
        }

        public static void clearMap()
        {
            Types.Map.Tileset = 1;
            Types.Map.Name = "Map";
            Types.Map.MaxX = 50;
            Types.Map.MaxY = 50;

            // Set Static Var //
            Static._MaxX = Types.Map.MaxX;
            Static._MaxY = Types.Map.MaxY;

            // Restructure the Array!
            Types.Map.Tile = new Types.Tile_Struct[Types.Map.MaxX, Types.Map.MaxY];
            Types.Map.SoundID = new Int16[Types.Map.MaxX, Types.Map.MaxY];
            Autotiles.Autotile = new Autotiles.Autotile_Struct[Types.Map.MaxX, Types.Map.MaxY];

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    Array.Resize<Types.TileData_Struct>(ref Types.Map.Tile[x, y].Layer, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    Array.Resize<byte>(ref Types.Map.Tile[x, y].Autotile, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    Array.Resize<Autotiles.QuarterTile_Struct>(ref Autotiles.Autotile[x, y].Layer, 5);
                }
            }

            // Resize all Layers
            for (int x = 0; x <= Types.Map.MaxX - 1; x++)
            {
                for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                {
                    for (int layer = 0; layer <= 4; layer++)
                    {
                        Array.Resize<Autotiles.Destination_Struct>(ref Autotiles.Autotile[x, y].Layer[layer].QuarterTile, 5);
                        Array.Resize<int>(ref Autotiles.Autotile[x, y].Layer[layer].sX, 5);
                        Array.Resize<int>(ref Autotiles.Autotile[x, y].Layer[layer].sY, 5);
                    }
                }
            }

            Array.Clear(Types.Map.SoundID, 0, Types.Map.SoundID.Length);

            cacheTiles();
        }

        public static void loadMap(int MapNum)
        {
            clearMap();

            DirectoryInfo dir = new DirectoryInfo("Content\\Maps\\");
            using (FileStream stream = new FileStream(dir.FullName + MapNum + ".bin", FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Types.Map.Name = reader.ReadString();
                    Types.Map.Revision = reader.ReadInt32();
                    Types.Map.Music = reader.ReadByte();
                    Types.Map.Moral = reader.ReadByte();
                    Types.Map.Weather = reader.ReadByte();
                    Types.Map.Tileset = reader.ReadInt32();
                    Types.Map.Up = reader.ReadInt16();
                    Types.Map.Down = reader.ReadInt16();
                    Types.Map.Left = reader.ReadInt16();
                    Types.Map.Right = reader.ReadInt16();
                    Types.Map.BootMap = reader.ReadInt16();
                    Types.Map.BootX = reader.ReadByte();
                    Types.Map.BootY = reader.ReadByte();
                    Types.Map.MaxX = reader.ReadByte();
                    Types.Map.MaxY = reader.ReadByte();

                    // Restructure the Array!
                    Types.Map.Tile = new Types.Tile_Struct[Types.Map.MaxX, Types.Map.MaxY];
                    Types.Map.SoundID = new Int16[Types.Map.MaxX, Types.Map.MaxY];
                    Autotiles.Autotile = new Autotiles.Autotile_Struct[Types.Map.MaxX, Types.Map.MaxY];

                    // Resize all Layers
                    for (int x = 0; x <= Types.Map.MaxX - 1; x++)
                    {
                        for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                        {
                            Array.Resize<Types.TileData_Struct>(ref Types.Map.Tile[x, y].Layer, 5);
                        }
                    }

                    // Resize all Layers
                    for (int x = 0; x <= Types.Map.MaxX - 1; x++)
                    {
                        for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                        {
                            Array.Resize<byte>(ref Types.Map.Tile[x, y].Autotile, 5);
                        }
                    }

                    // Resize all Layers
                    for (int x = 0; x <= Types.Map.MaxX - 1; x++)
                    {
                        for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                        {
                            Array.Resize<Autotiles.QuarterTile_Struct>(ref Autotiles.Autotile[x, y].Layer, 5);
                        }
                    }

                    // Resize all Layers
                    for (int x = 0; x <= Types.Map.MaxX - 1; x++)
                    {
                        for (int y = 0; y <= Types.Map.MaxY - 1; y++)
                        {
                            for (int layer = 0; layer <= 4; layer++)
                            {
                                Array.Resize<Autotiles.Destination_Struct>(ref Autotiles.Autotile[x, y].Layer[layer].QuarterTile, 5);
                                Array.Resize<int>(ref Autotiles.Autotile[x, y].Layer[layer].sX, 5);
                                Array.Resize<int>(ref Autotiles.Autotile[x, y].Layer[layer].sY, 5);
                            }
                        }
                    }

                    // Set Static Var //
                    Static._MaxX = Types.Map.MaxX;
                    Static._MaxY = Types.Map.MaxY;

                    for (int X = 0; X <= Types.Map.MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= Types.Map.MaxY - 1; Y++)
                        {
                            for (int I = 0; I <= 4; I++)
                            {
                                Types.Map.Tile[X, Y].Layer[I].Tileset = reader.ReadByte();
                                Types.Map.Tile[X, Y].Layer[I].X = reader.ReadByte();
                                Types.Map.Tile[X, Y].Layer[I].Y = reader.ReadByte();

                                Types.Map.Tile[X, Y].Autotile[I] = reader.ReadByte();

                                Types.Map.Tile[X, Y].Type = reader.ReadByte();
                                Types.Map.Tile[X, Y].Data1 = reader.ReadInt32();
                                Types.Map.Tile[X, Y].Data2 = reader.ReadInt32();
                                Types.Map.Tile[X, Y].Data3 = reader.ReadInt32();
                                Types.Map.Tile[X, Y].DirBlock = reader.ReadByte();
                            }
                        }
                    }

                    for (int X = 0; X <= Types.Map.MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= Types.Map.MaxY - 1; Y++)
                        {
                            Types.Map.SoundID[X, Y] = reader.ReadInt16();
                        }
                    }

                    reader.Close();
                }
            }
        }

        public static void saveMap(int MapNum)
        {
            int MaxX = Types.Map.MaxX;
            int MaxY = Types.Map.MaxY;
            DirectoryInfo dir = new DirectoryInfo("Content\\Maps\\");
            using (FileStream stream = new FileStream(dir.FullName + MapNum + ".bin", FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(Types.Map.Name);
                    writer.Write(Types.Map.Revision);
                    writer.Write(Types.Map.Music);
                    writer.Write(Types.Map.Moral);
                    writer.Write(Types.Map.Weather);
                    writer.Write(Types.Map.Tileset);
                    writer.Write(Types.Map.Up);
                    writer.Write(Types.Map.Down);
                    writer.Write(Types.Map.Left);
                    writer.Write(Types.Map.Right);
                    writer.Write(Types.Map.BootMap);
                    writer.Write(Types.Map.BootX);
                    writer.Write(Types.Map.BootY);
                    writer.Write(Types.Map.MaxX);
                    writer.Write(Types.Map.MaxY);

                    for (int X = 0; X <= MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= MaxY - 1; Y++)
                        {
                            for (int I = 0; I <= 4; I++)
                            {
                                writer.Write(Types.Map.Tile[X, Y].Layer[I].Tileset);
                                writer.Write(Types.Map.Tile[X, Y].Layer[I].X);
                                writer.Write(Types.Map.Tile[X, Y].Layer[I].Y);

                                writer.Write(Types.Map.Tile[X, Y].Autotile[I]);

                                writer.Write(Types.Map.Tile[X, Y].Type);
                                writer.Write(Types.Map.Tile[X, Y].Data1);
                                writer.Write(Types.Map.Tile[X, Y].Data2);
                                writer.Write(Types.Map.Tile[X, Y].Data3);
                                writer.Write(Types.Map.Tile[X, Y].DirBlock);
                            }
                        }
                    }

                    for (int X = 0; X <= MaxX - 1; X++)
                    {
                        for (int Y = 0; Y <= MaxY - 1; Y++)
                        {
                            writer.Write(Types.Map.SoundID[X, Y]);
                        }
                    }

                    writer.Close();

                }
            }
        }

        ///////////////////////
        // Cache Tile Layers //
        ///////////////////////
        public static void cacheTiles()
        {
            byte Layer;
            int X;
            int Y;

            Array.Resize<Types.TileLayer_Struct>(ref Types.TileLayer, 5);

            //Loop through each layer and check which tiles there are that will need to be drawn
            for (Layer = 0; Layer <= 4; Layer++)
            {
                //Allocate enough memory for all the tiles
                Array.Resize<Types.CachedTile_Struct>(ref Types.TileLayer[Layer].Tile, (Static.MaxY - Static.MinY + 1) * (Static.MaxX - Static.MinX + 1));

                //Clear the number of tiles
                Types.TileLayer[Layer].NumTiles = 0;

                //Loop through all the tiles within the buffer's range
                for (Y = Static.MinY; Y <= (Static.MaxY); Y++)
                {
                    for (X = Static.MinX; X <= (Static.MaxX); X++)
                    {
                        if (X >= 0)
                        {
                            if (Y >= 0)
                            {
                                if (X <= Types.Map.MaxX - 1)
                                {
                                    if (Y <= Types.Map.MaxY - 1)
                                    {
                                        if (Types.Map.Tile[X, Y].Layer[Layer].Tileset > 0)
                                        {
                                            // Destination Positioning //
                                            Types.TileLayer[Layer].Tile[Types.TileLayer[Layer].NumTiles].dx = X;      // Store Destination X
                                            Types.TileLayer[Layer].Tile[Types.TileLayer[Layer].NumTiles].dy = Y;      // Store Destination Y
                                            Types.TileLayer[Layer].Tile[Types.TileLayer[Layer].NumTiles].PixelPosX = X * 32;
                                            Types.TileLayer[Layer].Tile[Types.TileLayer[Layer].NumTiles].PixelPosY = Y * 32;

                                            // Source Positioning //
                                            Types.TileLayer[Layer].Tile[Types.TileLayer[Layer].NumTiles].sRect
                                                = new Microsoft.Xna.Framework.Rectangle
                                                    (
                                                    Types.Map.Tile[X, Y].Layer[Layer].X * 32,
                                                    Types.Map.Tile[X, Y].Layer[Layer].Y * 32,
                                                    32,
                                                    32
                                                    );

                                            // The tile is valid to be used, so raise the count
                                            Types.TileLayer[Layer].NumTiles = Types.TileLayer[Layer].NumTiles + 1;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // We got all the information we need, now resize the array as small as possible to save RAM, then do the same for every other layer :o
                if (Types.TileLayer[Layer].NumTiles > 0)
                {
                    Array.Resize<Types.CachedTile_Struct>(ref Types.TileLayer[Layer].Tile, (int)Types.TileLayer[Layer].NumTiles);
                }
                else
                {
                    Array.Clear(Types.TileLayer[Layer].Tile, 0, Types.TileLayer[Layer].Tile.Length);
                }

            }

        }

        ///////////////////////
        // Menu Alert System //
        ///////////////////////
        public static void menuAlertMsg(string Caption, int Colour)
        {
            Static.menuAlertColour = Colour;
            Static.menuAlertMessage = Caption;
            Static.menuAlertTimer = (int)Mirage._gameTime.TotalGameTime.TotalMilliseconds + 3000;
        }

        //////////////
        // Chat Box //
        //////////////
        public static void updateTextBuffer()
        {
            int j = 0;
            int over = 0;

            // Check if the width is larger then the screen
            if (Static.EnterTextBufferWidth > UI_Game.gameWindow.chatWindow.Texture.W - 60)
            {
                for (int x = 0; x <= Static.EnterTextBuffer.Length; x++) // Loop through the characters backwards
                {
                    j = j + General.getStringWidth(1, Static.EnterTextBuffer.Substring(x, 1));
                    if (j >= UI_Game.gameWindow.chatWindow.Texture.W - 60) { over = x; break; }
                }
                Static.ShownText = Static.EnterTextBuffer.Substring(Static.EnterTextBuffer.Length - over);
            }
            else
            {
                Static.ShownText = Static.EnterTextBuffer;// Set the shown text buffer to the full buffer
            }
        }

        //////////////////////
        // String Functions //
        //////////////////////

        // Check Username & Password for 3.No Chars //
        public static bool isLoginLegal(String Username, String Password)
        {
            if (Username.Length > 2) { if (Password.Length > 2) { return true; } }
            return false;
        }

        // Check Username & Password for 3.No Chars //
        public static bool isRegisterLegal(String Username, String Password)
        {
            if (Username.Length > 2) { if (Password.Length > 2) { return true; } }
            return false;
        }

        // Check Valid Ascii Range //
        public static bool validCharacter(int KeyAscii)
        {
            // Remove bad characters
            if (KeyAscii >= 32) return true;
            return false;
        }

        // String Width //
        public static int getStringWidth(int Font, string text)
        {
            GfxManager GfxM = GfxManager.Instance;
            Vector2 value = GfxM.FONT_TEXTURE[Font].MeasureString(text);
            return (int)value.X;
        }

        // String Height //
        public static int getStringHeight(int Font, string text)
        {
            GfxManager GfxM = GfxManager.Instance;
            Vector2 value = GfxM.FONT_TEXTURE[Font].MeasureString(text);
            return (int)value.Y;
        }

        // String Word Wrap //
        public static String wordWrap(int Font, String text, int Width)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');
            GfxManager GfxM = GfxManager.Instance;

            foreach (String word in wordArray)
            {
                if (GfxM.FONT_TEXTURE[Font].MeasureString(line + word).Length() > Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

        ////////////////////////
        // Compression System //
        ////////////////////////

        // Compress Using GZIP //
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

        // Decompress Using GZIP //
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
    }
}
