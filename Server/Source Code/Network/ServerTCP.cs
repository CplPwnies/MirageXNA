using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;	// Used to local machine info
using System.Collections;
using MirageXNA;
using MirageXNA.Network;
using MirageXNA.Core;
using MirageXNA.Globals;
using Lidgren.Network;

namespace MirageXNA.Network
{
    class ServerTCP
    {

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

        public static NetServer sSock;
        private static NetOutgoingMessage outgoingPacket;

        /////////////////////////////
        ////// INIT SERVER TCP //////
        /////////////////////////////
        public static void initServerTCP()
        {
            // Parse IP //
            IPAddress ip = System.Net.IPAddress.Parse("127.0.0.1");

            // Set Up Players //
            for (int LoopI = 0; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                Static.connectedClients.Add(null);
            }

            // SERVER TCP CONFIG //
            NetPeerConfiguration config = new NetPeerConfiguration("MirageXNA");
            //config.LocalAddress = ip;
            config.Port = 7001;
            config.AcceptIncomingConnections = true;
            config.MaximumConnections = 100;
            config.ConnectionTimeout = 5;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            sSock = new NetServer(config);
            sSock.Start();
            sSock.Socket.Blocking = false;
        }

        public static bool IsPlaying(Int32 Index)
        {
            if (Types.Players[Index].InGame)
            {
                // Check Connection To Server ..
                NetConnection tempClient = (NetConnection)Static.connectedClients[Index];
                if (tempClient.Status == NetConnectionStatus.Connected)
                    return true;
            }
            return false;
        }

        public static bool IsConnected(Int32 Index)
        {
            NetConnection tempClient = (NetConnection)Static.connectedClients[Index];
            if (tempClient.Status == NetConnectionStatus.Connected)
            {
                return true;
            }

            return false;
        }

        // Method to send the message //
        public static void SendDataTo(int Index, NetOutgoingMessage Data, NetDeliveryMethod deliveryMethod)
        {
            //if (Index > Static.connectedClients.Count - 1) return;
            outgoingPacket = ServerTCP.sSock.CreateMessage(Data.LengthBytes);
            outgoingPacket.Write(Data);
            ServerTCP.sSock.SendMessage(outgoingPacket, (NetConnection)Static.connectedClients[Index], deliveryMethod);
        }

        // Method to send the message to all //
        public static void SendDataToAll(NetOutgoingMessage Data)
        {
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                if (Types.Players[LoopI].InGame)
                {
                    SendDataTo(LoopI, Data, NetDeliveryMethod.ReliableOrdered);
                }
            }
        }

        // Method to send the message to all but index //
        public static void SendDataToAllBut(int Index, NetOutgoingMessage Data)
        {
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                if (Types.Players[LoopI].InGame)
                {
                    if (LoopI != Index)
                    {
                        SendDataTo(LoopI, Data, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
        }

        // Method to send the message to map //
        public static void SendDataToMap(int mapNum, NetOutgoingMessage Data)
        {
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                if (Types.Players[LoopI].InGame)
                {
                    if (Commands.getPlayerMap(LoopI) == mapNum)
                    {
                        SendDataTo(LoopI, Data, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
        }

        // Method to send the message to map but index //
        public static void SendDataToMapBut(int Index, int mapNum, NetOutgoingMessage Data)
        {
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                if (Types.Players[LoopI].InGame)
                {
                    if (Commands.getPlayerMap(LoopI) == mapNum)
                    {
                        if (LoopI != Index)
                        {
                            SendDataTo(LoopI, Data, NetDeliveryMethod.ReliableOrdered);
                        }
                    }
                }
            }
        }

        public static void SendAlertMessage(Int32 Index, String Message)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(8 + Message.Length);
            TempBuffer.Write((int)ServerPackets.SAlertMsg);
            TempBuffer.Write(Message);
            SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }

        public static void SendPlayerMovement(Int32 Index, Int32 Movement)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(24);
            TempBuffer.Write((int)ServerPackets.SPlayerMove);
            TempBuffer.Write(Index);
            TempBuffer.Write(Types.Players[Index].X);
            TempBuffer.Write(Types.Players[Index].Y);
            TempBuffer.Write(Types.Players[Index].Dir);
            TempBuffer.Write(Movement);
            SendDataToAllBut(Index, TempBuffer);
        }

        public static void SendPlayerDirectionToMapBut(Int32 Index, Int32 Direction)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage();
            TempBuffer.Write((int)ServerPackets.SPlayerDir);
            TempBuffer.Write(Index);
            TempBuffer.Write(Direction);
            SendDataToMapBut(Index, Commands.getPlayerMap(Index), TempBuffer);
        }

        public static void SendJoinMap(int Index)
        {
            // Send all players on current map to index
            for (int LoopI = 1; LoopI <= Constant.MAX_PLAYERS; LoopI++)
            {
                if (ServerTCP.IsPlaying(LoopI))
                {
                    if (LoopI != Index)
                    {
                        if (Types.Players[LoopI].Map == Types.Players[Index].Map)
                        {
                            SendDataTo(Index, SendPlayerData(LoopI), NetDeliveryMethod.ReliableOrdered);
                            Thread.Sleep(1);
                        }
                    }
                }
            }

            // Send index's player data to everyone on the map including himself
            SendDataToAll(SendPlayerData(Index));
        }

        public static void SendMap(Int32 Index, Int32 mapNum)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(Types.MapCache[mapNum].Data.LengthBytes + 4);
            TempBuffer.Write((int)ServerPackets.SMapData);
            TempBuffer.Write(Types.MapCache[mapNum].Data);
            SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }

        public static void SendLeftGame(Int32 Index)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(SendPlayerData(Index).LengthBytes + 8);
            TempBuffer.Write((int)ServerPackets.SPlayerData);
            TempBuffer.Write(Index);
            TempBuffer.Write(SendPlayerData(Index));
            SendDataToAllBut(Index, TempBuffer);
        }

        // Send Message To Player Only //
        public static void SendPlayerMsg(Int32 Index, String Msg)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage();
            TempBuffer.Write((int)ServerPackets.SChatMsg);
            TempBuffer.Write(Msg);
            SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }

        // Send Message To Map //
        public static void SendMapMsg(Int32 Index, String Msg)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage();
            TempBuffer.Write((int)ServerPackets.SChatMsg);
            TempBuffer.Write(Msg);
            SendDataToMap(Types.Players[Index].Map, TempBuffer);
        }

        // Usable Byte Array Data //
        public static NetOutgoingMessage SendPlayerData(Int32 Index)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage();
            TempBuffer.Write((int)ServerPackets.SPlayerData);
            TempBuffer.Write(Index);
            TempBuffer.Write(Types.Players[Index].Name);
            TempBuffer.Write(Types.Players[Index].Sprite);
            TempBuffer.Write(Types.Players[Index].Level);
            TempBuffer.Write(Types.Players[Index].Map);
            TempBuffer.Write(Types.Players[Index].X);
            TempBuffer.Write(Types.Players[Index].Y);
            TempBuffer.Write(Types.Players[Index].Dir);
            TempBuffer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Strength]);
            TempBuffer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Defence]);
            TempBuffer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Magic]);
            TempBuffer.Write(Types.Players[Index].Stats[(int)Enumerations.Stats.Speed]);
            TempBuffer.Write(Types.Players[Index].Points);
            return TempBuffer;
        }

        // Check The Client To See if He Already Has The Map //
        public static void SendCheckMap(Int32 Index, Int32 mapNum, Int32 Revision)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(12);
            TempBuffer.Write((int)ServerPackets.SCheckForMap);
            TempBuffer.Write(mapNum);
            TempBuffer.Write(Revision);
            SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }

        // Spawn Npc To Map //
        public static void SendSpawnNpc(Int32 mapNum, Int32 mapNpcNum)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(32);
            TempBuffer.Write((int)ServerPackets.SSpawnNpc);
            TempBuffer.Write(mapNpcNum);
            TempBuffer.Write(Types.Npc[Types.MapNpcs[mapNum].Npc[mapNpcNum].Num].Name);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].Num);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].X);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].Y);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].Sprite);
            SendDataToMap(mapNum, TempBuffer);
        }

        // Send Npc Movement //
        public static void SendNpcMove(Int32 mapNum, Int32 mapNpcNum)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(20);
            TempBuffer.Write((int)ServerPackets.SNpcMove);
            TempBuffer.Write(mapNpcNum);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].X);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].Y);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir);
            SendDataToMap(mapNum, TempBuffer);
        }

        // Send Npc Movement //
        public static void SendNpcDir(Int32 mapNum, Int32 mapNpcNum)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(12);
            TempBuffer.Write((int)ServerPackets.SNpcDir);
            TempBuffer.Write(mapNpcNum);
            TempBuffer.Write(Types.MapNpcs[mapNum].Npc[mapNpcNum].Dir);
            SendDataToMap(mapNum, TempBuffer);
        }

        // Send Map Done //
        public static void SendMapDone(Int32 Index)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(4);
            TempBuffer.Write((int)ServerPackets.SMapDone);
            SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }

        // Npc Dead //
        public static void SendNpcDead(Int32 mapNum, Int32 mapNpcNum)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(8);
            TempBuffer.Write((int)ServerPackets.SNpcDead);
            TempBuffer.Write(mapNpcNum);
            SendDataToMap(mapNum, TempBuffer);
        }

        public static void SendVital(Int32 Index, Enumerations.Vitals Vital)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(16);
            TempBuffer.Write((int)ServerPackets.SPlayerVital);
            TempBuffer.Write((int)Vital);
            TempBuffer.Write(Commands.getPlayerMaxVital(Index, Vital));
            TempBuffer.Write(Commands.getPlayerVital(Index, Vital));
            SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }

        public static void SendExp(Int32 Index)
        {
            NetOutgoingMessage TempBuffer = ServerTCP.sSock.CreateMessage(12);
            TempBuffer.Write((int)ServerPackets.SPlayerExp);
            TempBuffer.Write(Commands.getPlayerMaxExp(Index));
            TempBuffer.Write(Commands.getPlayerExp(Index));
            SendDataTo(Index, TempBuffer, NetDeliveryMethod.ReliableOrdered);
        }
    }
}