using Microsoft.Xna.Framework;

namespace Raytracer.Scene
{
    public interface ILight
    {
        Vector3 Position { get; }

        /// <summary>
        /// Defaults to 1f
        /// </summary>
        float Intensity { get; }

        Color Color { get; }
    }
}
