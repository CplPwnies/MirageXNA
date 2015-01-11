///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using MirageXNA;
using MirageXNA.Global;
using MirageXNA.Engine;

namespace MirageXNA.Network
{
    class HandleTCP
    {

        // Packets sent by client to server //
        public enum ClientPackets
        {
            None,
            CPing,
            CLogin,
            CRegister,
            CPlayerMove,
            CPlayerDir,
            CAttack,
            CMapMsg,
            CRequestNewMap,
            CNeedMap,
            CTrainStat,

            // Keep below all
            totalClient
        }

        // Packets sent by server to client //
        public enum ServerPackets
        {
            None,
            SSendPing,
            SAlertMsg,
            SLoginOk,
            SPlayerMove,
            SPlayerDir,
            SPlayerData,
            SMapData,
            SChatMsg,
            SCheckForMap,
            SSpawnNpc,
            SNpcMove,
            SNpcDir,
            SMapDone,
            SNpcDead,
            SPlayerVital,
            SPlayerExp,

            // Keep below all
            totalServer
        }

        public static void HandleData(NetIncomingMessage Data)
        {
            // Packet Header //
            Int32 MsgType = Data.ReadInt32();

            // Check Packet Header Number //
            if (MsgType < 0 && MsgType >= (int)ServerPackets.totalServer) 
                return;

            // Handle Incoming Message //
            switch (MsgType)
            {
                case (int)ServerPackets.None: break;
                case (int)ServerPackets.SSendPing: HandleSendPing(); break;
                case (int)ServerPackets.SAlertMsg: HandleAlertMsg(Data); break;
                case (int)ServerPackets.SLoginOk: HandleLoginOk(Data); break;
                case (int)ServerPackets.SPlayerMove: HandlePlayerMovement(Data); break;
                case (int)ServerPackets.SPlayerDir: HandlePlayerDirection(Data); break;
                case (int)ServerPackets.SPlayerData: HandlePlayerData(Data); break;
                case (int)ServerPackets.SMapData: HandleMapData(Data); break;
                case (int)ServerPackets.SChatMsg: HandleMapMsg(Data); break;
                case (int)ServerPackets.SCheckForMap: HandleCheckForMap(Data); break;
                case (int)ServerPackets.SSpawnNpc: HandleSpawnNpc(Data); break;
                case (int)ServerPackets.SNpcMove: HandleMoveNpc(Data); break;
                case (int)ServerPackets.SNpcDir: HandleNpcDir(Data); break;
                case (int)ServerPackets.SMapDone: HandleMapDone(Data); break;
                case (int)ServerPackets.SNpcDead: HandleNpcDead(Data); break;
                case (int)ServerPackets.SPlayerVital: HandlePlayerVital(Data); break;
                case (int)ServerPackets.SPlayerExp: HandlePlayerExp(Data); break;
            }

        }

        public static void HandleSendPing()
        {
            Static.PingEnd = Environment.TickCount;
            Static.Ping = Static.PingEnd - Static.PingStart;
        }

        public static void HandleAlertMsg(NetIncomingMessage Data)
        {
            string M;

            M = Data.ReadString();
            General.menuAlertMsg(M, 1);
        }

        public static void HandleLoginOk(NetIncomingMessage Data)
        {
            // Set Index
            Static.MyIndex = Data.ReadInt32();

            // Clear Players
            for (int I = 1; I <= Constant.MAX_PLAYERS; I++)
            {
                General.clearPlayer(I);
            }

            // Load InGame
            UI_Game.initGameGui();
            Static.ShowGameWindow = new bool[Constant.maxWindows];

            // Cache Autotile Postions //
            Autotiles.cacheAutotileArray();

            //
            Static.EnterTextBuffer = String.Empty;

            // Show Windows On Start Up Here //
            Static.ShowGameWindow[Constant.chatWindow] = true;

            Static.menuType = Constant.inGame;

            // Force Tileset Update //
            Static.LastTileX = -131313;        // Force Map Update //
            Static.LastTileY = -131313;        // Force Map Update //
        }

        // HANDLE PLAYER MOVEMENT //
        public static void HandlePlayerMovement(NetIncomingMessage Data)
        {
            // Read Packet Information
             Int32 PlayerIndex = Data.ReadInt32();

            // Player Can Move - Update to all online //
            Types.Players[PlayerIndex].X = Data.ReadInt32();
            Types.Players[PlayerIndex].Y = Data.ReadInt32();
            Types.Players[PlayerIndex].Dir = Data.ReadInt32();
            Types.Players[PlayerIndex].Moving = Data.ReadInt32();

            Types.Players[PlayerIndex].OffsetX = 0;
            Types.Players[PlayerIndex].OffsetY = 0;

            switch (Types.Players[PlayerIndex].Dir)
            {
                case Constant.DIR_NORTH:
                    Types.Players[PlayerIndex].OffsetY = 32;
                    break;
                case Constant.DIR_SOUTH:
                    Types.Players[PlayerIndex].OffsetY = 32 * -1;
                    break;
                case Constant.DIR_WEST:
                    Types.Players[PlayerIndex].OffsetX = 32;
                    break;
                case Constant.DIR_EAST:
                    Types.Players[PlayerIndex].OffsetX = 32 * -1;
                    break;
            }
        }

        // HANDLE PLAYER MOVEMENT //
        public static void HandlePlayerDirection(NetIncomingMessage Data)
        {
            // Read Packet Information
            Int32 PlayerIndex = Data.ReadInt32();

            // Player Can Move - Update to all online //
            Types.Players[PlayerIndex].Dir = Data.ReadInt32();
            Types.Players[PlayerIndex].Moving = 0;
            Types.Players[PlayerIndex].OffsetX = 0;
            Types.Players[PlayerIndex].OffsetY = 0;
        }

        // HANDLE JOIN GAME //
        public static void HandlePlayerData(NetIncomingMessage Data)
        {
            // Read Packet Information
            Int32 i = Data.ReadInt32();

            // Player Can Move - Update to all online //
            Types.Players[i].Name = Data.ReadString();
            Types.Players[i].Sprite = Data.ReadInt32();
            Types.Players[i].Level = Data.ReadInt32();
            Types.Players[i].Map = Data.ReadInt32();
            Types.Players[i].X = Data.ReadInt32();
            Types.Players[i].Y = Data.ReadInt32();
            Types.Players[i].Dir = Data.ReadInt32();

            Types.Players[i].Stats = new int[(int)Enumerations.Stats.statCount];
            Types.Players[i].Stats[(int)Enumerations.Stats.Strength] = Data.ReadInt32();
            Types.Players[i].Stats[(int)Enumerations.Stats.Defence] = Data.ReadInt32();
            Types.Players[i].Stats[(int)Enumerations.Stats.Magic] = Data.ReadInt32();
            Types.Players[i].Stats[(int)Enumerations.Stats.Speed] = Data.ReadInt32();
            
            Types.Players[i].Points = Data.ReadInt32();

            // Reset Offset //
            Types.Players[i].OffsetX = 0;
            Types.Players[i].OffsetY = 0;
        }

        // HANDLE MAP DATA //
        public static void HandleMapData(NetIncomingMessage Data)
        {

            // Clear Map //
            General.clearMap();

            // Map Number //
            Int32 mapNum = Data.ReadInt32();

            // Map Info //
            Types.Map.Name = Data.ReadString();
            Types.Map.Music = Data.ReadByte();
            Types.Map.Revision = Data.ReadInt32();
            Types.Map.Moral = Data.ReadByte();
            Types.Map.Weather = Data.ReadByte();
            Types.Map.Tileset = Data.ReadInt32();
            Types.Map.Up = Data.ReadInt16();
            Types.Map.Down = Data.ReadInt16();
            Types.Map.Left = Data.ReadInt16();
            Types.Map.Right = Data.ReadInt16();
            Types.Map.BootMap = Data.ReadInt16();
            Types.Map.BootX = Data.ReadByte();
            Types.Map.BootY = Data.ReadByte();
            Types.Map.MaxX = Data.ReadByte();
            Types.Map.MaxY = Data.ReadByte();

            // Get Max X & Y //
            byte MaxX = Types.Map.MaxX;
            byte MaxY = Types.Map.MaxY;

            // Clear Map //
            General.resizeMap(MaxX, MaxY);

            // Tiles //
            for (int X = 0; X <= MaxX - 1; X++)
            {
                for (int Y = 0; Y <= MaxY - 1; Y++)
                {
                    for (int I = 0; I <= 4; I++)
                    {
                        Types.Map.Tile[X, Y].Layer[I].X = Data.ReadByte();
                        Types.Map.Tile[X, Y].Layer[I].Y = Data.ReadByte();
                        Types.Map.Tile[X, Y].Layer[I].Tileset = Data.ReadByte();
                        Types.Map.Tile[X, Y].Autotile[I] = Data.ReadByte();
                    }
                    Types.Map.Tile[X, Y].Type = Data.ReadByte();
                    Types.Map.Tile[X, Y].Data1 = Data.ReadInt32();
                    Types.Map.Tile[X, Y].Data2 = Data.ReadInt32();
                    Types.Map.Tile[X, Y].Data3 = Data.ReadInt32();
                    Types.Map.Tile[X, Y].DirBlock = Data.ReadByte();
                }
            }

            // ****** Send Map SoundID ******
            for (int X = 0; X <= MaxX - 1; X++)
            {
                for (int Y = 0; Y <= MaxY - 1; Y++)
                {
                    Types.Map.SoundID[X, Y] = Data.ReadInt16();
                }
            }

            // Save //
            General.saveMap(mapNum);
        }

        // HANDLE MAP MSG //
        public static void HandleMapMsg(NetIncomingMessage Data)
        {
            // Read Packet Information
            string Msg = Data.ReadString();
            UI_Game.AddText(Msg, 1);
        }

        // HANDLE MAP MSG //
        public static void HandleCheckForMap(NetIncomingMessage Data)
        {

            // Erase all players except self //
            for (int i = 1; i <= Constant.MAX_PLAYERS - 1; i++)
            {
                if (i != Static.MyIndex)
                    Types.Players[i].Map = 0;
            }

            // Clear Temp Values //
            General.clearMap();
            General.clearMapNpcs();

            // Get Packet Info //
            int mapNum = Data.ReadInt32();
            int mapRev = Data.ReadInt32();

            // Check If Have Have Map //
            bool needMap = true;
            DirectoryInfo dir = new DirectoryInfo("Content\\Maps\\");
            string mapDir = dir.FullName + mapNum + ".bin";
            if (File.Exists(mapDir))
            {
                General.loadMap(mapNum);
                if (Types.Map.Revision == mapRev)
                    needMap = false;
            }

            // Send Data To Server //
            ClientTCP.SendNeedMap(needMap);
        }

        // HANDLE SPAWN NPC //
        public static void HandleSpawnNpc(NetIncomingMessage Data)
        {
            // Read Npc Num //
            Int32 mapNpcNum = Data.ReadInt32();

            // Npc Can Move //
            Types.MapNpcs[mapNpcNum].Name = Data.ReadString();
            Types.MapNpcs[mapNpcNum].Num = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].X = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].Y = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].Dir = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].Sprite = Data.ReadInt32();

            // Set Movment //
            Types.MapNpcs[mapNpcNum].OffsetX = 0;
            Types.MapNpcs[mapNpcNum].OffsetY = 0;
            Types.MapNpcs[mapNpcNum].Moving = 0;

            // Active Npc //
            Types.MapNpcs[mapNpcNum].Active = true;
        }

        // HANDLE SPAWN NPC //
        public static void HandleMoveNpc(NetIncomingMessage Data)
        {
            // Read Npc Num //
            Int32 mapNpcNum = Data.ReadInt32();

            // Npc Can Move //
            Types.MapNpcs[mapNpcNum].X = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].Y = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].Dir = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].OffsetX = 0;
            Types.MapNpcs[mapNpcNum].OffsetY = 0;
            Types.MapNpcs[mapNpcNum].Moving = 1;

            switch (Types.MapNpcs[mapNpcNum].Dir)
            {
                case Constant.DIR_NORTH:
                    Types.MapNpcs[mapNpcNum].OffsetY = 32;
                    break;
                case Constant.DIR_SOUTH:
                    Types.MapNpcs[mapNpcNum].OffsetY = 32 * -1;
                    break;
                case Constant.DIR_WEST:
                    Types.MapNpcs[mapNpcNum].OffsetX = 32;
                    break;
                case Constant.DIR_EAST:
                    Types.MapNpcs[mapNpcNum].OffsetX = 32 * -1;
                    break;
            }
        }

        // HANDLE SPAWN NPC //
        public static void HandleNpcDir(NetIncomingMessage Data)
        {
            // Read Npc Num //
            Int32 mapNpcNum = Data.ReadInt32();

            // Npc Can Move //
            Types.MapNpcs[mapNpcNum].Dir = Data.ReadInt32();
            Types.MapNpcs[mapNpcNum].OffsetX = 0;
            Types.MapNpcs[mapNpcNum].OffsetY = 0;
            Types.MapNpcs[mapNpcNum].Moving = 0;
        }

        // HANDLE MAP MSG //
        public static void HandleMapDone(NetIncomingMessage Data)
        {

            // Catch Map Tiles //
            General.cacheTiles();
            Autotiles.cacheAutotiles();

            // Play Map Music //
            GameContentManager.Instance.PlayMusic(Types.Map.Music);
        }

        // Handle Npc Dead //
        public static void HandleNpcDead(NetIncomingMessage Data)
        {
            // Read Npc Num //
            Int32 mapNpcNum = Data.ReadInt32();
            General.clearMapNpc(mapNpcNum);
        }

        // Handle Player Vitals //
        public static void HandlePlayerVital(NetIncomingMessage Data)
        {
            Int32 Vital = Data.ReadInt32();
            Types.Players[Static.MyIndex].MaxVitals[Vital] = Data.ReadInt32();
            Types.Players[Static.MyIndex].Vitals[Vital] = Data.ReadInt32();
        }

        // Handle Player Exp //
        public static void HandlePlayerExp(NetIncomingMessage Data)
        {
            Types.Players[Static.MyIndex].MaxExp = Data.ReadInt32();
            Types.Players[Static.MyIndex].Exp = Data.ReadInt32();
        }

    }
}
