using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using MirageXNA.Globals;

namespace MirageXNA.Globals
{
    class Types
    {
        ///////////////////
        ////// Types //////
        ///////////////////
        public static Class_Struct[] Class = new Class_Struct[Constant.MAX_CLASSES + 1];
        public static Player_Struct[] Players = new Player_Struct[Constant.MAX_PLAYERS + 1];
        public static Map_Struct[] Map = new Map_Struct[Constant.MAX_MAPS + 1];
        public static Npc_Struct[] Npc = new Npc_Struct[Constant.MAX_NPCS + 1];
        public static MapData_Struct[] MapNpcs = new MapData_Struct[Constant.MAX_MAPS + 1];

        // Cached Structures //
        public static Cached_Struct[] MapCache = new Types.Cached_Struct[Constant.MAX_MAPS + 1];
        public static NpcData_Struct[] NpcCache = new NpcData_Struct[Constant.MAX_MAPS + 1];

        ///////////////////////////////
        ////// Account Structure //////
        ///////////////////////////////
        public struct Account_Struct
        {
            // Name //
            public string Name;
            public string Pass;

            // Players //
            public Player_Struct[] Player;
        }

        /////////////////////////////
        ////// Class Structure //////
        /////////////////////////////
        public struct Class_Struct
        {
            // Name //
            public string Name;

            // Starting Stats //
            public int[] Stats;

            // Sprites //
            public int[] MaleSprite;
            public int[] FemaleSprite;
        }
        
        //////////////////////////////
        ////// Player Structure //////
        //////////////////////////////
        public struct Player_Struct
        {
            public string Name;
            public string Password;
            public byte Access;

            // Paperdoll //
            public int Sprite;

            // Movement //
            public int Map;
            public int X;
            public int Y;
            public int Dir;

            // Player Elements //
            public int[] Vitals;
            public int[] Stats;

            // Leveling //
            public int Level;
            public int Exp;
            public int Points;

            // Temp - Not Saved //
            public bool InGame;
            public bool StopRegen;
            public long StopRegenTimer;
        }

        ///////////////////////////////////
        ////// Cached Tile Structure //////
        ///////////////////////////////////
        public struct Cached_Struct
        {
            public NetOutgoingMessage Data;
        }

        ////////////////////////////
        ////// Tile Structure //////
        ////////////////////////////
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

        ///////////////////////////
        ////// Map Structure //////
        ///////////////////////////
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

        ///////////////////////////
        ////// Npc Structure //////
        ///////////////////////////
        public struct Npc_Struct
        {
            // Name //
            public string Name;

            // Required Parameters //
            public int Sprite;
            public int Range;
            public int Behaviour;
            public int SpawnSecs;
            public int Health;
            public int Exp;

            //
            public int[] Stats;
        }

        public struct MapData_Struct
        {
            public MapNpc_Struct[] Npc;
            
        }

        public struct NpcData_Struct
        {
            public NpcCache_Struct[] Npc;
            public int maxNpcs;
        }

        ///////////////////////////////
        ////// Map Npc Structure //////
        ///////////////////////////////
        public struct MapNpc_Struct
        {
            // Npc Number //
            public int Num;

            // Position //
            public int X;
            public int Y;
            public int Dir;
            public int Sprite;

            // Vitals
            public int[] Vitals;

            // Target //
            public int Target;
            public int TargetType;
            public int Range;

            // Spawn
            public int SpawnWait;
            public int AttackTimer;
        }

        public struct NpcCache_Struct
        {
            public int Num;
            public int X;
            public int Y;
            public int Dir;
            public int Sprite;
        }

    }
}
