///////////////////////////////////////////////////
////// MIRAGE XNA - DEVELOPED BY MARK MORRIS //////
//////          WWW.RPGCREATION.COM          //////
///////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MirageXNA.Engine
{
    static class Audio
    {
        private static SoundEffect _mySoundeffect;

        public static SoundEffect _mHover;
        public static SoundEffect _mClick;

        public static void PlayWav(SoundEffect Wav)
        {
            //_mySoundeffect = Wav;
            //_mySoundeffect.Play();
        }

    }
}
