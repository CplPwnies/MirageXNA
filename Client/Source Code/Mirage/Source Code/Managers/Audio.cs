///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MirageXNA
{
    public class AudioManager
    {
        // Private Constructor
        private AudioManager() { }

        // Singleton Accessor
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AudioManager();

                return instance;
            }
        }

        // Data Members
        private Song BgmSong;
        //private int maxSongs = 1;
        private SoundEffect[] listEffects;
        private int maxSndFx = 1;

        // Music Methods
        public void PlaySong(ContentManager Content, int ID)
        {
            if (ID > 0)
            {
                BgmSong = (Content.Load<Song>("Music\\" + ID));
                MediaPlayer.Play(BgmSong);
            }
        }

        // Sound Effects Methods
        public void initSoundFx()
        {
            // Loop through folder scanning for textures to load
            string currentFolder = "Sounds\\";

            DirectoryInfo dir = new DirectoryInfo("Content\\" + currentFolder);
            while (File.Exists(dir.FullName + maxSndFx + ".xnb") == true)
            {
                Array.Resize<SoundEffect>(ref listEffects, maxSndFx + 1);
                maxSndFx++;
            }
            --maxSndFx;
        }

        public void UnloadEffect(int ID)
        {
            if (ID >= 0 && ID < listEffects.Count())
            {
                listEffects[ID] = null;
            }
        }
        public void PlayEffect(int ID)
        {
            if (ID > 0 && ID < listEffects.Count())
            {
                if (listEffects[ID] == null)
                    listEffects[ID] = GameContentManager.Instance.gameContent.Load<SoundEffect>("Sounds\\" + ID);

                listEffects[ID].Play(1.0f, 0, 0);
            }
        }
        public void ClearAllEffects()
        {
            for (int i = 0; i < listEffects.Count(); ++i)
            {
                listEffects[i] = null;
            }

            GC.Collect();
        }
    }
}