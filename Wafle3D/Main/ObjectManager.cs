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

        public int LoadModel(string path)
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
                //modelMesh.size = mesh.VertexCount;
                //modelMesh.type = mesh.PrimitiveType;
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

                for (int i = 0; i < mesh.TextureCoordinateChannels.Length; i++)
                {
                }
            }

            modelMesh.vertices = vertices.ToArray();
            modelMesh.indices = indices.ToArray();
            modelMesh.texCoords = texCoords.ToArray();

            _models.Add(modelMesh);
            return _models.Count - 1;
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

        public float[] LoadObjToVertex(string path)
        {
            List<float> v = new List<float>();
            List<float> vFinal = new List<float>();

            using (StreamReader streamR = new StreamReader(path))
            {
                string line;

                while ((line = streamR.ReadLine()) != null)
                {
                    string[] info = line.Split(' ');

                    if (info[0] == "v")
                    {
                        v.Add(float.Parse(info[1]));
                        v.Add(float.Parse(info[2]));
                        v.Add(float.Parse(info[3]));
                    }
                    else if (info[0] == "f")
                    {
                        string[] vertexDesc = line.Trim('f').Split(new char[] { '/', ' ' });
                    }
                }

                streamR.Close();
            }

            return v.ToArray();
        }
        
        public int[] LoadObjToIndices(string path)
        {
            List<int> indices = new List<int>();

            using (StreamReader streamR = new StreamReader(path))
            {
                string line;

                while ((line = streamR.ReadLine()) != null)
                {
                    string[] info = line.Split(' ');

                    if (info[0] == "f")
                    {
                        string[] indiceLine = line.Trim('f').Split(new char[] { '/', ' ' });

                        Console.WriteLine(int.Parse(indiceLine[1]) - 1);

                        indices.Add(int.Parse(indiceLine[1]) - 1);
                    }
                }

                streamR.Close();
            }

            return indices.ToArray();
        }

        public float[] LoadObjToTexcoords(string path)
        {
            List<float> texCoords = new List<float>();

            using (StreamReader streamR = new StreamReader(path))
            {
                string line;

                while ((line = streamR.ReadLine()) != null)
                {
                    string[] info = line.Split(' ');
                    if (info[0] == "vt")
                    {
                        texCoords.Add(float.Parse(info[1]));
                        texCoords.Add(float.Parse(info[2]));
                    }
                }

                streamR.Close();
            }

            return texCoords.ToArray();
        }
    }
}
