using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Lidgren.Network;
using MirageXNA.Core;
using MirageXNA.Network;

namespace MirageXNA.Globals
{
    public static class Static
    {
        public static bool[] PlayersOnMap = new bool[Constant.MAX_PLAYERS + 1];

        // Attributes //
        //public static ArrayList m_aryClients = new ArrayList(Constant.MAX_PLAYERS);	// List of Client Connections
        public static List<NetConnection> connectedClients = new List<NetConnection>(Constant.MAX_PLAYERS + 1);	// List of Client Connections

        // Random Number Generator //
        public static Random Rnd = new Random();

        public static frmServer serverWindow;
        
        // Server Thread //
        public static bool ServerRunning = true;

        // Total Players //
        public static int TotalPlayersOnline = 0;

        // Used for outputting text
        public static int NumLines = -1;

        public static void TextAdd(string text)
        {
            NumLines = NumLines + 1;

            if (NumLines >= 100)
            {
                serverWindow.tbLog.Text = String.Empty;
                NumLines = 0;
            }

            serverWindow.tbLog.Text = serverWindow.tbLog.Text + Environment.NewLine + text;
            serverWindow.tbLog.SelectionStart = serverWindow.tbLog.Text.Length;
        }

        public static void updateListView()
        {
            for (int i = 0; i <= Constant.MAX_PLAYERS; i++)
            {
                serverWindow.lvwInfo.Items.Add(Convert.ToString(i));
                serverWindow.lvwInfo.Items[i].Text = Convert.ToString(i);
                serverWindow.lvwInfo.Items[i].SubItems.Add(String.Empty);
                serverWindow.lvwInfo.Items[i].SubItems.Add(String.Empty);
                serverWindow.lvwInfo.Items[i].SubItems.Add(String.Empty);
            }
        }

    }
}
