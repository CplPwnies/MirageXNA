///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MirageXNA.Global;
using MirageXNA.Network;

namespace MirageXNA.Engine
{
    class Logic
    {
        //////////////////
        // Render Logic //
        //////////////////
        public static void renderAutotile(int LayerNum, int dx, int dy, int quarterNum, int X, int Y)
        {
            int Yoffset = 0; int Xoffset = 0;

            // calculate the offset
            switch (Types.Map.Tile[X, Y].Autotile[LayerNum])
            {
                case Constant.AUTOTILE_WATERFALL: Yoffset = (Static.WaterfallAnim - 1) * 32; break;
                case Constant.AUTOTILE_ANIM: Xoffset = Static.AutoTileAnim * 64; break;
                case Constant.AUTOTILE_CLIFF: Yoffset = -32; break;
            }

            // Draw the quarter
            GfxManager.Instance.drawTexture(Static.Tex_Tilesets[Types.Map.Tile[X, Y].Layer[LayerNum].Tileset], dx, dy, 16, 16, Autotiles.Autotile[X, Y].Layer[LayerNum].sX[quarterNum] + Xoffset, Autotiles.Autotile[X, Y].Layer[LayerNum].sY[quarterNum] + Yoffset, 16, 16, Color.White);
        }

        public static void renderPlayer(Int32 Index, int dx, int dy)
        {
            int Anim = 1; int sX; int sY;
            int spriteTop = 0;
            GfxManager GfxM = GfxManager.Instance;

            // Check Subscript //
            int spriteNum = Types.Players[Index].Sprite;
            if (spriteNum < 1 | spriteNum > Static.Sprites_Count) return;

            // Walking Animation //
            switch (Types.Players[Index].Dir)
            {
                case Constant.DIR_NORTH:
                    if (Types.Players[Index].OffsetY > Constant.MoveSize / 4)
                    { Anim = Types.Players[Index].Step; }
                    spriteTop = 3;
                    break;
                case Constant.DIR_SOUTH:
                    if (Types.Players[Index].OffsetY < -Constant.MoveSize / 4)
                    { Anim = Types.Players[Index].Step; }
                    spriteTop = 0;
                    break;
                case Constant.DIR_WEST:
                    if (Types.Players[Index].OffsetX > Constant.MoveSize / 4)
                    { Anim = Types.Players[Index].Step; }
                    spriteTop = 1;
                    break;
                case Constant.DIR_EAST:
                    if (Types.Players[Index].OffsetX < -Constant.MoveSize / 4)
                    { Anim = Types.Players[Index].Step; }
                    spriteTop = 2;
                    break;
            }

            // Get The Source RECT //
            sX = Anim * 32;
            sY = spriteTop * 32;

            // Render Sprite //
            GfxM.drawTexture(Static.Tex_Sprites[spriteNum],
                dx,
                dy,
                Constant.PIC_Y,
                Constant.PIC_X,
                sX,
                sY,
                Constant.PIC_Y,
                Constant.PIC_X,
                Color.White);

            if (UI_Tweaks.DrawPlayerNames)
            {
                // Above Player
                int NameX = (dx + 16) - (General.getStringWidth(1, Types.Players[Index].Name) / 2);
                int NameY = dy - 16;
                if (NameY < 0) NameY = 0;
                if (NameX < 0) NameX = 0;
                if (NameX > 1024 - General.getStringWidth(1, Types.Players[Index].Name)) NameX = 1024 - General.getStringWidth(1, Types.Players[Index].Name);
                Vector2 namePos = new Vector2(NameX, NameY);
                GfxM.drawText(1, Types.Players[Index].Name, namePos, Color.White);
            }
        }

        public static void renderNpc(Int32 mapNpcNum, int dx, int dy)
        {
            int Anim = 1; int sX; int sY;
            int spriteTop = 0;
            GfxManager GfxM = GfxManager.Instance;

            // Check Subscript //
            int spriteNum = Types.MapNpcs[mapNpcNum].Sprite;
            if (spriteNum < 1 | spriteNum > Static.Sprites_Count) return;

            // Walking Animation //
            switch (Types.MapNpcs[mapNpcNum].Dir)
            {
                case Constant.DIR_NORTH:
                    if (Types.MapNpcs[mapNpcNum].OffsetY > 8)
                    { Anim = Types.MapNpcs[mapNpcNum].Step; }
                    spriteTop = 3;
                    break;
                case Constant.DIR_SOUTH:
                    if (Types.MapNpcs[mapNpcNum].OffsetY < -8)
                    { Anim = Types.MapNpcs[mapNpcNum].Step; }
                    spriteTop = 0;
                    break;
                case Constant.DIR_WEST:
                    if (Types.MapNpcs[mapNpcNum].OffsetX > 8)
                    { Anim = Types.MapNpcs[mapNpcNum].Step; }
                    spriteTop = 1;
                    break;
                case Constant.DIR_EAST:
                    if (Types.MapNpcs[mapNpcNum].OffsetX < -8)
                    { Anim = Types.MapNpcs[mapNpcNum].Step; }
                    spriteTop = 2;
                    break;
            }

            // Get The Source RECT //
            sX = Anim * 32;
            sY = spriteTop * 32;

            // Render Sprite //
            GfxM.drawTexture(Static.Tex_Sprites[spriteNum],
                dx,
                dy,
                Constant.PIC_Y,
                Constant.PIC_X,
                sX,
                sY,
                Constant.PIC_Y,
                Constant.PIC_X,
                Color.White);

            if (UI_Tweaks.DrawNpcNames)
            {
                // Above Npc
                Vector2 namePos = new Vector2((dx + 16) - (General.getStringWidth(1, Types.MapNpcs[mapNpcNum].Name) / 2), dy - 16);
                GfxM.drawText(1, Types.MapNpcs[mapNpcNum].Name, namePos, Color.White);
            }
        }

        /////////////////////////
        // Scrolling Map Logic //
        /////////////////////////
        public static Rectangle nextFrame()
        {
            Rectangle Offset = new Rectangle(0, 0, 0, 0);
            int OffsetX;
            int OffsetY;
            int StartX;
            int StartY;
            int EndX;
            int EndY;

            // Set Offset
            OffsetX = Types.Players[Static.MyIndex].OffsetX + Constant.MoveSize;
            OffsetY = Types.Players[Static.MyIndex].OffsetY + Constant.MoveSize;

            // PreCache
            StartX = Types.Players[Static.MyIndex].X - Static.HalfX;
            StartY = Types.Players[Static.MyIndex].Y - Static.HalfY;

            if (StartX < 0)
            {
                OffsetX = 0;
                if (StartX == -1)
                {
                    if (Types.Players[Static.MyIndex].OffsetX > 0)
                    {
                        OffsetX = Types.Players[Static.MyIndex].OffsetX;
                    }
                }
                StartX = 0;
            }

            if (StartY < 0)
            {
                OffsetY = 0;
                if (StartY == -1)
                {
                    if (Types.Players[Static.MyIndex].OffsetY > 0)
                    {
                        OffsetY = Types.Players[Static.MyIndex].OffsetY;
                    }
                }
                StartY = 0;
            }

            EndX = StartX + (Static.TILE_MAX_MAPX + 1) + 1;
            EndY = StartY + (Static.TILE_MAX_MAPY + 1) + 1;

            if (EndX > (Types.Map.MaxX - 1))
            {
                OffsetX = Constant.MoveSize;

                if (EndX == Types.Map.MaxX)
                {
                    if (Types.Players[Static.MyIndex].OffsetX < 0)
                    {
                        OffsetX = Types.Players[Static.MyIndex].OffsetX + Constant.MoveSize;
                    }
                }

                EndX = (Types.Map.MaxX - 1);
                StartX = EndX - Static.TILE_MAX_MAPX - 1;
            }

            if (EndY > (Types.Map.MaxY - 1))
            {
                OffsetY = Constant.MoveSize;

                if (EndY == Types.Map.MaxY)
                {
                    if (Types.Players[Static.MyIndex].OffsetY < 0)
                    {
                        OffsetY = Types.Players[Static.MyIndex].OffsetY + Constant.MoveSize;
                    }
                }

                EndY = (Types.Map.MaxY - 1);
                StartY = EndY - Static.TILE_MAX_MAPY - 1;
            }

            Static.ScreenMinY = OffsetY;
            Static.ScreenMaxY = Static.ScreenMinY + Static.ScreenY;
            Static.ScreenMinX = OffsetX;
            Static.ScreenMaxX = Static.ScreenMinX + Static.ScreenX;

            //Check if we need to update the graphics
            if (StartX != Static.LastTileX | StartY != Static.LastTileY)
            {
                Static.MinY = StartY;
                Static.MaxY = EndY;
                Static.MinX = StartX;
                Static.MaxX = EndX;

                //Update the last position
                Static.LastTileX = StartX;
                Static.LastTileY = StartY;

                //Re-create the tile layers
                General.cacheTiles();
            }

            Offset.X = (Static.MinX * Constant.MoveSize) + OffsetX;
            Offset.Y = (Static.MinY * Constant.MoveSize) + OffsetY;
            return Offset;
        }

        //////////////////
        // Player Logic //
        //////////////////

        public static void processMovement(Int32 Index)
        {
            int MovementSpeed = 4;

            switch (Types.Players[Index].Dir)
            {
                case Constant.DIR_NORTH:
                    Types.Players[Index].OffsetY = Types.Players[Index].OffsetY - MovementSpeed;
                    if (Types.Players[Index].OffsetY < 0) Types.Players[Index].OffsetY = 0;
                    break;
                case Constant.DIR_SOUTH:
                    Types.Players[Index].OffsetY = Types.Players[Index].OffsetY + MovementSpeed;
                    if (Types.Players[Index].OffsetY > 0) Types.Players[Index].OffsetY = 0;
                    break;
                case Constant.DIR_WEST:
                    Types.Players[Index].OffsetX = Types.Players[Index].OffsetX - MovementSpeed;
                    if (Types.Players[Index].OffsetX < 0) Types.Players[Index].OffsetX = 0;
                    break;
                case Constant.DIR_EAST:
                    Types.Players[Index].OffsetX = Types.Players[Index].OffsetX + MovementSpeed;
                    if (Types.Players[Index].OffsetX > 0) Types.Players[Index].OffsetX = 0;
                    break;
            }

            // Check if completed walking over to the next tile
            if (Types.Players[Index].Moving > 0)
            {
                switch (Types.Players[Index].Dir)
                {

                    case Constant.DIR_EAST:
                        if (Types.Players[Index].OffsetX >= 0 & Types.Players[Index].OffsetY >= 0)
                        {
                            Types.Players[Index].Moving = 0;
                            if (Types.Players[Index].Step == 0)
                                Types.Players[Index].Step = 2;
                            else
                                Types.Players[Index].Step = 0;
                        }
                        break;

                    case  Constant.DIR_SOUTH:
                        if (Types.Players[Index].OffsetX >= 0 & Types.Players[Index].OffsetY >= 0)
                        {
                            Types.Players[Index].Moving = 0;
                            if (Types.Players[Index].Step == 0)
                                Types.Players[Index].Step = 2;
                            else
                                Types.Players[Index].Step = 0;
                        }
                        break;

                    case Constant.DIR_WEST:
                        if (Types.Players[Index].OffsetX <= 0 & Types.Players[Index].OffsetY <= 0)
                        {
                            Types.Players[Index].Moving = 0;
                            if (Types.Players[Index].Step == 0)
                            {
                                Types.Players[Index].Step = 2;
                            }
                            else
                            {
                                Types.Players[Index].Step = 0;
                            }
                        }
                        break;

                    case Constant.DIR_NORTH:
                        if (Types.Players[Index].OffsetX <= 0 & Types.Players[Index].OffsetY <= 0)
                        {
                            Types.Players[Index].Moving = 0;
                            if (Types.Players[Index].Step == 0)
                            {
                                Types.Players[Index].Step = 2;
                            }
                            else
                            {
                                Types.Players[Index].Step = 0;
                            }
                        }
                        break;
                }
            }
        }

        public static bool checkDirection(byte direction)
        {
            int X = 0; int Y = 0;

            switch (direction)
            {
                case Constant.DIR_NORTH:
                    X = Types.Players[Static.MyIndex].X;
                    Y = Types.Players[Static.MyIndex].Y - 1;
                    break;
                case Constant.DIR_SOUTH:
                    X = Types.Players[Static.MyIndex].X;
                    Y = Types.Players[Static.MyIndex].Y + 1;
                    break;
                case Constant.DIR_WEST:
                    X = Types.Players[Static.MyIndex].X - 1;
                    Y = Types.Players[Static.MyIndex].Y;
                    break;
                case Constant.DIR_EAST:
                    X = Types.Players[Static.MyIndex].X + 1;
                    Y = Types.Players[Static.MyIndex].Y;
                    break;
            }

            // Check for blocked map tile
            if (Types.Map.Tile[X, Y].Type == Constant.TILE_TYPE_BLOCKED)
            {
                return true;
            }

            // Check for Npcs
            for (int i = 1; i <= Constant.MAX_MAP_NPCS; i++)
            {
                if (Types.MapNpcs[i].Active)
                {
                    if (Types.MapNpcs[i].X == X)
                    {
                        if (Types.MapNpcs[i].Y == Y)
                        {
                            return true;
                        }
                    }
                }
            }

            // Check for Players
            for (int i = 1; i <= Constant.MAX_PLAYERS; i++)
            {
                if (ClientTCP.IsPlaying(i) && Types.Players[i].Map == Types.Players[Static.MyIndex].Map)
                {
                    if (Types.Players[i].X == X)
                    {
                        if (Types.Players[i].Y == Y)
                        {
                            return true;
                        }
                    }
                }
            }

            // No Blocks - Can Move //
            return false;
        }

        public static bool canMove()
        {
            bool cMove = true;
            int dir = Types.Players[Static.MyIndex].Dir;

            if (Types.Players[Static.MyIndex].Moving != 0) { return false; }
            if (Static.DirUp)
            {
                Types.Players[Static.MyIndex].Dir = Constant.DIR_NORTH;
                if (Types.Players[Static.MyIndex].Y > 0)
                {
                    if (checkDirection(Constant.DIR_NORTH))
                    {
                        cMove = false;

                        // Change Direction //
                        if (dir != Constant.DIR_NORTH)
                        {
                            ClientTCP.SendPlayerDir();
                        }
                    }
                }
                else
                {
                    // Check if they can warp to a new map
                    if (Types.Map.Up > 0)
                    {
                        ClientTCP.SendPlayerRequestNewMap();
                        //CanMoveNow = False
                    }
                    return false;
                }
                return cMove;
            }

            if (Static.DirDown)
            {
                Types.Players[Static.MyIndex].Dir = Constant.DIR_SOUTH;
                if (Types.Players[Static.MyIndex].Y < (Static.MAX_MAPY))
                {
                    if (checkDirection(Constant.DIR_SOUTH))
                    {
                        cMove = false;

                        // Change Direction //
                        if (dir != Constant.DIR_SOUTH)
                        {
                            ClientTCP.SendPlayerDir();
                        }
                    }
                }
                else
                {
                    // Check if they can warp to a new map
                    if (Types.Map.Down > 0)
                    {
                        ClientTCP.SendPlayerRequestNewMap();
                        //CanMoveNow = False
                    }
                    return false;
                }
                return cMove;
            }

            if (Static.DirLeft)
            {
                Types.Players[Static.MyIndex].Dir = Constant.DIR_WEST;
                if (Types.Players[Static.MyIndex].X > 0)
                {
                    if (checkDirection(Constant.DIR_WEST))
                    {
                        cMove = false;

                        // Change Direction //
                        if (dir != Constant.DIR_WEST)
                        {
                            ClientTCP.SendPlayerDir();
                        }
                    }
                }
                else
                {
                    // Check if they can warp to a new map
                    if (Types.Map.Left > 0)
                    {
                        ClientTCP.SendPlayerRequestNewMap();
                        //CanMoveNow = False
                    }
                    return false;
                }
                return cMove;
            }
            if (Static.DirRight)
            {
                Types.Players[Static.MyIndex].Dir = Constant.DIR_EAST;
                if (Types.Players[Static.MyIndex].X < (Static.MAX_MAPX ))
                {
                    if (checkDirection(Constant.DIR_EAST))
                    {
                        cMove = false;

                        // Change Direction //
                        if (dir != Constant.DIR_EAST)
                        {
                            ClientTCP.SendPlayerDir();
                        }
                    }
                }
                else
                {
                    // Check if they can warp to a new map
                    if (Types.Map.Right > 0)
                    {
                        ClientTCP.SendPlayerRequestNewMap();
                        //CanMoveNow = False
                    }
                    return false;
                }
                return cMove;
            }

            // Return Final Value //
            return cMove;
        }

        public static void setMovement(byte Direction)
        {
            switch (Direction)
            {
                case Constant.DIR_NORTH: Static.DirUp = true; Static.DirDown = false; Static.DirLeft = false; Static.DirRight = false; break;
                case Constant.DIR_SOUTH: Static.DirUp = false; Static.DirDown = true; Static.DirLeft = false; Static.DirRight = false; break;
                case Constant.DIR_EAST: Static.DirUp = false; Static.DirDown = false; Static.DirLeft = false; Static.DirRight = true; break;
                case Constant.DIR_WEST: Static.DirUp = false; Static.DirDown = false; Static.DirLeft = true; Static.DirRight = false; break;
            }

        }

        public static void checkMovement()
        {
            if (Static.DirUp | Static.DirDown | Static.DirLeft | Static.DirRight)
            {
                if (canMove())
                {
                    Types.Players[Static.MyIndex].Moving = Constant.MOVING_RUNNING;

                    switch (Types.Players[Static.MyIndex].Dir)
                    {
                        case Constant.DIR_NORTH:
                            ClientTCP.SendPlayerMove();
                            Types.Players[Static.MyIndex].OffsetY = Constant.MoveSize;
                            Types.Players[Static.MyIndex].Y = Types.Players[Static.MyIndex].Y - 1;
                            break;
                        case Constant.DIR_SOUTH:
                            ClientTCP.SendPlayerMove();
                            Types.Players[Static.MyIndex].OffsetY = Constant.MoveSize * -1;
                            Types.Players[Static.MyIndex].Y = Types.Players[Static.MyIndex].Y + 1;
                            break;
                        case Constant.DIR_WEST:
                            ClientTCP.SendPlayerMove();
                            Types.Players[Static.MyIndex].OffsetX = Constant.MoveSize;
                            Types.Players[Static.MyIndex].X = Types.Players[Static.MyIndex].X - 1;
                            break;
                        case Constant.DIR_EAST:
                            ClientTCP.SendPlayerMove();
                            Types.Players[Static.MyIndex].OffsetX = Constant.MoveSize * -1;
                            Types.Players[Static.MyIndex].X = Types.Players[Static.MyIndex].X + 1;
                            break;
                    }
                }
            }
        }

        public static void checkAttack()
        {
            int Tick = (int)Mirage._gameTime.TotalGameTime.TotalMilliseconds;

            // Check to see if we want to stop making him attack
            if (Types.Players[Static.MyIndex].AttackTimer + 1000 < Tick)
            {
                Types.Players[Static.MyIndex].Attacking = 0;
                Types.Players[Static.MyIndex].AttackTimer = 0;
            }

            if (Static.Ctrl)
            {
                if (Types.Players[Static.MyIndex].AttackTimer + 1000 < Tick)
                {
                    if (Types.Players[Static.MyIndex].Attacking == 0)
                    {
                        Types.Players[Static.MyIndex].Attacking = 1;
                        Types.Players[Static.MyIndex].AttackTimer = Tick;
                        ClientTCP.SendAttack();
                    }
                }
            }
        }

        ///////////////
        // Npc Logic //
        ///////////////

        public static void processNpcMovement(Int32 Index)
        {
            int MovementSpeed = 2;

            switch (Types.MapNpcs[Index].Dir)
            {
                case Constant.DIR_NORTH:
                    Types.MapNpcs[Index].OffsetY = Types.MapNpcs[Index].OffsetY - MovementSpeed;
                    if (Types.MapNpcs[Index].OffsetY < 0) Types.MapNpcs[Index].OffsetY = 0;
                    break;
                case Constant.DIR_SOUTH:
                    Types.MapNpcs[Index].OffsetY = Types.MapNpcs[Index].OffsetY + MovementSpeed;
                    if (Types.MapNpcs[Index].OffsetY > 0) Types.MapNpcs[Index].OffsetY = 0;
                    break;
                case Constant.DIR_WEST:
                    Types.MapNpcs[Index].OffsetX = Types.MapNpcs[Index].OffsetX - MovementSpeed;
                    if (Types.MapNpcs[Index].OffsetX < 0) Types.MapNpcs[Index].OffsetX = 0;
                    break;
                case Constant.DIR_EAST:
                    Types.MapNpcs[Index].OffsetX = Types.MapNpcs[Index].OffsetX + MovementSpeed;
                    if (Types.MapNpcs[Index].OffsetX > 0) Types.MapNpcs[Index].OffsetX = 0;
                    break;
            }

            // Check if completed walking over to the next tile
            if (Types.MapNpcs[Index].Moving > 0)
            {
                switch (Types.MapNpcs[Index].Dir)
                {

                    case Constant.DIR_EAST:
                        if (Types.MapNpcs[Index].OffsetX >= 0 & Types.MapNpcs[Index].OffsetY >= 0)
                        {
                            Types.MapNpcs[Index].Moving = 0;
                            if (Types.MapNpcs[Index].Step == 0)
                                Types.MapNpcs[Index].Step = 2;
                            else
                                Types.MapNpcs[Index].Step = 0;
                        }
                        break;

                    case Constant.DIR_SOUTH:
                        if (Types.MapNpcs[Index].OffsetX >= 0 & Types.MapNpcs[Index].OffsetY >= 0)
                        {
                            Types.MapNpcs[Index].Moving = 0;
                            if (Types.MapNpcs[Index].Step == 0)
                                Types.MapNpcs[Index].Step = 2;
                            else
                                Types.MapNpcs[Index].Step = 0;
                        }
                        break;

                    case Constant.DIR_WEST:
                        if (Types.MapNpcs[Index].OffsetX <= 0 & Types.MapNpcs[Index].OffsetY <= 0)
                        {
                            Types.MapNpcs[Index].Moving = 0;
                            if (Types.MapNpcs[Index].Step == 0)
                            {
                                Types.MapNpcs[Index].Step = 2;
                            }
                            else
                            {
                                Types.MapNpcs[Index].Step = 0;
                            }
                        }
                        break;

                    case Constant.DIR_NORTH:
                        if (Types.MapNpcs[Index].OffsetX <= 0 & Types.MapNpcs[Index].OffsetY <= 0)
                        {
                            Types.MapNpcs[Index].Moving = 0;
                            if (Types.MapNpcs[Index].Step == 0)
                            {
                                Types.MapNpcs[Index].Step = 2;
                            }
                            else
                            {
                                Types.MapNpcs[Index].Step = 0;
                            }
                        }
                        break;
                }
            }
        }
    }


}
