///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////

using System;

// Handle Keyboard & Mouse Input //
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MirageXNA.Global;
using MirageXNA.Engine;
using MirageXNA.Network;

namespace MirageXNA.Engine
{
    public static class UI_Game
    {
        // Chat Window //
        public struct ChatWindow
        {
            public Types.Grh Texture;
            public Types.Grh_Button Button;
            public Types.Grh TextInput;
        }

        // Menu Window //
        public struct MainWindow
        {
            public Types.Grh Texture;
            public Types.Grh_Button[] Button;
        }

        // Char Window //
        public struct CharWindow
        {
            public Types.Grh Texture;
            public Types.Grh[] Textures;
            public Types.Grh_Text[] Text;
        }

        // Inventory Window //
        public struct InventoryWindow
        {
            public Types.Grh Texture;
            public Types.Grh[] Textures;
        }

        // Spell Window //
        public struct SpellWindow
        {
            public Types.Grh Texture;
            public Types.Grh[] Textures;
        }

        // All Windows //
        public struct GameWindows
        {
            public ChatWindow chatWindow;
            public MainWindow menuWindow;
            public CharWindow charWindow;
            public InventoryWindow invWindow;
            public SpellWindow spellWindow;
        }

        // Handles All In Game Windows //
        public static GameWindows gameWindow = new GameWindows();

        //////////////
        // Text Box //
        //////////////

        // Text buffer //
        public struct ChatTextBuffer
        {
            public string Text;
            public int Color;
        }
        public static ChatTextBuffer[] chatBuffer;

        ////////////////////
        // Initialisation //
        ////////////////////
        public static void initGameGui()
        {
            /// Initialize managers ///
            gameWindow.chatWindow.Button = new Types.Grh_Button();
            chatBuffer = new ChatTextBuffer[200];

            for (int i = 0; i <= 199; i++)
            {
                chatBuffer[i].Text = String.Empty;
                chatBuffer[i].Color = 0;
            }

            //////////////
            // Init Gui //
            //////////////

            // Inventory Window //
            gameWindow.invWindow.Texture.X = UI_Tweaks.charWindowX;
            gameWindow.invWindow.Texture.Y = UI_Tweaks.charWindowY;
            gameWindow.invWindow.Texture.W = UI_Tweaks.charWindowW;
            gameWindow.invWindow.Texture.H = UI_Tweaks.charWindowH;
            gameWindow.invWindow.Texture.TEXTURE = Static.Tex_Gui[7];

            // Spell Window //
            gameWindow.spellWindow.Texture.X = UI_Tweaks.charWindowX;
            gameWindow.spellWindow.Texture.Y = UI_Tweaks.charWindowY;
            gameWindow.spellWindow.Texture.W = UI_Tweaks.charWindowW;
            gameWindow.spellWindow.Texture.H = UI_Tweaks.charWindowH;
            gameWindow.spellWindow.Texture.TEXTURE = Static.Tex_Gui[7];

            #region Character Window
            // Character Window //
            gameWindow.charWindow.Texture.X = UI_Tweaks.charWindowX;
            gameWindow.charWindow.Texture.Y = UI_Tweaks.charWindowY;
            gameWindow.charWindow.Texture.W = UI_Tweaks.charWindowW;
            gameWindow.charWindow.Texture.H = UI_Tweaks.charWindowH;
            gameWindow.charWindow.Texture.TEXTURE = Static.Tex_Gui[8];

            // Resize Button Array (4) //
            gameWindow.charWindow.Textures = new Types.Grh[4];

            // Players Face //
            gameWindow.charWindow.Textures[0].X = 7;
            gameWindow.charWindow.Textures[0].Y = 38;
            gameWindow.charWindow.Textures[0].W = 96;
            gameWindow.charWindow.Textures[0].H = 96;
            gameWindow.charWindow.Textures[0].TEXTURE = 0;

            // Weapon Slot //
            gameWindow.charWindow.Textures[1].X = 0;
            gameWindow.charWindow.Textures[1].Y = 0;
            gameWindow.charWindow.Textures[1].W = 32;
            gameWindow.charWindow.Textures[1].H = 32;
            gameWindow.charWindow.Textures[1].TEXTURE = 0;

            // Armor Slot //
            gameWindow.charWindow.Textures[2].X = 0;
            gameWindow.charWindow.Textures[2].Y = 0;
            gameWindow.charWindow.Textures[2].W = 32;
            gameWindow.charWindow.Textures[2].H = 32;
            gameWindow.charWindow.Textures[2].TEXTURE = 0;

            // Helmet Slot //
            gameWindow.charWindow.Textures[3].X = 0;
            gameWindow.charWindow.Textures[3].Y = 0;
            gameWindow.charWindow.Textures[3].W = 0;
            gameWindow.charWindow.Textures[3].H = 0;
            gameWindow.charWindow.Textures[3].TEXTURE = 0;

            // Resize Text Array (6) //
            gameWindow.charWindow.Text = new Types.Grh_Text[7];

            // Strength //
            gameWindow.charWindow.Text[0].CAPTION = Enumerations.Stats.Strength.ToString();
            gameWindow.charWindow.Text[0].FONT = 1;
            gameWindow.charWindow.Text[0].X = UI_Tweaks.charWindowStat1X;
            gameWindow.charWindow.Text[0].Y = UI_Tweaks.charWindowStat1Y;
            gameWindow.charWindow.Text[0].W = General.getStringWidth(1, Enumerations.Stats.Strength.ToString());
            gameWindow.charWindow.Text[0].H = General.getStringHeight(1, Enumerations.Stats.Strength.ToString());
            gameWindow.charWindow.Text[0].COLOUR = Color.White;

            // Defence //
            gameWindow.charWindow.Text[1].CAPTION = Enumerations.Stats.Defence.ToString();
            gameWindow.charWindow.Text[1].FONT = 1;
            gameWindow.charWindow.Text[1].X = UI_Tweaks.charWindowStat2X;
            gameWindow.charWindow.Text[1].Y = UI_Tweaks.charWindowStat2Y;
            gameWindow.charWindow.Text[1].W = General.getStringWidth(1, Enumerations.Stats.Defence.ToString());
            gameWindow.charWindow.Text[1].H = General.getStringHeight(1, Enumerations.Stats.Defence.ToString());
            gameWindow.charWindow.Text[1].COLOUR = Color.White;

            // Magic //
            gameWindow.charWindow.Text[2].CAPTION = Enumerations.Stats.Magic.ToString();
            gameWindow.charWindow.Text[2].FONT = 1;
            gameWindow.charWindow.Text[2].X = UI_Tweaks.charWindowStat3X;
            gameWindow.charWindow.Text[2].Y = UI_Tweaks.charWindowStat3Y;
            gameWindow.charWindow.Text[2].W = General.getStringWidth(1, Enumerations.Stats.Magic.ToString());
            gameWindow.charWindow.Text[2].H = General.getStringHeight(1, Enumerations.Stats.Magic.ToString());
            gameWindow.charWindow.Text[2].COLOUR = Color.White;

            // Speed //
            gameWindow.charWindow.Text[3].CAPTION = Enumerations.Stats.Speed.ToString();
            gameWindow.charWindow.Text[3].FONT = 1;
            gameWindow.charWindow.Text[3].X = UI_Tweaks.charWindowStat4X;
            gameWindow.charWindow.Text[3].Y = UI_Tweaks.charWindowStat4Y;
            gameWindow.charWindow.Text[3].W = General.getStringWidth(1, Enumerations.Stats.Speed.ToString());
            gameWindow.charWindow.Text[3].H = General.getStringHeight(1, Enumerations.Stats.Speed.ToString());
            gameWindow.charWindow.Text[3].COLOUR = Color.White;

            // Player Name //
            gameWindow.charWindow.Text[4].CAPTION = String.Empty;
            gameWindow.charWindow.Text[4].FONT = 1;
            gameWindow.charWindow.Text[4].X = UI_Tweaks.charWindowNameX;
            gameWindow.charWindow.Text[4].Y = UI_Tweaks.charWindowNameY;
            gameWindow.charWindow.Text[4].W = General.getStringWidth(1, String.Empty);
            gameWindow.charWindow.Text[4].H = General.getStringHeight(1, String.Empty);
            gameWindow.charWindow.Text[4].COLOUR = Color.White;

            // Player Points //
            gameWindow.charWindow.Text[5].CAPTION = String.Empty;
            gameWindow.charWindow.Text[5].FONT = 1;
            gameWindow.charWindow.Text[5].X = UI_Tweaks.charWindowPointsX;
            gameWindow.charWindow.Text[5].Y = UI_Tweaks.charWindowPointsY;
            gameWindow.charWindow.Text[5].W = General.getStringWidth(1, String.Empty);
            gameWindow.charWindow.Text[5].H = General.getStringHeight(1, String.Empty);
            gameWindow.charWindow.Text[5].COLOUR = Color.Green;

            // Player Exp //
            gameWindow.charWindow.Text[6].CAPTION = String.Empty;
            gameWindow.charWindow.Text[6].FONT = 1;
            gameWindow.charWindow.Text[6].X = UI_Tweaks.charWindowExpX;
            gameWindow.charWindow.Text[6].Y = UI_Tweaks.charWindowExpY;
            gameWindow.charWindow.Text[6].W = General.getStringWidth(1, String.Empty);
            gameWindow.charWindow.Text[6].H = General.getStringHeight(1, String.Empty);
            gameWindow.charWindow.Text[6].COLOUR = Color.Lime;
            #endregion

            #region MainMenu Main Menu GUI

            // Menu Window //
            gameWindow.menuWindow.Texture.X = 782;
            gameWindow.menuWindow.Texture.Y = 682;
            gameWindow.menuWindow.Texture.W = 232;
            gameWindow.menuWindow.Texture.H = 76;
            gameWindow.menuWindow.Texture.TEXTURE = Static.Tex_Gui[6];

            // Resize Button Array (6) //
            gameWindow.menuWindow.Button = new Types.Grh_Button[6];

            // Text Input //
            gameWindow.menuWindow.Button[0].X = 6;
            gameWindow.menuWindow.Button[0].Y = 6;
            gameWindow.menuWindow.Button[0].W = 69;
            gameWindow.menuWindow.Button[0].H = 29;
            gameWindow.menuWindow.Button[0].TEXTURE = 7;
            gameWindow.menuWindow.Button[0].STATE = Constant.BUTTON_STATE_NORMAL;

            gameWindow.menuWindow.Button[1].X = 6;
            gameWindow.menuWindow.Button[1].Y = 41;
            gameWindow.menuWindow.Button[1].W = 69;
            gameWindow.menuWindow.Button[1].H = 29;
            gameWindow.menuWindow.Button[1].TEXTURE = 8;
            gameWindow.menuWindow.Button[1].STATE = Constant.BUTTON_STATE_NORMAL;

            gameWindow.menuWindow.Button[2].X = 81;
            gameWindow.menuWindow.Button[2].Y = 6;
            gameWindow.menuWindow.Button[2].W = 69;
            gameWindow.menuWindow.Button[2].H = 29;
            gameWindow.menuWindow.Button[2].TEXTURE = 9;
            gameWindow.menuWindow.Button[2].STATE = Constant.BUTTON_STATE_NORMAL;

            gameWindow.menuWindow.Button[3].X = 81;
            gameWindow.menuWindow.Button[3].Y = 41;
            gameWindow.menuWindow.Button[3].W = 69;
            gameWindow.menuWindow.Button[3].H = 29;
            gameWindow.menuWindow.Button[3].TEXTURE = 10;
            gameWindow.menuWindow.Button[3].STATE = Constant.BUTTON_STATE_NORMAL;

            gameWindow.menuWindow.Button[4].X = 156;
            gameWindow.menuWindow.Button[4].Y = 6;
            gameWindow.menuWindow.Button[4].W = 69;
            gameWindow.menuWindow.Button[4].H = 29;
            gameWindow.menuWindow.Button[4].TEXTURE = 11;
            gameWindow.menuWindow.Button[4].STATE = Constant.BUTTON_STATE_NORMAL;

            gameWindow.menuWindow.Button[5].X = 156;
            gameWindow.menuWindow.Button[5].Y = 41;
            gameWindow.menuWindow.Button[5].W = 69;
            gameWindow.menuWindow.Button[5].H = 29;
            gameWindow.menuWindow.Button[5].TEXTURE = 12;
            gameWindow.menuWindow.Button[5].STATE = Constant.BUTTON_STATE_NORMAL;
            #endregion

            #region Chatbox Chatbox GUI

            // Chat Window //
            gameWindow.chatWindow.Texture.X = 10;
            gameWindow.chatWindow.Texture.Y = 630;
            gameWindow.chatWindow.Texture.W = 400;
            gameWindow.chatWindow.Texture.H = 128;
            gameWindow.chatWindow.Texture.TEXTURE = Static.Tex_Gui[4];

            // Text Input //
            gameWindow.chatWindow.TextInput.X = 10;
            gameWindow.chatWindow.TextInput.Y = 102;
            gameWindow.chatWindow.TextInput.W = 380;
            gameWindow.chatWindow.TextInput.H = 16;
            gameWindow.chatWindow.TextInput.TEXTURE = Static.Tex_Gui[4];
            #endregion
        }

        //////////////
        // Chat Box //
        //////////////

        // Add Text To Chat Buffer //
        public static void AddText(string Text, int Color)
        {
            int LastSpace;
            int Size = 0;
            int i = 0;
            int B;

            // Clear the values for the new line
            Size = 0;
            B = 0;
            LastSpace = 1;

            // Loop through all the characters
            for (i = 0; i <= Text.Length - 1; i++)
            {
                // If it is a space, store it so we can easily break at it
                string subString = Text.Substring(i, 1);
                switch (subString)
                {
                    case " ": LastSpace = i; break;
                    case "_": LastSpace = i; break;
                    case "-": LastSpace = i; break;
                }

                // Add up the size - Do not count the "|" character (high-lighter)!
                if (subString != "|")
                {
                    Size = Size + General.getStringWidth(1, subString);
                }

                // Check for too large of a size
                if (Size > gameWindow.chatWindow.Texture.W)
                {
                    //Check if the last space was too far back
                    if (i - LastSpace > 10)
                    {
                        //Too far away to the last space, so break at the last character
                        string text = Text.Substring(B, (i - 1) - B);
                        // Move all other text up //
                        for (int LoopC = (200 - 2); LoopC >= 0; LoopC--)
                        {
                            chatBuffer[LoopC + 1] = chatBuffer[LoopC];
                        }
                        chatBuffer[1].Text = text;
                        chatBuffer[1].Color = Color;
                        B = i - 1;
                        Size = 0;
                    }
                    else
                    {
                        //Break at the last space to preserve the word
                        string text = Text.Substring(B, LastSpace - B);

                        // Move all other text up //
                        for (int LoopC = (200 - 2); LoopC >= 0; LoopC--)
                        {
                            chatBuffer[LoopC + 1] = chatBuffer[LoopC];
                        }
                        chatBuffer[1].Text = text;
                        chatBuffer[1].Color = Color;
                        B = LastSpace + 1;

                        //Count all the words we ignored (the ones that weren't printed, but are before "i")
                        int count = i - LastSpace;
                        string Preserved = Text.Substring(LastSpace, count);
                        Size = General.getStringWidth(1, Preserved);
                    }
                }

                //This handles the remainder
                if (i == Text.Length - 1)
                {
                    if (B != i)
                    {
                        string text = Text.Substring(B, i + 1 - B);
                        // Move all other text up //
                        for (int LoopC = (200 - 2); LoopC >= 0; LoopC--)
                        {
                            chatBuffer[LoopC + 1] = chatBuffer[LoopC];
                        }
                        chatBuffer[1].Text = text;
                        chatBuffer[1].Color = Color;
                    }
                }

            }

        }

        // Handle Typed Text From Chat Buffer //
        public static void handleText()
        {
            // Get Typed Input //
            string ChatText = Static.EnterTextBuffer;
            bool sendMsg = true;

            switch (ChatText.Substring(0, 1))
            {
                // Broadcast Message //
                case "'":

                    break;

                // Player Commands //
                case "/":
                    string Text = ChatText.Substring(1);
                    string[] command = Text.Split(' ');

                    // Get Command //
                    switch (command[0])
                    {
                        case "help":
                            AddText("** Game Controls **", 1);
                            AddText("WASD = Movement", 1);
                            AddText("SHIFT = Hyper Speed", 1);
                            AddText("CTRL = Talk to Npc", 1);
                            sendMsg = false;
                            break;
                        default:
                            AddText("Not a valid command", 1);
                            sendMsg = false;
                            break;
                    }
                    break;
            }

            // Cleans Chat Buffer //
            Static.EnterTextBuffer = String.Empty;
            Static.EnterTextBufferWidth = 10;
            Static.ShownText = String.Empty;

            // Send Map Message //
            if (ChatText.Length > 0 && sendMsg)
            {
                ClientTCP.MapMsg(ChatText);
            }

        }

    }
}
