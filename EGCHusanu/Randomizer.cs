using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGCHusanu
{
    internal class Randomizer
    {
        private Random r;

        public Randomizer()
        {
            r = new Random();
        }

        public int GetRandomOffsetPositive(int maxval)
        {
            int genInteger = r.Next(0, maxval);

            return genInteger;
        }

        public Color GetRandomColor()
        {
            int genA = r.Next(0, 255);
            int genR = r.Next(0, 255);
            int genG = r.Next(0, 255);
            int genB = r.Next(0, 255);


            Color c = Color.FromArgb(genA, genG, genB, genB);
            
            return c;
        }
    }
}
