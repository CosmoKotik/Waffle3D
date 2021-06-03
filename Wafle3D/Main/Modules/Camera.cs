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
        public void MoveCamera(Vector3 position, Vector3 direction)
        {
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
            Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
             new Vector3(0.0f, 0.0f, 0.0f),
             new Vector3(0.0f, 1.0f, 0.0f));


            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraDirection = Vector3.Normalize(position - cameraTarget);

            view = Matrix4.LookAt(position, position + direction, up);
        }
    }
}
