using System;
using MirageXNA.Engine;

namespace MirageXNA
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Mirage MirageXNA = new Mirage())
            {
                MirageXNA.Run();
            }
        }
    }
#endif
}

