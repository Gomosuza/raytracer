using Microsoft.Xna.Framework;

namespace Raytracer.Scene.Camera
{
    public class FpsCamera : ICamera
    {
        private readonly Vector3 _lookAt;

        public FpsCamera(Vector3 pos, Vector3 lookAt)
        {
            Position = pos;
            _lookAt = lookAt;
        }

        public Vector3 Position { get; }

        public Ray GetRayForRasterPosition(int x, int y, int width, int height)
        {
            // x is in range 0 - width, we need it to be -1 to 1
            var scalarOffsetX = -1f + x / (float)width * 2f;
            // y is in range 0 - height, we need it to be -1 to 1
            // needs to be the inverse of x since x is left to right both in world and raster space
            // but -1 y is down in world space but up in raster space
            var scalarOffsetY = 1f - y / (float)height * 2f;

            var direction = _lookAt - Position;
            direction.Normalize();
            var dir = direction + scalarOffsetX * Vector3.Right + scalarOffsetY * Vector3.Up;
            dir.Normalize();
            return new Ray(Position, dir);
        }
    }
}
