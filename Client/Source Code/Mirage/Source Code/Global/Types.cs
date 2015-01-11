///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MirageXNA.Global
{
    public class Types
    {
        public static Player_Struct[] Players = new Player_Struct[Constant.MAX_PLAYERS + 1];
        public static Map_Struct Map;
        public static MapNpc_Struct[] MapNpcs = new MapNpc_Struct[Constant.MAX_MAP_NPCS + 1];

        // Tile Cache //
        public static TileLayer_Struct[] TileLayer; //32x32 Cache

        //////////////////////
        // Window Strutures //
        /// //////////////////
        public struct Grh
        {
            public int X;
            public int Y;
            public int W;
            public int H;
            public int sX;
            public int sY;
            public int sW;
            public int sH;
            public int TEXTURE;
            public Grh_Text Text;
        }
        public struct Grh_Button
        {
            public int X;
            public int Y;
            public int W;
            public int H;
            public int TEXTURE;
            public int STATE;
        }
        public struct Grh_Text
        {
            public string CAPTION;
            public int X;
            public int Y;
            public int W;
            public int H;
            public Color COLOUR;
            public int FONT;
        }

        // 2D Texture //
        public struct Texture2D_struct
        {
            public Texture2D Texture;
            public int Width;
            public int Height;
            public string Path;
            public long UnloadTimer;
            public bool Loaded;
        }

        //////////////////////
        // Player Strutures //
        /// //////////////////
        public struct Player_Struct
        {
            // Player Name //
            public string Name;

            // Paperdoll //
            public int Sprite;

            // Movement //
            public int Map;
            public int X;
            public int Y;
            public int Dir;

            // Player Elements //
            public int[] Stats;
            public int[] Vitals;
            public int[] MaxVitals;

            // Leveling //
            public int Level;
            public int Exp;
            public int MaxExp;
            public int Points;

            // Movement //
            public int OffsetX;
            public int OffsetY;
            public int Moving;
            public int Step;

            // Attacking //
            public byte Attacking;
            public int AttackSpeed;
            public int AttackTimer;
        }

        ///////////////////
        // Map Strutures //
        /// ///////////////
        public struct TileData_Struct
        {
            public byte X;
            public byte Y;
            public byte Tileset;
        }

        public struct Tile_Struct
        {
            public TileData_Struct[] Layer;
            public byte[] Autotile;
            public byte Type;
            public Int32 Data1;
            public Int32 Data2;
            public Int32 Data3;
            public byte DirBlock;
        }

        public struct Map_Struct
        {
            public string Name;
            public Int32 Revision;
            public byte Moral;
            public Int32 Tileset;
            public Int16 Up;
            public Int16 Down;
            public Int16 Left;
            public Int16 Right;
            public byte Music;
            public Int16 BootMap;
            public byte BootX;
            public byte BootY;
            public byte MaxX;
            public byte MaxY;
            public Tile_Struct[,] Tile;
            public byte Weather;
            public Int16[,] SoundID;
        }

        ///////////////////////////////////////////////////////////////
        // Used to hold the graphic layers in a quick-to-draw format //
        ///////////////////////////////////////////////////////////////
        public struct CachedTile_Struct
        {
            public int dx;
            public int dy;
            public Rectangle sRect;
            public int PixelPosX;
            public int PixelPosY;
        }

        public struct TileLayer_Struct
        {
            public CachedTile_Struct[] Tile;
            public int NumTiles;
        }

        ////////////////////////////////
        ////// Autotile Structure //////
        ////////////////////////////////
        public struct Destination_Struct
        {
            public int X;
            public int Y;
        }

        public struct QuarterTile_Struct
        {
            public Destination_Struct[] QuarterTile;    //4
            public byte RenderState;
            public int[] sX;    //4
            public int[] sY;    //4
        }

        public struct Autotile_Struct
        {
            public QuarterTile_Struct[] Layer; //5
        }

        ///////////////////////////////
        ////// Map Npc Structure //////
        ///////////////////////////////
        public struct MapNpc_Struct
        {
            // Npc Number //
            public string Name;
            public int Num;
            public int Sprite;
            public bool Active;

            // Position //
            public int Map;
            public int X;
            public int Y;
            public int Dir;

            // Target //
            public int Target;
            public int TargetType;

            // Client Use //
            public int OffsetX;
            public int OffsetY;
            public int Moving;
            public int Step;
        }
    }
}
