using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Wafle3D.Core
{
    class Texture
    {
        int Handle;

        List<Image<Rgba32>> images = new List<Image<Rgba32>>();
        List<byte[]> pix = new List<byte[]>();

        public Texture() 
        {
            
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            //Handle = GL.GenTexture();
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public int AddTexture(string path, bool isSetting = false, int id = 0)
        {
            try
            {
                Handle = GL.GenTexture();

                if (path == null)
                    path = @"Textures/gray.png";

                using (Image<Rgba32> image = Image.Load<Rgba32>(path))
                {
                    image.Mutate(x => x.Flip(FlipMode.Vertical));

                    List<byte> pixels = new List<byte>(4 * image.Width * image.Height);

                    for (int y = 0; y < image.Height; y++)
                    {
                        var row = image.GetPixelRowSpan(y);

                        for (int x = 0; x < image.Width; x++)
                        {
                            pixels.Add(row[x].R);
                            pixels.Add(row[x].G);
                            pixels.Add(row[x].B);
                            pixels.Add(row[x].A);
                        }
                    }
                    GL.BindTexture(TextureTarget.Texture2D, Handle);


                    if (!isSetting)
                    {
                        images.Add(image);
                        pix.Add(pixels.ToArray());
                    }
                    else
                    {
                        images[id] = image;
                        pix[id] = pixels.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return images.Count - 1;
        }

        public void loadTexture(int id)
        {
            //Handle = GL.GenTexture();
            BindTexture(id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, images[id].Width, images[id].Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pix[id]);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void BindTexture(int id)
        {
            Use();
            GL.BindTexture(TextureTarget.Texture2D, id);
        }
    }
}
