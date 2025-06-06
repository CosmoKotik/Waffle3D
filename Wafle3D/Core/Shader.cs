﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;

namespace Wafle3D.Core
{
    public class Shader
    {
        int Handle;
        int VertexShader;
        int FragmentShader;
        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource;

            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            Console.WriteLine(GL.GetError());

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            Console.WriteLine(GL.GetError());

            GL.CompileShader(VertexShader);

            Console.WriteLine(GL.GetError());

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != String.Empty)
                Console.WriteLine(infoLogVert);

            Console.WriteLine(GL.GetError());

            GL.CompileShader(FragmentShader);

            Console.WriteLine(GL.GetError());

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            Console.WriteLine(GL.GetError());


            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            //Console.WriteLine(GL.GetError());

            GL.LinkProgram(Handle);

            Console.WriteLine(GL.GetError());

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            Console.WriteLine(GL.GetError());
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(location, value);
        }
        
        public void SetFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(location, value);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            GL.UseProgram(Handle);

            int location = GL.GetUniformLocation(Handle, name);

            GL.UniformMatrix4(location, true, ref matrix);
        }

        public void SetVector3(string name, Vector3 vector)
        {
            GL.UseProgram(Handle);

            int location = GL.GetUniformLocation(Handle, name);

            GL.Uniform3(location, ref vector);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }
    }
}
