using Microsoft.Xna.Framework;

namespace Raytracer.Scene.Objects
{
    public class Sphere : ISceneObject
    {
        private readonly BoundingSphere _sphere;

        public Sphere(Vector3 pos, float radius, ISurface surface)
        {
            _sphere = new BoundingSphere(pos, radius);
            Surface = surface;
        }

        public ISurface Surface { get; }

        public float? Intersects(Ray ray)
            => ray.Intersects(_sphere);

        public Vector3 Normal(Vector3 position)
            => Vector3.Normalize(position - _sphere.Center);
    }
}
