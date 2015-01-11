///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Lidgren.Network;

using MirageXNA;
using MirageXNA.Global;
using MirageXNA.Network;

namespace MirageXNA.Engine
{
    // XNA Window //
    public class Mirage : Microsoft.Xna.Framework.Game
    {
        // XNA Texture Manager
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static SoundEffect soundEffect;
        private Timers Time;
        private InputState inputState;

        // Routine Timers //
        private const long UpdateTime_Input = 60;
        private long LastUpdateTime_Input;

        // Game Time //
        public static GameTime _gameTime;

        public Mirage()
        {
            // Init Window //
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = true;
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Init Content //
            GameContentManager.Instance.initContentManager(Services);

            // Init Textures //
            GfxManager.Instance.initTextures();

            // Init TCP //
            NetworkManager.Instance.initTCP();

            // Init Audio //
            GameContentManager.Instance.PlayMusic(6);

            // Init Audio Effects //
            AudioManager.Instance.initSoundFx();

            // Static Class Init //
            UI_Menu.initGUI();              // Game Graphical User Interface

            // Private Class Init //
            inputState = new InputState();  // Game Input
            Time = new Timers();            // Game Timers

            // Init Game //
            base.Initialize();              
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            int Tick = (int)gameTime.TotalGameTime.TotalMilliseconds;
            _gameTime = gameTime;

            // Start Shutdown //
            if (Static.shutDown) { GC.Collect(); this.Exit(); }

            // Check Input //
            if (LastUpdateTime_Input < Tick)
            {
                if (inputState.justPressed(Keys.Escape)) this.Exit();
                inputState.Update();
                LastUpdateTime_Input = Tick + UpdateTime_Input;
            }

            // Update Logic //
            switch (Static.menuType)
            {
                case Constant.inMenu: Time.updateMenuLoop(Tick); break;
                case Constant.inGame: Time.updateGameLoop(Tick); break;
            }

            // Handle Received Data //
            NetworkManager.Instance.ReceiveData();

            General.calculateFPS(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Rectangle Offset;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GfxManager GfxM = GfxManager.Instance;

            if (Static.menuType == Constant.inMenu)
            {
                // Render Menu //
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

                // ****** Parallax ****** // //Replace 0 with ParallaxX got effect
                GfxM.drawTexture(Static.Tex_Gui[2], 0, 0, 768, 800, 0, 0, 768, 800, Color.White);
                GfxM.drawTexture(Static.Tex_Gui[2], 0 + 800, 0, 768, 800, 0, 0, 768, 800, Color.White);

                renderMenu();

                Vector2 Pos = new Vector2(10, 10);
                // Draw the string
                GfxM.drawText(0, "X: " + InputState.curMouse.X, Pos, Color.Black);

                Pos = new Vector2(10, 30);
                // Draw the string
                GfxM.drawText(0, "Y: " + InputState.curMouse.Y, Pos, Color.Black);

                // Render Alert Message //
                if (Static.menuAlertTimer > 0)
                {
                    GfxM.drawTexture(Static.Tex_Gui[2], 0, 0, 768, 1024, 0, 0, 768, 1024, new Color(0, 0, 0, 100));
                    Pos = new Vector2(512 - (General.getStringWidth(0, Static.menuAlertMessage) / 2), 200);
                    GfxM.drawText(0, Static.menuAlertMessage, Pos, Color.Black);
                }

                spriteBatch.End();
            }

            if (Static.menuType == Constant.inGame)
            {
                // Scrolling Map Algorithm //
                Offset = Logic.nextFrame();

                // Draw the sprite
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

                // RENDER LOWER MAP LAYER //
                for (int LoopL = 0; LoopL <= 2; LoopL++)
                {
                    for (int LoopT = 1; LoopT <= Types.TileLayer[LoopL].NumTiles; LoopT++)
                    {
                        int Text = LoopT - 1;
                        if (Autotiles.Autotile[Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy].Layer[LoopL].RenderState == Autotiles.RENDER_STATE_NORMAL)
                        {
                            GfxM.drawTexture(Static.Tex_Tilesets[Types.Map.Tile[Types.TileLayer[LoopL].Tile[Text].dx,
                                Types.TileLayer[LoopL].Tile[Text].dy].Layer[LoopL].Tileset],
                                Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X),
                                Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y),
                                32,
                                32,
                                Types.TileLayer[LoopL].Tile[Text].sRect);
                        }
                        else if (Autotiles.Autotile[Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy].Layer[LoopL].RenderState == Autotiles.RENDER_STATE_AUTOTILE)
                        {
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X), Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y), 1, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X) + 16, Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y), 2, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X), Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y) + 16, 3, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X) + 16, Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y) + 16, 4, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                        }
                    }
                }

                //////////////////////////////
                // Y-Based Sprite Rendering //
                //////////////////////////////
                for (int LoopY = Static.MinY; LoopY <= Static.MaxY; LoopY++)
                {

                    // RENDER PLAYERS //
                    for (int LoopI = 0; LoopI <= Constant.MAX_PLAYERS - 1; LoopI++)
                    {
                        if (ClientTCP.IsPlaying(LoopI))
                        {
                            if (Types.Players[Static.MyIndex].Map == Types.Players[LoopI].Map)
                            {
                                if (Types.Players[LoopI].Y == LoopY)
                                {
                                    int PlayerX = ((Types.Players[LoopI].X * Constant.MoveSize) + Types.Players[LoopI].OffsetX) - Offset.X;
                                    int PlayerY = ((Types.Players[LoopI].Y * Constant.MoveSize) + Types.Players[LoopI].OffsetY) - Offset.Y;
                                    Logic.renderPlayer(LoopI, PlayerX, PlayerY - 6);
                                }
                            }
                        }
                    }

                    // RENDER NPCS //
                    for (int LoopN = 1; LoopN <= Constant.MAX_MAP_NPCS - 1; LoopN++)
                    {
                        if (Types.MapNpcs[LoopN].Active)
                        {
                            if (Types.MapNpcs[LoopN].Y == LoopY)
                            {
                                int X = ((Types.MapNpcs[LoopN].X * Constant.MoveSize) + Types.MapNpcs[LoopN].OffsetX) - Offset.X;
                                int Y = ((Types.MapNpcs[LoopN].Y * Constant.MoveSize) + Types.MapNpcs[LoopN].OffsetY) - Offset.Y;
                                Logic.renderNpc(LoopN, X, Y - 6);
                            }
                        }
                    }

                }


                // RENDER HIGHER MAP LAYER //
                for (int LoopL = 3; LoopL <= 4; LoopL++)
                {
                    for (int LoopT = 1; LoopT <= Types.TileLayer[LoopL].NumTiles; LoopT++)
                    {
                        int Text = LoopT - 1;

                        if (Autotiles.Autotile[Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy].Layer[LoopL].RenderState == Autotiles.RENDER_STATE_NORMAL)
                        {
                            GfxM.drawTexture(Static.Tex_Tilesets[Types.Map.Tile[Types.TileLayer[LoopL].Tile[Text].dx,
                                Types.TileLayer[LoopL].Tile[Text].dy].Layer[LoopL].Tileset],
                                Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X),
                                Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y),
                                32,
                                32,
                                Types.TileLayer[LoopL].Tile[Text].sRect);
                        }
                        else if (Autotiles.Autotile[Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy].Layer[LoopL].RenderState == Autotiles.RENDER_STATE_AUTOTILE)
                        {
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X), Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y), 1, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X) + 16, Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y), 2, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X), Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y) + 16, 3, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                            Logic.renderAutotile(LoopL, Types.TileLayer[LoopL].Tile[Text].PixelPosX - (Offset.X) + 16, Types.TileLayer[LoopL].Tile[Text].PixelPosY - (Offset.Y) + 16, 4, Types.TileLayer[LoopL].Tile[Text].dx, Types.TileLayer[LoopL].Tile[Text].dy);
                        }
                    }
                }

                // Render Windows //
                renderGameWindows(Constant.inventoryWindow);
                renderGameWindows(Constant.spellWindow);
                renderGameWindows(Constant.chatWindow);
                renderGameWindows(Constant.charWindow);
                renderGameWindows(Constant.menuWindow);

                // Render Bars
                renderPlayerBars();

                // RENDER PING //
                Vector2 Pos = new Vector2(10, 10);
                GfxM.drawText(1, "Ping: " + Static.Ping, Pos, Color.White);

                spriteBatch.End();
            }

            Window.Title = Constant.GAME_NAME + " FPS: " + Static.fps;

            base.Draw(gameTime);
        }

        //////////
        // Menu //
        //////////
        public void renderMenu()
        {
            int LoopI;
            int var = Static.curMainMenu;
            Static.creditsAlpha = (byte)MathHelper.Clamp(Static.creditsAlpha, 0, 255);
            Color _TEMPCOLOR = new Color(255, 255, 255, Static.creditsAlpha);
            string rName = String.Empty;
            string rPass = String.Empty;
            GfxManager GfxM = GfxManager.Instance;

            switch (var)
            {

                /// Main Screen ///
                #region MainScreen Render
                case Constant.curMenuMain:

                    // MAIN
                    GfxM.drawTexture(UI_Menu.MenuWindow.mainwindow.Texture.TEXTURE,
                    UI_Menu.MenuWindow.mainwindow.Texture.X,
                    UI_Menu.MenuWindow.mainwindow.Texture.Y,
                    768,
                    UI_Menu.MenuWindow.mainwindow.Texture.W,
                    UI_Menu.MenuWindow.mainwindow.Texture.X,
                    UI_Menu.MenuWindow.mainwindow.Texture.Y,
                    768,
                    UI_Menu.MenuWindow.mainwindow.Texture.W,
                    Color.White);

                    // Render Menu Buttons
                    for (LoopI = 1; LoopI <= 5; LoopI++)
                    {
                        if (UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_NORMAL)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons[UI_Menu.MenuWindow.mainwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].W,
                            Color.White);
                        }
                        else if (UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_HOVER)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons_H[UI_Menu.MenuWindow.mainwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].W,
                            Color.White);
                        }
                        else if (UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_CLICK)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons_C[UI_Menu.MenuWindow.mainwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.mainwindow.Button[LoopI].W,
                            Color.White);
                        }

                    }
                    break;
                #endregion

                /// Login Screen ///
                #region LoginScreen Render
                case Constant.curMenuLogin:

                    // MAIN
                    GfxM.drawTexture(UI_Menu.MenuWindow.mainwindow.Texture.TEXTURE,
                    UI_Menu.MenuWindow.loginwindow.Texture.X,
                    UI_Menu.MenuWindow.loginwindow.Texture.Y,
                    768,
                    UI_Menu.MenuWindow.loginwindow.Texture.W,
                    UI_Menu.MenuWindow.loginwindow.Texture.X,
                    UI_Menu.MenuWindow.loginwindow.Texture.Y,
                    768,
                    UI_Menu.MenuWindow.loginwindow.Texture.W,
                    Color.White);

                    // Render Surfaces
                    for (LoopI = 1; LoopI <= 2; LoopI++)
                    {
                        GfxM.drawTexture(Static.Tex_Gui[UI_Menu.MenuWindow.loginwindow.Textures[LoopI].TEXTURE],
                        UI_Menu.MenuWindow.loginwindow.Textures[LoopI].X,
                        UI_Menu.MenuWindow.loginwindow.Textures[LoopI].Y,
                        UI_Menu.MenuWindow.loginwindow.Textures[LoopI].H,
                        UI_Menu.MenuWindow.loginwindow.Textures[LoopI].W,
                        0,
                        0,
                        UI_Menu.MenuWindow.loginwindow.Textures[LoopI].H,
                        UI_Menu.MenuWindow.loginwindow.Textures[LoopI].W,
                        Color.White);
                    }

                    Vector2 textPos = new Vector2(UI_Menu.MenuWindow.loginwindow.Textures[1].X, UI_Menu.MenuWindow.loginwindow.Textures[1].Y - 20);
                    GfxM.drawText(0, "Username", textPos, Color.Black);
                    textPos = new Vector2(UI_Menu.MenuWindow.loginwindow.Textures[2].X, UI_Menu.MenuWindow.loginwindow.Textures[2].Y - 20);
                    GfxM.drawText(0, "Password", textPos, Color.Black);

                    rName = Static.Username;
                    if (Static.loginInput == 1) { rName = rName + "|"; }
                    Vector2 Pos = new Vector2(UI_Menu.MenuWindow.loginwindow.Textures[1].X + 7, UI_Menu.MenuWindow.loginwindow.Textures[1].Y + 5);
                    GfxM.drawText(0, rName, Pos, Color.Black);

                    if (Static.Password.Length > 0)
                    {
                        for (LoopI = 1; LoopI <= Static.Password.Length; LoopI++)
                        {
                            rPass = rPass + "*";
                        }
                    }
                    if (Static.loginInput == 2) { rPass = rPass + "|"; }
                    Pos = new Vector2(UI_Menu.MenuWindow.loginwindow.Textures[2].X + 7, UI_Menu.MenuWindow.loginwindow.Textures[2].Y + 5);
                    GfxM.drawText(0, rPass, Pos, Color.Black);

                    // Render Menu Buttons
                    for (LoopI = 1; LoopI <= 2; LoopI++)
                    {
                        if (UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_NORMAL)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons[UI_Menu.MenuWindow.loginwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].W,
                            Color.White);
                        }
                        else if (UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_HOVER)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons_H[UI_Menu.MenuWindow.loginwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].W,
                            Color.White);
                        }
                        else if (UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_CLICK)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons_C[UI_Menu.MenuWindow.loginwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.loginwindow.Button[LoopI].W,
                            Color.White);
                        }

                    }
                    break;
                #endregion

                /// Register Screen ///
                #region RegisterScreen Render
                case Constant.curMenuRegister:

                    // MAIN
                    GfxM.drawTexture(UI_Menu.MenuWindow.registerwindow.Texture.TEXTURE,
                    UI_Menu.MenuWindow.registerwindow.Texture.X,
                    UI_Menu.MenuWindow.registerwindow.Texture.Y,
                    768,
                    UI_Menu.MenuWindow.registerwindow.Texture.W,
                    UI_Menu.MenuWindow.registerwindow.Texture.X,
                    UI_Menu.MenuWindow.registerwindow.Texture.Y,
                    768,
                    UI_Menu.MenuWindow.registerwindow.Texture.W,
                    Color.White);

                    // Render Selected Sprite //
                    GfxM.drawTexture(Static.Tex_Sprites[Static.rSprite], 496, 260, 32, 32, 32, 0, 32, 32, Color.White);

                    // Render Surfaces
                    for (LoopI = 1; LoopI <= UI_Menu.Max_Register_Textures; LoopI++)
                    {
                        GfxM.drawTexture(Static.Tex_Gui[UI_Menu.MenuWindow.registerwindow.Textures[LoopI].TEXTURE],
                        UI_Menu.MenuWindow.registerwindow.Textures[LoopI].X,
                        UI_Menu.MenuWindow.registerwindow.Textures[LoopI].Y,
                        UI_Menu.MenuWindow.registerwindow.Textures[LoopI].H,
                        UI_Menu.MenuWindow.registerwindow.Textures[LoopI].W,
                        0,
                        0,
                        UI_Menu.MenuWindow.registerwindow.Textures[LoopI].H,
                        UI_Menu.MenuWindow.registerwindow.Textures[LoopI].W,
                        Color.White);
                    }

                    textPos = new Vector2(UI_Menu.MenuWindow.registerwindow.Textures[1].X, UI_Menu.MenuWindow.registerwindow.Textures[1].Y - 20);
                    GfxM.drawText(0, "Username", textPos, Color.Black);
                    textPos = new Vector2(UI_Menu.MenuWindow.registerwindow.Textures[2].X, UI_Menu.MenuWindow.registerwindow.Textures[2].Y - 20);
                    GfxM.drawText(0, "Password", textPos, Color.Black);

                    rName = Static.rUsername;
                    if (Static.registerInput == 1) { rName = rName + "|"; }
                    Pos = new Vector2(UI_Menu.MenuWindow.registerwindow.Textures[1].X + 7, UI_Menu.MenuWindow.registerwindow.Textures[1].Y + 5);
                    GfxM.drawText(0, rName, Pos, Color.Black);


                    if (Static.rPassword.Length > 0)
                    {
                        for (LoopI = 1; LoopI <= Static.rPassword.Length; LoopI++)
                        {
                            rPass = rPass + "*";
                        }
                    }

                    if (Static.registerInput == 2) { rPass = rPass + "|"; }
                    Pos = new Vector2(UI_Menu.MenuWindow.registerwindow.Textures[2].X + 7, UI_Menu.MenuWindow.registerwindow.Textures[2].Y + 5);
                    GfxM.drawText(0, rPass, Pos, Color.Black);

                    // Render Menu Buttons
                    for (LoopI = 1; LoopI <= UI_Menu.Max_Register_Buttons; LoopI++)
                    {
                        if (UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_NORMAL)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons[UI_Menu.MenuWindow.registerwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].W,
                            Color.White);
                        }
                        else if (UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_HOVER)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons_H[UI_Menu.MenuWindow.registerwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].W,
                            Color.White);
                        }
                        else if (UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE == Constant.BUTTON_STATE_CLICK)
                        {
                            GfxM.drawTexture(Static.Tex_Buttons_C[UI_Menu.MenuWindow.registerwindow.Button[LoopI].TEXTURE],
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].X,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].Y,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].W,
                            0,
                            0,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].H,
                            UI_Menu.MenuWindow.registerwindow.Button[LoopI].W,
                            Color.White);
                        }                       

                    }

                    // Render Text
                    for (LoopI = 1; LoopI <= UI_Menu.Max_Register_Text; LoopI++)
                    {
                        GfxM.drawText(UI_Menu.MenuWindow.registerwindow.Text[LoopI].FONT,
                            UI_Menu.MenuWindow.registerwindow.Text[LoopI].CAPTION,
                            Pos = new Vector2(UI_Menu.MenuWindow.registerwindow.Text[LoopI].X, UI_Menu.MenuWindow.registerwindow.Text[LoopI].Y),
                            UI_Menu.MenuWindow.registerwindow.Text[LoopI].COLOUR);
                    }

                    break;
                #endregion

            }

            // Credits //
            GfxM.drawTexture(Static.Tex_Gui[5],
            0,
            0,
            768,
            1024,
            0,
            0,
            768,
            1024,
            _TEMPCOLOR);

        }

        // Player Bars
        public void renderPlayerBars()
        {
            double maxWidth = 0;
            GfxManager GfxM = GfxManager.Instance;

            for (int I = 0; I <= (int)Enumerations.Vitals.vitalCount - 1; I++)
            {
                if (Types.Players[Static.MyIndex].MaxVitals[I] > 0)
                {
                    maxWidth = ((((double)Types.Players[Static.MyIndex].Vitals[I] / 200) / ((double)Types.Players[Static.MyIndex].MaxVitals[I] / 200)) * 200);
                    GfxM.drawTexture(Static.Tex_Gui[9],
                        20,
                        10 + (I * 25),
                        20,
                        200,
                        0,
                        0,
                        20,
                        200,
                        Color.White);

                    GfxM.drawTexture(Static.Tex_Gui[10] + I,
                        20,
                        10 + (I * 25),
                        20,
                        (int)maxWidth,
                        0,
                        0,
                        20,
                        200, Color.White);
                }
            }
        }

        //////////////////
        // Game Windows //
        //////////////////

        public void renderGameWindows(int windowIndex)
        {
            if (Static.ShowGameWindow[windowIndex])
            {
                switch (windowIndex)
                {
                    case Constant.inventoryWindow:
                        renderInvWindow();
                        break;
                    case Constant.spellWindow:
                        renderSpellWindow();
                        break;
                    case Constant.chatWindow:
                        renderChatWindow();
                        break;
                    case Constant.charWindow: 
                        renderCharWindow();
                        break;
                    case Constant.menuWindow:
                        renderMenuWindow(); 
                        break;
                }
            }
        }

        public void renderChatWindow()
        {
            GfxManager GfxM = GfxManager.Instance;

            // Main Window Background //
            GfxM.drawTexture(UI_Game.gameWindow.chatWindow.Texture.TEXTURE,
            UI_Game.gameWindow.chatWindow.Texture.X,
            UI_Game.gameWindow.chatWindow.Texture.Y,
            UI_Game.gameWindow.chatWindow.Texture.H,
            UI_Game.gameWindow.chatWindow.Texture.W,
            0,
            0,
            UI_Game.gameWindow.chatWindow.Texture.H,
            UI_Game.gameWindow.chatWindow.Texture.W,
            Color.White);

            // Set the position //
            if (Static.ChatBox_Chunk <= 1) { Static.ChatBox_Chunk = 1; }

            // Loop through each buffer string //
            for (int LoopC = (10 * Static.ChatBox_Chunk) - (9); LoopC <= 10 * Static.ChatBox_Chunk; LoopC++)
            {
                if (LoopC > Constant.maxChat) { break; }
                if (Static.ChatBox_Chunk * 10 > Constant.maxChat) { Static.ChatBox_Chunk = Static.ChatBox_Chunk - 1; }

                // Set the base position //
                int X = UI_Game.gameWindow.chatWindow.Texture.X + 5; //+ GameGUI.gameWindow.chatWindow.Texture.X;
                int Y = UI_Game.gameWindow.chatWindow.Texture.Y + 2; //+ GameGUI.gameWindow.chatWindow.Texture.Y;

                // Set the Y position to be used //
                Y = (Y - (LoopC * 10) + (10 * Static.ChatBox_Chunk * 10));

                // Don't bother with empty strings //
                if (UI_Game.chatBuffer[LoopC].Text.Length != 0)
                {
                    Vector2 Pos = new Vector2(X, Y);
                    GfxM.drawText(1, UI_Game.chatBuffer[LoopC].Text, Pos, Color.White);
                }
            }

            // Draw entered text
            if (Static.EnterText)
            {
                // Set the base position //
                int X = UI_Game.gameWindow.chatWindow.Texture.X + 10;
                int Y = UI_Game.gameWindow.chatWindow.Texture.Y + 106;

                if (Static.EnterTextBufferWidth == 0) { Static.EnterTextBufferWidth = 1; }   // Dividing by 0 is never good
                Vector2 Pos = new Vector2(X, Y);
                GfxM.drawText(1, "Text: " + Static.ShownText + "|", Pos, Color.White);
            }

        }

        public void renderInvWindow()
        {
            GfxManager GfxM = GfxManager.Instance;

            // Main Window Background //
            GfxM.drawTexture(UI_Game.gameWindow.invWindow.Texture.TEXTURE,
            UI_Game.gameWindow.invWindow.Texture.X,
            UI_Game.gameWindow.invWindow.Texture.Y,
            UI_Game.gameWindow.invWindow.Texture.H,
            UI_Game.gameWindow.invWindow.Texture.W,
            0,
            0,
            UI_Game.gameWindow.invWindow.Texture.H,
            UI_Game.gameWindow.invWindow.Texture.W,
            Color.White);

            int dX = UI_Game.gameWindow.invWindow.Texture.X;
            int dY = UI_Game.gameWindow.invWindow.Texture.Y;
        }

        public void renderSpellWindow()
        {
            GfxManager GfxM = GfxManager.Instance;

            // Main Window Background //
            GfxM.drawTexture(UI_Game.gameWindow.spellWindow.Texture.TEXTURE,
            UI_Game.gameWindow.spellWindow.Texture.X,
            UI_Game.gameWindow.spellWindow.Texture.Y,
            UI_Game.gameWindow.spellWindow.Texture.H,
            UI_Game.gameWindow.spellWindow.Texture.W,
            0,
            0,
            UI_Game.gameWindow.spellWindow.Texture.H,
            UI_Game.gameWindow.spellWindow.Texture.W,
            Color.White);

            int dX = UI_Game.gameWindow.spellWindow.Texture.X;
            int dY = UI_Game.gameWindow.spellWindow.Texture.Y;
        }

        public void renderCharWindow()
        {
            Vector2 Pos;
            GfxManager GfxM = GfxManager.Instance;

            // Main Window Background //
            GfxM.drawTexture(UI_Game.gameWindow.charWindow.Texture.TEXTURE,
            UI_Game.gameWindow.charWindow.Texture.X,
            UI_Game.gameWindow.charWindow.Texture.Y,
            UI_Game.gameWindow.charWindow.Texture.H,
            UI_Game.gameWindow.charWindow.Texture.W,
            0,
            0,
            UI_Game.gameWindow.charWindow.Texture.H,
            UI_Game.gameWindow.charWindow.Texture.W,
            Color.White);

            int dX = UI_Game.gameWindow.charWindow.Texture.X;
            int dY = UI_Game.gameWindow.charWindow.Texture.Y;

            // Set Face Sprite //
            UI_Game.gameWindow.charWindow.Textures[0].TEXTURE = Static.Tex_Faces[Types.Players[Static.MyIndex].Sprite];

            // Set Stat Caption //
            UI_Game.gameWindow.charWindow.Text[0].CAPTION = UI_Tweaks.getStatText(Enumerations.Stats.Strength);
            UI_Game.gameWindow.charWindow.Text[1].CAPTION = UI_Tweaks.getStatText(Enumerations.Stats.Defence);
            UI_Game.gameWindow.charWindow.Text[2].CAPTION = UI_Tweaks.getStatText(Enumerations.Stats.Magic);
            UI_Game.gameWindow.charWindow.Text[3].CAPTION = UI_Tweaks.getStatText(Enumerations.Stats.Speed);

            // Set Points Label //
            UI_Game.gameWindow.charWindow.Text[5].CAPTION = UI_Tweaks.getPointsText();
            UI_Game.gameWindow.charWindow.Text[6].CAPTION = UI_Tweaks.getExpText();

            // Set Player Name - Note: This should be done once //
            UI_Tweaks.setNameVector2();

            // Render Textures
            for (int LoopI = 0; LoopI <= 3; LoopI++)
            {
                GfxM.drawTexture(UI_Game.gameWindow.charWindow.Textures[LoopI].TEXTURE,
                dX + UI_Game.gameWindow.charWindow.Textures[LoopI].X,
                dY + UI_Game.gameWindow.charWindow.Textures[LoopI].Y,
                UI_Game.gameWindow.charWindow.Textures[LoopI].H,
                UI_Game.gameWindow.charWindow.Textures[LoopI].W,
                0,
                0,
                UI_Game.gameWindow.charWindow.Textures[LoopI].H,
                UI_Game.gameWindow.charWindow.Textures[LoopI].W,
                Color.White);
            }

            // Render Text
            for (int LoopI = 0; LoopI <= UI_Game.gameWindow.charWindow.Text.Length - 1; LoopI++)
            {
                int windowX = UI_Game.gameWindow.charWindow.Texture.X + UI_Game.gameWindow.charWindow.Text[LoopI].X;
                int windowY = UI_Game.gameWindow.charWindow.Texture.Y + UI_Game.gameWindow.charWindow.Text[LoopI].Y;
                Pos = new Vector2(windowX, windowY);
                GfxM.drawText(UI_Game.gameWindow.charWindow.Text[LoopI].FONT,
                    UI_Game.gameWindow.charWindow.Text[LoopI].CAPTION,
                    Pos,
                    UI_Game.gameWindow.charWindow.Text[LoopI].COLOUR);
            }
        }

        public void renderMenuWindow()
        {
            int dX = UI_Game.gameWindow.menuWindow.Texture.X;
            int dY = UI_Game.gameWindow.menuWindow.Texture.Y;

            // Get Graphics Manager Accessor //
            GfxManager GfxM = GfxManager.Instance;

            // Main Window Background //
            GfxM.drawTexture(UI_Game.gameWindow.menuWindow.Texture.TEXTURE,
            dX,
            dY,
            UI_Game.gameWindow.menuWindow.Texture.H,
            UI_Game.gameWindow.menuWindow.Texture.W);

            // Render Menu Buttons
            for (int LoopI = 0; LoopI <= 5; LoopI++)
            {
                if (UI_Game.gameWindow.menuWindow.Button[LoopI].STATE == Constant.BUTTON_STATE_NORMAL)
                {
                    GfxM.drawTexture(Static.Tex_Buttons[UI_Game.gameWindow.menuWindow.Button[LoopI].TEXTURE],
                    dX + UI_Game.gameWindow.menuWindow.Button[LoopI].X,
                    dY + UI_Game.gameWindow.menuWindow.Button[LoopI].Y,
                    UI_Game.gameWindow.menuWindow.Button[LoopI].H,
                    UI_Game.gameWindow.menuWindow.Button[LoopI].W);
                }
                else if (UI_Game.gameWindow.menuWindow.Button[LoopI].STATE == Constant.BUTTON_STATE_HOVER)
                {
                    GfxM.drawTexture(Static.Tex_Buttons_H[UI_Game.gameWindow.menuWindow.Button[LoopI].TEXTURE],
                    dX + UI_Game.gameWindow.menuWindow.Button[LoopI].X,
                    dY + UI_Game.gameWindow.menuWindow.Button[LoopI].Y,
                    UI_Game.gameWindow.menuWindow.Button[LoopI].H,
                    UI_Game.gameWindow.menuWindow.Button[LoopI].W);
                }
                else if (UI_Game.gameWindow.menuWindow.Button[LoopI].STATE == Constant.BUTTON_STATE_CLICK)
                {
                    GfxM.drawTexture(Static.Tex_Buttons_C[UI_Game.gameWindow.menuWindow.Button[LoopI].TEXTURE],
                    dX + UI_Game.gameWindow.menuWindow.Button[LoopI].X,
                    dY + UI_Game.gameWindow.menuWindow.Button[LoopI].Y,
                    UI_Game.gameWindow.menuWindow.Button[LoopI].H,
                    UI_Game.gameWindow.menuWindow.Button[LoopI].W);
                }

            }
        }

    }
}
