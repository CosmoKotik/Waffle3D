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

using Wafle3D;
using Wafle3D.Core.Modules;
using System.Reflection;

namespace Wafle3D.Core
{
    public class WafleEngine : GameWindow
    {
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        int uvBuffer;
        
        public ObjectManager ObjectManager;
        private Camera cam;

        private List<ModelMesh> _models = new List<ModelMesh>();

        Texture texture;
        Shader shader;

        private List<string> ScriptNames = new List<string>();
        public List<WafleBehaviour> Scripts = new List<WafleBehaviour>();

        public WafleEngine(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Starting");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            for (int i = 0; i < Scripts.Count; i++)
            {
                //Invoking the external script
                WafleBehaviour scriptObject = Scripts[i];

                //scriptObject.gameEngine = this;
                //Calling OnUpdate() method
                scriptObject.OnUpdate();
            }

            base.OnRenderFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Console.WriteLine(GL.GetString(All.Version));

            //Initializing object manager, texture and shaders
            ObjectManager = new ObjectManager();
            shader = new Shader(@"Shaders/shader.vert", @"Shaders/shader.frag");
            texture = new Texture();
            cam = new Camera();

            //Adding scripts
            ScriptNames.Add("Movement");

            for (int i = 0; i < ScriptNames.Count; i++)
            {
                //Invoking the external script
                Type scriptType = Type.GetType(ScriptNames[i]);
                ConstructorInfo scriptConstructor = scriptType.GetConstructor(Type.EmptyTypes);
                WafleBehaviour scriptObject = (WafleBehaviour)scriptConstructor.Invoke(new object[] { });

                Scripts.Add(scriptObject);

                //Setting the gameEngine
                scriptObject.gameEngine = this;
                //Calling OnLoad() method
                scriptObject.OnLoad();
            }


            //Enabling DepthTest
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            //Adding objects and displaying the id
            Console.WriteLine("id: " + CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), Matrix4.CreateTranslation(0.0f, -3.0f, -4.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0))));
            Console.WriteLine("id: " + CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Toad/Toad.obj"), Matrix4.CreateTranslation(100.0f, 0.0f, -533.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0))));
            Console.WriteLine("id: " + CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Goomba/Goomba.fbx"), Matrix4.CreateTranslation(-10.0f, 0.0f, -10.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90))));
            Console.WriteLine("id: " + CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Mario/Mario.fbx"), Matrix4.CreateTranslation(150.0f, 0.0f, -422.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180))));

            

            //Setting custom textures to the objects
            SetTexture(@"Models/gray.png", 0);
            SetTexture(@"Models/Mario64/Toad/Toad_grp.png", 1);
            SetTexture(@"Models/Mario64/Goomba/GoombaTex.png", 2);
            SetTexture(@"Models/Mario64/Mario/Mario64Body_alb.png", 3);

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            //Clearing all the buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteBuffer(uvBuffer);

            for (int i = 0; i < _models.Count; i++)
            {
                GL.DeleteBuffer(_models[i].vbo);
                GL.DeleteBuffer(_models[i].ebo);
            }

            shader.Dispose();
            base.OnUnload(e);
        }

        public int CreateObject(ModelMesh mesh, Matrix4 position, Matrix4 rotation)
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

            _models.Add(mesh);
            return mesh.id;
        }

        public void SetRotation(Matrix4 rotation, int id)
        {
            //Setting the rotation of an object from the outside
            _models[id].rotation = rotation;
        }

        public void SetTexture(string path, int id)
        {
            //Setting a custom texture of an object from the outside
            _models[id].diffusePath = path;
            texture.AddTexture(path, true, id);
        }

        public void SetPosition(Vector3 position, int id)
        {
            //Set the position of an object from the outside
            _models[id].position = Matrix4.CreateTranslation(position);
        }

        private void CreateVertexBuffer(ModelMesh mesh) 
        {
            texture.AddTexture(mesh.diffusePath);

            float[] vertices = mesh.vertices;
            int[] indices = mesh.indices;
            float[] texCoords = mesh.texCoords;

            GL.BindVertexArray(0);
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

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
            //Creating a mesh
            ModelMesh mesh = _models[id];

            //Loading texture by id
            texture.Use();
            texture.loadTexture(id);
            //texture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
            //Aplying the texture
            //shader.Use();

            //Creating a camera perspective view
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.01f, 10000.0f);

            //Binding the vertexes and the buffers, then clearing it
            GL.BindVertexArray(mesh.vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ebo);
            GL.DrawElements(All.Triangles, mesh.size, All.UnsignedInt, (IntPtr)0);
            GL.BindVertexArray(0);

            //Trasfering data(position, rotation) to the shader
            shader.SetMatrix4("rotation", rotation);
            shader.SetMatrix4("position", position);
            shader.SetMatrix4("scale", scale);
            shader.SetMatrix4("projection", projection);
        }

        float kx = 0, ky = 0, mx = 0, my = 0;
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            kx = Input.GetAxis("Keyboard X");
            ky = Input.GetAxis("Keyboard Y");
            my = Input.GetAxis("Mouse Y") * 0.1f;
            mx = Input.GetAxis("Mouse X") * 0.1f;

            cam.MoveCamera(new Vector3(kx, 0, ky));
            cam.UpdateCamera();
            cam.RotateCamera(new Vector3(mx, 0, my));
            cam.UpdateCamera();

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

                RenderObject(pos * cam.GetView(), rot, scale, id);
            }
            
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
