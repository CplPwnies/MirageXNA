///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using MirageXNA.Global;

namespace MirageXNA.Global
{
    public static class Autotiles
    {

        #region Autotile Structures
        public struct Destination_Struct
        {
            public int X;
            public int Y;
        }

        public struct QuarterTile_Struct
        {
            public Destination_Struct[] QuarterTile;    //4
            public byte RenderState;
            public int[] sX;    //4
            public int[] sY;    //4
        }

        public struct Autotile_Struct
        {
            public QuarterTile_Struct[] Layer; //5
        }
        #endregion

        #region Autotile Variables

        // Autotiling //
        public static Destination_Struct[] autoInner = new Destination_Struct[5];   //4
        public static Destination_Struct[] autoNW = new Destination_Struct[5];      //4
        public static Destination_Struct[] autoNE = new Destination_Struct[5];      //4
        public static Destination_Struct[] autoSW = new Destination_Struct[5];      //4
        public static Destination_Struct[] autoSE = new Destination_Struct[5];      //4
        public static Autotile_Struct[,] Autotile;                                  //X,Y

        // Rendering //
        public static byte RENDER_STATE_NONE = 0;
        public static byte RENDER_STATE_NORMAL = 1;
        public static byte RENDER_STATE_AUTOTILE = 2;
        #endregion

        /////////////////////////////
        ////// Cache Autotiles //////
        /////////////////////////////
        public static void cacheAutotileArray()
        {
            // For simplicity's sake we cache all subtile SOURCE positions in to an array.
            // We also give letters to each subtile for easy rendering tweaks. ;]

            // Inner tiles (Top right subtile region)
            // NW - a
            autoInner[1].X = 32;
            autoInner[1].Y = 0;

            // NE - b
            autoInner[2].X = 48;
            autoInner[2].Y = 0;

            // SW - c
            autoInner[3].X = 32;
            autoInner[3].Y = 16;

            // SE - d
            autoInner[4].X = 48;
            autoInner[4].Y = 16;

            // Outer Tiles - NW (bottom subtile region)
            // NW - e
            autoNW[1].X = 0;
            autoNW[1].Y = 32;

            // NE - f
            autoNW[2].X = 16;
            autoNW[2].Y = 32;

            // SW - g
            autoNW[3].X = 0;
            autoNW[3].Y = 48;

            // SE - h
            autoNW[4].X = 16;
            autoNW[4].Y = 48;

            // Outer Tiles - NE (bottom subtile region)
            // NW - i
            autoNE[1].X = 32;
            autoNE[1].Y = 32;

            // NE - g
            autoNE[2].X = 48;
            autoNE[2].Y = 32;

            // SW - k
            autoNE[3].X = 32;
            autoNE[3].Y = 48;

            // SE - l
            autoNE[4].X = 48;
            autoNE[4].Y = 48;

            // Outer Tiles - SW (bottom subtile region)
            // NW - m
            autoSW[1].X = 0;
            autoSW[1].Y = 64;

            // NE - n
            autoSW[2].X = 16;
            autoSW[2].Y = 64;

            // SW - o
            autoSW[3].X = 0;
            autoSW[3].Y = 80;

            // SE - p
            autoSW[4].X = 16;
            autoSW[4].Y = 80;

            // Outer Tiles - SE (bottom subtile region)
            // NW - q
            autoSE[1].X = 32;
            autoSE[1].Y = 64;

            // NE - r
            autoSE[2].X = 48;
            autoSE[2].Y = 64;

            // SW - s
            autoSE[3].X = 32;
            autoSE[3].Y = 80;

            // SE - t
            autoSE[4].X = 48;
            autoSE[4].Y = 80;
        }

        public static void cacheAutotiles()
        {
            // Procedure used to cache autotile positions. All positioning is
            // independant from the tileset. Calculations are convoluted and annoying.
            // Maths is not my strong point. Luckily we're caching them so it's a one-off
            // thing when the map is originally loaded. As such optimisation isn't an issue.
            for (int X = 0; X <= (Types.Map.MaxX - 1); X++)
            {
                for (int Y = 0; Y <= (Types.Map.MaxY - 1); Y++)
                {
                    for (int LayerNum = 0; LayerNum <= 4; LayerNum++)
                    {
                        // calculate the subtile positions and place them
                        calculateAutotile(X, Y, LayerNum);
                        // cache the rendering state of the tiles and set them
                        cacheRenderState(X, Y, LayerNum);
                    }
                }
            }
        }

        /////////////////////////////
        ////// Autotile System //////
        /////////////////////////////
        public static void setAutotile(int LayerNum, int X, int Y, byte tileQuarter, string autoTileLetter)
        {

            switch (autoTileLetter)
            {
                case "a":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoInner[1].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoInner[1].Y;
                    return;
                case "b":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoInner[2].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoInner[2].Y;
                    return;
                case "c":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoInner[3].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoInner[3].Y;
                    return;
                case "d":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoInner[4].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoInner[4].Y;
                    return;
                case "e":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNW[1].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNW[1].Y;
                    return;
                case "f":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNW[2].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNW[2].Y;
                    return;
                case "g":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNW[3].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNW[3].Y;
                    return;
                case "h":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNW[4].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNW[4].Y;
                    return;
                case "i":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNE[1].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNE[1].Y;
                    return;
                case "j":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNE[2].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNE[2].Y;
                    return;
                case "k":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNE[3].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNE[3].Y;
                    return;
                case "l":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoNE[4].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoNE[4].Y;
                    return;
                case "m":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSW[1].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSW[1].Y;
                    return;
                case "n":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSW[2].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSW[2].Y;
                    return;
                case "o":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSW[3].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSW[3].Y;
                    return;
                case "p":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSW[4].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSW[4].Y;
                    return;
                case "q":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSE[1].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSE[1].Y;
                    return;
                case "r":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSE[2].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSE[2].Y;
                    return;
                case "s":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSE[3].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSE[3].Y;
                    return;
                case "t":
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].X = autoSE[4].X;
                    Autotile[X, Y].Layer[LayerNum].QuarterTile[tileQuarter].Y = autoSE[4].Y;
                    return;
            }
        }

        public static void cacheRenderState(int X, int Y, int LayerNum)
        {
            int quarterNum;

            // exit out early
            if ((X < 0 | X > Types.Map.MaxX - 1) | (Y < 0 | Y > Types.Map.MaxY - 1)) { return; }

            // check if the tile can be rendered
            if (Types.Map.Tile[X, Y].Layer[LayerNum].Tileset <= 0 | Types.Map.Tile[X, Y].Layer[LayerNum].Tileset > Static.Tilesets_Count)
            {
                Autotile[X, Y].Layer[LayerNum].RenderState = RENDER_STATE_NONE;
                return;
            }

            // check if it's a key - hide mask if key is closed
            //if (LayerNum == 1)
            //{
            //if (clsTypes.Map.Tile[X, Y].Type == TILE_TYPE_KEY) //TILE_TYPE_KEY
            //{
            //if (TempTile(X, Y).DoorOpen == 0)
            //{
            //    Autotile[X, Y].Layer[LayerNum].RenderState = RENDER_STATE_NONE;
            //    return;
            //}
            //else
            //{
            //    Autotile[X, Y].Layer[LayerNum].RenderState = RENDER_STATE_NORMAL;
            //    return;
            //}
            //}
            //}

            // check if it needs to be rendered as an autotile
            if ((Types.Map.Tile[X, Y].Autotile[LayerNum] == Constant.AUTOTILE_NONE) | (Types.Map.Tile[X, Y].Autotile[LayerNum] == Constant.AUTOTILE_FAKE))    //| Options.Autotile = 0
            {
                // default to... default
                Autotile[X, Y].Layer[LayerNum].RenderState = RENDER_STATE_NORMAL;
            }
            else
            {
                Autotile[X, Y].Layer[LayerNum].RenderState = RENDER_STATE_AUTOTILE;

                // cache tileset positioning
                for (quarterNum = 1; quarterNum <= 4; quarterNum++)
                {
                    Autotile[X, Y].Layer[LayerNum].sX[quarterNum] = (Types.Map.Tile[X, Y].Layer[LayerNum].X * 32) + Autotile[X, Y].Layer[LayerNum].QuarterTile[quarterNum].X;
                    Autotile[X, Y].Layer[LayerNum].sY[quarterNum] = (Types.Map.Tile[X, Y].Layer[LayerNum].Y * 32) + Autotile[X, Y].Layer[LayerNum].QuarterTile[quarterNum].Y;
                }
            }

        }

        public static void calculateAutotile(int X, int Y, int LayerNum)
        {
            // Right, so we've split the tile block in to an easy to remember
            // collection of letters. We now need to do the calculations to find
            // out which little lettered block needs to be rendered. We do this
            // by reading the surrounding tiles to check for matches.

            // First we check to make sure an autotile situation is actually there.
            // Then we calculate eXactly which situation has arisen.
            // The situations are "inner", "outer", "horizontal", "vertical" and "fill".

            // EXit out if we don't have an auatotile
            if (Types.Map.Tile[X, Y].Autotile[LayerNum] == 0) { return; }
            
            // Okay, we have autotiling but which one?
            switch (Types.Map.Tile[X, Y].Autotile[LayerNum])
            {

                // Anything
                case 0:
                // Don't need to render anything... 

                // Normal or animated - same difference
                case Constant.AUTOTILE_NORMAL:
                    // North West Quarter
                    CalculateNW_Normal(LayerNum, X, Y);

                    // North East Quarter
                    CalculateNE_Normal(LayerNum, X, Y);

                    // South West Quarter
                    CalculateSW_Normal(LayerNum, X, Y);

                    // South East Quarter
                    CalculateSE_Normal(LayerNum, X, Y);

                    return;

                // Render Nothing
                case Constant.AUTOTILE_FAKE: return;

                // Normal or animated - same difference
                case Constant.AUTOTILE_ANIM:
                    // North West Quarter
                    CalculateNW_Normal(LayerNum, X, Y);

                    // North East Quarter
                    CalculateNE_Normal(LayerNum, X, Y);

                    // South West Quarter
                    CalculateSW_Normal(LayerNum, X, Y);

                    // South East Quarter
                    CalculateSE_Normal(LayerNum, X, Y);

                    return;

                // Cliff
                case Constant.AUTOTILE_CLIFF:
                    // North West Quarter
                    CalculateNW_Cliff(LayerNum, X, Y);

                    // North East Quarter
                    CalculateNE_Cliff(LayerNum, X, Y);

                    // South West Quarter
                    CalculateSW_Cliff(LayerNum, X, Y);

                    // South East Quarter
                    CalculateSE_Cliff(LayerNum, X, Y);

                    return;

                // Waterfalls
                case Constant.AUTOTILE_WATERFALL:
                    // North West Quarter
                    CalculateNW_Waterfall(LayerNum, X, Y);

                    // North East Quarter
                    CalculateNE_Waterfall(LayerNum, X, Y);

                    // South West Quarter
                    CalculateSW_Waterfall(LayerNum, X, Y);

                    // South East Quarter
                    CalculateSE_Waterfall(LayerNum, X, Y);

                    return;
            }
        }

        ///////////////////////
        // Normal autotiling //
        ///////////////////////
        public static void CalculateNW_Normal(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // North West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y - 1)) tmpTile[1] = true;

            // North
            if (checkTileMatch(LayerNum, X, Y, X, Y - 1)) tmpTile[2] = true;

            // West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y)) tmpTile[3] = true;

            // Get Autotile Type
            if (!tmpTile[2] & !tmpTile[3]) { Autotile_Type = Constant.AUTO_INNER; }          // Calculate Situation - Inner
            if (!tmpTile[2] & tmpTile[3]) { Autotile_Type = Constant.AUTO_HORIZONTAL; }      // Horizontal
            if (tmpTile[2] & !tmpTile[3]) { Autotile_Type = Constant.AUTO_VERTICAL; }        // Vertical
            if (!tmpTile[1] & tmpTile[2] & tmpTile[3]) { Autotile_Type = Constant.AUTO_OUTER; } // Outer
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) { Autotile_Type = Constant.AUTO_FILL; }      // Fill

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER: setAutotile(LayerNum, X, Y, 1, "e"); return;
                case Constant.AUTO_OUTER: setAutotile(LayerNum, X, Y, 1, "a"); return;
                case Constant.AUTO_HORIZONTAL: setAutotile(LayerNum, X, Y, 1, "i"); return;
                case Constant.AUTO_VERTICAL: setAutotile(LayerNum, X, Y, 1, "m"); return;
                case Constant.AUTO_FILL: setAutotile(LayerNum, X, Y, 1, "q"); return;
            }

        }

        public static void CalculateNE_Normal(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // North
            if (checkTileMatch(LayerNum, X, Y, X, Y - 1)) tmpTile[1] = true;

            // North East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y - 1)) tmpTile[2] = true;

            // East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y)) tmpTile[3] = true;

            // Calculate Situation - Inner
            if (!tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_INNER;
            // Horizontal
            if (!tmpTile[1] & tmpTile[3]) Autotile_Type = Constant.AUTO_HORIZONTAL;
            // Vertical
            if (tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_VERTICAL;
            // Outer
            if (tmpTile[1] & !tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_OUTER;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_FILL;

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER:
                    setAutotile(LayerNum, X, Y, 2, "j"); return;
                case Constant.AUTO_OUTER:
                    setAutotile(LayerNum, X, Y, 2, "b"); return;
                case Constant.AUTO_HORIZONTAL:
                    setAutotile(LayerNum, X, Y, 2, "f"); return;
                case Constant.AUTO_VERTICAL:
                    setAutotile(LayerNum, X, Y, 2, "r"); return;
                case Constant.AUTO_FILL:
                    setAutotile(LayerNum, X, Y, 2, "n"); return;
            }

        }

        public static void CalculateSW_Normal(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y)) tmpTile[1] = true;

            // South West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y + 1)) tmpTile[2] = true;

            // South
            if (checkTileMatch(LayerNum, X, Y, X, Y + 1)) tmpTile[3] = true;

            // Calculate Situation - Inner
            if (!tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_INNER;
            // Horizontal
            if (tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_HORIZONTAL;
            // Vertical
            if (!tmpTile[1] & tmpTile[3]) Autotile_Type = Constant.AUTO_VERTICAL;
            // Outer
            if (tmpTile[1] & !tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_OUTER;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_FILL;

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER:
                    setAutotile(LayerNum, X, Y, 3, "o"); return;
                case Constant.AUTO_OUTER:
                    setAutotile(LayerNum, X, Y, 3, "c"); return;
                case Constant.AUTO_HORIZONTAL:
                    setAutotile(LayerNum, X, Y, 3, "s"); return;
                case Constant.AUTO_VERTICAL:
                    setAutotile(LayerNum, X, Y, 3, "g"); return;
                case Constant.AUTO_FILL:
                    setAutotile(LayerNum, X, Y, 3, "k"); return;
            }
        }

        public static void CalculateSE_Normal(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // South
            if (checkTileMatch(LayerNum, X, Y, X, Y + 1)) tmpTile[1] = true;

            // South East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y + 1)) tmpTile[2] = true;

            // East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y)) tmpTile[3] = true;

            // Calculate Situation - Inner
            if (!tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_INNER;
            // Horizontal
            if (!tmpTile[1] & tmpTile[3]) Autotile_Type = Constant.AUTO_HORIZONTAL;
            // Vertical
            if (tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_VERTICAL;
            // Outer
            if (tmpTile[1] & !tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_OUTER;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_FILL;

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER:
                    setAutotile(LayerNum, X, Y, 4, "t"); return;
                case Constant.AUTO_OUTER:
                    setAutotile(LayerNum, X, Y, 4, "d"); return;
                case Constant.AUTO_HORIZONTAL:
                    setAutotile(LayerNum, X, Y, 4, "p"); return;
                case Constant.AUTO_VERTICAL:
                    setAutotile(LayerNum, X, Y, 4, "l"); return;
                case Constant.AUTO_FILL:
                    setAutotile(LayerNum, X, Y, 4, "h"); return;
            }

        }

        //////////////////////////
        // Waterfall autotiling //
        //////////////////////////
        public static void CalculateNW_Waterfall(int LayerNum, int X, int Y)
        {
            bool tmpTile = false;

            // West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y)) tmpTile = true;

            // Actually place the subtile
            if (tmpTile)
            {
                // EXtended
                setAutotile(LayerNum, X, Y, 1, "i");
            }
            else
            {
                // Edge
                setAutotile(LayerNum, X, Y, 1, "e");
            }
        }

        public static void CalculateNE_Waterfall(int LayerNum, int X, int Y)
        {
            bool tmpTile = false;

            // East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y)) tmpTile = true;

            // Actually place the subtile
            if (tmpTile)
            {
                // EXtended
                setAutotile(LayerNum, X, Y, 2, "f");
            }
            else
            {
                // Edge
                setAutotile(LayerNum, X, Y, 2, "j");
            }
        }

        public static void CalculateSW_Waterfall(int LayerNum, int X, int Y)
        {
            bool tmpTile = false;

            // West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y)) tmpTile = true;

            // Actually place the subtile
            if (tmpTile)
            {
                // EXtended
                setAutotile(LayerNum, X, Y, 3, "k");
            }
            else
            {
                // Edge
                setAutotile(LayerNum, X, Y, 3, "g");
            }
        }

        public static void CalculateSE_Waterfall(int LayerNum, int X, int Y)
        {
            bool tmpTile = false;

            // East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y)) tmpTile = true;

            // Actually place the subtile
            if (tmpTile)
            {
                // EXtended
                setAutotile(LayerNum, X, Y, 4, "h");
            }
            else
            {
                // Edge
                setAutotile(LayerNum, X, Y, 4, "l");
            }
        }

        //////////////////////
        // Cliff autotiling //
        //////////////////////
        public static void CalculateNW_Cliff(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // North West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y - 1)) tmpTile[1] = true;

            // North
            if (checkTileMatch(LayerNum, X, Y, X, Y - 1)) tmpTile[2] = true;

            // West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y)) tmpTile[3] = true;

            // Calculate Situation - Horizontal
            if (!tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_HORIZONTAL;
            // Vertical
            if (tmpTile[2] & !tmpTile[3]) Autotile_Type = Constant.AUTO_VERTICAL;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_FILL;
            // Inner
            if (!tmpTile[2] & !tmpTile[3]) Autotile_Type = Constant.AUTO_INNER;

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER:
                    setAutotile(LayerNum, X, Y, 1, "e"); return;
                case Constant.AUTO_HORIZONTAL:
                    setAutotile(LayerNum, X, Y, 1, "i"); return;
                case Constant.AUTO_VERTICAL:
                    setAutotile(LayerNum, X, Y, 1, "m"); return;
                case Constant.AUTO_FILL:
                    setAutotile(LayerNum, X, Y, 1, "q"); return;
            }
        }

        public static void CalculateNE_Cliff(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // North
            if (checkTileMatch(LayerNum, X, Y, X, Y - 1)) tmpTile[1] = true;

            // North East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y - 1)) tmpTile[2] = true;

            // East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y)) tmpTile[3] = true;

            // Calculate Situation - Horizontal
            if (!tmpTile[1] & tmpTile[3]) Autotile_Type = Constant.AUTO_HORIZONTAL;
            // Vertical
            if (tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_VERTICAL;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_FILL;
            // Inner
            if (!tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_INNER;

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER:
                    setAutotile(LayerNum, X, Y, 2, "j"); return;
                case Constant.AUTO_HORIZONTAL:
                    setAutotile(LayerNum, X, Y, 2, "f"); return;
                case Constant.AUTO_VERTICAL:
                    setAutotile(LayerNum, X, Y, 2, "r"); return;
                case Constant.AUTO_FILL:
                    setAutotile(LayerNum, X, Y, 2, "n"); return;
            }
        }

        public static void CalculateSW_Cliff(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y)) tmpTile[1] = true;

            // South West
            if (checkTileMatch(LayerNum, X, Y, X - 1, Y + 1)) tmpTile[2] = true;

            // South
            if (checkTileMatch(LayerNum, X, Y, X, Y + 1)) tmpTile[3] = true;

            // Calculate Situation - Horizontal
            if (tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_HORIZONTAL;
            // Vertical
            if (!tmpTile[1] & tmpTile[3]) Autotile_Type = Constant.AUTO_VERTICAL;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_FILL;
            // Inner
            if (!tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_INNER;

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER:
                    setAutotile(LayerNum, X, Y, 3, "o"); return;
                case Constant.AUTO_HORIZONTAL:
                    setAutotile(LayerNum, X, Y, 3, "s"); return;
                case Constant.AUTO_VERTICAL:
                    setAutotile(LayerNum, X, Y, 3, "g"); return;
                case Constant.AUTO_FILL:
                    setAutotile(LayerNum, X, Y, 3, "k"); return;
            }
        }

        public static void CalculateSE_Cliff(int LayerNum, int X, int Y)
        {
            bool[] tmpTile = new bool[4];
            byte Autotile_Type = 0;

            // South
            if (checkTileMatch(LayerNum, X, Y, X, Y + 1)) tmpTile[1] = true;

            // South East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y + 1)) tmpTile[2] = true;

            // East
            if (checkTileMatch(LayerNum, X, Y, X + 1, Y)) tmpTile[3] = true;

            // Calculate Situation -  Horizontal
            if (!tmpTile[1] & tmpTile[3]) Autotile_Type = Constant.AUTO_HORIZONTAL;
            // Vertical
            if (tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_VERTICAL;
            // Fill
            if (tmpTile[1] & tmpTile[2] & tmpTile[3]) Autotile_Type = Constant.AUTO_FILL;
            // Inner
            if (!tmpTile[1] & !tmpTile[3]) Autotile_Type = Constant.AUTO_INNER;

            // Actually place the subtile
            switch (Autotile_Type)
            {
                case Constant.AUTO_INNER:
                    setAutotile(LayerNum, X, Y, 4, "t"); return;
                case Constant.AUTO_HORIZONTAL:
                    setAutotile(LayerNum, X, Y, 4, "p"); return;
                case Constant.AUTO_VERTICAL:
                    setAutotile(LayerNum, X, Y, 4, "l"); return;
                case Constant.AUTO_FILL:
                    setAutotile(LayerNum, X, Y, 4, "h"); return;
            }
        }

        ////////////////////
        // Autotile Match //
        ////////////////////
        public static bool checkTileMatch(int LayerNum, int X1, int Y1, int X2, int Y2)
        {
            // if it's off the map then set it as autotile and eXit out early
            if (X2 < 0 | X2 > (Types.Map.MaxX - 1) | Y2 < 0 | Y2 > (Types.Map.MaxY - 1))
            {
                return true;
            }

            // fakes ALWAYS return true
            if (Types.Map.Tile[X2, Y2].Autotile[LayerNum] == Constant.AUTOTILE_FAKE)
            {
                return true;
            }

            // check neighbour is an autotile
            if (Types.Map.Tile[X2, Y2].Autotile[LayerNum] == 0)
            {
                return false;
            }

            // check we're a matching
            if (Types.Map.Tile[X1, Y1].Layer[LayerNum].Tileset != Types.Map.Tile[X2, Y2].Layer[LayerNum].Tileset)
            {
                return false;
            }

            // check tiles match
            if (Types.Map.Tile[X1, Y1].Layer[LayerNum].X != Types.Map.Tile[X2, Y2].Layer[LayerNum].X)
            {
                return false;
            }

            if (Types.Map.Tile[X1, Y1].Layer[LayerNum].Y != Types.Map.Tile[X2, Y2].Layer[LayerNum].Y)
            {
                return false;
            }

            return true;
        }

    }
}
