using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace Wafle3D.Core.Modules
{
    public class Camera
    {
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;

        public Vector3 Position;
        public Vector3 Rotation;

        public Matrix4 projection;
        private Matrix4 _view;

        private Vector3 _lastRotation = Vector3.Zero;
        private float _yaw = -MathHelper.PiOver2;
        private float _pitch;
        private bool _is2D;

        public Camera(int width, int height, bool is2D = false)
        {
            this._is2D = is2D;

            //Creating a camera perspective view
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), width / (float)height, 0.01f, 10000.0f);
        }

        public Matrix4 GetProjection()
        {
            return projection;
        }

        public void MoveCamera(Vector3 pos)
        {
            //Position = Vector3.Normalize(Vector3.Cross(_front, _up)) + pos;
            Position += _right * pos.X;
            Position += _up * pos.Y;
            if (!_is2D)
                Position += _front * pos.Z;
        }
        public void MoveCamera(Vector2 pos)
        {
            //Position = Vector3.Normalize(Vector3.Cross(_front, _up)) + pos;
            Position += _right * pos.X;
            Position += _up * pos.Y;
        }

        public void RotateCamera(Vector3 rot, bool force = false)
        {
            if (_is2D)
                rot = Vector3.Zero;

            if (_lastRotation == Vector3.Zero)
                _lastRotation = new Vector3(rot.X, rot.Y, rot.Z);
            else
            {
                float deltaX = rot.X - _lastRotation.X;
                float deltaY = rot.Z - _lastRotation.Z;
                _lastRotation = new Vector3(rot.X, rot.Y, rot.Z);

                if (force)
                {
                    _yaw += deltaX;
                    _pitch += deltaY;
                }
                else
                {
                    _yaw += deltaX;
                    _pitch -= deltaY;
                }

                Rotation = new Vector3(_yaw, _pitch, 0);

            }
                //Rotation = rot;
        }

        public Matrix4 GetView()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        public void UpdateCamera()
        {
            _front.X = (float)Math.Cos(MathHelper.DegreesToRadians(_pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(_yaw));
            _front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(_pitch));
            _front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(_pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(_yaw));

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}
