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
using Wafle3D.Core;

namespace Wafle3D.Main
{
    public class Game_Copy : GameWindow
    {
        int pgmID;

        int attribute_vcol;
        int attribute_vpos;
        int uniform_mview;

        int vbo_position;
        int vbo_color;
        int vbo_mview;

        Vector3[] vertdata;
        Vector3[] coldata;
        Matrix4[] mviewdata;

        int[] indicedata;

        float[] vertices =
        {
            //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        Shader shader;

        public Game_Copy(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            Console.WriteLine("Starting");
        }

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        int vsID;
        int fsID;

        private void InitProgram()
        {
            pgmID = GL.CreateProgram();
            shader = new Shader(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.vert", @"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.frag");
            shader.Use();

            //loadShader(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.vert", ShaderType.VertexShader, pgmID, out vsID);
            //loadShader(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.frag", ShaderType.FragmentShader, pgmID, out fsID);

            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(pgmID, "vColor");
            uniform_mview = GL.GetUniformLocation(pgmID, "modelview");

            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnRenderFrame(e);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);



            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.UniformMatrix4(uniform_mview, false, ref mviewdata[0]);
            GL.UseProgram(pgmID);



            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);

            GL.Flush();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitProgram();

            vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f, 0f),
                new Vector3( 0.8f, -0.8f, 0f),
                new Vector3( 0f,  0.8f, 0f)};


            coldata = new Vector3[] { new Vector3(1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f,  1f, 0f)};


            mviewdata = new Matrix4[]{
                Matrix4.Identity
            };



            //Title = "Hello OpenTK!";
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        }

        /*protected override void OnLoad(EventArgs e)
        {
            shader = new Shader(@"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.vert", @"D:\DEV\Visual Sudio Projects\Wafle3D\Wafle3D\Shaders\shader.frag");

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);





            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            /*Texture t = new Texture();
            t.Use();

            shader.Use();

            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);

            Console.WriteLine("Error: " + GL.GetError());

            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            Console.WriteLine("Error: " + GL.GetError());

            Ebo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData()

            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            // 2. copy our vertices array in a buffer for OpenGL to use
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            // 3. then set our vertex attributes pointers
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, texCoordLocation, 5);


            base.OnLoad(e); 
        }*/

        protected override void OnUnload(EventArgs e)
        {
            /*VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);*/

            shader.Dispose();

            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            //Texture t = new Texture();
            //t.Use();

            /*int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);

            
            //GL.VertexAttribPointer(texCoordLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            
            //GL.VertexAttribPointer(texCoordLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            shader.Use();

            GL.BindVertexArray(VertexArrayObject);
            // 2. copy our vertices array in a buffer for OpenGL to use
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            // 3. then set our vertex attributes pointers
            GL.VertexAttribPointer(texCoordLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(texCoordLocation);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, texCoordLocation, 0);*/

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
