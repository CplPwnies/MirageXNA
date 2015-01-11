using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;

using MirageXNA;
using MirageXNA.Core;
using MirageXNA.Network;
using MirageXNA.Globals;

namespace MirageXNA.Core
{
    class Server
    {
        /////////////////////////////
        ////// START OF SERVER //////
        /////////////////////////////
        public static void Main(string[] args)
        {
            int Time1 = Environment.TickCount; int Time2 = 0;

            // Show Server //
            Static.serverWindow = new frmServer();
            Static.serverWindow.Show();
            Static.updateListView();

            // Init Message //
            Static.TextAdd("Initisalising Server...");

            // Clear Arrays //
            Static.TextAdd("Clearing resources...");
            General.clearAllPlayers();
            General.clearAllMapNpcs();

            // Init Socket
            Static.TextAdd("Loading Sockets...");
            ServerTCP.initServerTCP();

            // Load Maps //
            Static.TextAdd("Loading Maps...");
            Database.loadMaps();

            // Load Npcs //
            Static.TextAdd("Loading Npcs...");
            Database.loadAllNpcs();

            // Load Classes //
            Database.LoadClasses();

            // Spawn All Npcs To Map
            GameLogic.SpawnAllNpcs();

            // Return Time Took To Start Server //
            Time2 = Environment.TickCount;
            Static.TextAdd("Initialization complete. Server loaded in " + (Time2 - Time1) + "ms.");

            // Start Server Loop - Set Static Form To Main! //
            Application.Idle += new EventHandler(Serverloop.Loop);
            Application.Run(Static.serverWindow);
        }

    }
}
