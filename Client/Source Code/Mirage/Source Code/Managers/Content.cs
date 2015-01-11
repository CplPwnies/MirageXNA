using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using MirageXNA.Global;

namespace MirageXNA
{
    class GameContentManager
    {
        // XNA Game Content //
        public ContentManager gameContent;

        // Private Constructor
        private GameContentManager() { }

        // Singleton Accessor
        private static GameContentManager instance;
        public static GameContentManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameContentManager();

                return instance;
            }
        }

        // Init
        public void initContentManager(GameServiceContainer Services)
        {
            gameContent = new ContentManager(Services, "Content");
        }

        // Load Gfx Content
        public void loadTexture(int TextureNum)
        {
            Types.Texture2D_struct[] TEXTURE = GfxManager.Instance.TEXTURE;

            if (TextureNum != Static.CurrentTexture)
            {
                if (TextureNum > Static.mTextureNum) { TextureNum = Static.mTextureNum; }
                if (TextureNum < 0) { TextureNum = 0; }

                // Check if texture is loaded //
                if (TextureNum != -1)
                {
                    if (!TEXTURE[TextureNum].Loaded)
                    {
                        TEXTURE[TextureNum].Texture = GameContentManager.Instance.gameContent.Load<Texture2D>(TEXTURE[TextureNum].Path);
                        TEXTURE[TextureNum].Height = TEXTURE[TextureNum].Width;
                        TEXTURE[TextureNum].Width = TEXTURE[TextureNum].Height;
                        TEXTURE[TextureNum].UnloadTimer = System.Environment.TickCount;
                        TEXTURE[TextureNum].Loaded = true;
                    }
                }
                Static.CurrentTexture = TextureNum;
            }
        }

        // Load Audio Content
        public void PlayMusic(int ID)
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
            AudioManager.Instance.PlaySong(gameContent, ID);
        }
        public void StopMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = false;
        }

        // Load Sound Effects

    }
}
