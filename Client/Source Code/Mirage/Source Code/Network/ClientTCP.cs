///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

using MirageXNA.Global;
using MirageXNA.Engine;

namespace MirageXNA.Network
{
    class ClientTCP
    {
        ///////////////////////////
        ////// TCP Functions //////
        ///////////////////////////

        // Check if player exists //
        public static bool IsPlaying(Int32 Index)
        {
            if (Types.Players[Index].Name == null) { return false; }
            if (Types.Players[Index].Name.Length > 0) { return true; }
            return false;
        }

        /////////////////////
        ////// PACKETS //////
        /////////////////////

        // Send Request Ping //
        public static void SendGetPing()
        {
            NetworkManager TcpM = NetworkManager.Instance;
            NetOutgoingMessage TempBuffer = TcpM.TCPClient.CreateMessage(4);
            Static.PingStart = Environment.TickCount;
            TempBuffer.Write((int)HandleTCP.ClientPackets.CPing);
            TcpM.SendData(TempBuffer);
        }

        // Send Login //
        public static void SendLogin(string lName, string lPass)
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage();
            TempBuffer.Write((int)HandleTCP.ClientPackets.CLogin);
            TempBuffer.Write(lName);
            TempBuffer.Write(lPass);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Send New Registration //
        public static void SendRegister(string rName, string rPass, int rSprite)
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage();
            TempBuffer.Write((int)HandleTCP.ClientPackets.CRegister);
            TempBuffer.Write(rName);
            TempBuffer.Write(rPass);
            TempBuffer.Write(rSprite);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Send Player Movement //
        public static void SendPlayerMove()
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage(24);
            TempBuffer.Write((int)HandleTCP.ClientPackets.CPlayerMove);
            TempBuffer.Write(Types.Players[Static.MyIndex].Dir);
            TempBuffer.Write(Types.Players[Static.MyIndex].Moving);
            TempBuffer.Write(Types.Players[Static.MyIndex].X);
            TempBuffer.Write(Types.Players[Static.MyIndex].Y);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Send Player Direction //
        public static void SendPlayerDir()
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage(8);
            TempBuffer.Write((int)HandleTCP.ClientPackets.CPlayerDir);
            TempBuffer.Write(Types.Players[Static.MyIndex].Dir);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Say Message //
        public static void MapMsg(string Msg)
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage();
            TempBuffer.Write((int)HandleTCP.ClientPackets.CMapMsg);
            TempBuffer.Write(Msg);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Request New Map //
        public static void SendPlayerRequestNewMap()
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage(8);
            TempBuffer.Write((int)HandleTCP.ClientPackets.CRequestNewMap);
            TempBuffer.Write(Types.Players[Static.MyIndex].Dir);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Send Need Map //
        public static void SendNeedMap(bool needMap)
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage();
            TempBuffer.Write((int)HandleTCP.ClientPackets.CNeedMap);
            TempBuffer.Write(needMap);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Send Train Stat //
        public static void SendTrainStat(byte statNum)
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage(5);
            TempBuffer.Write((int)HandleTCP.ClientPackets.CTrainStat);
            TempBuffer.Write(statNum);
            NetworkManager.Instance.SendData(TempBuffer);
        }

        // Send Attack //
        public static void SendAttack()
        {
            NetOutgoingMessage TempBuffer = NetworkManager.Instance.TCPClient.CreateMessage(4);
            TempBuffer.Write((int)HandleTCP.ClientPackets.CAttack);
            NetworkManager.Instance.SendData(TempBuffer);
        }

    }
}
