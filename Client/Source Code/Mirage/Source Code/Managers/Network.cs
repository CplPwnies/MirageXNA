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
using MirageXNA.Network;

namespace MirageXNA
{
    class NetworkManager
    {
        // Private Constructor
        private NetworkManager() { }

        // Singleton Accessor
        static NetworkManager instance;
        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new NetworkManager();
                return instance;
            }
        }

        // TCP Client //
        public NetClient TCPClient;
        private NetIncomingMessage incMsg;

        // Init Network Controller
        public void initTCP()
        {
            // Networking //
            NetPeerConfiguration config = new NetPeerConfiguration("MirageXNA");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.ConnectionTimeout = 5;
            TCPClient = new NetClient(config);
            TCPClient.Start();
            TCPClient.Socket.Blocking = false;

            // Check if server is online //
            TCPClient.DiscoverKnownPeer(Static.IpAddress, Static.Port);
        }

        // Discover Server
        public bool DiscoverServer()
        {
            if (IsConnected())
                return true;

            if (NetworkManager.Instance.TCPClient.DiscoverKnownPeer(Static.IpAddress, Static.Port))
                return true;

            return false;
        }

        // Check if player is connected
        public bool IsConnected()
        {
            if (TCPClient.ConnectionStatus == NetConnectionStatus.Connected) return true; else return false;
        }

        // Send Message
        public void SendData(NetOutgoingMessage Data)
        {
            TCPClient.SendMessage(Data, NetDeliveryMethod.ReliableOrdered);
        }

        // Check Incoming Message
        public void ReceiveData()
        {
            // READ INCOMING MESSAGE //
            while ((incMsg = TCPClient.ReadMessage()) != null)
            {
                switch (incMsg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        TCPClient.Connect(incMsg.SenderEndpoint); break;
                    case NetIncomingMessageType.Data:
                        HandleTCP.HandleData(incMsg); break;
                }
                TCPClient.Recycle(incMsg);
            }
        }


    }
}
