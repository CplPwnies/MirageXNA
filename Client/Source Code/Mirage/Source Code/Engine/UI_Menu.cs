///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;

using MirageXNA.Global;

namespace MirageXNA.Engine
{
    public static class UI_Menu
    {
        public const int Window_Main = new int();

        // ***** Main Menu Window *****
        public const int Max_Main_Buttons = 5;
        public struct MainWindow
        {
            public Types.Grh Texture;
            public Types.Grh_Button[] Button;
        }

        // ***** Login Window *****
        public const int Max_Login_Buttons = 2;
        public const int Max_Login_Textures = 2;
        public struct LoginWindow
        {
            public Types.Grh Texture;
            public Types.Grh_Button[] Button;
            public Types.Grh[] Textures;
        }

        // ***** Register Window *****
        public const int Max_Register_Buttons = 2;
        public const int Max_Register_Textures = 2;
        public const int Max_Register_Text = 3;
        public struct RegisterWindow
        {
            public Types.Grh Texture;
            public Types.Grh_Button[] Button;
            public Types.Grh[] Textures;
            public Types.Grh_Text[] Text;
        }

        public struct MenuWindow
        {
            public static MainWindow mainwindow;
            public static LoginWindow loginwindow;
            public static RegisterWindow registerwindow;
        }

        public static void initGUI()
        {
            /// Initialize managers ///
            #region Initializers
            MenuWindow.mainwindow.Button = new Types.Grh_Button[Max_Main_Buttons + 1];
            MenuWindow.loginwindow.Button = new Types.Grh_Button[Max_Login_Buttons + 1];
            MenuWindow.loginwindow.Textures = new Types.Grh[Max_Login_Textures + 1];
            MenuWindow.registerwindow.Button = new Types.Grh_Button[Max_Register_Buttons + 1];
            MenuWindow.registerwindow.Textures = new Types.Grh[Max_Register_Textures + 1];
            MenuWindow.registerwindow.Text = new Types.Grh_Text[Max_Register_Text + 1];
            #endregion

            /// Main Menu Initisalizer ///
            #region MainMenu
            // Main
            MenuWindow.mainwindow.Texture.X = 0;
            MenuWindow.mainwindow.Texture.Y = 0;
            MenuWindow.mainwindow.Texture.W = 1024;
            MenuWindow.mainwindow.Texture.H = 768;
            MenuWindow.mainwindow.Texture.TEXTURE = Static.Tex_Gui[1];

            // LOGIN
            MenuWindow.mainwindow.Button[1].X = 512 - 150;
            MenuWindow.mainwindow.Button[1].Y = 300;
            MenuWindow.mainwindow.Button[1].W = 300;
            MenuWindow.mainwindow.Button[1].H = 60;
            MenuWindow.mainwindow.Button[1].TEXTURE = 1;
            MenuWindow.mainwindow.Button[1].STATE = Constant.BUTTON_STATE_NORMAL;
        
            // REGISTER
            MenuWindow.mainwindow.Button[2].X = 512 - 150;
            MenuWindow.mainwindow.Button[2].Y = 370;
            MenuWindow.mainwindow.Button[2].W = 300;
            MenuWindow.mainwindow.Button[2].H = 60;
            MenuWindow.mainwindow.Button[2].TEXTURE = 2;
            MenuWindow.mainwindow.Button[2].STATE = Constant.BUTTON_STATE_NORMAL;
        
            // OPTIONS
            MenuWindow.mainwindow.Button[3].X = 512 - 150;
            MenuWindow.mainwindow.Button[3].Y = 440;
            MenuWindow.mainwindow.Button[3].W = 300;
            MenuWindow.mainwindow.Button[3].H = 60;
            MenuWindow.mainwindow.Button[3].TEXTURE = 3;
            MenuWindow.mainwindow.Button[3].STATE = Constant.BUTTON_STATE_NORMAL;
        
            // CREDITS
            MenuWindow.mainwindow.Button[4].X = 512 - 150;
            MenuWindow.mainwindow.Button[4].Y = 510;
            MenuWindow.mainwindow.Button[4].W = 300;
            MenuWindow.mainwindow.Button[4].H = 60;
            MenuWindow.mainwindow.Button[4].TEXTURE = 4;
            MenuWindow.mainwindow.Button[4].STATE = Constant.BUTTON_STATE_NORMAL;
        
            // EXIT GAME
            MenuWindow.mainwindow.Button[5].X = 512 - 150;
            MenuWindow.mainwindow.Button[5].Y = 580;
            MenuWindow.mainwindow.Button[5].W = 300;
            MenuWindow.mainwindow.Button[5].H = 60;
            MenuWindow.mainwindow.Button[5].TEXTURE = 5;
            MenuWindow.mainwindow.Button[5].STATE = Constant.BUTTON_STATE_NORMAL;
            #endregion

            /// Login Menu Initisalizer ///
            #region LoginMenu
            // Main
            MenuWindow.loginwindow.Texture.X = 0;
            MenuWindow.loginwindow.Texture.Y = 0;
            MenuWindow.loginwindow.Texture.W = 1024;
            MenuWindow.loginwindow.Texture.H = 1024;
            MenuWindow.loginwindow.Texture.TEXTURE = Static.Tex_Gui[1];

            // LOGIN
            MenuWindow.loginwindow.Button[1].X = 363;
            MenuWindow.loginwindow.Button[1].Y = 500;
            MenuWindow.loginwindow.Button[1].W = 150;
            MenuWindow.loginwindow.Button[1].H = 30;
            MenuWindow.loginwindow.Button[1].TEXTURE = 15;
            MenuWindow.loginwindow.Button[1].STATE = Constant.BUTTON_STATE_NORMAL;

            // BACK
            MenuWindow.loginwindow.Button[2].X = 520;
            MenuWindow.loginwindow.Button[2].Y = 500;
            MenuWindow.loginwindow.Button[2].W = 150;
            MenuWindow.loginwindow.Button[2].H = 30;
            MenuWindow.loginwindow.Button[2].TEXTURE = 6;
            MenuWindow.loginwindow.Button[2].STATE = Constant.BUTTON_STATE_NORMAL;

            // INPUT BOX
            MenuWindow.loginwindow.Textures[1].X = 512 - 128;
            MenuWindow.loginwindow.Textures[1].Y = 300;
            MenuWindow.loginwindow.Textures[1].W = 256;
            MenuWindow.loginwindow.Textures[1].H = 32;
            MenuWindow.loginwindow.Textures[1].TEXTURE = 3;

            // INPUT BOX
            MenuWindow.loginwindow.Textures[2].X = 512 - 128;
            MenuWindow.loginwindow.Textures[2].Y = 350;
            MenuWindow.loginwindow.Textures[2].W = 256;
            MenuWindow.loginwindow.Textures[2].H = 32;
            MenuWindow.loginwindow.Textures[2].TEXTURE = 3;
            #endregion

            /// Regsiter Menu Initisalizer ///
            #region RegisterMenu
            // Main
            MenuWindow.registerwindow.Texture.X = 0;
            MenuWindow.registerwindow.Texture.Y = 0;
            MenuWindow.registerwindow.Texture.W = 1024;
            MenuWindow.registerwindow.Texture.H = 1024;
            MenuWindow.registerwindow.Texture.TEXTURE = Static.Tex_Gui[1];

            // REGISTER
            MenuWindow.registerwindow.Button[1].X = 363;
            MenuWindow.registerwindow.Button[1].Y = 500;
            MenuWindow.registerwindow.Button[1].W = 150;
            MenuWindow.registerwindow.Button[1].H = 30;
            MenuWindow.registerwindow.Button[1].TEXTURE = 16;
            MenuWindow.registerwindow.Button[1].STATE = Constant.BUTTON_STATE_NORMAL;

            // BACK
            MenuWindow.registerwindow.Button[2].X = 520;
            MenuWindow.registerwindow.Button[2].Y = 500;
            MenuWindow.registerwindow.Button[2].W = 150;
            MenuWindow.registerwindow.Button[2].H = 30;
            MenuWindow.registerwindow.Button[2].TEXTURE = 6;
            MenuWindow.registerwindow.Button[2].STATE = Constant.BUTTON_STATE_NORMAL;

            // INPUT BOX
            MenuWindow.registerwindow.Textures[1].X = 512 - 128;
            MenuWindow.registerwindow.Textures[1].Y = 300;
            MenuWindow.registerwindow.Textures[1].W = 256;
            MenuWindow.registerwindow.Textures[1].H = 32;
            MenuWindow.registerwindow.Textures[1].TEXTURE = 3;

            // INPUT BOX
            MenuWindow.registerwindow.Textures[2].X = 512 - 128;
            MenuWindow.registerwindow.Textures[2].Y = 350;
            MenuWindow.registerwindow.Textures[2].W = 256;
            MenuWindow.registerwindow.Textures[2].H = 32;
            MenuWindow.registerwindow.Textures[2].TEXTURE = 3;

            // TEXT SELECT SPRITE
            MenuWindow.registerwindow.Text[1].CAPTION = "[Select Class]";
            MenuWindow.registerwindow.Text[1].X = 512 - General.getStringWidth(1, UI_Menu.MenuWindow.registerwindow.Text[1].CAPTION) / 2; ; ;
            MenuWindow.registerwindow.Text[1].Y = 400;
            MenuWindow.registerwindow.Text[1].W = General.getStringWidth(1, MenuWindow.registerwindow.Text[1].CAPTION);
            MenuWindow.registerwindow.Text[1].H = General.getStringHeight(1, MenuWindow.registerwindow.Text[1].CAPTION);
            MenuWindow.registerwindow.Text[1].FONT = 1;
            MenuWindow.registerwindow.Text[1].COLOUR = Color.White;

            // TEXT SELECT CLASS
            MenuWindow.registerwindow.Text[2].CAPTION = "[Select Sex]";
            MenuWindow.registerwindow.Text[2].X = 512 - General.getStringWidth(1, UI_Menu.MenuWindow.registerwindow.Text[2].CAPTION) / 2; ;
            MenuWindow.registerwindow.Text[2].Y = 420;
            MenuWindow.registerwindow.Text[2].W = General.getStringWidth(1, MenuWindow.registerwindow.Text[2].CAPTION);
            MenuWindow.registerwindow.Text[2].H = General.getStringHeight(1, MenuWindow.registerwindow.Text[2].CAPTION);
            MenuWindow.registerwindow.Text[2].FONT = 1;
            MenuWindow.registerwindow.Text[2].COLOUR = Color.White;

            // TEXT SELECT CLASS
            MenuWindow.registerwindow.Text[3].CAPTION = "[Select Sprite]";
            MenuWindow.registerwindow.Text[3].X = 512 - General.getStringWidth(1, UI_Menu.MenuWindow.registerwindow.Text[3].CAPTION) / 2;
            MenuWindow.registerwindow.Text[3].Y = 440;
            MenuWindow.registerwindow.Text[3].W = General.getStringWidth(1, MenuWindow.registerwindow.Text[3].CAPTION);
            MenuWindow.registerwindow.Text[3].H = General.getStringHeight(1, MenuWindow.registerwindow.Text[3].CAPTION);
            MenuWindow.registerwindow.Text[3].FONT = 1;
            MenuWindow.registerwindow.Text[3].COLOUR = Color.White;
            #endregion

        }

    }
}
