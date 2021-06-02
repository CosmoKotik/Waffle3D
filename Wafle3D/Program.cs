using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafle3D.Main;

namespace Wafle3D
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WafleEngine game = new WafleEngine(800, 600, "Wafle demo"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60.0);
            }
        }
    }
}
