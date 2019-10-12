using Microsoft.Xna.Framework;

namespace Raytracer.Scene
{
    public interface ISurface
    {
        /// <summary>
        /// Returns the power of the reflection at the given location.
        /// </summary>
        float Reflect(Vector3 position);

        /// <summary>
        /// Returns the diffuse color of the reflection at the given location.
        /// </summary>
        Vector3 Diffuse(Vector3 position);

        /// <summary>
        /// Returns the specular color of the reflection at the given location.
        /// </summary>
        Vector3 Specular(Vector3 position);

        float Shininess { get; }
    }
}
