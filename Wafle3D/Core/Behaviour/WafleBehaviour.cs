using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wafle3D.Core;
using Wafle3D.Core.Modules.Lighting;
using static Wafle3D.Core.Modules.Lighting.Light;

namespace Wafle3D
{
    public class WafleBehaviour
    {
        public WafleEngine gameEngine;
        public GameObject gameObject;

        public string Name;

        public virtual void OnLoad() { }
        public virtual void OnUpdate() { }

        public void CreateObject(ModelMesh mesh, Vector3 position, Vector3 rotation, Vector3 scale, LightType type = LightType.nul, Light lightOptions = null)
        {
            gameEngine.CreateObject(mesh, position, rotation, scale, type, lightOptions);
        }

        public WafleBehaviour GetBehaviour()
        {
            return this;
        }
    }
}
