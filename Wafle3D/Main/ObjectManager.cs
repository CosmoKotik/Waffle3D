using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Unmanaged;
using Assimp.Configs;

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

        public void LoadModel()
        {
            AssimpContext context = new AssimpContext();


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
        
        public float[] LoadObjToIndices(string path)
        {
            List<float> indices = new List<float>();

            using (StreamReader streamR = new StreamReader(path))
            {
                string line;

                while ((line = streamR.ReadLine()) != null)
                {
                    string[] info = line.Split(' ');

                    if (info[0] == "f")
                    {
                        string[] indiceLine = line.Trim('f').Split(new char[] { '/', ' ' });

                        Console.WriteLine(float.Parse(indiceLine[1]) - 1);

                        indices.Add(float.Parse(indiceLine[1]) - 1);
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
