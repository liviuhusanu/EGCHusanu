using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGCHusanu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Window3D w1 = new Window3D())
            {
                w1.Run(30.0, 0.0);
            }    
        }
    }
}
