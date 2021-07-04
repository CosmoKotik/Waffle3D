using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wafle3D.Core;

namespace Wafle3D
{
    public class WafleBehaviour
    {
        public WafleEngine gameEngine;
        public GameObject gameObject;

        public string Name;

        public virtual void OnLoad() { }
        public virtual void OnUpdate() { }

        public void CreateObject(ModelMesh mesh, Vector3 position, Vector3 rotation)
        {
            Matrix4 RX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            Matrix4 RY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            Matrix4 RZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));

            Matrix4 rot = RX * RY * RZ;
            
            gameEngine.CreateObject(mesh, Matrix4.CreateTranslation(position), rot);
        }

        public WafleBehaviour GetBehaviour()
        {
            return this;
        }
    }
}
