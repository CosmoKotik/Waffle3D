using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Input;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Wafle3D.Main.ObjectManager;

namespace Wafle3D.Main
{
    public class WafleEngine : GameWindow
    {
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;

        int mouseX;
        int mouseY;

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

        float speed = 3;

        int pgmID;

        float[] vertices = { };
        float[] verticeTexture = { };
        float[] indices = { };

        float[] customVertices = { };
        float[] customTexCoords = { };

        private ObjectManager _objectManager;
        private List<float[]> _objectVertices = new List<float[]>();

        public WafleEngine(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Starting");
        }

        Shader shader;

        Texture texture1;
        Texture texture2;

        private void InitProgram()
        {
            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnRenderFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            _objectManager = new ObjectManager();

            vertices = _objectManager.LoadObjToVertex(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Models\Cube.obj");
            verticeTexture = _objectManager.LoadObjToTexcoords(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Models\Cube.obj");
            indices = _objectManager.LoadObjToIndices(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Models\Cube.obj");

            int x = 0;

            int vCount = 0;
            int vtCount = 0;
            int length = (vertices.Length + verticeTexture.Length);

            List<float> cv = new List<float>();

            for (int i = 0; i < length; i++)
            {
                //Console.WriteLine(i + "  " + x);

                if (x <= 2)
                {
                    if (vCount < vertices.Length)
                        cv.Add(vertices[vCount]);
                    else
                        cv.Add(0);
                    vCount++;
                    //Console.Write(v[i] + ", ");
                }
                else
                {
                    if (vtCount < verticeTexture.Length)
                        cv.Add(verticeTexture[vtCount]);
                    else
                        cv.Add(0);
                    vtCount++;
                    //Console.Write(vt[i / 4] + ", ");
                }

                //Console.Write(cv[i] + ", ");

                x++;

                if (x == 5)
                {
                    x = 0;
                    //onsole.WriteLine(" ");
                }
            }

            Console.WriteLine("cube: " + cubeVertices.Length + "  copy: " + cv.Count);
            /*for (int i = 0; i < cv.Count; i++)
            {
                if (cubeVertices[i] == cv[i])
                    Console.WriteLine("Good: " + i);
                else
                    Console.WriteLine("Bad: " + i);
            }*/

            customVertices = cv.ToArray();

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            GL.GenBuffers(1, out ElementBufferObject);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            texture1 = new Texture(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Textures\stone_wall.png");
            texture2 = new Texture(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Textures\Grafiti.png");

            shader = new Shader(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.vert", @"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.frag");
            texture1.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            shader.SetInt("texture1", 0);
            //texture2.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture1);
            shader.SetInt("texture2", 1);
            shader.Use();

            CreateVertexBuffer(0);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);

            shader.Dispose();
            base.OnUnload(e);
        }

        public void AddObject(ObjectType type, string objPath, int id)
        {
            switch (type)
            {
                case ObjectType.Cube:
                    _objectVertices[id] = _objectManager.cubeVertices;
                    break;
                case ObjectType.Custom:

                    break;
            }
        }


        float angleX = 0;
        float angleY = 0;
        float angleZ = 0;

        float[] vertices2 = {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left 
        };

        int[] indices2 = {  // note that we start from 0!
            0, 3, 7,   // first triangle
            5, 1, 5    // second triangle
        };

        private void CreateVertexBuffer(int id) 
        {
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices2.Length * sizeof(float), indices2, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //GL.BufferData(BufferTarget.ArrayBuffer, vt.Length * sizeof(float), vt, BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

        }

        [Obsolete]
        private void RenderObject(Matrix4 position, Matrix4 rotation, Matrix4 scale, int id)
        {
            /*Matrix4 RX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angleX));
            Matrix4 RY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angleY));
            Matrix4 RZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angleZ));

            Matrix4 rotation = RX * RY * RZ;*/

            //Matrix4 position = Matrix4.CreateTranslation(0.0f, 0.0f, -2.0f);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.01f, 10000.0f);

            //Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);

            //Matrix4 trans = scale * rotation;

            //GL.BindVertexArray(VertexArrayObject);
            //shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.DrawElements(All.Triangles, vertices.Length, All.UnsignedInt, (IntPtr)0);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            //GL.BindVertexArray(0);

            //shader.SetMatrix4("transform", trans);

            shader.SetMatrix4("rotation", rotation);
            shader.SetMatrix4("position", position);
            shader.SetMatrix4("scale", scale);
            shader.SetMatrix4("projection", projection);

            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //t.Use();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 RX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(mouseY));
            Matrix4 RY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(mouseX));
            Matrix4 RZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0));

            Matrix4 rotation = RX * RY * RZ;

            RenderObject(Matrix4.CreateTranslation(0.0f, 0.0f, -4.0f), rotation, Matrix4.CreateScale(0.5f, 0.5f, 0.5f), 0);
            //RenderObject(Matrix4.CreateTranslation(-1.0f, 0.0f, -3.0f), rotation, Matrix4.CreateScale(0.5f, 0.5f, 0.5f), 0);
            //RenderObject(Matrix4.CreateTranslation(0.0f, -1.0f, -3.0f), rotation, Matrix4.CreateScale(0.5f, 1f, 0.5f), 0);
            //RenderObject(Matrix4.CreateTranslation(0.0f, 1.0f, -5.0f), rotation, Matrix4.CreateScale(0.5f, 0.5f, 0.5f), 0);
            //RenderObject(Matrix4.CreateTranslation(0.0f, 0.0f, -5.0f), rotation, Matrix4.CreateScale(0.5f, 0.5f, 0.5f), 0);

            angleX = angleX + 2;
            angleY = angleY + 2;
            angleZ = angleZ + 2;

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
    }
}
