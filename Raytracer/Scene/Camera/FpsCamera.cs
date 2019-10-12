using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raytracer.Scene.Camera
{
    public class FpsCamera : ICamera
    {
        private Vector3 _direction;
        private readonly GraphicsDevice _graphicsDevice;

        public FpsCamera(
            GraphicsDevice graphicsDevice,
            Vector3 pos,
            Vector3 lookAt)
        {
            Position = pos;
            _direction = lookAt - Position;
            _direction.Normalize();

            _graphicsDevice = graphicsDevice;
        }

        public Vector3 Position { get; private set; }

        public bool Dirty { get; private set; }

        public Ray GetRayForRasterPosition(int x, int y, int width, int height)
        {
            // x is in range 0 - width, we need it to be -1 to 1
            // y is in range 0 - height, we need it to be -1 to 1
            // also consider aspectRatio to prevent view distortion
            var aspectRatio = _graphicsDevice.Viewport.AspectRatio;
            var scalarOffsetX = (-1f + x / (float)width * 2f) * aspectRatio;
            // needs to be the inverse of x since x is left to right both in world and raster space
            // but -1 y is down in world space but up in raster space
            var scalarOffsetY = 1f - y / (float)height * 2f;

            var dir = _direction + scalarOffsetX * Vector3.Right + scalarOffsetY * Vector3.Up;
            dir.Normalize();
            return new Ray(Position, dir);
        }

        public void Move(Vector3 direction)
        {
            Position += direction;
            Dirty = true;
        }
    }
}
