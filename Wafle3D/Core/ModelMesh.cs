using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using OpenTK;
using static Wafle3D.Core.Modules.Lighting.Light;

namespace Wafle3D.Core
{
    public class ModelMesh
    {
        public bool isLight = false;

        public float[] vertices = { 0 };
        public int[] indices = { 0 };
        public int size = 0;

        public float[] texCoords = { 0 };
        public Vector3 Normals;

        public string diffusePath;
        public int id;

        //Lighting stuff
        public int lightId;
        public int shininess = 2; // more shininess = more lag = pc strugling
        public float intensity = 1.0f;
        public float pointSize = 1.0f;
        public Vector3 color = Vector3.One;
        public LightType lightType = LightType.nul;

        public int vao;
        public int ebo;
        public int vbo;

        public Matrix4 position = Matrix4.Zero;
        public Matrix4 rotation = Matrix4.Zero;
        public Matrix4 scale;

        public List<GameObject> gameObjects;

        public PrimitiveType type;
    }
}
