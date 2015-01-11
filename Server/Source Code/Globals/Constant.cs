using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MirageXNA.Globals
{
    class Constant
    {
        // [Game Logic] //
        public const byte MAX_POINTS = 2;   // Points Per Level

        // General Constants //
        public const string GAME_NAME = "Mirage XNA Client";
        public const string GAME_WEBSITE = "http://www.rpgcreation.com";
        public const byte MAX_MAPS = 10;
        public const byte MAX_CLASSES = 3;
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

        // Direction //
        public const byte DIR_NORTH = 0;
        public const byte DIR_SOUTH = 1;
        public const byte DIR_WEST = 2;
        public const byte DIR_EAST = 3;

        // Player Movement //
        public const byte MOVING_WALKING = 1;
        public const byte MOVING_RUNNING = 2;

        // Admin Constants //
        public const byte ADMIN_MONITOR = 1;
        public const byte ADMIN_MAPPER = 2;
        public const byte ADMIN_DEVELOPER = 3;
        public const byte ADMIN_CREATOR = 4;

        // NPC Constants //
        public const byte NPC_BEHAVIOUR_ATTACK_ROAMING = 0;
        public const byte NPC_BEHAVIOUR_ATTACK_STANDING = 1;
        public const byte NPC_BEHAVIOUR_GUARD_ROAMING = 2;
        public const byte NPC_BEHAVIOUR_GUARD_STANDING = 3;
        public const byte NPC_BEHAVIOUR_FRIENDLY_ROAMING = 4;
        public const byte NPC_BEHAVIOUR_FRIENDLY_STANDING = 5;
        public const byte NPC_BEHAVIOUR_SELLER_ROAMING = 6;
        public const byte NPC_BEHAVIOUR_SELLER_STANDING = 7;
        public const byte NPC_BEHAVIOUR_QUEST_ROAMING = 8;
        public const byte NPC_BEHAVIOUR_QUEST_STANDING = 9;

        // Target Type constants //
        public const byte TARGET_TYPE_NONE = 0;
        public const byte TARGET_TYPE_PLAYER = 1;
        public const byte TARGET_TYPE_NPC = 2;
        public const byte NPC_TARGET_PLAYER = 3;
        public const byte NPC_TARGET_NPC = 4;
    }
}
