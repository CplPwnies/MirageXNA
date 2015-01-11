using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MirageXNA.Core;

namespace MirageXNA.Globals
{
    public static class Commands
    {
        // Retrieve Player Name //
        public static string getPlayerName(int Index)
        { return Types.Players[Index].Name; }

        // Retrieve Player Access //
        public static byte getPlayerAccess(int Index)
        { return Types.Players[Index].Access; }

        // Retrieve Player Sprite //
        public static int getPlayerSprite(int Index)
        { return Types.Players[Index].Sprite; }

        // Retrieve Player Map //
        public static int getPlayerMap(int Index)
        { return Types.Players[Index].Map; }

        // Retrieve Player X //
        public static int getPlayerX(int Index)
        { return Types.Players[Index].X; }

        // Retrieve Player Y //
        public static int getPlayerY(int Index)
        { return Types.Players[Index].Y; }

        // Retrieve Player Level //
        public static int getPlayerLevel(int Index)
        { return Types.Players[Index].Level; }

        // Set Player Level
        public static bool setPlayerLevel(int Index, int Level)
        { 
            if (Level > Constant.MAX_LEVELS)
            {
                Types.Players[Index].Level = Constant.MAX_LEVELS;
                return false;
            }
            Types.Players[Index].Level = Level;
            return true;
        }

        // Retrieve Player Points //
        public static int getPlayerPoints(int Index)
        { return Types.Players[Index].Points; }

        // Set Player Level
        public static void setPlayerPoints(int Index, int Points)
        { Types.Players[Index].Points = Points; }

        public static int getPlayerStat(int Index, Enumerations.Stats Stat)
        { return Types.Players[Index].Stats[(int)Stat]; }

        public static int getPlayerVital(int Index, Enumerations.Vitals Vital)
        { return Types.Players[Index].Vitals[(int)Vital]; }

        public static void setPlayerVital(int Index, Enumerations.Vitals Vital, int Amount)
        { 
            Types.Players[Index].Vitals[(int)Vital] = Amount;

            if (getPlayerVital(Index, Vital) > getPlayerMaxVital(Index, Vital))
                Types.Players[Index].Vitals[(int)Vital] = getPlayerMaxVital(Index, Vital);

            if (getPlayerVital(Index, Vital) < 0)
                Types.Players[Index].Vitals[(int)Vital] = 0;
        }

        public static int getPlayerMaxVital(int Index, Enumerations.Vitals Vital)
        {
            int maxVital = 0;

            switch (Vital)
            {
                case Enumerations.Vitals.HP:
                    maxVital = (getPlayerLevel(Index) * 2) + (getPlayerStat(Index, Enumerations.Stats.Defence) * 2) + 50;
                    return maxVital;

                case Enumerations.Vitals.MP:
                    maxVital = (getPlayerLevel(Index) * 2) + (getPlayerStat(Index, Enumerations.Stats.Magic) * 2) + 50;
                    return maxVital;

                case Enumerations.Vitals.SP:
                    maxVital = (getPlayerLevel(Index) * 2) + (getPlayerStat(Index, Enumerations.Stats.Speed) * 2) + 50;
                    return maxVital;
            }

            return maxVital;
        }

        // Retrieve Player Exp
        public static int getPlayerExp(int Index)
        { return Types.Players[Index].Exp; }

        // Set Player Exp
        public static void setPlayerExp(int Index, int Amount)
        { Types.Players[Index].Exp = Amount; }

        // Retrieve Player Max Exp
        public static int getPlayerMaxExp(int Index)
        { return 100 + (((getPlayerLevel(Index) ^ 2) * 10) * 2); }

        public static void GiveExp(int Index, int Exp)
        {
            if (Types.Players[Index].Level != Constant.MAX_LEVELS)
                setPlayerExp(Index, getPlayerExp(Index) + Exp);
            else
                setPlayerExp(Index, 0);
            General.CheckPlayerLevelUp(Index);
        }
    }
}
