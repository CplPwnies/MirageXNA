///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using MirageXNA.Engine;
using MirageXNA.Global;

namespace MirageXNA
{
    public class GfxManager
    {
        // XNA Textures & Fonts
        public Types.Texture2D_struct[] TEXTURE;
        public SpriteFont[] FONT_TEXTURE;

        // XNA Rectangle
        private Rectangle _dRect = new Rectangle(0, 0, 0, 0);
        private Rectangle _sRect = new Rectangle(0, 0, 0, 0);

        // Private Constructor
        private GfxManager(){}

        // Singleton Accessor
        static GfxManager instance;
        public static GfxManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GfxManager();
                return instance;
            }
        }

        ////////////////////
        // TEXTURE ENGINE //
        ////////////////////
        public void initTextures()
        {
            TEXTURE = new Types.Texture2D_struct[1];
            ContentManager gameContent = GameContentManager.Instance.gameContent;

            // ######################
            // ## Tileset Textures ##
            // ######################
            // Loop through folder scanning for textures to load
            string currentFolder = "Tiles\\";

            DirectoryInfo dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + Static.Tilesets_Count + ".xnb") == true)
            {
                Array.Resize<int>(ref Static.Tex_Tilesets, Static.Tilesets_Count + 1);
                Static.Tex_Tilesets[Static.Tilesets_Count] = setTexturePath(currentFolder + Static.Tilesets_Count);
                Static.Tilesets_Count++;
            }
            // decrement int due to 1 too many calls to the increment
            --Static.Tilesets_Count;

            // #####################
            // ## Sprite Textures ##
            // #####################
            // Loop through folder scanning for textures to load
            currentFolder = "Sprites\\";
            dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + Static.Sprites_Count + ".xnb") == true)
            {
                Array.Resize<int>(ref Static.Tex_Sprites, Static.Sprites_Count + 1);
                Static.Tex_Sprites[Static.Sprites_Count] = setTexturePath(currentFolder + Static.Sprites_Count);
                Static.Sprites_Count++;
            }
            // decrement int due to 1 too many calls to the increment
            --Static.Sprites_Count;

            // ####################
            // ## Faces Textures ##
            // ####################
            // Loop through folder scanning for textures to load
            currentFolder = "Faces\\";
            dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + Static.Faces_Count + ".xnb") == true)
            {
                Array.Resize<int>(ref Static.Tex_Faces, Static.Faces_Count + 1);
                Static.Tex_Faces[Static.Faces_Count] = setTexturePath(currentFolder + Static.Faces_Count);
                Static.Faces_Count++;
            }
            // decrement int due to 1 too many calls to the increment
            --Static.Faces_Count;

            // ##################
            // ## Gui Textures ##
            // ##################

            // Loop through folder scanning for textures to load
            currentFolder = "GUI\\";
            dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + Static.Gui_Count + ".xnb") == true)
            {
                Array.Resize<int>(ref Static.Tex_Gui, Static.Gui_Count + 1);
                Static.Tex_Gui[Static.Gui_Count] = setTexturePath(currentFolder + Static.Gui_Count);
                Static.Gui_Count++;
            }
            // decrement int due to 1 too many calls to the increment
            --Static.Gui_Count;

            // #####################
            // ## Button Textures ##
            // #####################

            // Loop through folder scanning for textures to load
            currentFolder = "Buttons\\";
            dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + Static.Buttons_Count + ".xnb") == true)
            {
                Array.Resize<int>(ref Static.Tex_Buttons, Static.Buttons_Count + 1);
                Static.Tex_Buttons[Static.Buttons_Count] = setTexturePath(currentFolder + Static.Buttons_Count);
                Static.Buttons_Count++;
            }
            // decrement int due to 1 too many calls to the increment
            --Static.Buttons_Count;

            // ###########################
            // ## Button Click Textures ##
            // ###########################

            // Loop through folder scanning for textures to load
            currentFolder = "Buttons\\";
            dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + Static.Buttons_C_Count + "_C.xnb") == true)
            {
                Array.Resize<int>(ref Static.Tex_Buttons_C, Static.Buttons_C_Count + 1);
                Static.Tex_Buttons_C[Static.Buttons_C_Count] = setTexturePath(currentFolder + Static.Buttons_C_Count + "_C");
                Static.Buttons_C_Count++;
            }
            // decrement int due to 1 too many calls to the increment
            --Static.Buttons_C_Count;

            // ###########################
            // ## Button Hover Textures ##
            // ###########################

            // Loop through folder scanning for textures to load
            currentFolder = "Buttons\\";
            dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + Static.Buttons_H_Count + "_H.xnb") == true)
            {
                Array.Resize<int>(ref Static.Tex_Buttons_H, Static.Buttons_H_Count + 1);
                Static.Tex_Buttons_H[Static.Buttons_H_Count] = setTexturePath(currentFolder + Static.Buttons_H_Count + "_H");
                Static.Buttons_H_Count++;
            }
            // decrement int due to 1 too many calls to the increment
            --Static.Buttons_H_Count;

            // ###########
            // ## FONTS ##
            // ###########

            // Load Fonts //
            FONT_TEXTURE = new SpriteFont[3];
            FONT_TEXTURE[0] = gameContent.Load<SpriteFont>("Fonts\\1");
            FONT_TEXTURE[1] = gameContent.Load<SpriteFont>("Fonts\\2");
            FONT_TEXTURE[2] = gameContent.Load<SpriteFont>("Fonts\\3");
        }
        private int setTexturePath(string path)
        {
            Static.mTextureNum = Static.mTextureNum + 1;
            Array.Resize<Types.Texture2D_struct>(ref TEXTURE, Static.mTextureNum + 1);
            TEXTURE[Static.mTextureNum].Path = path;
            TEXTURE[Static.mTextureNum].Loaded = false;
            return Static.mTextureNum;
        }

        ////////////////////
        // RENDER TEXTURE //
        ////////////////////
        public void drawTexture(int TextureNum, int X, int Y, int H, int W)
        {
            if (TextureNum == 0) return;

            // Set New Positions //
            _dRect = new Rectangle(X, Y, W, H);
            _sRect = new Rectangle(0, 0, W, H);

            GameContentManager.Instance.loadTexture(TextureNum);
            Mirage.spriteBatch.Draw(TEXTURE[TextureNum].Texture, _dRect, _sRect, Color.White);
            TEXTURE[TextureNum].UnloadTimer = System.Environment.TickCount;
        }
        public void drawTexture(int TextureNum, int X, int Y, int H, int W, Color Colour)
        {
            // Set New Positions //
            _dRect = new Rectangle(X, Y, W, H);
            _sRect = new Rectangle(0, 0, W, H);

            // Faster Here //
            if (TextureNum == 0)
                return;

            // Check if we need to load texture //
            GameContentManager.Instance.loadTexture(TextureNum);

            // Draw //
            Mirage.spriteBatch.Draw(TEXTURE[TextureNum].Texture,
                _dRect,
                _sRect,
                Colour);

            // Set Timer //
            TEXTURE[TextureNum].UnloadTimer = System.Environment.TickCount;
        }
        public void drawTexture(int TextureNum, int X, int Y, int H, int W, int sX, int sY, int sH, int sW, Color Colour)
        {
            // Set New Positions //
            _dRect = new Rectangle(X, Y, W, H);
            _sRect = new Rectangle(sX, sY, sW, sH);

            // Faster Here //
            if (TextureNum == 0)
                return;

            // Check if we need to load texture //
            GameContentManager.Instance.loadTexture(TextureNum);

            // Draw //
            Mirage.spriteBatch.Draw(TEXTURE[TextureNum].Texture,
                _dRect,
                _sRect,
                Colour);

            // Set Timer //
            TEXTURE[TextureNum].UnloadTimer = System.Environment.TickCount;
        }
        public void drawTexture(int TextureNum, Rectangle _dRect, Rectangle _sRect)
        {
            if (TextureNum == 0) return;
            GameContentManager.Instance.loadTexture(TextureNum);
            Mirage.spriteBatch.Draw(TEXTURE[TextureNum].Texture, _dRect, _sRect, Color.White);
            TEXTURE[TextureNum].UnloadTimer = System.Environment.TickCount;
        }
        public void drawTexture(int TextureNum, int X, int Y, int H, int W, Rectangle _sRect)
        {
            if (TextureNum == 0) return;

            // Set New Positions //
            _dRect = new Rectangle(X, Y, W, H);

            GameContentManager.Instance.loadTexture(TextureNum);
            Mirage.spriteBatch.Draw(TEXTURE[TextureNum].Texture, _dRect, _sRect, Color.White);
            TEXTURE[TextureNum].UnloadTimer = System.Environment.TickCount;
        }

        /////////////////
        // RENDER TEXT //
        /////////////////
        public void drawText(int fontNum, string text, Vector2 Pos, Color Colour)
        {
            Mirage.spriteBatch.DrawString(FONT_TEXTURE[fontNum], text, Pos, Colour);
        }
    }
}
