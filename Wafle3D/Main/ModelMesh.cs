using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using OpenTK;

namespace Wafle3D.Main
{
    public class ModelMesh
    {
        public float[] vertices;
        public int[] indices;
        public int size;

        public float[] texCoords;
        public float[] normal;

        public string diffusePath;
        public int id;

        public int vao;
        public int ebo;
        public int vbo;

        public Matrix4 position;
        public Matrix4 rotation;
        public Matrix4 scale;

        public PrimitiveType type;
    }
}
