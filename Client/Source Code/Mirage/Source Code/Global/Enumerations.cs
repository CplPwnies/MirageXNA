using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MirageXNA.Global
{
    public class Enumerations
    {
        // PC/NPC Statistics //
        public enum Stats
        {
            Strength = 0,
            Defence,
            Magic,
            Speed,

            // Keep below all
            statCount
        }

        // PC/NPC Vitals //
        public enum Vitals
        {
            HP = 0,
            MP,
            SP,

            // Keep below all
            vitalCount
        }
    }
}
