using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafle3D.Core.Modules
{
    public class Raycast
    {
        public int Width;
        public int Height;

        private Vector3 currentRay;

        private Matrix4 projectionMatrix;
        private Matrix4 viewMatrix;
        private Camera camera;

        private int RECURSION_COUNT = 200;
        private float RAY_RANGE = 600;

        public Raycast(Camera cam, Matrix4 projection, int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this.camera = cam;
            this.projectionMatrix = projection;
            this.viewMatrix = Matrix4.CreateTranslation(camera.Position);
        }

        public Vector3 GetRay()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector2 normalizedCoords = GetNormalizedCoords(mouseX, mouseY);
            Vector3 point = new Vector3(normalizedCoords.X * 10, normalizedCoords.Y * 10, 0);

            return point;
        }

        public Vector3 GetCurrentPoint()
        {
            if (intersectionInRange(1, RAY_RANGE, GetCurrentRay()))
            {
                return binarySearch(1, 4, RAY_RANGE, GetCurrentRay());
            }
            else
            {
                return Vector3.Zero;
            }
        }

        public Vector3 GetCurrentRay()
        {
            this.viewMatrix = Matrix4.CreateTranslation(camera.Position);
            currentRay = GetMouseRay();

            return currentRay;
        }

        private Vector3 GetMouseRay()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector2 normalizedCoords = GetNormalizedCoords(mouseX, mouseY);
            Vector4 clipCoords = new Vector4(normalizedCoords.X, normalizedCoords.Y, -1f, 1f);
            Vector4 eyeCoords = ToEyeSpace(clipCoords);
            Vector3 worldRay = ToWorldSpace(eyeCoords);

            return worldRay;
        }

        private Vector3 ToWorldSpace(Vector4 eyeCoords)
        {
            Matrix4 invertedView = Matrix4.Invert(viewMatrix);
            Vector4 rayWorld = invertedView * eyeCoords;
            Vector3 mouseRay = new Vector3(rayWorld.X, rayWorld.Y, rayWorld.Z);
            
            mouseRay.Normalize();

            return mouseRay;
        }

        private Vector4 ToEyeSpace(Vector4 clip)
        {
            Matrix4 invertedProj = Matrix4.Invert(projectionMatrix);
            Vector4 eyeCoords = invertedProj * clip;

            return new Vector4(eyeCoords.X, eyeCoords.Y, 1f, 0f);
        }

        private Vector2 GetNormalizedCoords(float mx, float my)
        {
            float x = 2.0f * mx / Width - 1.0f;
            float y = 1.0f - 2.0f * my / Height;

            return new Vector2(x, y);
        }

        public Vector3 GetPointOnRay(Vector3 ray, float distance)
        {
            Vector3 camPos = -camera.Position;
            Vector3 start = new Vector3(camPos.X, camPos.Y, camPos.Z);
            Vector3 scaledRay = new Vector3(ray.X * distance, ray.Y * distance, ray.Z * distance);

            return Vector3.Add(start, scaledRay);
        }

        private Vector3 binarySearch(int count, float start, float finish, Vector3 ray)
        {
            float half = start + ((finish - start) / 2f);
            if (count >= RECURSION_COUNT)
            {
                Vector3 endPoint = GetPointOnRay(ray, half);

                return endPoint;
            }
            if (intersectionInRange(start, half, ray))
            {
                return binarySearch(count + 1, start, half, ray);
            }
            else
            {
                return binarySearch(count + 1, half, finish, ray);
            }
        }

        private bool intersectionInRange(float start, float finish, Vector3 ray)
        {
            Vector3 startPoint = GetPointOnRay(ray, start);
            Vector3 endPoint = GetPointOnRay(ray, finish);
            if (!isUnderGround(startPoint) && isUnderGround(endPoint))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isUnderGround(Vector3 testPoint)
        {
            float height = 0;

            if (testPoint.Y < height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
