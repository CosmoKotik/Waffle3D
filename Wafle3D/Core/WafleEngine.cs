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
        /*
         * Code written by: CosmoKotik;
         * The code isn't best one, but at least something;
         * It is optimized and working fine;
        */

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        int uvBuffer;
        
        public ObjectManager ObjectManager;
        private Camera cam;

        private List<ModelMesh> _models = new List<ModelMesh>();
        public int LightCount = 0;
        public int LightPointCount = 0;
        public int TotalObjectCount = 0;
        private bool _is2D = false;

        Texture texture;
        Shader shader;
        Shader lightShader;
        public Raycast raycast;
        Matrix4 projection;


        private List<string> ScriptNames = new List<string>();
        public List<WafleBehaviour> Scripts = new List<WafleBehaviour>();

        public WafleEngine(int width, int height, string title, bool is2D) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 4, 1, GraphicsContextFlags.ForwardCompatible)
        {
            Console.WriteLine("Starting");
            this._is2D = is2D;
            
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
            cam = new Camera(1920, 1080, _is2D);
            raycast = new Raycast(cam, projection, 1920, 1080);

            cam.MoveCamera(new Vector3(23.31075f, 17.57434f, 26.73849f));
            cam.RotateCamera(new Vector3(-130.7706f, -26.7f, 0));
            //cam.RotateCamera(new Vector3(50, 0, 0));

            //Creating a camera perspective view
            //projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.01f, 10000.0f);
            projection = cam.GetProjection();

            //Adding scripts
            //ScriptNames.Add("Movement");
            ScriptNames.Add("CubeArray1");
            ScriptNames.Add("CubeArray2");
            ScriptNames.Add("CubeArray3");

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
            GL.Enable(EnableCap.Lighting);

            //Adding objects and displaying the id
            ModelMesh prnt = CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One);

            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt);

            ModelMesh prnt1 = CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(15, 0, 0), new Vector3(0, 0, 0), Vector3.One);

            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt1);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt1);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt1);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt1);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt1);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt1);

            ModelMesh prnt2 = CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(15, 0, 0), new Vector3(0, 0, 0), Vector3.One);

            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt2);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt2);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt2);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt2);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt2);
            CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, 0, 0), new Vector3(0, 0, 0), Vector3.One, prnt2);
            //CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Toad/Toad.obj"), new Vector3(100.0f, 0.0f, -533.0f), new Vector3(0, 0, 0), Vector3.One);
            //CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Goomba/Goomba.fbx"), new Vector3(-10.0f, 0.0f, 10.0f), new Vector3(0, 0, 0), Vector3.One);
            //CreateObject(ObjectManager.LoadModel(@"Models/Mario64/Mario/Mario.fbx"), new Vector3(150.0f, 0.0f, -422.0f), new Vector3(0, 0, 0), Vector3.One);
            //CreateObject(ObjectManager.LoadModel(@"Models/Random/gun.fbx"), new Vector3(0.0f, 0.0f, -0.0f), new Vector3(0, 0, 0), Vector3.One);
            //CreateObject(ObjectManager.LoadModel(@"Models/Random/Spaceshit.fbx"), new Vector3(10.0f, 0.0f, -0.0f), new Vector3(0, 0, 0), Vector3.One);

            //Light point
            //CreateObject(new ModelMesh(), Matrix4.CreateTranslation(0, 5, 2), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)), LightType.Point);
            //CreateObject(new ModelMesh(), Matrix4.CreateTranslation(0, -5, 2), Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)), LightType.Directional);
            //CreateObject(new ModelMesh(), new Vector3(0, 0, 0), new Vector3(0, 0, 0), Vector3.One * 1, null, LightType.Point, Light.Advanced(new Vector3(0, 100, 0), 0.1f, 1f, 16));
            CreateObject(new ModelMesh(), new Vector3(0, -5, -5), new Vector3(0, 0, 0), Vector3.One * 0.1f, null, LightType.Point, Light.Advanced(new Vector3(1, 0, 0), 1f, 2, 1));
            CreateObject(new ModelMesh(), new Vector3(0, 10, 0), new Vector3(0, 0, 0), Vector3.One * 0.1f, null, LightType.Point, Light.Advanced(new Vector3(0, 1, 0), 1f, 2, 1));
            CreateObject(new ModelMesh(), new Vector3(0, -5, 5), new Vector3(0, 0, 0), Vector3.One * 0.1f, null, LightType.Point, Light.Advanced(new Vector3(0, 0, 1), 1f, 2, 1));

            

            //Setting custom textures to the objects
            //SetTexture(@"Models/Mario64/Toad/Toad_grp.png", 1);
            //SetTexture(@"Models/Mario64/Goomba/GoombaTex.png", 2);
            //SetTexture(@"Models/Mario64/Mario/Mario64Body_alb.png", 3);

            //CreateObject(ObjectManager.LoadModel(@"Models/Cube.fbx"), new Vector3(0.0f, -3.0f, -4.0f), new Vector3(0, 0, 0), Vector3.One);
            //CreateObject(ObjectManager.LoadModel("", ObjectManager.ObjectType.Plane), Vector3.Zero, Vector3.Zero, Vector3.One);
            //CreateObject(new ModelMesh(), new Vector3(0, -5, 2), new Vector3(0, 0, 0), Vector3.One * 1, null, LightType.Directional, Light.Advanced(Vector3.One, 1, 1, 2));

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

        public ModelMesh CreateObject(ModelMesh mesh)
        {
            GL.GenVertexArrays(1, out VertexArrayObject);
            GL.GenBuffers(1, out VertexBufferObject);
            GL.GenBuffers(1, out ElementBufferObject);


            if (mesh.isLight)
            {
                mesh.lightId = LightCount;
                switch (mesh.lightType)
                {
                    case LightType.Point:
                        LightPointCount++;
                        break;
                }

                LightCount++;
            }

            mesh.id = _models.Count;

            mesh.vao = VertexArrayObject;
            mesh.ebo = ElementBufferObject;
            mesh.vbo = VertexBufferObject;

            CreateVertexBuffer(mesh);

            _models.Add(mesh);
            TotalObjectCount++;

            return mesh;
        }

        public ModelMesh CreateObject(ModelMesh mesh, Vector3 position, Vector3 rotation, Vector3 Scale, ModelMesh parent = null, LightType type = LightType.nul, Light lightOptions = null)
        {
            GL.GenVertexArrays(1, out VertexArrayObject);
            GL.GenBuffers(1, out VertexBufferObject);
            GL.GenBuffers(1, out ElementBufferObject);

            Matrix4 RX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            Matrix4 RY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            Matrix4 RZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
            Matrix4 rot = RX * RY * RZ;

            mesh.rotation = rot;
            mesh.position = Matrix4.CreateTranslation(position);
            mesh.scale = Matrix4.CreateScale(Scale);
            mesh.id = _models.Count;
            mesh.parent = parent;

            if (type != LightType.nul)
            {
                mesh.isLight = true;
                mesh.lightId = LightCount;
                mesh.lightType = type;

                if (lightOptions != null)
                {
                    mesh.intensity = lightOptions.Intensity;
                    mesh.shininess = lightOptions.Snininess;
                    mesh.color = lightOptions.Color;
                    mesh.pointSize = lightOptions.Size;
                }

                switch (type)
                {
                    case LightType.Point:
                        LightPointCount++;
                        break;
                }

                LightCount++;
            }

            mesh.vao = VertexArrayObject;
            mesh.ebo = ElementBufferObject;
            mesh.vbo = VertexBufferObject;

            CreateVertexBuffer(mesh);

            _models.Add(mesh);
            TotalObjectCount++;

            return mesh;
        }

        public ModelMesh DestroyObject(ModelMesh mesh)
        {
            GL.DeleteBuffer(mesh.vao);
            GL.DeleteBuffer(mesh.ebo);
            _models.Remove(mesh);
            TotalObjectCount--;

            return mesh;
        }

        public void SetRotation(Vector3 rotation, int id)
        {
            /*Matrix4 rot =  Matrix4.Add(Matrix4.Add(
                            Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)), 
                            Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y))), 
                            Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z)));*/
            //Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)), Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)), Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z))
            //Setting the rotation of an object from the outside

            Matrix4 model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(rotation.X)) * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(rotation.Y)) * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(rotation.Z));

            //_models[id].rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) + Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) + Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
            _models[id].rotation = model;
            //_models[id].rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));

            //Console.WriteLine(_models[id].rotation);
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
            //lightShader.Use();

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
            //Console.WriteLine(texCoordsLocation);
            texture.loadTexture(tid);

        }

        private void RenderObject(Matrix4 position, Matrix4 rotation, Matrix4 scale, int id)
        {
            //Getting mesh
            ModelMesh mesh = _models[id];

            //Binding texture
            texture.Use();
            //lightShader.Use();
            texture.BindTexture(id);

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
            lightShader.SetInt("material.diffuse", 0);
            lightShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            lightShader.SetFloat("material.shininess", mesh.shininess);

            //Set light prop
            if (mesh.isLight)
            {
                //Refresh light
                //CreateObject(DestroyObject(mesh));


                switch (mesh.lightType)
                {
                    case LightType.Directional:
                        lightShader.SetVector3("dirLight.direction", mesh.rotation.ExtractRotation().Xyz); // Light direction/rotation

                        lightShader.SetVector3("dirLight.ambient", mesh.color * mesh.intensity); // Light intensity and color
                        lightShader.SetVector3("dirLight.diffuse", new Vector3(0.5f, 0.5f, 0.5f));
                        lightShader.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));
                        break;
                    case LightType.Point:
                        lightShader.SetVector3($"pointLights[" + mesh.lightId + "].position", mesh.position.ExtractTranslation());

                        lightShader.SetInt("PointLightSize", LightPointCount);

                        float distance = GetDistanceVec3(mesh.position.ExtractTranslation(), cam.Position);

                        float constant = 1.0f;
                        float linear = 0.09f;
                        float quadratic = 0.032f;

                        //float attenuation = 1.0f / (constant + linear * distance + quadratic * (distance * distance));
                        float attenuation = 1.0f;

                        lightShader.SetVector3("pointLights[" + mesh.lightId + "].ambient", (mesh.color * mesh.intensity) * attenuation); // Light intensity and color
                        lightShader.SetVector3("pointLights[" + mesh.lightId + "].diffuse", new Vector3(0.5f, 0.5f, 0.5f) * attenuation);
                        lightShader.SetVector3("pointLights[" + mesh.lightId + "].specular", new Vector3(0.5f, 0.5f, 0.5f) * attenuation);

                        lightShader.SetFloat("pointLights[" + mesh.lightId + "].constant", constant);
                        lightShader.SetFloat("pointLights[" + mesh.lightId + "].linear", linear);
                        lightShader.SetFloat("pointLights[" + mesh.lightId + "].quadratic", quadratic);

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

            //Binding the vertexes and the buffers, then clearing it, UPDATE => NO EBO/VBO on draw
            GL.BindVertexArray(mesh.vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ebo);
            
            //Drawing shit
            GL.DrawElements(All.Triangles, mesh.size, All.UnsignedInt, (IntPtr)0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.BindVertexArray(0);

        }

        public float GetDistanceVec3(Vector3 target, Vector3 from)
        {
            float x = target.X - from.X;
            float y = target.Y - from.Y;
            float z = target.Z - from.Z;

            return x + y + z;
        }

        float kx = 0, ky = 0, mx = 0, my = 0;
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (Input.GetMouseDown(MouseButton.Right))
            {
                kx = Input.GetAxis("Keyboard X");
                ky = Input.GetAxis("Keyboard Y");
                my = Input.GetAxis("Mouse Y") * 0.1f;
                mx = Input.GetAxis("Mouse X") * 0.1f;
            
                cam.MoveCamera(new Vector3(kx, 0, ky));
                cam.RotateCamera(new Vector3(mx, 0, my));
                Console.WriteLine("pos: " + cam.Position.ToString());
                Console.WriteLine("rot: " + cam.Rotation.ToString());
            }

            cam.UpdateCamera();

            //Console.WriteLine(raycast.GetRay());

            int m_id = 0;
            for (int i = 0; i < _models.Count; i++)
            {
                m_id = i;

                if (m_id == _models.Count)
                    m_id = 0;

                Matrix4 pos;
                Matrix4 rot;
                Matrix4 scale;

                if (_models[i].parent != null)
                {
                    pos = _models[i].position * _models[i].parent.rotation * _models[i].parent.position;
                    rot = _models[i].rotation * _models[i].parent.rotation;

                    //Console.Write("PARENT " + _models[i].parent.position);
                }
                else 
                {
                    pos = _models[i].position;
                    rot = _models[i].rotation;
                }

                scale = _models[i].scale;

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
