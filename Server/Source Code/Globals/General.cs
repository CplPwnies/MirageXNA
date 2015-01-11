using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MirageXNA.Core;
using MirageXNA.Network;

namespace MirageXNA.Globals
{
    class General
    {

        // Game
        public static void joinGame(Int32 Index)
        {
            // Player Ingame //
            Types.Players[Index].InGame = true;

            // Update Total Players //
            Static.TotalPlayersOnline++;

            // Update the Log //
            Static.serverWindow.lvwInfo.Items[Index].SubItems[1].Text = Commands.getPlayerName(Index);
            Static.serverWindow.lvwInfo.Items[Index].SubItems[2].Text = Convert.ToString(Commands.getPlayerAccess(Index));
            Static.serverWindow.lvwInfo.Items[Index].SubItems[3].Text = String.Empty;

            // Make Room Here For Player Info //
            // SendInv
            // SendSpells

            // Game Elements //
            // SpawnAllMapNpcs

            // Send Vitals
            for (int I = 0; I <= (int)Enumerations.Vitals.vitalCount - 1; I++)
            {
                ServerTCP.SendVital(Index, (Enumerations.Vitals)I);
            }

            // Send Experiance
            ServerTCP.SendExp(Index);

            // Warp the player to his saved location //
            playerWarp(Index,
                Commands.getPlayerMap(Index),
                Commands.getPlayerX(Index),
                Commands.getPlayerY(Index));

            // Send Welcome Message //
            if (Static.TotalPlayersOnline - 1 > 0)
                ServerTCP.SendPlayerMsg(Index, "There are currently " + Static.TotalPlayersOnline + " players online.");
            else 
                ServerTCP.SendPlayerMsg(Index, "There is no one online.");

            ServerTCP.SendMapMsg(Index, Types.Players[Index].Name + " has joined " + Constant.GAME_NAME);
            Static.TextAdd(Types.Players[Index].Name + " has enterd the game.");
        }
        public static void leftGame(int Index)
        {
            if (Types.Players[Index].InGame)
            {
                // Update Total Players //
                Static.TotalPlayersOnline--;

                // Update the Log //
                Static.serverWindow.lvwInfo.Items[Index].SubItems[1].Text = String.Empty;
                Static.serverWindow.lvwInfo.Items[Index].SubItems[2].Text = String.Empty;
                Static.serverWindow.lvwInfo.Items[Index].SubItems[3].Text = String.Empty;

                // Save Player //
                Database.savePlayer(Index);

                // Send Leave Message //
                ServerTCP.SendMapMsg(Index, Types.Players[Index].Name + " has left " + Constant.GAME_NAME);
                Static.TextAdd(Types.Players[Index].Name + " has left the game.");

                // Clear Player - Update Clients //
                clearPlayer(Index);
                ServerTCP.SendLeftGame(Index);
            }
        }

        // Clear Players
        public static void clearAllPlayers()
        {
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS - 1; LoopI++)
            {
                clearPlayer(LoopI);
            }
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

            Types.Players[Index].InGame = false;
        }

        // Clear Map Npcs
        public static void clearAllMapNpcs()
        {
            for (int mapNum = 0; mapNum <= Constant.MAX_MAPS - 1; mapNum++)
            {
                Types.MapNpcs[mapNum].Npc = new Types.MapNpc_Struct[Constant.MAX_MAP_NPCS + 1];
                for (int mapNpcNum = 1; mapNpcNum <= Constant.MAX_MAP_NPCS; mapNpcNum++)
                {
                    clearMapNpc(mapNum, mapNpcNum);
                }
            }
        }
        public static void clearMapNpc(int mapNum, int mapNpcNum)
        {
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Num = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Sprite = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Range = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].X = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Y = -1;

            Types.MapNpcs[mapNum].Npc[mapNpcNum].Vitals = new int[(int)Enumerations.Vitals.vitalCount];
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Vitals[(int)Enumerations.Vitals.HP] = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Vitals[(int)Enumerations.Vitals.MP] = -1;
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Vitals[(int)Enumerations.Vitals.SP] = -1;
        }

        // Player Warp
        public static void playerWarp(int Index, int mapNum = 0, int X = 0, int Y = 0)
        {
            int oldMap;

            // Check for subscript out of range
            if ((!ServerTCP.IsPlaying(Index)) | (mapNum <= 0) | (mapNum > Constant.MAX_MAPS))
                return;

            // Check if you are out of bounds
            if (X > Types.Map[mapNum].MaxX) { X = Types.Map[mapNum].MaxX; }
            if (Y > Types.Map[mapNum].MaxY) { Y = Types.Map[mapNum].MaxY; }

            // Save old map to send erase player data to
            oldMap = Types.Players[Index].Map;

            if (oldMap != mapNum)
            {
                // REASON I DONT NEED THIS YET IS BECAUSE WHEN I USE SENDJOINMAP,
                // I SEND TO ALL NOT JUST THE PLAYERS ON THE MAP.
                // THIS UPDATES ALL THE CLIENTS SO NO NEED TO LEAVE MAP.
                // HOW EVER THIS WILL BE ADDED IN.
                //SendLeaveMap(Index, OldMap)
            }

            // Set Player Position //
            Types.Players[Index].Map = mapNum;
            Types.Players[Index].X = X;
            Types.Players[Index].Y = Y;
    
            ServerTCP.SendCheckMap(Index, mapNum, Types.Map[mapNum].Revision);
        }

        // Check Player Level Up
        public static void CheckPlayerLevelUp(int Index)
        {    
            int level_count = 0;
            while (Commands.getPlayerExp(Index) >= Commands.getPlayerMaxExp(Index)) 
            {
                int expRollover = Commands.getPlayerExp(Index) - Commands.getPlayerMaxExp(Index);
        
                // can level up?
                if (!Commands.setPlayerLevel(Index, Commands.getPlayerLevel(Index) + 1))
                    return;

                Commands.setPlayerPoints(Index, Commands.getPlayerPoints(Index) + Constant.MAX_POINTS);
                Commands.setPlayerExp(Index, expRollover);
                level_count = level_count + 1;
            } 
    
            if (level_count > 0)
            {
                if (level_count == 1)
                    ServerTCP.SendPlayerMsg(Index, "You have gained " + level_count + " level!");
                else
                    ServerTCP.SendPlayerMsg(Index, "You have gained " + level_count + " levels!");

                //SendEXP index
                ServerTCP.SendDataTo(Index, ServerTCP.SendPlayerData(Index), Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
                ServerTCP.SendVital(Index, Enumerations.Vitals.HP);
                ServerTCP.SendVital(Index, Enumerations.Vitals.MP);
                ServerTCP.SendVital(Index, Enumerations.Vitals.SP);
            }
        }
    }
}
