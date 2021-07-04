using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafle3D.Main;

namespace Wafle3D.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WafleEngine game = new WafleEngine(800, 600, "Wafle demo"))
            {
                game.Run(60.0);
                game.VSync = OpenTK.VSyncMode.Off;
            }
        }
    }
}
