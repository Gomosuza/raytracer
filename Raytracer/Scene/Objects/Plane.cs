using Microsoft.Xna.Framework;
using XPlane = Microsoft.Xna.Framework.Plane;

namespace Raytracer.Scene.Objects
{
    public class Plane : ISceneObject
    {
        private readonly XPlane _plane;

        public Plane(Vector3 normal, float d, ISurface surface)
        {
            Surface = surface;
            _plane = new XPlane(normal, d);
        }

        public ISurface Surface { get; }

        public float? Intersects(Ray ray)
            => ray.Intersects(_plane);

        public Vector3 Normal(Vector3 position)
            => _plane.Normal;
    }
}
