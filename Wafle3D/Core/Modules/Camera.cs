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

        private Vector3 _position;
        private Vector3 _rotation;
        private Matrix4 _view;

        private Vector3 _lastRotation = Vector3.Zero;
        private float _yaw = -MathHelper.PiOver2;
        private float _pitch;


        public void MoveCamera(Vector3 pos)
        {
            //_position = Vector3.Normalize(Vector3.Cross(_front, _up)) + pos;
            _position += _right * pos.X;
            _position += _front * pos.Z;
        }
        public void RotateCamera(Vector3 rot)
        {
            if (_lastRotation == Vector3.Zero)
                _lastRotation = new Vector3(rot.X, rot.Y, rot.Z);
            else
            {
                float deltaX = rot.X - _lastRotation.X;
                float deltaY = rot.Z - _lastRotation.Z;
                _lastRotation = new Vector3(rot.X, rot.Y, rot.Z);

                _yaw += deltaX;
                _pitch -= deltaY;

            }
                //_rotation = rot;
        }

        public Matrix4 GetView()
        {
            return Matrix4.LookAt(_position, _position + _front, _up);
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
