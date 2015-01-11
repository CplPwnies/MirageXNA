///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework.Input;

namespace MirageXNA.Global
{
    public static class Static
    {

        // IP & Port //
        public static string IpAddress = "127.0.0.1";
        public static int Port = 7001;

        // Atempt Login //
        public static bool atemptLogin = false;

        // Shutdown //
        public static bool shutDown = false;

        // Input Controls //
        public static Keys PlayerUp = Keys.W;
        public static Keys PlayerDown = Keys.S;
        public static Keys PlayerLeft = Keys.A;
        public static Keys PlayerRight = Keys.D;

        public static int MyIndex;

        // Fps Count //
        public static long totalTime = 0;
        public static long frameCount = 0;
        public static long fps = 0;
        
        // Pinging //
        public static long PingStart;
        public static long PingEnd;
        public static long Ping;

        // Main Menu - Globals //
        public static byte menuType = 0;
        public static byte curMainMenu = 1;
        public static bool showCredits = false;
        public static int creditsAlpha = 0;

        // Menu Alert Message //
        public static int menuAlertColour;
        public static string menuAlertMessage;
        public static long menuAlertTimer;

        // Login //
        public static byte loginInput = 1;
        public static string Username = String.Empty;
        public static string Password = String.Empty;

        // Register //
        public static byte registerInput = 1;
        public static string rUsername = String.Empty;
        public static string rPassword = String.Empty;
        public static int rSprite = 1;

        // Directions Booleans //
        public static bool DirUp = false;
        public static bool DirDown = false;
        public static bool DirLeft = false;
        public static bool DirRight = false;
        public static bool Ctrl = false;

        // Chat //
        public static int ChatBox_Chunk;
        public static byte Line_Amount;

        // Chat Var //
        public static string ShownText;
        public static bool EnterText;
        public static string EnterTextBuffer;
        public static int EnterTextBufferWidth;

        public static byte lastClickedWindow;
        public static bool[] ShowGameWindow;

        // Pass MapX & MapY to other objects //
        public static int _MaxX;
        public static int _MaxY;

        // Scrolling Map //
        public static int MinX;
        public static int MaxX;
        public static int MinY;
        public static int MaxY;
        public static int ScreenMinY;   //Start Y pos on current screen
        public static int ScreenMaxY;   //End Y pos on current screen
        public static int ScreenMinX;   //Start X pos on current screen
        public static int ScreenMaxX;   //End X pos on current screen
        public static int MiniMapX;     //Offset
        public static int MiniMapY;
        public static int LastTileX;
        public static int LastTileY;

        // Map Co-ords //
        public static int ScreenX = 1024;
        public static int ScreenY = 768;
        public static byte MAX_MAPX = 1024 / Constant.MoveSize - 1;
        public static byte MAX_MAPY = 768 / Constant.MoveSize - 1;
        public static int HalfX = MAX_MAPX / 2 - 1;
        public static int HalfY = MAX_MAPY / 2 - 1;

        // Tile Map
        public static byte TILE_MAX_MAPX = 1024 / 32 - 1;
        public static byte TILE_MAX_MAPY = 768 / 32 - 1;

        ////////////////////
        // TEXTURE SYSTEM //
        ////////////////////
        public static int[] Tex_Tilesets;
        public static int[] Tex_Sprites;
        public static int[] Tex_Faces;
        public static int[] Tex_Gui;
        public static int[] Tex_Buttons;
        public static int[] Tex_Buttons_C;
        public static int[] Tex_Buttons_H;

        public static int Tilesets_Count = 1;
        public static int Sprites_Count = 1;
        public static int Faces_Count = 1;
        public static int Gui_Count = 1;
        public static int Buttons_Count = 1;
        public static int Buttons_C_Count = 1;
        public static int Buttons_H_Count = 1;

        public static int mTextureNum;
        public static int CurrentTexture;

        // Map Animation
        public static byte MapAnim;
        public static byte WaterfallAnim;
        public static byte AutoTileAnim;

        // High Indexing
        public static int HighIndex;
        public static int npcHighIndex;

    }
}
