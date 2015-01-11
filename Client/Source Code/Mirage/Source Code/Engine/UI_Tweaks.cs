///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using MirageXNA.Engine;
using MirageXNA.Global;

namespace MirageXNA.Engine
{
    public static class UI_Tweaks
    {
        /////////////////////////////////////////////
        ////// IN GAME GUI SETTINGS & LOCATION //////
        /////////////////////////////////////////////

        // In Game Settings
        public const bool DrawPlayerNames = true;
        public const bool DrawNpcNames = false;

        // ABOUT //
        // THIS WILL CHANGE THE POSITION OF THE GUI IN GAME AND IN THE MAIN MENU.
        // THESE SETTINGS CAN BE CHANGED DO NOT ATTEMPT TO CHANGE GUI SETTINGS ANYWEAR ELSE. 
        // UNLESS YOU ARE AN EXPERIANCED PROGRAMMER.
        // IF YOU WANT TO CHANGE THE TEXTURE NUMBER SEE EITHER UI_GAME OR UI_MENU.

        ///////////////////////////////////
        ////// MAIN INTERFACE WINDOW //////
        ///////////////////////////////////

        // CHARACTER WINDOW - BACKGROUND //
        public const int charWindowX = 800;
        public const int charWindowY = 422;
        public const int charWindowW = 195;
        public const int charWindowH = 250;

        // CHARACTER WINDOW - STAT 1 TEXT //
        public const int charWindowStat1X = 110;
        public const int charWindowStat1Y = 36;

        // CHARACTER WINDOW - STAT 2 TEXT //
        public const int charWindowStat2X = 110;
        public const int charWindowStat2Y = 54;

        // CHARACTER WINDOW - STAT 3 TEXT //
        public const int charWindowStat3X = 110;
        public const int charWindowStat3Y = 72;

        // CHARACTER WINDOW - STAT 4 TEXT //
        public const int charWindowStat4X = 110;
        public const int charWindowStat4Y = 90;

        // CHARACTER WINDOW - POINTS TEXT //
        public const string charWindowPointsCaption = "Points: ";
        public const int charWindowPointsX = 110;
        public const int charWindowPointsY = 112;

        // CHARACTER WINDOW - EXP TEXT //
        public const string charWindowExpCaption = "Experiance: ";
        public const int charWindowExpX = 8;
        public const int charWindowExpY = 138;

        // CHARACTER WINDOW - PLAYER NAME //
        public const int charWindowNameX = 98;
        public const int charWindowNameY = 10;

        //////////////////////////////////////
        ////// IN GAME WINDOW FUNCTIONS //////
        //////////////////////////////////////
        public static string getStatText(Enumerations.Stats statNum)
        {
            if (statNum < 0 | (int)statNum > 4) return String.Empty;
            return statNum.ToString() + ": " + Types.Players[Static.MyIndex].Stats[(int)statNum];
        }

        public static string getPointsText()
        {
            return charWindowPointsCaption + Types.Players[Static.MyIndex].Points;
        }

        public static string getExpText()
        {
            return charWindowExpCaption + Types.Players[Static.MyIndex].Exp + " | " + Types.Players[Static.MyIndex].MaxExp;
        }

        public static void setNameVector2()
        {
            // GET PLAYER NAME & POSITION //
            string charWindowName = Types.Players[Static.MyIndex].Name + " - Level: " + Types.Players[Static.MyIndex].Level;
            Vector2 charWindowPos = new Vector2(charWindowNameX - (General.getStringWidth(1, charWindowName) / 2), charWindowNameY);

            // UPDATE PLAYER NAME CAPTION & POSITION //
            UI_Game.gameWindow.charWindow.Text[4].CAPTION = charWindowName;
            UI_Game.gameWindow.charWindow.Text[4].X = (int)charWindowPos.X;
            UI_Game.gameWindow.charWindow.Text[4].Y = (int)charWindowPos.Y;
        }

    }
}
