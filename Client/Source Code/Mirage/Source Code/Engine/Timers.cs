///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using MirageXNA.Global;
using MirageXNA.Network;

namespace MirageXNA.Engine
{
    public sealed class Timers
    {
        //////////
        // Menu //
        //////////

        // Routine Timers //
        private const long UpdateTime_Credits = 50;

        // Update Routines Checks //
        private long LastUpdateTime_Credits;

        ////////////////////
        // Routine Timers //
        ////////////////////
        private const long UpdateTime_GameInput = 50;
        private const long UpdateTime_WalkTimer = 30;
        private const long UpdateTime_MapAnim = 250;
        private const long UpdateTime_Ping = 1000;

        ////////////////////////////
        // Update Routines Checks //
        ////////////////////////////
        private long LastUpdateTime_GameInput;
        private long LastUpdateTime_WalkTimer;
        private long LastUpdateTime_MapAnim;
        private long LastUpdateTime_Ping;        

        //////////////////////
        // Update Game Loop //
        //////////////////////
        public void updateGameLoop(long tickCount)
        {
            // Check Movement //
            if (LastUpdateTime_GameInput < tickCount)
            {
                Logic.checkMovement();
                Logic.checkAttack();
                LastUpdateTime_GameInput = tickCount + UpdateTime_GameInput;
            }

            // Process Movement //
            if (LastUpdateTime_WalkTimer < tickCount)
            {
                // Players //
                for (int LoopI = 0; LoopI <= Constant.MAX_PLAYERS; LoopI++)
                {
                    if (ClientTCP.IsPlaying(LoopI)) { Logic.processMovement(LoopI); }
                }

                // Npcs //
                for (int LoopI = 0; LoopI <= Constant.MAX_MAP_NPCS; LoopI++)
                {
                    if (Types.MapNpcs[LoopI].Num > -1) { Logic.processNpcMovement(LoopI); }
                }
                LastUpdateTime_WalkTimer = (long)tickCount + UpdateTime_WalkTimer;
            }

            // MapAnim
            if (LastUpdateTime_MapAnim < tickCount)
            {

                // Single Frame
                switch (Static.MapAnim)
                {
                    case 0: Static.MapAnim = 1; break;
                    case 1: Static.MapAnim = 0; break;
                }

                // Animate Waterfalls
                switch (Static.WaterfallAnim)
                {
                    case 0: Static.WaterfallAnim = 1; break;
                    case 1: Static.WaterfallAnim = 2; break;
                    case 2: Static.WaterfallAnim = 0; break;
                }

                // Animate Autotiles
                switch (Static.AutoTileAnim)
                {
                    case 0: Static.AutoTileAnim = 1; break;
                    case 1: Static.AutoTileAnim = 2; break;
                    case 2: Static.AutoTileAnim = 0; break;
                }

                LastUpdateTime_MapAnim = tickCount + UpdateTime_MapAnim;
            }

            // Send Ping Request //
            if (LastUpdateTime_Ping < tickCount)
            {
                ClientTCP.SendGetPing();
                LastUpdateTime_Ping = tickCount + UpdateTime_Ping;
            }

            if (NetworkManager.Instance.TCPClient.ConnectionStatus == NetConnectionStatus.Disconnected)
            {
                GC.Collect();   // Collect Garbage
                Static.shutDown = true;    // Exit Game
            }

        }

        //////////////////////
        // Update Menu Loop //
        //////////////////////
        public void updateMenuLoop(long tickCount)
        {

            // Send Ping Request //
            if (LastUpdateTime_Credits < tickCount)
            {
                if (Static.showCredits)
                {
                    Static.creditsAlpha = Static.creditsAlpha + 15;
                    if (Static.creditsAlpha > 255) { Static.creditsAlpha = 255; }
                }
                else
                {
                    Static.creditsAlpha = Static.creditsAlpha - 15;
                    if (Static.creditsAlpha < 0) { Static.creditsAlpha = 0; }
                }
                LastUpdateTime_Credits = tickCount + UpdateTime_Credits;
            }

            // Erase Menu Alert Message //
            if (Static.menuAlertTimer < tickCount)
            {
                Static.menuAlertMessage = String.Empty;
                Static.menuAlertColour = 0;
                Static.menuAlertTimer = 0;
            }

        }
    }
}
