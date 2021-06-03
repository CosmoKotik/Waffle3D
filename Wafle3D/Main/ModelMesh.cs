using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;

namespace Wafle3D.Main
{
    public class ModelMesh
    {
        public float[] vertices;
        public int[] indices;
        public int size;

        public float[] texCoords;
        public float[] normal;
        
        public PrimitiveType type;
    }
}
