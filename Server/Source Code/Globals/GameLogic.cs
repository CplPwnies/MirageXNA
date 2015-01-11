using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MirageXNA.Core;
using MirageXNA.Network;

namespace MirageXNA.Globals
{
    public class GameLogic
    {

        // MAJOR OVERHAUL OF THIS SUB MUCH BETTER THAN BEFORE - MARK MORRIS
        public static void PlayerMove(Int32 Index, int Dir, int Movement)
        {
            int NewX = 0;
            int NewY = 0;
            int NewMapX = 0;
            int NewMapY = 0;

            bool isPlaying = ServerTCP.IsPlaying(Index);

            // Check for subscript out of range
            if (isPlaying = false | Dir < Constant.DIR_NORTH | Dir > Constant.DIR_EAST | Movement < 1 | Movement > 2)
            {
                return;
            }

            Types.Players[Index].Dir = Dir;
            //Moved = NO
            int mapNum = Types.Players[Index].Map;

            switch (Dir)
            {
                case Constant.DIR_NORTH:
                    NewX = Types.Players[Index].X;
                    NewY = Types.Players[Index].Y - 1;
                    break;
                case Constant.DIR_SOUTH:
                    NewX = Types.Players[Index].X;
                    NewY = Types.Players[Index].Y + 1;
                    break;
                case Constant.DIR_WEST:
                    NewX = Types.Players[Index].X - 1;
                    NewY = Types.Players[Index].Y;
                    break;
                case Constant.DIR_EAST:
                    NewX = Types.Players[Index].X + 1;
                    NewY = Types.Players[Index].Y;
                    break;
            }

            // CHECK BOUNDRIES
            if (Dir == Constant.DIR_NORTH)
            {
                if (Types.Players[Index].Y <= 0)
                {
                    if (Types.Map[mapNum].Up > 0)
                    {
                        NewMapY = Types.Map[Types.Map[mapNum].Up].MaxY - 1;
                        General.playerWarp(Index, Types.Map[Commands.getPlayerMap(Index)].Up, Commands.getPlayerX(Index), NewMapY);
                        //Log GetPlayerName(Index) & " Warped to map" & GetPlayerMap(Index), CodeTracker '//\\LOGLINE//\\
                        return;
                    }
                }
            }

            if (Dir == Constant.DIR_SOUTH)
            {
                if (Types.Players[Index].Y >= Types.Map[mapNum].MaxY)
                {
                    if (Types.Map[mapNum].Down > 0)
                    {
                        NewMapY = 0;
                        General.playerWarp(Index, Types.Map[Commands.getPlayerMap(Index)].Down, Commands.getPlayerX(Index), NewMapY);
                        //Log GetPlayerName(Index) & " Warped to map" & GetPlayerMap(Index), CodeTracker '//\\LOGLINE//\\
                        return;
                    }
                }
            }

            if (Dir == Constant.DIR_WEST)
            {
                if (Types.Players[Index].X <= 0)
                {
                    if (Types.Map[mapNum].Left > 0)
                    {
                        NewMapX = Types.Map[Types.Map[mapNum].Left].MaxX;
                        General.playerWarp(Index, Types.Map[Commands.getPlayerMap(Index)].Left, NewMapX, Commands.getPlayerY(Index));
                        //Log GetPlayerName(Index) & " Warped to map" & GetPlayerMap(Index), CodeTracker '//\\LOGLINE//\\
                        return;
                    }
                }
            }

            if (Dir == Constant.DIR_EAST)
            {
                if (Types.Players[Index].X >= Types.Map[mapNum].MaxX)
                {
                    if (Types.Map[mapNum].Right > 0)
                    {
                        NewMapX = 0;
                        General.playerWarp(Index, Types.Map[Commands.getPlayerMap(Index)].Right, NewMapX, Commands.getPlayerY(Index));
                        //Log GetPlayerName(Index) & " Warped to map" & GetPlayerMap(Index), CodeTracker '//\\LOGLINE//\\
                        return;
                    }
                }
            }

            // Move Player On Free Tile //
            Types.Players[Index].X = NewX;
            Types.Players[Index].Y = NewY;
            ServerTCP.SendPlayerMovement(Index, Movement);
        }

        public static void cacheMapNpcs(int mapNum)
        {
            Types.NpcCache[mapNum].Npc = new Types.NpcCache_Struct[Constant.MAX_MAP_NPCS + 1];
            
            int mapNpcNum = 1;
            for (int X = 0; X <= Types.Map[mapNum].MaxX - 1; X++)
            {
                for (int Y = 0; Y <= Types.Map[mapNum].MaxY - 1; Y++)
                {
                    if (Types.Map[mapNum].Tile[X, Y].Type == Constant.TILE_TYPE_NPCSPAWN)
                    {
                        if (Types.Map[mapNum].Tile[X, Y].Data1 >= 0)
                        {
                            if (mapNpcNum < Constant.MAX_MAP_NPCS)
                            {
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Num = Types.Map[mapNum].Tile[X, Y].Data1;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Sprite = Types.Npc[Types.NpcCache[mapNum].Npc[mapNpcNum].Num].Sprite;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Dir = Types.Map[mapNum].Tile[X, Y].Data2;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].X = X;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Y = Y;
                                mapNpcNum++;
                                break;
                            }
                        }
                    }
                }
            }
            Types.NpcCache[mapNum].maxNpcs = mapNpcNum;
        }

        public void cacheAllNpcs()
        {
            int mapNpcNum = 0;
            for (int mapNum = 1; mapNum <= Constant.MAX_MAPS; mapNum++)
            {
                for (int X = 0; X <= Types.Map[mapNum].MaxX - 1; X++)
                {
                    for (int Y = 0; Y <= Types.Map[mapNum].MaxY - 1; Y++)
                    {
                        if (Types.Map[mapNum].Tile[X, Y].Type == Constant.TILE_TYPE_NPCSPAWN)
                        {
                            if (Types.Map[mapNum].Tile[X, Y].Data1 >= 0)
                            {
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Num = Types.Map[mapNum].Tile[X, Y].Data1;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Sprite = Types.Npc[Types.MapNpcs[mapNum].Npc[mapNpcNum].Num].Sprite;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Dir = Types.Map[mapNum].Tile[X, Y].Data2;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].X = X;
                                Types.NpcCache[mapNum].Npc[mapNpcNum].Y = Y;
                                mapNpcNum++;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void SpawnAllNpcs()
        {
            for (int i = 1; i <= Constant.MAX_MAPS - 1; i++)
            {
                GameLogic.cacheMapNpcs(i);
                SpawnMapNpcs(i);
            }
        }

        public static void SpawnMapNpcs(int mapNum)
        {
            for (int i = 1; i <= Constant.MAX_MAP_NPCS - 1; i++)
            {
                if (Types.NpcCache[mapNum].Npc[i].Num > 0)
                    SpawnNpc(i, mapNum);
            }
        }

        public static void SpawnNpc(int mapNpcNum, int mapNum)
        {
            bool Spawned = false;

            // Check for subscript out of range
            if (mapNpcNum <= 0 || mapNpcNum > Constant.MAX_MAP_NPCS - 1 || mapNum <= 0 || mapNum > Constant.MAX_MAPS - 1) return;

            int NpcNum = Types.NpcCache[mapNum].Npc[mapNpcNum].Num;

            if (NpcNum > 0)
            {
                // Set Npc //
                Types.MapNpcs[mapNum].Npc[mapNpcNum].Sprite = Types.NpcCache[mapNum].Npc[mapNpcNum].Sprite;
                Types.MapNpcs[mapNum].Npc[mapNpcNum].Num = NpcNum;
                Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = 0;
                Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = 0;
                Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir = Constant.DIR_SOUTH;

                // Set Vitals
                Types.MapNpcs[mapNum].Npc[mapNpcNum].Vitals[(int)Enumerations.Vitals.HP] = Types.Npc[NpcNum].Health;

                // Spawn //
                if (Types.NpcCache[mapNum].Npc[mapNpcNum].X >= 0 && Types.NpcCache[mapNum].Npc[mapNpcNum].X < Types.Map[mapNum].MaxX - 1)
                {
                    if (Types.NpcCache[mapNum].Npc[mapNpcNum].Y >= 0 && Types.NpcCache[mapNum].Npc[mapNpcNum].Y < Types.Map[mapNum].MaxY - 1)
                    {
                        Types.MapNpcs[mapNum].Npc[mapNpcNum].X = Types.NpcCache[mapNum].Npc[mapNpcNum].X;
                        Types.MapNpcs[mapNum].Npc[mapNpcNum].Y = Types.NpcCache[mapNum].Npc[mapNpcNum].Y;
                        Spawned = true;
                    }
                }

                // Find Free Tile //
                if (!Spawned)
                {
                    for (int X = 0; X <= Types.Map[mapNum].MaxX; X++)
                    {
                        for (int Y = 0; Y <= Types.Map[mapNum].MaxY; Y++)
                        {
                            if (NpcTileIsOpen(mapNum, X, Y))
                            {
                                Types.MapNpcs[mapNum].Npc[mapNpcNum].X = X;
                                Types.MapNpcs[mapNum].Npc[mapNpcNum].Y = Y;
                                Spawned = true;
                            }
                        }
                    }
                }

                // If we suceeded in spawning then send it to everyone
                if (Spawned)
                    ServerTCP.SendSpawnNpc(mapNum, mapNpcNum);
            }

        }

        public static bool NpcTileIsOpen(int mapNum, int X, int Y)
        {

            // CHECK FOR PLAYERS ON MAP //
            if (Static.PlayersOnMap[mapNum])
            {
                for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS - 1; LoopI++)
                {
                    if (ServerTCP.IsPlaying(LoopI))
                    {
                        if (Commands.getPlayerMap(LoopI) == mapNum)
                        {
                            if (Commands.getPlayerX(LoopI) == X)
                            {
                                if (Commands.getPlayerY(LoopI) == Y)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            
            // CHECK FOR NPCS ON MAP //
            for (int LoopI = 1; LoopI <= Constant.MAX_MAP_NPCS - 1; LoopI++)
            {
                if (Types.MapNpcs[mapNum].Npc[LoopI].Num > 0)
                {
                    if (Types.MapNpcs[mapNum].Npc[LoopI].X == X)
                    {
                        if (Types.MapNpcs[mapNum].Npc[LoopI].Y == Y)
                        {
                            return false;
                        }
                    }
                }
            }

            // CHECK TILE TYPE //
            if (Types.Map[mapNum].Tile[X, Y].Type != Constant.TILE_TYPE_WALKABLE)
            {
                if (Types.Map[mapNum].Tile[X, Y].Type != Constant.TILE_TYPE_NPCSPAWN)
                {
                    if (Types.Map[mapNum].Tile[X, Y].Type != Constant.TILE_TYPE_ITEM)
                    {
                        return false;
                    }
                }
            }

            return true;
    
        }

        // ################################
        // ##      Basic Calculations    ##
        // ################################

        public static int DirToX(byte X, byte Dir)
        {
            // Set Position
            int DirToX = X;
    
            if (Dir == Constant.DIR_NORTH)
                return X;
    
            if (Dir == Constant.DIR_SOUTH)
                return X;
    
            // Calc
            DirToX = X + ((Dir * 2) - 5);
            return DirToX;
        }
        public static int DirToY(byte Y, byte Dir)
        {
    
            // Set Position
            int DirToY = Y;

            if (Dir == Constant.DIR_WEST)
                return Y;
    
            if (Dir == Constant.DIR_EAST)
                return Y;
    
            // ****** Calc ******
            DirToY = Y + ((Dir * 2) - 1);
            return DirToY;
        }
    
    }
}
