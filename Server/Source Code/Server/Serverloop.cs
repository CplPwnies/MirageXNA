using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Lidgren.Network;

using MirageXNA.Network;
using MirageXNA.Globals;
using MirageXNA.Core;

namespace MirageXNA.Core
{
    class Serverloop
    {
        // Adjust these values accordingly depending on how often you want routines to update
        // Low values = faster updating (smoother gameplay), but more CPU usage
        public static long UpdateRate_SpellBuffer = 200;
        public static long UpdateRate_GameAI = 500;
        public static long UpdateRate_ShutDown = 1000;
        public static long UpdateRate_UserStopRegen = 5000;
        public static long UpdateRate_NpcStopRegen = 5000;
        public static long UpdateRate_UserRecover = 10000;
        public static long UpdateRate_NpcRecover = 10000;
        public static long UpdateRate_MapItems = 300000;
        public static long UpdateRate_SavePlayers = 600000;

        // Holds the last time an update was made for any of the above constants
        public static long LastUpdate_SpellBuffer;
        public static long LastUpdate_GameAI;
        public static long LastUpdate_ShutDown;
        public static long LastUpdate_UserStopRegen;
        public static long LastUpdate_NpcStopRegen;
        public static long LastUpdate_UserRecover;
        public static long LastUpdate_NpcRecover;
        public static long LastUpdate_MapItems;
        public static long LastUpdate_SavePlayers;

        // Stop Excessive Looping //
        public static bool UpdateUserRecover = false;
        public static bool UpdateUserStopRegen = false;
        public static bool UpdateUserSpells = false;
        public static bool UpdateNpcMovement = false;
        public static bool UpdateNpcRecover = false;
        public static bool UpdateNpcStopRegen = false;

        public static void Loop(object sender, EventArgs e)
        {
            bool UpdateNpc = false;
            bool UpdatePlayer = false;

            while (Static.ServerRunning)
            {
                long Tick = Environment.TickCount;

                // Check for Disconnections //
                for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS - 1; LoopI++)
                {
                    if (Types.Players[LoopI].InGame)
                    {
                        if (!ServerTCP.IsConnected(LoopI))
                        {
                            General.leftGame(LoopI);
                            Static.connectedClients.Insert(LoopI, null);
                            General.clearPlayer(LoopI);
                        }
                    }
                }

                // Player Recovery //
                if (LastUpdate_UserRecover + UpdateRate_UserRecover < Tick)
                {
                    UpdateUserRecover = true;
                    LastUpdate_UserRecover = Tick;
                    UpdatePlayer = true;
                }

                // Can Player Recovery //
                if (LastUpdate_UserStopRegen + UpdateRate_UserStopRegen < Tick)
                {
                    UpdateUserStopRegen = true;
                    LastUpdate_UserStopRegen = Tick;
                    UpdatePlayer = true;
                }

                // Npc AI //
                if (LastUpdate_GameAI + UpdateRate_GameAI < Tick)
                {
                    UpdateNpcMovement = true;
                    LastUpdate_GameAI = Tick;
                    UpdateNpc = true;
                }

                // Incoming Data //
                HandleTCP.ReceivedData();

                // Update Player //
                if (UpdatePlayer)
                {
                    UpdatePlayerAI();
                    UpdatePlayer = false;
                }

                // Update NPC AI //
                if (UpdateNpc)
                {
                    UpdateNpcAI();
                    UpdateNpc = false;
                }

                // Let Windows Process Other Events //
                Application.DoEvents();
            }
        }

        public static void UpdatePlayerAI()
        {
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                if (ServerTCP.IsPlaying(LoopI))
                {

                    // ****** Stop Regen Timer ******
                    if (UpdateUserStopRegen)
                    {
                        if (Types.Players[LoopI].StopRegen)
                        {
                            if (Types.Players[LoopI].StopRegenTimer + 5000 < Environment.TickCount)
                            {
                                Types.Players[LoopI].StopRegen = false;
                                Types.Players[LoopI].StopRegenTimer = 0;
                            }
                        }
                    }

                    // ****** Regen Timer ******
                    if (UpdateUserRecover)
                    {
                        if (!Types.Players[LoopI].StopRegen)
                        {
                            // ****** Regen HP ******
                            if (Commands.getPlayerVital(LoopI, Enumerations.Vitals.HP) != Commands.getPlayerMaxVital(LoopI, Enumerations.Vitals.HP))
                            {
                                Commands.setPlayerVital(LoopI, Enumerations.Vitals.HP, Commands.getPlayerVital(LoopI, Enumerations.Vitals.HP) + 10);
                                ServerTCP.SendVital(LoopI, Enumerations.Vitals.HP);
                            }

                            // ****** Regen MP ******
                            if (Commands.getPlayerVital(LoopI, Enumerations.Vitals.MP) != Commands.getPlayerMaxVital(LoopI, Enumerations.Vitals.MP))
                            {
                                Commands.setPlayerVital(LoopI, Enumerations.Vitals.MP, Commands.getPlayerVital(LoopI, Enumerations.Vitals.MP) + 10);
                                ServerTCP.SendVital(LoopI, Enumerations.Vitals.MP);
                            }

                            // ****** Regen SP ******
                            if (Commands.getPlayerVital(LoopI, Enumerations.Vitals.SP) != Commands.getPlayerMaxVital(LoopI, Enumerations.Vitals.SP))
                            {
                                Commands.setPlayerVital(LoopI, Enumerations.Vitals.SP, Commands.getPlayerVital(LoopI, Enumerations.Vitals.SP) + 10);
                                ServerTCP.SendVital(LoopI, Enumerations.Vitals.SP);
                            }
                        }
                    }
                }
            }
        }

        public static void UpdateNpcAI()
        {
            int N;
            int TickCount = Environment.TickCount;
            int NpcNum;
            int Target = 0;
            int TargetType = 0;
            bool DidWalk = false;
            int TargetX = 0;
            int TargetY= 0;
            bool hasTarget;

            ///////////////////////////////////////
            ////// Loop Through All The Maps //////
            ///////////////////////////////////////
            for (int mapNum = 1; mapNum <= Constant.MAX_MAPS - 1; mapNum++)
            {
                //if (Static.PlayersOnMap[mapNum])
                //{
                    //TickCount = GetTickCount;
                    for (int mapNpcNum = 1; mapNpcNum <= Constant.MAX_MAP_NPCS - 1; mapNpcNum++)
                    {
                        NpcNum = Types.MapNpcs[mapNum].Npc[mapNpcNum].Num;

                        if (UpdateNpcMovement)
                        {

                            /////////////////////////////////////////////////
                            ////// This is used for ATTACKING ON SIGHT //////
                            /////////////////////////////////////////////////

                            // Make sure theres a npc with the map
                            if (NpcNum > -1)
                            {
                                // If the npc is a attack on sight, search for a player on the map
                                if ((Types.Npc[NpcNum].Behaviour == ((byte)Constant.NPC_BEHAVIOUR_ATTACK_ROAMING)) || (Types.Npc[NpcNum].Behaviour == ((byte)Constant.NPC_BEHAVIOUR_ATTACK_STANDING)))
                                {

                                    /////////////////////////////
                                    ////// PLAYER IN RANGE //////
                                    /////////////////////////////
                                    for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
                                    {
                                        if (Commands.getPlayerMap(LoopI) == mapNum && Types.MapNpcs[mapNum].Npc[mapNpcNum].Target == 0 && Commands.getPlayerAccess(LoopI) <= Constant.ADMIN_MONITOR)
                                        {
                                            // ****** Set Range ******
                                            N = Types.Npc[NpcNum].Range;

                                            int DistanceX = Types.MapNpcs[mapNum].Npc[mapNpcNum].X - Commands.getPlayerX(LoopI);
                                            int DistanceY = Types.MapNpcs[mapNum].Npc[mapNpcNum].Y - Commands.getPlayerY(LoopI);

                                            // Make sure we get a positive value
                                            if (DistanceX < 0) DistanceX = DistanceX * -1;
                                            if (DistanceY < 0) DistanceY = DistanceY * -1;

                                            // Are they in range?  if so GET'M!
                                            if ((DistanceX <= N) && (DistanceY <= N))
                                            {
                                                if (Types.Npc[NpcNum].Behaviour == Constant.NPC_BEHAVIOUR_ATTACK_ROAMING)
                                                {
                                                    Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = Constant.TARGET_TYPE_PLAYER;
                                                    Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = LoopI;
                                                }
                                            }
                                        }
                                    }

                                    //////////////////////////
                                    ////// NPC IN RANGE //////
                                    //////////////////////////
                                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Target == 0)
                                    {
                                        for (int LoopI = 1; LoopI <= Constant.MAX_MAP_NPCS - 1; LoopI++)
                                        {
                                            if (LoopI != mapNpcNum)
                                            {
                                                if (Types.MapNpcs[mapNum].Npc[LoopI].Num > 0)
                                                {

                                                    N = Types.Npc[NpcNum].Range;

                                                    int DistanceX = Types.MapNpcs[mapNum].Npc[mapNpcNum].X - Types.MapNpcs[mapNum].Npc[LoopI].X;
                                                    int DistanceY = Types.MapNpcs[mapNum].Npc[mapNpcNum].Y - Types.MapNpcs[mapNum].Npc[LoopI].Y;

                                                    // Make sure we get a positive value
                                                    if (DistanceX < 0) DistanceX = DistanceX * -1;
                                                    if (DistanceY < 0) DistanceY = DistanceY * -1;

                                                    // Are they in range?  if so GET'M!
                                                    if ((DistanceX <= N) && (DistanceY <= N))
                                                    {
                                                        if (Types.Npc[NpcNum].Behaviour == Constant.NPC_BEHAVIOUR_ATTACK_ROAMING)
                                                        {
                                                            Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = Constant.TARGET_TYPE_NPC;
                                                            Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = LoopI;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }

                            hasTarget = false;

                            /////////////////////////////////////////////
                            // This is used for NPC walking/targetting //
                            /////////////////////////////////////////////

                            // Make sure theres a npc with the map
                            if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Num > 0)
                            {
                                // ****** Walk/Target ******
                                Target = Types.MapNpcs[mapNum].Npc[mapNpcNum].Target;
                                TargetType = Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType;

                                // Check to see if its time for the npc to walk
                                if (Types.Npc[NpcNum].Behaviour != Constant.NPC_BEHAVIOUR_SELLER_ROAMING || Types.Npc[NpcNum].Behaviour != Constant.NPC_BEHAVIOUR_SELLER_STANDING)
                                {
                                    if (TargetType == Constant.TARGET_TYPE_PLAYER)
                                    {
                                        if (Target > 0)
                                        {
                                            if (ServerTCP.IsPlaying(Target) && Commands.getPlayerMap(Target) == mapNum)
                                            {
                                                DidWalk = false;
                                                hasTarget = true;
                                                TargetY = Commands.getPlayerY(Target);
                                                TargetX = Commands.getPlayerX(Target);
                                            }
                                            else
                                            {
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = Constant.TARGET_TYPE_NONE;
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = Constant.TARGET_TYPE_NONE;
                                            }
                                        }
                                    }
                                    else if (TargetType == Constant.TARGET_TYPE_NPC)
                                    {
                                        if (Target > 0)
                                        {
                                            if (Types.MapNpcs[mapNum].Npc[Target].Num > 0)
                                            {
                                                DidWalk = false;
                                                hasTarget = true;
                                                TargetY = Types.MapNpcs[mapNum].Npc[Target].Y;
                                                TargetX = Types.MapNpcs[mapNum].Npc[Target].X;
                                            }
                                            else
                                            {
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = Constant.TARGET_TYPE_NONE;
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = Constant.TARGET_TYPE_NONE;
                                            }
                                        }
                                    }

                                    if (hasTarget)
                                    {
                                        if (!DidWalk)
                                        DidWalk = AI_NpcMove(mapNum, mapNpcNum, TargetX, TargetY);
                                        if (!DidWalk)
                                        AI_NpcCollision(mapNum, mapNpcNum, Target);
                                    }
                                    else
                                    {
                                        if (Static.Rnd.Next(0, 4) == 1)
                                        {
                                            int Dir = (Static.Rnd.Next(0, 4));
                                            if (CanNpcMove(mapNum, mapNpcNum, Dir))
                                            {
                                                NpcMove(mapNum, mapNpcNum, Dir);
                                            }
                                        }
                                    }

                                }
                            }

                            /////////////////////////////////////////////
                            // This is used for npcs to attack targets //
                            /////////////////////////////////////////////
                            if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Num > 0)
                            {
                                Target = Types.MapNpcs[mapNum].Npc[mapNpcNum].Target;
                                TargetType = Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType;

                                if (Target > 0)
                                {

                                    switch (TargetType)
                                    {

                                        case Constant.TARGET_TYPE_PLAYER:

                                            if (ServerTCP.IsPlaying(Target) && Commands.getPlayerMap(Target) == mapNum)
                                            {
                                                //If Npc(MapNpc(mapNum).Npc(X).Num).Stat(Stats.Willpower) > 0 Then
                                                //   If Npc(MapNpc(mapNum).Npc(X).Num).Skill(1) > 0 Then
                                                //        Call Npc_Skill_AddToBuffer(Target, mapNum, X)
                                                //    End If
                                                //End If
                                                Combat.NpcAttackPlayer(mapNpcNum, Target);
                                            }
                                            else
                                            {
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = 0;        // Player left map or game, set target to 0
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = 0;    // clear
                                            }

                                            break;

                                        case Constant.TARGET_TYPE_NPC:

                                            if (Types.MapNpcs[mapNum].Npc[Target].Num > 0)
                                            {
                                                //If CanNpcAttackNpc(mapNum, X, Target) Then
                                                //    Damage = GetNpcDamage(NpcNum) - GetNpcProtection(MapNpc(mapNum).Npc(Target).Num)
                                                //    If Damage < 1 Then Damage = 1
                                                //    Call NpcAttackNpc(mapNum, X, Target, Damage)
                                                //End If
                                            }
                                            else
                                            {
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].Target = 0;        // Player left map or game, set target to 0
                                                Types.MapNpcs[mapNum].Npc[mapNpcNum].TargetType = 0;    // clear
                                            }

                                            break;
                                    }
                                }
                            }

                        }

                        // Spawning an NPC
                        if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Num == 0 && Types.NpcCache[mapNum].Npc[mapNpcNum].Num > 0)
                        {
                            if (TickCount > Types.MapNpcs[mapNum].Npc[mapNpcNum].SpawnWait + (Types.Npc[Types.NpcCache[mapNum].Npc[mapNpcNum].Num].SpawnSecs * 1000))
                            {
                                GameLogic.SpawnNpc(mapNpcNum, mapNum);
                            }
                        }

                        // DO EVENTS //
                        Application.DoEvents();
                    }

                //}
            }

            UpdateNpcMovement = false;
            UpdateNpcRecover = false;
        }

        public static bool AI_NpcMove(int mapNum, int mapNpcNum, int TargetX, int TargetY)
        {
            // Lets move the npc //
            switch (Static.Rnd.Next(0, 4))
            {
                case 0:

                    ////// NORTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y > TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH);
                            return true;
                        }
                    }

                    ////// SOUTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y < TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH);
                            return true;
                        }
                    }

                    ////// WEST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X > TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_WEST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_WEST);
                            return true;
                        }
                    }

                    ////// EAST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X < TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_EAST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_EAST);
                            return true;
                        }
                    }
                    break;


                case 1:

                    ////// EAST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X < TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_EAST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_EAST);
                            return true;
                        }
                    }

                    ////// WEST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X > TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_WEST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_WEST);
                            return true;
                        }
                    }

                    ////// SOUTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y < TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH);
                            return true;
                        }
                    }

                    ////// NORTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y > TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH);
                            return true;
                        }
                    }

                    break;

                case 2:

                    ////// SOUTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y < TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH);
                            return true;
                        }
                    }

                    ////// NORTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y > TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH);
                            return true;
                        }
                    }

                    ////// EAST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X < TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_EAST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_EAST);
                            return true;
                        }
                    }

                    ////// WEST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X > TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_WEST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_WEST);
                            return true;
                        }
                    }

                    break;

                case 3:

                    ////// WEST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X > TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_WEST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_WEST);
                            return true;
                        }
                    }

                    ////// EAST //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X < TargetX)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_EAST))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_EAST);
                            return true;
                        }
                    }

                    ////// NORTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y > TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_NORTH);
                            return true;
                        }
                    }

                    ////// SOUTH //////
                    if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Y < TargetY)
                    {
                        if (CanNpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH))
                        {
                            NpcMove(mapNum, mapNpcNum, Constant.DIR_SOUTH);
                            return true;
                        }
                    }

                    break;
            }

            return false;
        }

        public static void AI_NpcCollision(int mapNum, int mapNpcNum, int Target)
        {
            // Check if we can't move and if player is behind something and if we can just switch dirs
            if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X - 1 == Commands.getPlayerX(Target) && Types.MapNpcs[mapNum].Npc[mapNpcNum].Y == Commands.getPlayerY(Target))
            {
                if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir != Constant.DIR_WEST) NpcDir(mapNum, mapNpcNum, Constant.DIR_WEST);
                return;
            }
    
            if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X + 1 == Commands.getPlayerX(Target) && Types.MapNpcs[mapNum].Npc[mapNpcNum].Y == Commands.getPlayerY(Target))
            {
                if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir != Constant.DIR_EAST) NpcDir(mapNum, mapNpcNum, Constant.DIR_EAST);
                return;
            }

            if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X == Commands.getPlayerX(Target) && Types.MapNpcs[mapNum].Npc[mapNpcNum].Y - 1 == Commands.getPlayerY(Target))
            {
                if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir != Constant.DIR_NORTH) NpcDir(mapNum, mapNpcNum, Constant.DIR_NORTH);
                return;
            }

            if (Types.MapNpcs[mapNum].Npc[mapNpcNum].X == Commands.getPlayerX(Target) && Types.MapNpcs[mapNum].Npc[mapNpcNum].Y + 1 == Commands.getPlayerY(Target))
            {
                if (Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir != Constant.DIR_SOUTH) NpcDir(mapNum, mapNpcNum, Constant.DIR_SOUTH);
                return;
            }

             // We could not move so player must be behind something, walk randomly.
            if (Static.Rnd.Next(0, 2) == 1)
            {
                int Dir = (byte)(Static.Rnd.Next(0, 4));
                if (CanNpcMove(mapNum, mapNpcNum, Dir))
                {
                    NpcMove(mapNum, mapNpcNum, Dir);
                }
            }
    
        }

        public static bool CanNpcMove(int mapNum, int mapNpcNum, int Dir)
        {
            int NewX = 0;
            int NewY = 0;
    
            // Check for subscript out of range
            if (mapNum <= 0 || mapNum > Constant.MAX_MAPS - 1 || mapNpcNum <= 0 || mapNpcNum > Constant.MAX_MAP_NPCS - 1 || Dir < Constant.DIR_NORTH || Dir > Constant.DIR_EAST)
                //Log "Subscript Out of Range: Sub CanNpcMove", General '//\\LOGLINE//\\
                return false;
    
            bool canMove = true;
            int X = Types.MapNpcs[mapNum].Npc[mapNpcNum].X;
            int Y = Types.MapNpcs[mapNum].Npc[mapNpcNum].Y;

            switch (Dir)
            {
                case Constant.DIR_NORTH:
                    if (Y > 0)
                    {
                        NewX = X;
                        NewY = Y - 1;
                    }
                    else canMove = false; break;
                case Constant.DIR_SOUTH:
                    if (Y < Types.Map[mapNum].MaxY)
                    {
                        NewX = X;
                        NewY = Y + 1;
                    }
                    else canMove = false; break;
                case Constant.DIR_WEST:
                    if (X > 0)
                    {
                        NewX = X - 1;
                        NewY = Y;
                    }
                    else canMove = false; break;
                case Constant.DIR_EAST:
                    if (X < Types.Map[mapNum].MaxX)
                    {
                        NewX = X + 1;
                        NewY = Y;
                    }
                    else canMove = false; break;
            }
            
            if (NewX < 0) canMove = false;
            if (NewY < 0) canMove = false;
            if (NewX > Types.Map[mapNum].MaxX - 1) canMove = false;
            if (NewY > Types.Map[mapNum].MaxY - 1) canMove = false;

            ////// Check Boundries //////
            if (!canMove)
                //Log "Npc_Boundry_Check = " & False, CodeTracker
                return false;

            // Check Map Tiles //
            switch (Types.Map[mapNum].Tile[NewX, NewY].Type)
            {
                case Constant.TILE_TYPE_BLOCKED: return false;
                case Constant.TILE_TYPE_NPCAVOID: return false;
            }

            // Check Players //
            for (int i = 1; i <= Constant.MAX_PLAYERS; i++)
            {
                if (ServerTCP.IsPlaying(i))
                {
                    if (Types.Players[i].Map == mapNum)
                    {
                        if (Types.Players[i].X == NewX)
                        {
                            if (Types.Players[i].Y == NewY)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // Check Npcs //
            for (int i = 1; i <= Constant.MAX_MAP_NPCS; i++)
            {
                if (i != mapNpcNum)
                {
                    if (Types.MapNpcs[mapNum].Npc[i].Num > 0)
                    {
                        if (Types.MapNpcs[mapNum].Npc[i].X == NewX)
                        {
                            if (Types.MapNpcs[mapNum].Npc[i].Y == NewY)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            ////// Can Move //////
            return true;

        }

        public static void NpcMove(int mapNum, int mapNpcNum, int Dir)
        {
            // Check for subscript out of range
            if (mapNum <= 0 || mapNum > Constant.MAX_MAPS - 1) return;
    
            if (mapNpcNum <= 0 || mapNpcNum > Constant.MAX_MAP_NPCS - 1) return;
    
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir = Dir;
    
            switch (Dir)
            {
                case Constant.DIR_NORTH:
                    Types.MapNpcs[mapNum].Npc[mapNpcNum].Y--; break;
                case Constant.DIR_SOUTH:
                    Types.MapNpcs[mapNum].Npc[mapNpcNum].Y++; break;
                case Constant.DIR_WEST:
                    Types.MapNpcs[mapNum].Npc[mapNpcNum].X--; break;
                case Constant.DIR_EAST:
                    Types.MapNpcs[mapNum].Npc[mapNpcNum].X++; break;
            }

            // Move Npc //
            ServerTCP.SendNpcMove(mapNum, mapNpcNum);
        }

        public static void NpcDir(int mapNum, int mapNpcNum, int Dir)
        {

            // Checks //
            if (mapNum <= 0 || mapNum > Constant.MAX_MAPS - 1 || mapNpcNum <= 0 || mapNpcNum > Constant.MAX_MAP_NPCS - 1 || Dir < Constant.DIR_NORTH || Dir > Constant.DIR_EAST)
                return;

            // Set Values
            Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir = Dir;

            // Move Npc //
            ServerTCP.SendNpcDir(mapNum, mapNpcNum);
        }

    }

}
