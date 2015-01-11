///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

namespace MirageXNA.Global
{
    public class Constant
    {
        /////////////////
        // Main Window //
        /// /////////////
        public const byte inMenu = 0;
        public const byte inGame = 1;

        // Player Movement (Pixels)
        public const byte MoveSize = 32;

        ///////////////////////////
        // Menu Window Constants //
        ///////////////////////////

        // Main Menu - Selected Window Constants //
        public const byte curMenuMain = 1;
        public const byte curMenuLogin = 2;
        public const byte curMenuRegister = 3;
        public const byte curMenuOptions = 4;
        public const byte curMenuCredits = 5;

        ///////////////////////////
        // Game Window Constants //
        ///////////////////////////

        // Game - Selected Window Constants //
        public const byte chatWindow = 1;
        public const byte charWindow = 2;
        public const byte inventoryWindow = 3;
        public const byte spellWindow = 4;
        public const byte menuWindow = 5;

        // Max //
        public const byte maxWindows = 6;

        // Chat Window - GUI Constants //
        public const int Max_Chat_Buttons = 2;
        public const int Max_Chat_Textures = 2;

        public const int maxChat = 200;

        ////////////////////////
        // Game GUI Constants //
        ////////////////////////

        // Button Constants //
        public const int BUTTON_STATE_NORMAL = 1;
        public const int BUTTON_STATE_HOVER = 2;
        public const int BUTTON_STATE_CLICK = 3;

        //////////////////////
        // Player Constants //
        //////////////////////

        // Player Size //
        public const byte PIC_X = 32;
        public const byte PIC_Y = 32;

        // Direction //
        public const byte DIR_NORTH = 0;
        public const byte DIR_SOUTH = 1;
        public const byte DIR_WEST = 2;
        public const byte DIR_EAST = 3;

        // Player Movement //
        public const byte MOVING_WALKING = 1;
        public const byte MOVING_RUNNING = 2;

        ///////////////////
        // Tile Consants //
        ///////////////////
        public const byte TILE_TYPE_WALKABLE = 0;
        public const byte TILE_TYPE_BLOCKED = 1;
        public const byte TILE_TYPE_WARP = 2;
        public const byte TILE_TYPE_ITEM = 3;
        public const byte TILE_TYPE_NPCAVOID = 4;
        public const byte TILE_TYPE_KEY = 5;
        public const byte TILE_TYPE_KEYOPEN = 6;
        public const byte TILE_TYPE_RESOURCE = 7;
        public const byte TILE_TYPE_DOOR = 8;
        public const byte TILE_TYPE_NPCSPAWN = 9;
        public const byte TILE_TYPE_SHOP = 10;
        public const byte TILE_TYPE_BANK = 11;
        public const byte TILE_TYPE_HEAL = 12;
        public const byte TILE_TYPE_TRAP = 13;
        public const byte TILE_TYPE_CHAT = 14;

        ///////////////////////
        // General Constants //
        ///////////////////////

        // Game Info Constants //
        public const string GAME_NAME = "Mirage XNA Client";
        public const string GAME_WEBSITE = "http://www.rpgcreation.com";

        // Max Sizes //
        public const byte MAX_MAPS = 10;
        public const byte MAX_PLAYERS = 255;
        public const byte MAX_ELEMENTS = 25;
        public const byte MAX_ITEMS = 255;
        public const byte MAX_NPCS = 255;
        public const byte MAX_ANIMATIONS = 255;
        public const byte MAX_INV = 35;
        public const byte MAX_SKILLS = 35;
        public const byte MAX_MAP_ITEMS = 255;
        public const byte MAX_MAP_NPCS = 30;
        public const byte MAX_SHOPS = 50;
        public const byte MAX_SPELLS = 255;
        public const byte MAX_TRADES = 35;
        public const byte MAX_RESOURCES = 100;
        public const byte MAX_LEVELS = 100;
        public const byte MAX_BANK = 42;
        public const byte MAX_PARTYS = 35;
        public const byte MAX_PARTY_MEMBERS = 4;
        public const byte MAX_CONVS = 255;
        public const byte MAX_NPC_DROPS = 10;
        public const byte MAX_QUESTS = 255;
        public const byte MAX_PLAYER_QUESTS = 10;

        ////////////////////////
        // Autotile Constants //
        ////////////////////////

        // Autotiles
        public const int AUTO_INNER = 1;
        public const int AUTO_OUTER = 2;
        public const int AUTO_HORIZONTAL = 3;
        public const int AUTO_VERTICAL = 4;
        public const int AUTO_FILL = 5;

        // Autotile Types
        public const int AUTOTILE_NONE = 0;
        public const int AUTOTILE_NORMAL = 1;
        public const int AUTOTILE_FAKE = 2;
        public const int AUTOTILE_ANIM = 3;
        public const int AUTOTILE_CLIFF = 4;
        public const int AUTOTILE_WATERFALL = 5;
    }
}
