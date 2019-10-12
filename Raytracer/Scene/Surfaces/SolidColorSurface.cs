using Microsoft.Xna.Framework;

namespace Raytracer.Scene.Surfaces
{
    public class SolidColorSurface : ISurface
    {
        private readonly Vector3 _color;

        public SolidColorSurface(Color color)
        {
            _color = color.ToVector3();
        }

        public float Shininess => 200;

        public Vector3 Diffuse(Vector3 position)
            => _color;

        public float Reflect(Vector3 position)
            => 0.5f;

        public Vector3 Specular(Vector3 position)
            => new Vector3(0.1f);
    }
}
