using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafle3D.Core.Modules
{
    public class Mathf
    {
        static float _f;
        static float _start;
        static float _end;
        public static float Lerp(float start, float end, float interpolation)
        {
            _start = start;
            _end = end;

            _f = start;
            for (int i = (int)_f; i < end; i++)
            {
                
                _f += interpolation;
                return _f;
            }
            return 0;
        }

        public static float Normalize(float min, float max)
        {
            return min - (max / min);
        }

    }
}
