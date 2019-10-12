using Microsoft.Xna.Framework;

namespace Raytracer.Scene.Camera
{
    public interface ICamera
    {
        Vector3 Position { get; }

        /// <summary>
        /// Given a screen of size width*height this will return slightly rotated rays for each x/y combination.
        /// </summary>
        public Ray GetRayForRasterPosition(int x, int y, int width, int height);
    }
}
