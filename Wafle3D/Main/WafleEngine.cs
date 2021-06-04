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

        float[] vertices;
        int[] indices;
        float[] texCoords;

        private ObjectManager _objectManager;
        private List<float[]> _objectVertices = new List<float[]>();

        private List<ModelMesh> _models = new List<ModelMesh>();

        Texture texture;

        public WafleEngine(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Starting");
        }

        Shader shader;
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

            //texture1 = new Texture(@"Models\Mario64\Toad_grp.png");
            //texture2 = new Texture(@"Textures\Grafiti.png");

            shader = new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag");
            texture = new Texture();
            //texture1.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            //shader.SetInt("texture1", 0);
            //texture2.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture1);
            //shader.SetInt("texture2", 1);

            //shader.Use();

            //Console.WriteLine("Diek: " + _objectManager.LoadModel(@"Models\Bite.fbx"));
            //Console.WriteLine("Cube: " + _objectManager.LoadModel(@"Models\Cube.obj"));

            //Texture texture = new Texture(@"Models\Mario64\Toad\Toad_grp.png");
            //texture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            ////shader.SetInt("texture1", 0);
            //shader.Use();
            Console.WriteLine("id: " + AddObject(_objectManager.LoadModel(@"Models\Cube.fbx"), Matrix4.CreateTranslation(0.0f, -3.0f, -4.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0))));
            Console.WriteLine("id: " + AddObject(_objectManager.LoadModel(@"Models\Mario64\Toad\Toad.obj"), Matrix4.CreateTranslation(-100.0f, 0.0f, -533.0f), Matrix4.CreateRotationX(-90)));
            Console.WriteLine("id: " + AddObject(_objectManager.LoadModel(@"Models\Mario64\Goomba\Goomba.fbx"), Matrix4.CreateTranslation(0.0f, 0.0f, -10.0f), Matrix4.CreateRotationX(-90)));
            Console.WriteLine("id: " + AddObject(_objectManager.LoadModel(@"Models\Mario64\Mario\Mario.fbx"), Matrix4.CreateTranslation(150.0f, 0.0f, -422.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180))));

            SetTexture(@"Models\gray.png", 0);
            SetTexture(@"Models\Mario64\Toad\Toad_grp.png", 1);
            SetTexture(@"Models\Mario64\Goomba\GoombaTex.png", 2);
            SetTexture(@"Models\Mario64\Mario\Mario64Body_alb.png", 3);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);

            shader.Dispose();
            base.OnUnload(e);
        }

        public int AddObject(ModelMesh mesh, Matrix4 position, Matrix4 rotation)
        {
            GL.GenVertexArrays(1, out VertexArrayObject);
            GL.GenBuffers(1, out VertexBufferObject);
            GL.GenBuffers(1, out ElementBufferObject);

            mesh.vao = VertexArrayObject;
            mesh.ebo = ElementBufferObject;
            mesh.vbo = VertexBufferObject;

            mesh.position = position;
            mesh.rotation = rotation;
            mesh.scale = Matrix4.CreateScale(1f, 1f, 1f);

            CreateVertexBuffer(mesh);

            Console.WriteLine(mesh.scale);

            _models.Add(mesh);
            return mesh.id;
        }

        public void SetRotation(Matrix4 rotation, int id)
        {
            _models[id].rotation = rotation;
        }

        public void SetTexture(string path, int id)
        {
            _models[id].diffusePath = path;
            texture.AddTexture(path, true, id);
        }

        float angleX = 0;
        float angleY = 0;
        float angleZ = 0;

        private void CreateVertexBuffer(ModelMesh mesh) 
        {
            //ModelMesh mesh = _models[id];
            Console.WriteLine(mesh.diffusePath);
            texture.AddTexture(mesh.diffusePath);

            vertices = mesh.vertices;
            indices = mesh.indices;
            texCoords = mesh.texCoords;

            //GL.GenVertexArrays(1, out VertexArrayObject);
            GL.BindVertexArray(0);
            GL.BindVertexArray(VertexArrayObject);

            //GL.GenBuffers(1, out VertexBufferObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //GL.GenBuffers(1, out ElementBufferObject);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.GenBuffers(1, out uvBuffer);

            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

        }

        private void RenderObject(Matrix4 position, Matrix4 rotation, Matrix4 scale, int id)
        {
            ModelMesh mesh = _models[id];

            texture.loadTexture(id);
            texture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            shader.Use();

            /*Matrix4 RX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angleX));
            Matrix4 RY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angleY));
            Matrix4 RZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angleZ));

            Matrix4 rotation = RX * RY * RZ;*/

            //Matrix4 position = Matrix4.CreateTranslation(0.0f, 0.0f, -2.0f);
            //CreateVertexBuffer(mesh);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80f), Width / (float)Height, 0.01f, 10000.0f);

            //Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);

            //Matrix4 trans = scale * rotation;

            //GL.BindVertexArray(VertexArrayObject);
            Console.WriteLine(mesh.id + ":  " + mesh.position);
            GL.BindVertexArray(mesh.vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ebo);
            GL.DrawElements(All.Triangles, mesh.size, All.UnsignedInt, (IntPtr)0);
            //GL.DrawArrays(All.Triangles, 0, indices.Length);
            GL.BindVertexArray(0);

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

            //SetRotation(rotation, 3);

            //CreateVertexBuffer();

            int m_id = 0;
            for (int i = 0; i < _models.Count; i++)
            {
                m_id = i + 1;

                if (m_id == _models.Count)
                    m_id = 0;

                Matrix4 pos = _models[m_id].position;
                Matrix4 rot = _models[m_id].rotation;
                Matrix4 scale = _models[m_id].scale;

                int id = _models[i].id;

                RenderObject(pos, rot, scale, id);
            }
            //RenderObject(Matrix4.CreateTranslation(-3.0f, -2.0f, -5.0f), rotation, Matrix4.CreateScale(0.1f, 0.1f, 0.1f), 1);
            //RenderObject(Matrix4.CreateTranslation(3.0f, -2.0f, -662.0f), rotation, Matrix4.CreateScale(1f, 1f, 1f), 2);

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
