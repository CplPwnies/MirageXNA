using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MirageXNA;
using MirageXNA.Core;
using MirageXNA.Network;
using MirageXNA.Globals;

namespace MirageXNA.Core
{
    public class Combat
    {

        // ##############################
        // ##    Basic Calculations    ##
        // ##############################

        public static int GetNpcDamage(int NpcNum)
        {
            // ****** Clear Data******
            int GetNpcDamage = 0;

            // ****** Subscript Out Of Range ******
            if (NpcNum <= 0 || NpcNum > Constant.MAX_NPCS)
                return 0;

            int Damage = 2;

            // ****** Initial Damage ******
            GetNpcDamage = Damage + (int)((double)((Damage) * 2.5) * (Types.Npc[NpcNum].Stats[(int)Enumerations.Stats.Strength]));
            return GetNpcDamage;
        }
        public static int GetNpcProtection(int NpcNum)
        {
    
            // ****** Clear Data******
            int GetNpcProtection = 0;

            // ****** Subscript Out Of Range ******
            if (NpcNum <= 0 || NpcNum > Constant.MAX_NPCS)
                return 0;

            int Defence = 2;

            // ****** Retrive END ******
            GetNpcProtection = Defence + (int)((double)((Defence) * 1.5) * (Types.Npc[NpcNum].Stats[(int)Enumerations.Stats.Defence]));
            return GetNpcProtection;
        }
        public static int GetPlayerDamage(int Index)
        {
            // ****** Clear Data******
            int GetPlayerDamage = 0;
    
            // Prevent subscript out of range
            if (!ServerTCP.IsPlaying(Index) || Index <= 0 || Index > Constant.MAX_PLAYERS)
                return 0;

            int Damage = 2;

            // ****** Initial Damage ******
            GetPlayerDamage = Damage + (int)((double)((Damage) * 2.5) * (Commands.getPlayerStat(Index, Enumerations.Stats.Strength)));
            return GetPlayerDamage;
        }
        public static int GetPlayerProtection(int Index)
        {    
            // ****** Clear Data******
            int GetPlayerProtection = 0;

            // Prevent subscript out of range
            if (!ServerTCP.IsPlaying(Index) || Index <= 0 || Index > Constant.MAX_PLAYERS)
                return 0;

            int Defence = 2;

            // ****** Initial Protection ******
            GetPlayerProtection = Defence + (int)((double)((Defence) * 1.5) * (Commands.getPlayerStat(Index, Enumerations.Stats.Defence)));
            return GetPlayerProtection;
        }

        // ##########################
        // ##    Attack Methods    ##
        // ##########################

        // Player vs Player
        public static bool CanPlayerAttackPlayer(Int32 Attacker, Int32 Victim)
        {
            // Check for subscript out of range
            if (!ServerTCP.IsPlaying(Victim)) return false;

            // Make sure they not attacking ourselfs
            if (Attacker == Victim) return false;

            switch (Types.Players[Attacker].Dir)
            {
                case Constant.DIR_NORTH:
                    if ((Types.Players[Victim].Y + 1 != Types.Players[Attacker].Y) & (Types.Players[Victim].X != Types.Players[Attacker].Y)) 
                        return false;
                    break;
                case Constant.DIR_SOUTH:
                    if ((Types.Players[Victim].Y - 1 != Types.Players[Attacker].Y) & (Types.Players[Victim].X != Types.Players[Attacker].Y)) 
                        return false;
                    break;
                case Constant.DIR_WEST:
                    if ((Types.Players[Victim].Y != Types.Players[Attacker].Y) & (Types.Players[Victim].X + 1 != Types.Players[Attacker].Y)) 
                        return false;
                    break;
                case Constant.DIR_EAST:
                    if ((Types.Players[Victim].Y != Types.Players[Attacker].Y) & (Types.Players[Victim].X - 1 != Types.Players[Attacker].Y)) 
                        return false;
                    break;
            }

            return true;
        }
        public static void PlayerAttackPlayer(Int32 Attacker, Int32 Victim)
        {
            if (CanPlayerAttackPlayer(Attacker,Victim))
            {

            }
        }
        public static void PlayerDamagePlayer(Int32 Attacker, Int32 Victim, Int32 Damage)
        {
            // Check for subscript out of range
            if (!ServerTCP.IsPlaying(Attacker) || !ServerTCP.IsPlaying(Victim))
                return;

            // set
            int mapNum = Commands.getPlayerMap(Attacker);

        }

        // Player vs Npc
        public static bool CanPlayerAttackNpc(Int32 Attacker, Int32 Victim)
        {    
            // Check for subscript out of range
            if (!ServerTCP.IsPlaying(Attacker) || Victim <= 0 || Victim > Constant.MAX_MAP_NPCS)
                return false;

            int mapNum = Commands.getPlayerMap(Attacker);
            int npcNum = Types.MapNpcs[mapNum].Npc[Victim].Num;

            // Check for subscript out of range
            if (npcNum < 0 || npcNum > Constant.MAX_NPCS)
                return false;

            // Make sure the npc isn't already dead
            if (Types.Npc[npcNum].Behaviour != Constant.NPC_BEHAVIOUR_FRIENDLY_ROAMING && Types.Npc[npcNum].Behaviour != Constant.NPC_BEHAVIOUR_FRIENDLY_STANDING)
            {
                if (Types.Npc[npcNum].Behaviour != Constant.NPC_BEHAVIOUR_SELLER_ROAMING && Types.Npc[npcNum].Behaviour != Constant.NPC_BEHAVIOUR_SELLER_STANDING)
                {
                    if (Types.MapNpcs[mapNum].Npc[Victim].Vitals[(int)Enumerations.Vitals.HP] <= 0)
                        return false;
                }
            }

            if (npcNum > 0)
            {
                byte Dir = (byte)Types.Players[Attacker].Dir;
                int X = GameLogic.DirToX((byte)Types.Players[Attacker].X, Dir);
                int Y = GameLogic.DirToY((byte)Types.Players[Attacker].Y, Dir);

                if (Types.MapNpcs[mapNum].Npc[Victim].X == X)
                {
                    if (Types.MapNpcs[mapNum].Npc[Victim].Y == Y)
                    {
                        switch (Types.Npc[npcNum].Behaviour)
                        {
                            case Constant.NPC_BEHAVIOUR_FRIENDLY_ROAMING:
                                // Open Chat
                                return false;
                            case Constant.NPC_BEHAVIOUR_FRIENDLY_STANDING:
                                // Open Chat
                                return false;
                            case Constant.NPC_BEHAVIOUR_SELLER_ROAMING:
                                // Open Shop
                                return false;
                            case Constant.NPC_BEHAVIOUR_SELLER_STANDING:
                                // Open Shop
                                return false;
                            default:
                                return true;
                        }
                    }
                }
            }

            return false;
        }
        public static void PlayerAttackNpc(Int32 Attacker, Int32 Victim)
        {
            if (CanPlayerAttackNpc(Attacker, Victim))
            {
                int mapNum = Commands.getPlayerMap(Attacker);
                int npcNum = Types.MapNpcs[mapNum].Npc[Victim].Num;
                string npcName = Types.Npc[npcNum].Name;

                // Damage Algorithm
                int PlayerDamage = GetPlayerDamage(Attacker);
                int NpcProtection = GetNpcProtection(npcNum);

                double diff = PlayerDamage - NpcProtection;
                if (diff < 0) diff = 0;

                // Increase up to 10% for minimum damage
                double minDam = diff - Math.Round(diff * 0.1, 0);
                double maxDam = diff + Math.Round(diff * 0.1, 0);
                if (maxDam < minDam) minDam = maxDam;
                int Damage = Static.Rnd.Next((int)minDam, (int)maxDam);
                if (Damage > 0)
                {
                    PlayerDamageNpc(Attacker, Victim, Damage);
                    ServerTCP.SendPlayerMsg(Attacker, "You dealt " + Damage + " points of damage to " + npcName);
                }
                else
                    ServerTCP.SendPlayerMsg(Attacker, "Your attack did nothing.");
            }
        }
        public static void PlayerDamageNpc(Int32 Attacker, Int32 Victim, Int32 Damage)
        {
            // Check for subscript out of range
            if (!ServerTCP.IsPlaying(Attacker) || Victim <= 0 || Victim > Constant.MAX_MAP_NPCS)
                return;

            // set
            int mapNum = Commands.getPlayerMap(Attacker);
            int npcNum = Types.MapNpcs[mapNum].Npc[Victim].Num;

            if (Damage >= Types.MapNpcs[mapNum].Npc[Victim].Vitals[(int)Enumerations.Vitals.HP])
            {
                // Erase Map Npc
                EraseMapNpc(mapNum, Victim);

                // Give Player EXP
                ServerTCP.SendPlayerMsg(Attacker, "You gained " + Types.Npc[npcNum].Exp + " exp!");
                Commands.GiveExp(Attacker, Types.Npc[npcNum].Exp);

                // Send Experiance
                ServerTCP.SendExp(Attacker);
            }
            else
            {
                // Do Damage
                Types.MapNpcs[mapNum].Npc[Victim].Vitals[(int)Enumerations.Vitals.HP] =
                    (Types.MapNpcs[mapNum].Npc[Victim].Vitals[(int)Enumerations.Vitals.HP] - Damage);

                // Set Target
                Types.MapNpcs[mapNum].Npc[Victim].Target = Attacker;
                Types.MapNpcs[mapNum].Npc[Victim].TargetType = Constant.TARGET_TYPE_PLAYER;
            }

        }

        // Npc vs Player
        public static bool CanNpcAttackPlayer(Int32 Attacker, Int32 Victim)
        {
            // Prevent subscript out of range
            if (!ServerTCP.IsPlaying(Victim) || Attacker <= 0 || Attacker > Constant.MAX_MAP_NPCS)
                return false;

            // set
            int mapNum = Commands.getPlayerMap(Victim);
            int npcNum = Types.MapNpcs[mapNum].Npc[Attacker].Num;

            // Make sure the npc isn't already dead
            if (Types.MapNpcs[mapNum].Npc[Attacker].Vitals[(int)Enumerations.Vitals.HP] <= 0)
                return false;

            // Make sure npcs dont attack more then once a second
            if (Environment.TickCount < Types.MapNpcs[mapNum].Npc[Attacker].AttackTimer + 1000)
                return false;

            if (ServerTCP.IsPlaying(Victim))
            {
                if (npcNum > 0)
                {
                    int X = GameLogic.DirToX((byte)Types.MapNpcs[mapNum].Npc[Attacker].X, (byte)Types.MapNpcs[mapNum].Npc[Attacker].Dir);
                    int Y = GameLogic.DirToY((byte)Types.MapNpcs[mapNum].Npc[Attacker].Y, (byte)Types.MapNpcs[mapNum].Npc[Attacker].Dir);
                    if ((Types.Players[Victim].Y == Y) && (Types.Players[Victim].X == X))
                    {
                        Types.MapNpcs[mapNum].Npc[Attacker].AttackTimer = Environment.TickCount;
                        return true;
                    }
                }
            }

            return false;
        }
        public static void NpcAttackPlayer(Int32 Attacker, Int32 Victim)
        {
            if (CanNpcAttackPlayer(Attacker, Victim))
            {
                int mapNum = Commands.getPlayerMap(Victim);
                int npcNum = Types.MapNpcs[mapNum].Npc[Attacker].Num;

                // Damage Algorithm
                double diff = GetNpcDamage(npcNum) - GetPlayerProtection(Victim);
                if (diff < 0) diff = 0;

                // Increase up to 10% for minimum damage
                double minDam = diff - Math.Round(diff * 0.1, 0);
                double maxDam = diff + Math.Round(diff * 0.1, 0);
                if (maxDam < minDam) minDam = maxDam;
                int Damage = Static.Rnd.Next((int)minDam, (int)maxDam);
                if (Damage > 0)
                {
                    NpcDamagePlayer(Attacker, Victim, Damage);
                    ServerTCP.SendPlayerMsg(Victim, "You got hit for " + Damage + " points of damage");
                }
            }
        }
        public static void NpcDamagePlayer(Int32 Attacker, Int32 Victim, Int32 Damage)
        {
            // Check for subscript out of range
            if (!ServerTCP.IsPlaying(Victim) || Attacker <= 0 || Attacker > Constant.MAX_MAP_NPCS)
                return;

            // set
            int mapNum = Commands.getPlayerMap(Victim);
            int npcNum = Types.MapNpcs[mapNum].Npc[Attacker].Num;

            if (Damage >= Types.Players[Victim].Vitals[(int)Enumerations.Vitals.HP])
            {
                // Clear Npc Target
                Types.MapNpcs[mapNum].Npc[Attacker].Target = 0;
                Types.MapNpcs[mapNum].Npc[Attacker].TargetType = 0;

                // Erase Map Npc
                ErasePlayer(Victim);
            }
            else
            {
                // Do Damage
                Commands.setPlayerVital(Victim, Enumerations.Vitals.HP,
                    Commands.getPlayerVital(Victim, Enumerations.Vitals.HP) - Damage);
                ServerTCP.SendVital(Victim, Enumerations.Vitals.HP);
            }

        }

        // Erase From Map
        public static void ErasePlayer(Int32 Victim)
        {
            // Revive
            Types.Players[Victim].Vitals[(int)Enumerations.Vitals.HP] = 
                Commands.getPlayerMaxVital(Victim, Enumerations.Vitals.HP);
            Types.Players[Victim].Vitals[(int)Enumerations.Vitals.MP] =
                Commands.getPlayerMaxVital(Victim, Enumerations.Vitals.MP);
            Types.Players[Victim].Vitals[(int)Enumerations.Vitals.SP] =
                Commands.getPlayerMaxVital(Victim, Enumerations.Vitals.SP);

            // Respawn
            General.playerWarp(Victim, 1, 10, 10);

            // Send Vitals
            ServerTCP.SendVital(Victim, Enumerations.Vitals.HP);
            ServerTCP.SendVital(Victim, Enumerations.Vitals.MP);
            ServerTCP.SendVital(Victim, Enumerations.Vitals.SP);

            // Refresh
            ServerTCP.SendDataToMap(Commands.getPlayerMap(Victim), ServerTCP.SendPlayerData(Victim));
        }
        public static void EraseMapNpc(Int32 mapNum, Int32 Victim)
        {
            // Erase Values
            Types.MapNpcs[mapNum].Npc[Victim].Num = 0;
            Types.MapNpcs[mapNum].Npc[Victim].SpawnWait = Environment.TickCount;
            Types.MapNpcs[mapNum].Npc[Victim].Vitals[(int)Enumerations.Vitals.HP] = 0;

            // NPC Dead
            ServerTCP.SendNpcDead(mapNum, Victim);
        }
    }
}
