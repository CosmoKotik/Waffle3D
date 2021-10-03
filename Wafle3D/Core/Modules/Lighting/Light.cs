using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafle3D.Core.Modules.Lighting
{
    public class Light
    {
        public enum LightType { nul = 0, Directional = 1, Point = 2, Spot = 3 };

        public int Snininess = 2;
        public float Intensity = 1.0f;
        public Vector3 Color = Vector3.One;

        public static Light Advanced(Vector3 color, float intens = 1f, int shininess = 2)
        {
            Light l = new Light();

            l.Color = color;
            l.Intensity = intens;
            l.Snininess = shininess;

            return l;
        }
    }
}
