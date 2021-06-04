using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;

namespace Wafle3D.Main
{
    public class ObjectManager
    {
        public float[] cubeVertices = {
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        public enum ObjectType { Cube = 0, Sphere = 1, Custom = 2 }

        private List<ModelMesh> _models = new List<ModelMesh>();

        public ModelMesh LoadModel(string path)
        {
            AssimpContext context = new AssimpContext();
            const PostProcessSteps flags = PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.SortByPrimitiveType |
                                           PostProcessSteps.CalculateTangentSpace | PostProcessSteps.GenerateNormals |
                                           PostProcessSteps.Triangulate | PostProcessSteps.FixInFacingNormals |
                                           PostProcessSteps.JoinIdenticalVertices | PostProcessSteps.ValidateDataStructure |
                                           PostProcessSteps.MakeLeftHanded | PostProcessSteps.FlipWindingOrder |
                                           PostProcessSteps.OptimizeGraph | PostProcessSteps.OptimizeMeshes;

            Scene scene = context.ImportFile(path, flags);

            ModelMesh modelMesh = new ModelMesh();

            List<float> vertices = new List<float>();
            List<int> indices = new List<int>();
            List<float> texCoords = new List<float>();

            foreach (Mesh mesh in scene.Meshes)
            {
                for (int i = 0; i < mesh.VertexCount; i++)
                {
                    vertices.Add(mesh.Vertices[i].X);
                    vertices.Add(mesh.Vertices[i].Y);
                    vertices.Add(mesh.Vertices[i].Z);

                    texCoords.Add(mesh.TextureCoordinateChannels[0][i].X);
                    texCoords.Add(mesh.TextureCoordinateChannels[0][i].Y);
                    texCoords.Add(mesh.TextureCoordinateChannels[0][i].Z);
                }
                
                for (int i = 0; i < mesh.FaceCount; i++)
                {
                    indices.Add(mesh.Faces[i].Indices[0]);
                    indices.Add(mesh.Faces[i].Indices[1]);
                    indices.Add(mesh.Faces[i].Indices[2]);
                }
            }

            modelMesh.vertices = vertices.ToArray();
            modelMesh.indices = indices.ToArray();
            modelMesh.texCoords = texCoords.ToArray();

            modelMesh.size = indices.Count;

            if (scene.Materials[0].TextureDiffuse.FilePath != null)
            {
                string diffusePath = Path.GetDirectoryName(path) + @"\" + Path.GetFileName(scene.Materials[0].TextureDiffuse.FilePath);

                if (!Directory.Exists(diffusePath))
                    diffusePath = null;

                modelMesh.diffusePath = diffusePath;
            }

            modelMesh.id = _models.Count;

            _models.Add(modelMesh);
            return _models[_models.Count - 1];
        }

        public float[] GetVertices(int id)
        {
            return _models[id].vertices;
        }
        public int[] GetIndices(int id)
        {
            return _models[id].indices;
        }
        public float[] GetTexCoords(int id)
        {
            return _models[id].texCoords;
        }
    }
}
