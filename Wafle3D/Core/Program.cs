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
            using (WafleEngine game = new WafleEngine(1280, 720, "Wafle demo", false))
            {
                double fps = 60;
                game.VSync = OpenTK.VSyncMode.Off;
                game.Run(fps, fps);
            }
        }
    }
}
