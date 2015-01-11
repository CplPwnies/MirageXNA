using System;
using System.IO;
using Lidgren.Network;

using MirageXNA;
using MirageXNA.Core;
using MirageXNA.Globals;
using MirageXNA.Network;

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

        public static void ReceivedData()
        {
            NetIncomingMessage msg;

            while ((msg = ServerTCP.sSock.ReadMessage()) != null)
            {
                // Get Incoming Index //
                int Index = Static.connectedClients.IndexOf(msg.SenderConnection);

                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        ServerTCP.sSock.SendDiscoveryResponse(null, msg.SenderEndpoint);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage: Static.TextAdd(msg.ReadString()); break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        if (status == NetConnectionStatus.Connected)
                        {
                            // New Player Connected //
                            Static.connectedClients[Static.connectedClients.IndexOf(null, 1)] = (msg.SenderConnection);
                            Index = Static.connectedClients.IndexOf(msg.SenderConnection);
                            Types.Players[Index].InGame = false;
                            Types.Players[Index].Name = null;
                            Static.TextAdd(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " connected!");
                        }
                        if (status == NetConnectionStatus.Disconnected)
                        {
                            General.leftGame(Index);
                            Static.connectedClients[Index] = null;
                            General.clearPlayer(Index);
                        }
                        break;

                    // HANDLE DATA //
                    case NetIncomingMessageType.Data: HandleTCP.HandleData(Index, msg); break;

                    default:
                        Static.TextAdd("Unhandled type: " + msg.MessageType);
                        break;
                }
                ServerTCP.sSock.Recycle(msg);
            }
        }

        // HANDLE BYTE ARRAY FROM CLIENT //
        public static void HandleData(Int32 Index, NetIncomingMessage Data)
        {
            // Packet Header //
            Int32 MsgType = Data.ReadInt32();

            // Check Packet Number
            if (MsgType < 0 && MsgType >= (int)ClientPackets.totalClient) { Static.TextAdd("Packet Error"); return; }

            switch (MsgType)
            {
                case (int)ClientPackets.None: break;
                case (int)ClientPackets.CPing: HandleCheckPing(Index); break;
                case (int)ClientPackets.CLogin: HandleLogin(Index, Data); break;
                case (int)ClientPackets.CRegister: HandleRegister(Index, Data); break;
                case (int)ClientPackets.CPlayerMove: HandlePlayerMove(Index, Data); break;
                case (int)ClientPackets.CPlayerDir: HandlePlayerDir(Index, Data); break;
                case (int)ClientPackets.CAttack: HandleAttack(Index); break;
                case (int)ClientPackets.CMapMsg: HandleMapMsg(Index, Data); break;
                case (int)ClientPackets.CRequestNewMap: HandleRequestNewMap(Index, Data); break;
                case (int)ClientPackets.CNeedMap: HandleNeedMap(Index, Data); break;
                case (int)ClientPackets.CTrainStat: HandleTrainStat(Index, Data); break;
            }
        }

        // HANDLE PING //
        public static void HandleCheckPing(Int32 Index)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(4);
            TempBuffer.Write((int)ServerTCP.ServerPackets.SSendPing);
            ServerTCP.SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }

        // HANDLE REGISTER //
        public static void HandleRegister(Int32 Index, NetIncomingMessage Data)
        {
            string Name; string Pass; int Sprite;

            // Read Packet Information
            Name = Data.ReadString();
            Pass = Data.ReadString();
            Sprite = Data.ReadInt32();

            // Check Account Already Exists //
            DirectoryInfo dir = new DirectoryInfo("Data\\Accounts\\");
            string accountDir = dir.FullName + Name.ToLower() + ".bin";
            if (File.Exists(accountDir))
            {
                ServerTCP.SendAlertMessage(Index, "Account already exists.");
                return;
            }

            // Create Player //
            Types.Players[Index].Name = Name;
            Types.Players[Index].Password = Pass;
            Types.Players[Index].Sprite = Sprite;

            // Player Stating Position //
            Types.Players[Index].Map = 1;
            Types.Players[Index].X = 10;
            Types.Players[Index].Y = 10;
            Types.Players[Index].Dir = Constant.DIR_SOUTH;

            // Player Stating Statistics //
            Types.Players[Index].Stats[(int)Enumerations.Stats.Strength] = 1;
            Types.Players[Index].Stats[(int)Enumerations.Stats.Defence] = 1;
            Types.Players[Index].Stats[(int)Enumerations.Stats.Magic] = 0;
            Types.Players[Index].Stats[(int)Enumerations.Stats.Speed] = 0;
            Types.Players[Index].Level = 1;
            Types.Players[Index].Exp = 0;
            Types.Players[Index].Points = 20;

            // Set Vitals Full
            Types.Players[Index].Vitals[(int)Enumerations.Vitals.HP] = Commands.getPlayerMaxVital(Index, Enumerations.Vitals.HP);
            Types.Players[Index].Vitals[(int)Enumerations.Vitals.MP] = Commands.getPlayerMaxVital(Index, Enumerations.Vitals.MP);
            Types.Players[Index].Vitals[(int)Enumerations.Vitals.SP] = Commands.getPlayerMaxVital(Index, Enumerations.Vitals.SP);

            // Save Player & Login //
            Database.savePlayer(Index);

            // Send Data Back To Client
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage();
            TempBuffer.Write((int)ServerTCP.ServerPackets.SLoginOk);
            TempBuffer.Write(Index);
            ServerTCP.SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
            TempBuffer = null;

            // JOIN THE GAME //
            General.joinGame(Index);
        }

        // HANDLE LOGIN //
        public static void HandleLogin(Int32 Index, NetIncomingMessage Data)
        {
            string Login_Name = null;
            string Login_Pass = null;
            string tempPass = null;

            Login_Name = Data.ReadString();
            Login_Pass = Data.ReadString();

            DirectoryInfo dir = new DirectoryInfo("Data\\Accounts\\");
            string accountDir = dir.FullName + Login_Name.ToLower() + ".bin";
            if (File.Exists(accountDir))
            {
                using (FileStream stream = new FileStream(accountDir, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        tempPass = reader.ReadString();
                        tempPass = reader.ReadString();
                        reader.Close();
                    }
                }
            }
            else
            {
                ServerTCP.SendAlertMessage(Index, "Account does not exists.");
                return;
            }

            if (tempPass == Login_Pass)
            {

                if (Database.loadPlayer(Index, Login_Name))
                {
                    // Account Exist - Just log in //
                    ServerTCP.SendAlertMessage(Index, "Login Succesful. Joining Game...");

                    // Send Data Back To Client
                    NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage();
                    TempBuffer.Write((int)ServerTCP.ServerPackets.SLoginOk);
                    TempBuffer.Write(Index);
                    ServerTCP.SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
                    TempBuffer = null;

                    // JOIN THE GAME //
                    General.joinGame(Index);
                }
                else
                {
                    ServerTCP.SendAlertMessage(Index, "Account does not exists.");
                    return;
                }

            }
            else
            {
                ServerTCP.SendAlertMessage(Index, "Password Incorrect.");
                return;
            }

        }

        // HANDLE PLAYER MOVEMENT //
        public static void HandlePlayerMove(Int32 Index, NetIncomingMessage Data)
        {
            int X; int Y; int Dir; int Moving;

            // Read Packet Information
            Dir = Data.ReadInt32();
            Moving = Data.ReadInt32();
            X = Data.ReadInt32();
            Y = Data.ReadInt32();

            // Player Can Move - Update to all online //
            Types.Players[Index].Dir = Dir;
            Types.Players[Index].X = X;
            Types.Players[Index].Y = Y;

            GameLogic.PlayerMove(Index, Dir, Moving);
        }

        // HANDLE PLAYER MOVEMENT //
        public static void HandlePlayerDir(Int32 Index, NetIncomingMessage Data)
        {
            // Read Packet Information
            int Dir = Data.ReadInt32();

            // Player Can Move - Update to all online //
            Types.Players[Index].Dir = Dir;

            // Send Message //
            ServerTCP.SendPlayerDirectionToMapBut(Index, Dir);
        }

        // HANDLE PLAYER ATTACK //
        public static void HandleAttack(Int32 Index)
        {
            int TempIndex = 0;

            // ######################################
            // ##      Player Attacking Player     ##
            // ######################################
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                TempIndex = LoopI;
                if (TempIndex != Index)
                {
                    Combat.PlayerAttackPlayer(Index, LoopI);
                }
            }

            // ##################################
            // ##     Player Attacking Npc     ##
            // ##################################
            for (int LoopI = 1; LoopI <= Constant.MAX_MAP_NPCS; LoopI++)
            {
                Combat.PlayerAttackNpc(Index, LoopI);
            }

        }

        // HANDLE PLAYER MOVEMENT //
        public static void HandleMapMsg(Int32 Index, NetIncomingMessage Data)
        {
            // Read Packet Information
            string Msg = Data.ReadString();

            // Add Name to Message //
            Msg = Types.Players[Index].Name + ": " + Msg;

            // Send Message //
            ServerTCP.SendMapMsg(Index, Msg);
        }

        // Request For New Map //
        public static void HandleRequestNewMap(Int32 Index, NetIncomingMessage Data)
        {
            int Dir = Data.ReadInt32();

            // Prevent hacking
            if (Dir < Constant.DIR_NORTH || Dir > Constant.DIR_EAST)
            {
                return;
            }

            GameLogic.PlayerMove(Index, Dir, 1);
        }

        // HANDLE NEED MAP //
        public static void HandleNeedMap(Int32 Index, NetIncomingMessage Data)
        {
            bool needMap = Data.ReadBoolean();
            int mapNum = Commands.getPlayerMap(Index);

            if (needMap)
            {
                ServerTCP.SendMap(Index, Commands.getPlayerMap(Index));
            }

            // Spawn Npcs //
            for (int LoopI = 1; LoopI <= Types.NpcCache[mapNum].maxNpcs; LoopI++)
            {
                if (Types.NpcCache[mapNum].Npc[LoopI].Num > 0)
                    ServerTCP.SendSpawnNpc(mapNum, LoopI);
            }

            // Join Map //
            ServerTCP.SendJoinMap(Index);
            ServerTCP.SendMapDone(Index);
        }

        // HANDLE TRAIN STAT //
        public static void HandleTrainStat(Int32 Index, NetIncomingMessage Data)
        {
            // Check if we have points //
            if (Types.Players[Index].Points == 0)
            {
                ServerTCP.SendPlayerMsg(Index, "ALERT: You don't have any points");
                return;
            }

            // Temp Stat //
            byte statNum = Data.ReadByte();

            switch (statNum)
            {
                case 1: Types.Players[Index].Stats[(int)Enumerations.Stats.Strength]++;
                    ServerTCP.SendPlayerMsg(Index, "ALERT: You have trained your Strength!");
                    break;
                case 2: Types.Players[Index].Stats[(int)Enumerations.Stats.Defence]++;
                    ServerTCP.SendPlayerMsg(Index, "ALERT: You have trained your Defence!");
                    break;
                case 3: Types.Players[Index].Stats[(int)Enumerations.Stats.Magic]++;
                    ServerTCP.SendPlayerMsg(Index, "ALERT: You have trained your Magic!");
                    break;
                case 4: Types.Players[Index].Stats[(int)Enumerations.Stats.Speed]++;
                    ServerTCP.SendPlayerMsg(Index, "ALERT: You have trained your Speed!");
                    break;
            }

            // Remove Point //
            Types.Players[Index].Points--;

            // Update Self //
            ServerTCP.SendDataToMap(Types.Players[Index].Map, ServerTCP.SendPlayerData(Index));
        }

    }
}
