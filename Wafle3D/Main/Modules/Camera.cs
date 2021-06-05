using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace Wafle3D.Main.Modules
{
    public class Camera
    {
        public Matrix4 MoveCamera(Vector3 position)
        {
            Vector3 cameraTarget = Vector3.Zero;

            Vector3 up = Vector3.UnitY;

            Matrix4 view = Matrix4.LookAt(position, cameraTarget, up);

            if (position == Vector3.Zero)
                return Matrix4.Zero;

            return view;
        }
    }
}
