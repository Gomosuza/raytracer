using Microsoft.Xna.Framework;

namespace Raytracer.Scene.Camera
{
    public interface ICamera
    {
        Vector3 Position { get; }

        /// <summary>
        /// Indicates when a camera value has changed.
        /// This usually requires the scene to be redrawn.
        /// </summary>
        bool Dirty { get; }

        /// <summary>
        /// Given a screen of size width*height this will return slightly rotated rays for each x/y combination.
        /// </summary>
        Ray GetRayForRasterPosition(int x, int y, int width, int height);

        void Move(Vector3 direction);
    }
}
