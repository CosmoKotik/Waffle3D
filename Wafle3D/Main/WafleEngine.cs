using Assimp;
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

        int uvBuffer;

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
        float[] texCoords = { };
        int[] indices = { };

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

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            texture1 = new Texture(@"Textures\stone_wall.png");
            //texture2 = new Texture(@"Textures\Grafiti.png");

            shader = new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag");
            texture1.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            //shader.SetInt("texture1", 0);
            //texture2.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture1);
            //shader.SetInt("texture2", 1);
            shader.Use();

            _objectManager.LoadModel(@"Models\Bite.fbx");

            CreateVertexBuffer();

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

        private void CreateVertexBuffer() 
        {
            vertices = _objectManager.GetVertices(0);
            indices = _objectManager.GetIndices(0);
            texCoords = _objectManager.GetTexCoords(0);

            GL.GenVertexArrays(1, out VertexArrayObject);

            GL.BindVertexArray(VertexArrayObject);

            Console.WriteLine(GL.GetError());

            GL.GenBuffers(1, out VertexBufferObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            Console.WriteLine(GL.GetError());

            GL.GenBuffers(1, out ElementBufferObject);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            Console.WriteLine(GL.GetError());

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            Console.WriteLine(GL.GetError());

            GL.GenBuffers(1, out uvBuffer);

            Console.WriteLine(texCoords.Length);

            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);

            Console.WriteLine(GL.GetError());

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            Console.WriteLine(GL.GetError());

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
            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.DrawElements(All.Triangles, indices.Length, All.UnsignedInt, (IntPtr)0);
            //GL.DrawArrays(All.Triangles, 0, indices.Length);
            //GL.BindVertexArray(0);

            //shader.SetMatrix4("transform", trans);

            shader.SetMatrix4("rotation", rotation);
            Console.WriteLine(GL.GetError());
            shader.SetMatrix4("position", position);
            Console.WriteLine(GL.GetError());
            shader.SetMatrix4("scale", scale);
            Console.WriteLine(GL.GetError());
            shader.SetMatrix4("projection", projection);
            Console.WriteLine(GL.GetError());


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

            //CreateVertexBuffer();

            RenderObject(Matrix4.CreateTranslation(0.0f, 0.0f, -6.0f), rotation, Matrix4.CreateScale(0.5f, 0.5f, 0.5f), 0);
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
