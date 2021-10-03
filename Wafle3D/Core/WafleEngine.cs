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
using static Wafle3D.Core.Modules.Lighting.Light;
using Wafle3D.Core.Modules.Lighting;

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
        private int _lightCount = 0;
        private int _lightPointCount = 0;

        Texture texture;
        Shader shader;
        Shader lightShader;
        Matrix4 projection;

        private List<string> ScriptNames = new List<string>();
        public List<WafleBehaviour> Scripts = new List<WafleBehaviour>();

        public WafleEngine(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 4, 1, GraphicsContextFlags.ForwardCompatible)
        {
            Console.WriteLine("Starting");
            
        }

        public WafleEngine()
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
            lightShader = new Shader(@"Shaders/light.vert", @"Shaders/light.frag");
            texture = new Texture();
            cam = new Camera();

            //Creating a camera perspective view
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.01f, 10000.0f);

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

            //Enabling lightning
            //GL.Enable(EnableCap.Lighting);

            //Adding objects and displaying the id
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), Matrix4.CreateTranslation(0.0f, -3.0f, -4.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)));
            CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Toad/Toad.obj"), Matrix4.CreateTranslation(100.0f, 0.0f, -533.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)));
            CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Goomba/Goomba.fbx"), Matrix4.CreateTranslation(-10.0f, 0.0f, 10.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90)));
            CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Mario/Mario.fbx"), Matrix4.CreateTranslation(150.0f, 0.0f, -422.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180)));
            CreateObject(ObjectManager.LoadModel(@"Models/Random/gun.fbx"), Matrix4.CreateTranslation(0.0f, 0.0f, -0.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180)));
            CreateObject(ObjectManager.LoadModel(@"Models/Random/Spaceshit.fbx"), Matrix4.CreateTranslation(10.0f, 0.0f, -0.0f), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180)));

            //Light point
            //CreateObject(new ModelMesh(), Matrix4.CreateTranslation(0, 5, 2), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)), LightType.Point);
            //CreateObject(new ModelMesh(), Matrix4.CreateTranslation(0, -5, 2), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)), LightType.Directional);
            CreateObject(new ModelMesh(), Matrix4.CreateTranslation(0, -5, 2), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)), LightType.Point, Light.Advanced(Vector3.One, 2, 256));

            

            //Setting custom textures to the objects
            //SetTexture(@"Models/gray.png", 0);
            SetTexture(@"Models/Mario64/Toad/Toad_grp.png", 1);
            SetTexture(@"Models/Mario64/Goomba/GoombaTex.png", 2);
            SetTexture(@"Models/Mario64/Mario/Mario64Body_alb.png", 3);


            /*for (int i = 0; i < 1000; i++)
            {
                Random rnd = new Random();

                CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), Matrix4.CreateTranslation((float)rnd.NextDouble() * rnd.Next(1, 50), (float)rnd.NextDouble() * rnd.Next(1, 50), (float)rnd.NextDouble() * rnd.Next(1, 50)), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rnd.Next(0, 360))));
                //SetTexture(@"Models/Mario64/Mario/Mario64Body_alb.png", 3);
            }*/

            //GL.BindVertexArray(0);

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
            lightShader.Dispose();
            base.OnUnload(e);
        }

        public int CreateObject(ModelMesh mesh, Matrix4 position, Matrix4 rotation, LightType light = LightType.nul, Light lightOptions = null)
        {
            GL.GenVertexArrays(1, out VertexArrayObject);
            GL.GenBuffers(1, out VertexBufferObject);
            GL.GenBuffers(1, out ElementBufferObject);


            mesh.position = position;
            mesh.rotation = rotation;
            mesh.scale = Matrix4.CreateScale(1f, 1f, 1f);
            mesh.id = _models.Count;

            if (light != LightType.nul)
            {
                mesh.isLight = true;
                mesh.lightId = _lightCount;
                mesh.lightType = light;

                if (lightOptions != null)
                {
                    mesh.intensity = lightOptions.Intensity;
                    mesh.shininess = lightOptions.Snininess;
                    mesh.color = lightOptions.Color;
                }

                switch (light)
                {
                    case LightType.Point:
                        _lightPointCount++;
                        break;
                }

                _lightCount++;
            }

            mesh.vao = VertexArrayObject;
            mesh.ebo = ElementBufferObject;
            mesh.vbo = VertexBufferObject;

            CreateVertexBuffer(mesh);


            //GL.BindVertexArray(VertexArrayObject);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            _models.Add(mesh);
            return mesh.id;
        }

        public void SetRotation(Vector3 rotation, int id)
        {
            Matrix4 rot =  Matrix4.Add(Matrix4.Add(
                            Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)), 
                            Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y))), 
                            Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z)));

            //Setting the rotation of an object from the outside
            _models[id].rotation = rot;
        }

        public void SetTexture(string path, int id = 0)
        {
            //Setting a custom texture of an object from the outside
            texture.Use();
            _models[id].diffusePath = path;
            texture.AddTexture(path, true, id);

            texture.loadTexture(id);
        }

        public void SetPosition(Vector3 position, int id)
        {
            //Set the position of an object from the outside
            _models[id].position = Matrix4.CreateTranslation(position);
        }
        public void SetScale(Vector3 scale, int id)
        {
            //Set the position of an object from the outside
            _models[id].scale = Matrix4.CreateScale(scale);
        }

        private void CreateVertexBuffer(ModelMesh mesh) 
        {
            lightShader.Use();

            float[] vertices = mesh.vertices;
            int[] indices = mesh.indices;
            float[] texCoords = mesh.texCoords;

            int vertexLocation = 0;
            int texCoordsLocation = 2;

            if (mesh.isLight)
            {
                //Initialize the vao for the lamp, this is mostly the same as the code for the model cube
                VertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(VertexArrayObject);
                //We only need to bind to the VBO, the container's VBO's data already contains the correct data.
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
                //Set the vertex attributes (only position data for our lamp)
                int normalLocation = 1;
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

                GL.BindVertexArray(0);

                return;
            }

            texture.Use();
            int tid = texture.AddTexture(mesh.diffusePath);

            //onsole.WriteLine(vertexLocation);

            GL.BindVertexArray(0);
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.GenBuffers(1, out uvBuffer);

            GL.BindBuffer(BufferTarget.ArrayBuffer, uvBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(texCoordsLocation);
            GL.VertexAttribPointer(texCoordsLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            Console.WriteLine(texCoordsLocation);
            texture.loadTexture(tid);

        }

        private void RenderObject(Matrix4 position, Matrix4 rotation, Matrix4 scale, int id)
        {
            //Getting mesh
            ModelMesh mesh = _models[id];
            
            //Set Light prop
            lightShader.Use();
            lightShader.SetVector3("objectColor", new Vector3(1.0f, 1.5f, 1.31f));
            lightShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
            lightShader.SetVector3("viewPos", cam.Position);

            //Set position/rotation/camera
            lightShader.SetMatrix4("rotation", rotation); //model
            lightShader.SetMatrix4("position", position); //view
            lightShader.SetMatrix4("scale", scale); //scale
            lightShader.SetMatrix4("projection", projection); //camera projection

            //Set material
            //lightShader.SetVector3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));

            //texture.BindTexture(id);
            texture.Use();
            texture.BindTexture(id);

            lightShader.SetInt("material.diffuse", 0);
            lightShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            lightShader.SetFloat("material.shininess", mesh.shininess);

            //Set light prop
            //lightShader.SetVector3("light.position", cam.Position);

            Vector3 lightColor;
            float time = DateTime.Now.Second + DateTime.Now.Millisecond / 1000f;
            lightColor.X = (float)Math.Sin(time * 2.0f);
            lightColor.Y = (float)Math.Sin(time * 0.7f);
            lightColor.Z = (float)Math.Sin(time * 1.3f);

            float lightYaxis = (float)Math.Sin(time * 2.0f);

            Vector3 ambientColor = lightColor * new Vector3(2f);
            Vector3 diffuseColor = lightColor * new Vector3(5f);
            //Console.WriteLine(mesh.isLight + "  " + id);
            if (mesh.isLight)
            {
                //Console.WriteLine(mesh.position.ExtractTranslation());
                //lightShader.SetVector3("light.position", mesh.position.ExtractTranslation() * new Vector3(1, lightYaxis, 1));
                
                switch (mesh.lightType)
                {
                    case LightType.Directional:
                        lightShader.SetVector3("dirLight.direction", mesh.rotation.ExtractRotation().Xyz); // Light direction/rotation

                        lightShader.SetVector3("dirLight.ambient", mesh.color * mesh.intensity); // Light intensity and color
                        lightShader.SetVector3("dirLight.diffuse", new Vector3(0.5f, 0.5f, 0.5f));
                        lightShader.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));
                        break;
                    case LightType.Point:
                        lightShader.SetVector3($"pointLights[" + mesh.lightId + "].position", mesh.position.ExtractTranslation() * new Vector3(1, lightYaxis, 1));

                        lightShader.SetInt("PointLightSize", _lightPointCount);

                        lightShader.SetVector3("pointLights[" + mesh.lightId + "].ambient", mesh.color * mesh.intensity); // Light intensity and color
                        lightShader.SetVector3("pointLights[" + mesh.lightId + "].diffuse", new Vector3(0.5f, 0.5f, 0.5f));

                        lightShader.SetVector3("pointLights[" + mesh.lightId + "].specular", new Vector3(0.5f, 0.5f, 0.5f));

                        lightShader.SetFloat("pointLights[" + mesh.lightId + "].constant", 1.0f);
                        lightShader.SetFloat("pointLights[" + mesh.lightId + "].linear", 0.7f);
                        lightShader.SetFloat("pointLights[" + mesh.lightId + "].quadratic", 1.8f);
                        break;
                    case LightType.Spot:
                        lightShader.SetVector3("spotLight.direction", Vector3.Zero); // Light direction/rotation

                        lightShader.SetVector3("spotLight.position", mesh.position.ExtractTranslation());
                        lightShader.SetFloat("light.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5)));

                        lightShader.SetVector3("dirLight.ambient", mesh.color * mesh.intensity); // Light intensity and color
                        lightShader.SetVector3("dirLight.diffuse", new Vector3(0.5f, 0.5f, 0.5f));
                        lightShader.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));
                        break;
                }
            }
            /*lightShader.SetVector3("light.ambient", Vector3.One * 2);
            lightShader.SetVector3("light.diffuse", new Vector3(0.5f, 0.5f, 0.5f)); // darken the light a bit to fit the scene

            lightShader.SetVector3("light.specular", Vector3.Zero);
            lightShader.SetFloat("light.constant", 1.0f);
            lightShader.SetFloat("light.linear", 0.7f);
            lightShader.SetFloat("light.quadratic", 1.8f);*/

            //Binding the vertexes and the buffers, then clearing it, UPDATE => NO EBO/VBO on draw
            GL.BindVertexArray(mesh.vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ebo);
            

            GL.DrawElements(All.Triangles, mesh.size, All.UnsignedInt, (IntPtr)0);
            //GL.BindVertexArray(0);

        }

        float kx = 0, ky = 0, mx = 0, my = 0;
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            /*float x = (2.0f * Input.GetAxis("Mouse X")) / Width - 1f;
            float y = (1.0f - (2.0f * Input.GetAxis("Mouse Y")) / Height);
            float z = 1.0f;
            Vector3 ray_nds = new Vector3(x, y, z);
            Vector4 ray_clip = new Vector4(ray_nds.X, ray_nds.Y, -1.0f, 1.0f);
            Vector4 ray_eye = Matrix4.Invert(projection) * ray_clip;
            ray_eye = new Vector4(ray_eye.X, ray_eye.Y, ray_eye.Z, 0.0f);
            Vector3 ray_wor = (Matrix4.Invert(Matrix4.CreateTranslation(cam.Position)) * ray_eye).Xyz;
            ray_wor = Vector3.Normalize(ray_wor);

            Vector3 point = new Vector3(x, y, 0f);

            //Raycast ray = new Raycast(cam, projection, Width, Height);

            //SetScale(new Vector3(0.01f, 0.01f, 0.1f), 0);
            SetPosition(cam.Position * ray_wor, 0);
            //CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), Matrix4.CreateTranslation(ray_wor), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)));
            Console.WriteLine(point);*/

            if (Input.GetMouseDown(MouseButton.Right))
            {
                kx = Input.GetAxis("Keyboard X");
                ky = Input.GetAxis("Keyboard Y");
                my = Input.GetAxis("Mouse Y") * 0.1f;
                mx = Input.GetAxis("Mouse X") * 0.1f;

                cam.MoveCamera(new Vector3(kx, 0, ky));
                cam.RotateCamera(new Vector3(mx, 0, my));
            }
            cam.UpdateCamera();

            int m_id = 0;
            for (int i = 0; i < _models.Count; i++)
            {
                m_id = i;

                if (m_id == _models.Count)
                    m_id = 0;

                Matrix4 pos = _models[i].position;
                Matrix4 rot = _models[i].rotation;
                Matrix4 scale = _models[i].scale;

                int id = _models[i].id;

                RenderObject(pos * cam.GetView(), rot, scale, id);
            }

            //GL.Flush();
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        private Vector3 PickObjectOnScreen()
        {
            // viewport coordinate system
            // normalized device coordinates
            var x = (2f * Input.GetAxis("Mouse X")) / Width - 1f;
            var y = 1f - (2f * Input.GetAxis("Mouse X")) / Height;
            var z = 1f;
            var rayNormalizedDeviceCoordinates = new Vector3(x/ 2, y/2, z);

            // 4D homogeneous clip coordinates
            var rayClip = new Vector4(rayNormalizedDeviceCoordinates.X, rayNormalizedDeviceCoordinates.Y, -1f, 1f);

            Matrix4 camPos = Matrix4.CreateTranslation(cam.Position);
            // 4D eye (camera) coordinates
            var rayEye = camPos.Inverted() * rayClip;
            rayEye = new Vector4(rayEye.X, rayEye.Y, -1f, 0f);
            
            // 4D world coordinates
            Vector3 rayWorldCoordinates = (new Vector4(-cam.Position.X, cam.Position.Y, 0, 0) * rayEye).Xyz;
            //rayWorldCoordinates.Normalize();
            return rayWorldCoordinates;
        }

        public void Load()
        {
            OnLoad(null);
        }

        public void UnLoad()
        {
            OnUnload(null);
        }

        public void UpdateFrame()
        {
            OnUpdateFrame(null);
        }

        public void RenderFrame()
        {
            OnRenderFrame(null);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
    }
}
