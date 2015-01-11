///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using MirageXNA;
using MirageXNA.Global;
using MirageXNA.Network;
using MirageXNA.Engine;

namespace MirageXNA.Engine
{
    public class InputState
    {
        ///////////
        // Mouse //
        ///////////
        private MouseState newMouseState;
        private MouseState oldMouseState;
        public static Vector2 curMouse;
        private int lastClicked = 0;

        //////////////
        // Keyboard //
        //////////////
        private KeyboardState newKeyState;
        private KeyboardState oldKeyState;
        private int lastKeyPressed = 0;

        ////////////
        // Update //
        ////////////
        public void Update()
        {
            // Update Keyboard //
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();

            // Update Mouse //
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();

            handleKeyboardInput();
            handleMouseInput();
        }

        private bool checkCollision(float x1, float Y1, int W1, int H1, int x2, int y2, int W2, int H2)
        {
            if (x1 + W1 >= x2) { if (x1 <= x2 + W2) { if (Y1 + H1 >= y2) { if (Y1 <= y2 + H2) { return true; } } } }
            return false;
        }
        public bool justPressed(Keys key)
        {
            return newKeyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
        }
        public bool justClicked(byte mouseButton)
        {
            if (mouseButton == 1)
            {
                return (newMouseState.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton == ButtonState.Released);
            }
            else if(mouseButton == 2)
            {
                return (newMouseState.RightButton == ButtonState.Pressed) && (oldMouseState.RightButton == ButtonState.Released);
            }

            return false;
        }
        
        ////////////////////
        // Keyboard Input //
        ////////////////////

        // Get Ascii Value From Key Pressed //
        private string getAsciiValue(Keys Key)
        {
            string key = Key.ToString().ToLower();

            switch (Key)
            {
                case Keys.Enter: lastKeyPressed = (int)Keys.Enter; return String.Empty;
                case Keys.LeftShift: return String.Empty;
                case Keys.RightShift: return String.Empty;
                case Keys.Back: return String.Empty;
                case Keys.LeftAlt: return String.Empty;
                case Keys.RightAlt: return String.Empty;
                case Keys.LeftControl: return String.Empty;
                case Keys.RightControl: return String.Empty;
                case Keys.CapsLock: return String.Empty;
                case Keys.Oem8: return String.Empty;
                case Keys.LeftWindows: return String.Empty;
                case Keys.RightWindows: return String.Empty;
                case Keys.Apps: return String.Empty;
                case Keys.PageUp: return String.Empty;
                case Keys.PageDown: return String.Empty;
                case Keys.Insert: return String.Empty;
                case Keys.Delete: return String.Empty;
                case Keys.Home: return String.Empty;
                case Keys.End: return String.Empty;
                case Keys.Up: return String.Empty;
                case Keys.Down: return String.Empty;
                case Keys.Left: return String.Empty;
                case Keys.Right: return String.Empty;
                case Keys.NumLock: return String.Empty;
                case Keys.PrintScreen: return String.Empty;
                case Keys.Scroll: return String.Empty;
                case Keys.Pause: return String.Empty;
                case Keys.Tab: lastKeyPressed = (int)Keys.Tab; return String.Empty;
                case Keys.Space: return " ";
                case Keys.D0: return "0";
                case Keys.D1: return "1";
                case Keys.D2: return "2";
                case Keys.D3: return "3";
                case Keys.D4: return "4";
                case Keys.D5: return "5";
                case Keys.D6: return "6";
                case Keys.D7: return "7";
                case Keys.D8: return "8";
                case Keys.D9: return "9";
                case Keys.NumPad0: return "0";
                case Keys.NumPad1: return "1";
                case Keys.NumPad2: return "2";
                case Keys.NumPad3: return "3";
                case Keys.NumPad4: return "4";
                case Keys.NumPad5: return "5";
                case Keys.NumPad6: return "6";
                case Keys.NumPad7: return "7";
                case Keys.NumPad8: return "8";
                case Keys.NumPad9: return "9";
                case Keys.OemPeriod: return ".";
                case Keys.OemComma: return ",";
                case Keys.OemPlus: return "+";
                case Keys.OemMinus: return "-";
                case Keys.OemBackslash: return "\\";
                case Keys.OemQuestion: return "/";
                case Keys.OemTilde: return "#";
                case Keys.OemQuotes: return "'";
                case Keys.Add: return "+";
                case Keys.Subtract: return "-";
                case Keys.Multiply: return "*";
                case Keys.Divide: return "/";
            }
            
            return key;
        }

        // Handle all Keyboard Input //
        private void handleKeyboardInput()
        {

            // Exit Game //
            if (justPressed(Keys.Escape))
            {
                Static.shutDown = true;
            }

            // Full Screen //
            if (newKeyState.IsKeyDown(Keys.Enter) && (newKeyState.IsKeyDown(Keys.LeftAlt) || newKeyState.IsKeyDown(Keys.RightAlt)) && !oldKeyState.IsKeyDown(Keys.Enter))
            {
                Mirage.graphics.ToggleFullScreen();
            }

            // Key Down //
            handleKeyDown();

            // Key Pressed //
            handleKeyPressed();

            // Key Up //
            handleKeyUp();
        }
        private void handleKeyDown()
        {
            switch (Static.menuType)
            {
                case Constant.inMenu:

                    break;

                case Constant.inGame:

                    // Player Up Input //
                    if (newKeyState.IsKeyDown(Static.PlayerUp))
                        Logic.setMovement(Constant.DIR_NORTH);
                    else Static.DirUp = false;

                    // Player Down Input //
                    if (newKeyState.IsKeyDown(Static.PlayerDown))
                        Logic.setMovement(Constant.DIR_SOUTH);
                    else Static.DirDown = false;

                    // Player Left Input //
                    if (newKeyState.IsKeyDown(Static.PlayerLeft))
                        Logic.setMovement(Constant.DIR_WEST);
                    else Static.DirLeft = false;

                    // Player Right Input //
                    if (newKeyState.IsKeyDown(Static.PlayerRight))
                        Logic.setMovement(Constant.DIR_EAST);
                    else Static.DirRight = false;

                    // Player Attack Input //
                    if (newKeyState.IsKeyDown(Keys.LeftControl))
                        Static.Ctrl = true;
                    else Static.Ctrl = false;

                    // Enter //
                    if ((int)Keys.Enter != lastKeyPressed)
                    {
                        if (newKeyState.IsKeyDown(Keys.Enter))
                        {
                            // ***** Chat Window *****
                            if (Static.ShowGameWindow[Constant.chatWindow])
                            {
                                if (Static.EnterText)
                                {
                                    if (Static.EnterTextBuffer != String.Empty)
                                        UI_Game.handleText();
                                    Static.EnterText = false;
                                    lastKeyPressed = (int)Keys.Enter;
                                }
                                else
                                {
                                    Static.EnterText = true;
                                    lastKeyPressed = (int)Keys.Enter;
                                }
                            }
                        }
                    }

                    break;
            }
        }
        private void handleKeyPressed()
        {
            // Get Pressed Keys //
            Keys[] Key = newKeyState.GetPressedKeys();

            // No Keys Pressed Exit //
            if (Key.Length == 0) { lastKeyPressed = 0; return; }

            // Check Last Pressed Key //
            Keys tempKey = (Keys)lastKeyPressed;
            if (newKeyState.IsKeyUp(tempKey)) lastKeyPressed = 0;

            // Loop Through All Keys Pressed //
            for (int i = 0; i < Key.Length; i++)
            {
                int keyNumber = (int)Key[i];
                string Ascii = String.Empty;

                if (justPressed(Key[i]))
                {
                    if (lastKeyPressed != keyNumber)
                    {
                        // Check if the key is down //
                        if (newKeyState.IsKeyDown(Key[i]))
                        {
                            Ascii = getAsciiValue(Key[i]);

                            // Check if the key is down //
                            if (newKeyState.IsKeyDown(Keys.RightShift) | newKeyState.IsKeyDown(Keys.LeftShift))
                                Ascii = getAsciiValue(Key[i]).ToUpper();
                            lastKeyPressed = keyNumber;
                        }
                    }
                }

                switch (Static.menuType)
                {
                    case Constant.inMenu:

                        // Main Menu Only //
                        switch (Static.curMainMenu)
                        {
                            case Constant.curMenuLogin:

                                switch (Static.loginInput)
                                {
                                    case 1:
                                        if (justPressed(Keys.Tab)) { Static.loginInput = 2; }
                                        if (justPressed(Keys.Back)) { if (Static.Username.Length > 0) { Static.Username = Static.Username.Substring(0, Static.Username.Length - 1); lastKeyPressed = (int)Keys.Back; } }
                                        if (Static.Username.Length < 20) { Static.Username += Ascii; }
                                        break;
                                    case 2:
                                        if (justPressed(Keys.Tab)) { Static.loginInput = 1; }
                                        if (justPressed(Keys.Back)) { if (Static.Password.Length > 0) { Static.Password = Static.Password.Substring(0, Static.Password.Length - 1); lastKeyPressed = (int)Keys.Back; } }
                                        if (Static.Password.Length < 20) { Static.Password += Ascii; }
                                        break;
                                }
                                break;

                            case Constant.curMenuRegister:

                                switch (Static.registerInput)
                                {
                                    case 1:
                                        if (justPressed(Keys.Tab)) { Static.registerInput = 2; }
                                        if (justPressed(Keys.Back)) { if (Static.rUsername.Length > 0) { Static.rUsername = Static.rUsername.Substring(0, Static.rUsername.Length - 1); lastKeyPressed = (int)Keys.Back; } }
                                        if (Static.rUsername.Length < 20) { Static.rUsername += Ascii; }
                                        break;
                                    case 2:
                                        if (justPressed(Keys.Tab)) { Static.registerInput = 1; }
                                        if (justPressed(Keys.Back)) { if (Static.rPassword.Length > 0) { Static.rPassword = Static.rPassword.Substring(0, Static.rPassword.Length - 1); lastKeyPressed = (int)Keys.Back; } }
                                        if (Static.rPassword.Length < 20) { Static.rPassword += Ascii; }
                                        break;
                                }
                                break;
                        }

                        break;
                        
                    case Constant.inGame:

                        // Chat Box Input //
                        if (Static.EnterText)
                        {
                            bool B = false;

                            // Back Key //
                            if (justPressed(Keys.Back))
                            {
                                if (Static.EnterTextBuffer.Length > 0)
                                {
                                    Static.EnterTextBuffer = Static.EnterTextBuffer.Substring(0, Static.EnterTextBuffer.Length - 1);
                                    lastKeyPressed = (int)Keys.Back;
                                    B = true;
                                }
                            }

                            // Add to text buffer //
                            if (General.validCharacter((int)Key[i]))
                            {
                                if (Static.EnterTextBuffer.Length < 100)
                                {
                                    Static.EnterTextBuffer += Ascii;
                                    B = true;
                                }
                            }

                            //Update size
                            if (B)
                            {
                                Static.EnterTextBufferWidth = General.getStringWidth(1, Static.EnterTextBuffer);
                                General.updateTextBuffer();
                                Static.lastClickedWindow = 0;
                                B = false;
                            }
                        }

                        break;
                }

            }
        }
        private void handleKeyUp()
        {
            switch (Static.menuType)
            {
                case Constant.inMenu:

                    break;

                case Constant.inGame:

                    break;
            }
        }

        /////////////////
        // Mouse Input //
        /////////////////

        // Handle all Mouse Input //
        private void handleMouseInput()
        {
            curMouse.X = newMouseState.X;
            curMouse.Y = newMouseState.Y;

            HandleMouseMove();

            if (newMouseState.LeftButton == ButtonState.Pressed)
            {
                HandleMouseDown();
                lastClicked = 1;
            }
            else if (newMouseState.LeftButton == ButtonState.Released)
            {
                if (lastClicked == 1)
                { HandleMouseUp(); lastClicked = 0; }
            }

            if (newMouseState.RightButton == ButtonState.Pressed)
            {
                HandleMouseDown();
                lastClicked = 2;
            }
            else if (newMouseState.RightButton == ButtonState.Released)
            {
                if (lastClicked == 2)
                { HandleMouseUp(); lastClicked = 0; }
            }
        }

        // Main Handle Void //
        private void HandleMouseDown()
        {
            if (Static.menuType == Constant.inMenu) menu_handleMouseDown();
            if (Static.menuType == Constant.inGame) game_handleMouseDown();
        }
        private void HandleMouseMove()
        {
            if (Static.menuType == Constant.inMenu) menu_handleMouseMove();
            if (Static.menuType == Constant.inGame) game_handleMouseMove();
        }
        private void HandleMouseUp()
        {
            if (Static.menuType == Constant.inMenu) menu_handleMouseUp();
            if (Static.menuType == Constant.inGame) game_handleMouseUp();
        }

        // Menu Mouse Input //
        private void menu_handleMouseDown()
        {
            int var = Static.curMainMenu;
            if (justClicked(1))
            {
                switch (var)
                {
                    case 1: MenuWindow_Main_LeftClick(); break;
                    case 2: MenuWindow_Login_LeftClick(); break;
                    case Constant.curMenuRegister: MenuWindow_Register_LeftClick(); break;
                }
            }
        }
        private void menu_handleMouseMove()
        {
            int var = Static.curMainMenu;
            switch (var)
            {
                case 1: MenuWindow_Main_Move(); break;
                case 2: MenuWindow_Login_Move(); break;
                case Constant.curMenuRegister: MenuWindow_Register_Move(); break;
            }
        }
        private void menu_handleMouseUp()
        {
            int var = Static.curMainMenu;
            switch (var)
            {
                case 1: MenuWindow_Main_LeftRelease(); break;
                case 2: MenuWindow_Login_LeftRelease(); break;
                case Constant.curMenuRegister: MenuWindow_Register_LeftRelease(); break;
            }
        }

        // Main Menu Mouse Input //
        private void MenuWindow_Main_LeftClick()
        {
            int LoopI;

            // INPUT BUTTONS
            for (LoopI = 1; LoopI <= 5; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.mainwindow.Button[LoopI].X, UI_Menu.MenuWindow.mainwindow.Button[LoopI].Y, UI_Menu.MenuWindow.mainwindow.Button[LoopI].W, UI_Menu.MenuWindow.mainwindow.Button[LoopI].H))
                    UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE = 3;
            }
        }
        private void MenuWindow_Main_Move()
        {
            int LoopI;

            // INPUT BUTTONS
            for (LoopI = 1; LoopI <= 5; LoopI++)
            {
                // HANDLE HOVER
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.mainwindow.Button[LoopI].X, UI_Menu.MenuWindow.mainwindow.Button[LoopI].Y, UI_Menu.MenuWindow.mainwindow.Button[LoopI].W, UI_Menu.MenuWindow.mainwindow.Button[LoopI].H))
                {
                    if (UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE != 2) 
                    { 
                        UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE = 2; 
                        AudioManager.Instance.PlayEffect(1); 
                    }
                }
                else
                {
                    if (UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE != 1) { UI_Menu.MenuWindow.mainwindow.Button[LoopI].STATE = 1; }
                }
            }
        }
        private void MenuWindow_Main_LeftRelease()
        {
            // INPUT BUTTONS
            for (int LoopI = 1; LoopI <= 5; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.mainwindow.Button[LoopI].X, UI_Menu.MenuWindow.mainwindow.Button[LoopI].Y, UI_Menu.MenuWindow.mainwindow.Button[LoopI].W, UI_Menu.MenuWindow.mainwindow.Button[LoopI].H))
                {
                    switch (LoopI)
                    {
                        case 1: Static.curMainMenu = Constant.curMenuLogin; break;
                        case 2: Static.curMainMenu = Constant.curMenuRegister; break;
                        case 3: break;
                        case 4: if (Static.showCredits) { Static.showCredits = false; } else { Static.showCredits = true; } break;
                        case 5: GC.Collect(); Static.shutDown = true; break;
                    }
                }
            }
        }

        // Login Mouse Input //
        private void MenuWindow_Login_LeftClick()
        {
            int LoopI;

            // INPUT BUTTONS
            for (LoopI = 1; LoopI <= 2; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.loginwindow.Button[LoopI].X, UI_Menu.MenuWindow.loginwindow.Button[LoopI].Y, UI_Menu.MenuWindow.loginwindow.Button[LoopI].W, UI_Menu.MenuWindow.loginwindow.Button[LoopI].H))
                {
                    UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE = 3;
                }
            }

            // INPUT GRAPHICS
            for (LoopI = 1; LoopI <= 2; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.loginwindow.Textures[LoopI].X, UI_Menu.MenuWindow.loginwindow.Textures[LoopI].Y, UI_Menu.MenuWindow.loginwindow.Textures[LoopI].W, UI_Menu.MenuWindow.loginwindow.Textures[LoopI].H))
                {
                    switch (LoopI)
                    {
                        case 1: Static.loginInput = 1; break;
                        case 2: Static.loginInput = 2; break;
                    }
                }
            }

        }
        private void MenuWindow_Login_Move()
        {
            int LoopI;

            // INPUT BUTTONS
            for (LoopI = 1; LoopI <= 2; LoopI++)
            {
                // HANDLE HOVER
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.loginwindow.Button[LoopI].X, UI_Menu.MenuWindow.loginwindow.Button[LoopI].Y, UI_Menu.MenuWindow.loginwindow.Button[LoopI].W, UI_Menu.MenuWindow.loginwindow.Button[LoopI].H))
                {
                    if (UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE == 1 | UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE == 3) 
                    {
                        AudioManager.Instance.PlayEffect(1); 
                        UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE = 2;
                    }
                }
                else
                {
                    if (UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE != 1) 
                    { 
                        UI_Menu.MenuWindow.loginwindow.Button[LoopI].STATE = 1; 
                    }
                }
            }
        }
        private void MenuWindow_Login_LeftRelease()
        {
            // INPUT BUTTONS
            for (int LoopI = 1; LoopI <= 2; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.loginwindow.Button[LoopI].X, UI_Menu.MenuWindow.loginwindow.Button[LoopI].Y, UI_Menu.MenuWindow.loginwindow.Button[LoopI].W, UI_Menu.MenuWindow.loginwindow.Button[LoopI].H))
                {
                    switch (LoopI)
                    {
                        case 1:
                            if (NetworkManager.Instance.DiscoverServer())
                                {
                                    if (General.isLoginLegal(Static.Username, Static.Password))
                                    {
                                        General.menuAlertMsg("Connecting To Server...", 1);
                                        ClientTCP.SendLogin(Static.Username, Static.Password);
                                        Static.atemptLogin = true;
                                    }
                                    else { General.menuAlertMsg("Please enter a password and username.", 1); }
                                }
                                else { General.menuAlertMsg("Server Offline", 1); }
                            break;
                        case 2: Static.curMainMenu = Constant.curMenuMain; break;
                    }
                }
            }
        }

        // Login Mouse Input //
        private void MenuWindow_Register_LeftClick()
        {
            int LoopI;

            // INPUT BUTTONS
            for (LoopI = 1; LoopI <= 2; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.registerwindow.Button[LoopI].X, UI_Menu.MenuWindow.registerwindow.Button[LoopI].Y, UI_Menu.MenuWindow.registerwindow.Button[LoopI].W, UI_Menu.MenuWindow.registerwindow.Button[LoopI].H))
                {
                    UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE = 3;
                }
            }

            // INPUT GRAPHICS
            for (LoopI = 1; LoopI <= 2; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.registerwindow.Textures[LoopI].X, UI_Menu.MenuWindow.registerwindow.Textures[LoopI].Y, UI_Menu.MenuWindow.registerwindow.Textures[LoopI].W, UI_Menu.MenuWindow.registerwindow.Textures[LoopI].H))
                {
                    switch (LoopI)
                    {
                        case 1: Static.registerInput = 1; break;
                        case 2: Static.registerInput = 2; break;
                    }
                }
            }

        }
        private void MenuWindow_Register_Move()
        {
            int LoopI;

            // INPUT BUTTONS
            for (LoopI = 1; LoopI <= 2; LoopI++)
            {
                // HANDLE HOVER
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.registerwindow.Button[LoopI].X, UI_Menu.MenuWindow.registerwindow.Button[LoopI].Y, UI_Menu.MenuWindow.registerwindow.Button[LoopI].W, UI_Menu.MenuWindow.registerwindow.Button[LoopI].H))
                {
                    if (UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE == 1 | UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE == 3)
                    {
                        AudioManager.Instance.PlayEffect(1); 
                        UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE = 2;
                    }
                }
                else
                {
                    if (UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE != 1)
                    {
                        UI_Menu.MenuWindow.registerwindow.Button[LoopI].STATE = 1;
                    }
                }
            }

            // INPUT TEXT
            for (LoopI = 1; LoopI <= 3; LoopI++)
            {
                // HANDLE HOVER
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.registerwindow.Text[LoopI].X, UI_Menu.MenuWindow.registerwindow.Text[LoopI].Y, UI_Menu.MenuWindow.registerwindow.Text[LoopI].W, UI_Menu.MenuWindow.registerwindow.Text[LoopI].H))
                {
                    if (UI_Menu.MenuWindow.registerwindow.Text[LoopI].COLOUR != Color.Green) 
                    {
                        UI_Menu.MenuWindow.registerwindow.Text[LoopI].COLOUR = Color.Green;
                    }
                }
                else // RESET TEXT COLOUR
                {
                    if (UI_Menu.MenuWindow.registerwindow.Text[LoopI].COLOUR != Color.White)
                    {
                        UI_Menu.MenuWindow.registerwindow.Text[LoopI].COLOUR = Color.White;
                    }
                }
            }
        }
        private void MenuWindow_Register_LeftRelease()
        {
            // INPUT BUTTONS
            for (int LoopI = 1; LoopI <= 2; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.registerwindow.Button[LoopI].X, UI_Menu.MenuWindow.registerwindow.Button[LoopI].Y, UI_Menu.MenuWindow.registerwindow.Button[LoopI].W, UI_Menu.MenuWindow.registerwindow.Button[LoopI].H))
                {
                    switch (LoopI)
                    {
                        case 1:
                            if (NetworkManager.Instance.DiscoverServer())
                            {
                                if (General.isRegisterLegal(Static.rUsername, Static.rPassword))
                                {
                                    General.menuAlertMsg("Connecting To Server...", 1);
                                    ClientTCP.SendRegister(Static.rUsername, Static.rPassword, Static.rSprite);
                                }
                                else { General.menuAlertMsg("Please enter a username & password.", 1); }
                            }
                            else { General.menuAlertMsg("Server Offline", 1); }
                            break;
                        case 2: Static.curMainMenu = Constant.curMenuMain; break;
                    }
                }
            }

            // INPUT TEXT //
            for (int LoopI = 1; LoopI <= 3; LoopI++)
            {
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Menu.MenuWindow.registerwindow.Text[LoopI].X, UI_Menu.MenuWindow.registerwindow.Text[LoopI].Y, UI_Menu.MenuWindow.registerwindow.Text[LoopI].W, UI_Menu.MenuWindow.registerwindow.Text[LoopI].H))
                {
                    switch (LoopI)
                    {
                        case 1: break;
                        case 2: break;
                        case 3: General.changeSprite(); break;
                    }
                }
            }

        }

        // Game Mouse Input //
        private void game_handleMouseDown()
        {
            mouseLeftClick();
        }
        private void game_handleMouseMove()
        {
            mouseMove();
        }
        private void game_handleMouseUp()
        {
            mouseLeftRelease();
        }

        // Mouse State //
        private void mouseLeftClick()
        {
            // Start with the last clicked window, then move in order of importance //
            if (Static.lastClickedWindow > 0)
            {
                if (clickWindow(Static.lastClickedWindow) == 1) { return; }
            }

            for (byte i = 1; i <= Constant.maxWindows; i++)
            {
                if (Static.lastClickedWindow != i)
                {
                    if (clickWindow(i) == 1) { return; }
                }
            }
        }
        private void mouseMove()
        {
            hoverWindow();
        }
        private void mouseLeftRelease()
        {
            releaseWindow();
        }

        // Game Windows Input //
        private byte clickWindow(byte windowIndex)
        {
            switch (windowIndex)
            {
                case Constant.chatWindow:

                    if (Static.ShowGameWindow[Constant.chatWindow])
                    {
                        // Check Window Was Clicked //
                        if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Game.gameWindow.chatWindow.Texture.X, UI_Game.gameWindow.chatWindow.Texture.Y, UI_Game.gameWindow.chatWindow.Texture.W, UI_Game.gameWindow.chatWindow.Texture.H))
                        {
                            int windowX = UI_Game.gameWindow.chatWindow.Texture.X;
                            int windowY = UI_Game.gameWindow.chatWindow.Texture.Y;

                            // Check Enter Text Bar Was Clicked //
                            if (checkCollision(curMouse.X, curMouse.Y, 1, 1, windowX + UI_Game.gameWindow.chatWindow.TextInput.X, windowY + UI_Game.gameWindow.chatWindow.TextInput.Y, UI_Game.gameWindow.chatWindow.TextInput.W, UI_Game.gameWindow.chatWindow.TextInput.H))
                            {
                                Static.EnterText = true;
                            }

                            Static.lastClickedWindow = Constant.chatWindow;
                            //Static.SelGameWindow = Constant.chatWindow;
                            return 1;
                        }
                    }

                    break;

                case Constant.menuWindow:

                    if (Static.ShowGameWindow[Constant.menuWindow])
                    {
                        // Check Window Was Clicked //
                        if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Game.gameWindow.menuWindow.Texture.X, UI_Game.gameWindow.menuWindow.Texture.Y, UI_Game.gameWindow.menuWindow.Texture.W, UI_Game.gameWindow.menuWindow.Texture.H))
                        {
                            int windowX = UI_Game.gameWindow.menuWindow.Texture.X;
                            int windowY = UI_Game.gameWindow.menuWindow.Texture.Y;

                            // Check Enter Text Bar Was Clicked //
                            for (int LoopI = 0; LoopI <= 5; LoopI++)
                            {
                                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, windowX + UI_Game.gameWindow.menuWindow.Button[LoopI].X, windowY + UI_Game.gameWindow.menuWindow.Button[LoopI].Y, UI_Game.gameWindow.menuWindow.Button[LoopI].W, UI_Game.gameWindow.menuWindow.Button[LoopI].H))
                                {
                                    UI_Game.gameWindow.menuWindow.Button[LoopI].STATE = Constant.BUTTON_STATE_CLICK;
                                }
                            }

                            Static.lastClickedWindow = Constant.menuWindow;
                            //Static.SelGameWindow = Constant.menuWindow;
                            return 1;
                        }
                    }

                    break;

                case Constant.charWindow:

                    if (Static.ShowGameWindow[Constant.charWindow])
                    {
                        // Check Window Was Clicked //
                        if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Game.gameWindow.charWindow.Texture.X, UI_Game.gameWindow.charWindow.Texture.Y, UI_Game.gameWindow.charWindow.Texture.W, UI_Game.gameWindow.charWindow.Texture.H))
                        {
                            int windowX = UI_Game.gameWindow.charWindow.Texture.X;
                            int windowY = UI_Game.gameWindow.charWindow.Texture.Y;

                            Static.lastClickedWindow = Constant.charWindow;
                            //Static.SelGameWindow = Constant.menuWindow;
                            return 1;
                        }
                    }

                    break;
            }

            return 0;
        }
        private void hoverWindow()
        {
            int dX;
            int dY;
            int dW;
            int dH;

            // Temp - Always Show //
            Static.ShowGameWindow[Constant.menuWindow] = true;

            // MAIN MENU WITH 6 BUTTONS //
            if (Static.ShowGameWindow[Constant.menuWindow])
            {
                dX = UI_Game.gameWindow.menuWindow.Texture.X;
                dY = UI_Game.gameWindow.menuWindow.Texture.Y;
                dW = UI_Game.gameWindow.menuWindow.Texture.W;
                dH = UI_Game.gameWindow.menuWindow.Texture.H;

                // Check Inside Main Window Only //
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, dX, dY, dW, dH))
                {
                    // INPUT BUTTONS
                    for (int LoopI = 0; LoopI <= 5; LoopI++)
                    {
                        // HANDLE HOVER
                        if (checkCollision(curMouse.X, curMouse.Y, 1, 1, dX + UI_Game.gameWindow.menuWindow.Button[LoopI].X, dY + UI_Game.gameWindow.menuWindow.Button[LoopI].Y, UI_Game.gameWindow.menuWindow.Button[LoopI].W, UI_Game.gameWindow.menuWindow.Button[LoopI].H))
                        {
                            if (UI_Game.gameWindow.menuWindow.Button[LoopI].STATE != Constant.BUTTON_STATE_HOVER && UI_Game.gameWindow.menuWindow.Button[LoopI].STATE != Constant.BUTTON_STATE_CLICK)
                            {
                                AudioManager.Instance.PlayEffect(1); 
                                UI_Game.gameWindow.menuWindow.Button[LoopI].STATE = Constant.BUTTON_STATE_HOVER;
                            }
                        }
                        else
                        {
                            if (UI_Game.gameWindow.menuWindow.Button[LoopI].STATE != Constant.BUTTON_STATE_NORMAL)
                            {
                                UI_Game.gameWindow.menuWindow.Button[LoopI].STATE = Constant.BUTTON_STATE_NORMAL;
                            }
                        }
                    }
                }
            }

            // CHARACTER WINDOW //
            if (Static.ShowGameWindow[Constant.charWindow])
            {
                // Get Character Window X & Y //
                dX = UI_Game.gameWindow.charWindow.Texture.X;
                dY = UI_Game.gameWindow.charWindow.Texture.Y;
                dW = UI_Game.gameWindow.charWindow.Texture.W;
                dH = UI_Game.gameWindow.charWindow.Texture.H;

                // Check Inside Main Window Only //
                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, dX, dY, dW, dH))
                {
                    // INPUT TEXT
                    for (int LoopI = 0; LoopI <= 3; LoopI++)
                    {
                        // HANDLE HOVER
                        if (checkCollision(curMouse.X, curMouse.Y, 1, 1, dX + UI_Game.gameWindow.charWindow.Text[LoopI].X, dY + UI_Game.gameWindow.charWindow.Text[LoopI].Y, UI_Game.gameWindow.charWindow.Text[LoopI].W, UI_Game.gameWindow.charWindow.Text[LoopI].H))
                        {
                            if (UI_Game.gameWindow.charWindow.Text[LoopI].COLOUR != Color.Green)
                            {
                                UI_Game.gameWindow.charWindow.Text[LoopI].COLOUR = Color.Green;
                            }
                        }
                        else // RESET TEXT COLOUR
                        {
                            if (UI_Game.gameWindow.charWindow.Text[LoopI].COLOUR != Color.White)
                            {
                                UI_Game.gameWindow.charWindow.Text[LoopI].COLOUR = Color.White;
                            }
                        }
                    }
                }
            }

        }
        private void releaseWindow()
        {
            switch (Static.lastClickedWindow)
            {
                case Constant.menuWindow:

                    if (Static.ShowGameWindow[Constant.menuWindow])
                    {
                        // Check Window Was Clicked //
                        if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Game.gameWindow.menuWindow.Texture.X, UI_Game.gameWindow.menuWindow.Texture.Y, UI_Game.gameWindow.menuWindow.Texture.W, UI_Game.gameWindow.menuWindow.Texture.H))
                        {
                            int windowX = UI_Game.gameWindow.menuWindow.Texture.X;
                            int windowY = UI_Game.gameWindow.menuWindow.Texture.Y;

                            // Check Enter Text Bar Was Clicked //
                            for (int LoopI = 0; LoopI <= 5; LoopI++)
                            {
                                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, windowX + UI_Game.gameWindow.menuWindow.Button[LoopI].X, windowY + UI_Game.gameWindow.menuWindow.Button[LoopI].Y, UI_Game.gameWindow.menuWindow.Button[LoopI].W, UI_Game.gameWindow.menuWindow.Button[LoopI].H))
                                {
                                    // Reset Button //
                                    UI_Game.gameWindow.menuWindow.Button[LoopI].STATE = Constant.BUTTON_STATE_NORMAL;

                                    switch (LoopI)
                                    {
                                        case 0: showGameWindows(Constant.inventoryWindow); break;
                                        case 1: showGameWindows(Constant.chatWindow); break;
                                        case 2: showGameWindows(Constant.spellWindow); break;
                                        case 3: break;
                                        case 4: showGameWindows(Constant.charWindow); break;
                                        case 5: Static.shutDown = true; break;
                                    }
                                }
                            }

                            Static.lastClickedWindow = Constant.menuWindow;
                            //Static.SelGameWindow = Constant.menuWindow;
                        }
                    }

                    break;

                case Constant.charWindow:

                    if (Static.ShowGameWindow[Constant.charWindow])
                    {
                        // Check Window Was Clicked //
                        if (checkCollision(curMouse.X, curMouse.Y, 1, 1, UI_Game.gameWindow.charWindow.Texture.X, UI_Game.gameWindow.charWindow.Texture.Y, UI_Game.gameWindow.charWindow.Texture.W, UI_Game.gameWindow.charWindow.Texture.H))
                        {
                            int windowX = UI_Game.gameWindow.charWindow.Texture.X;
                            int windowY = UI_Game.gameWindow.charWindow.Texture.Y;

                            // INPUT TEXT //
                            for (int LoopI = 0; LoopI <= 3; LoopI++)
                            {
                                if (checkCollision(curMouse.X, curMouse.Y, 1, 1, windowX + UI_Game.gameWindow.charWindow.Text[LoopI].X, windowY + UI_Game.gameWindow.charWindow.Text[LoopI].Y, UI_Game.gameWindow.charWindow.Text[LoopI].W, UI_Game.gameWindow.charWindow.Text[LoopI].H))
                                {
                                    switch (LoopI)
                                    {
                                        case 0: ClientTCP.SendTrainStat(1); break;
                                        case 1: ClientTCP.SendTrainStat(2); break;
                                        case 2: ClientTCP.SendTrainStat(3); break;
                                        case 3: ClientTCP.SendTrainStat(4); break;
                                    }
                                }
                            }

                            Static.lastClickedWindow = Constant.charWindow;
                            //Static.SelGameWindow = Constant.menuWindow;
                        }
                    }

                    break;
            }
        }

        public static void showGameWindows(int windowNum)
        {
            if (Static.ShowGameWindow[windowNum])
            {
                // Hide Window //
                Static.ShowGameWindow[windowNum] = false;
            }
            else
            {
                // Only Hide able Windows //
                if (windowNum != Constant.chatWindow)
                {
                    Static.ShowGameWindow[Constant.inventoryWindow] = false;
                    Static.ShowGameWindow[Constant.spellWindow] = false;
                    Static.ShowGameWindow[Constant.charWindow] = false;
                }

                // Show Window //
                Static.ShowGameWindow[windowNum] = true;
            }
        }
    }
}
