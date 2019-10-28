using Microsoft.Xna.Framework;
using System;

namespace Raytracer.Scene.Surfaces
{
    public class CheckerboardSurface : ISurface
    {
        private readonly Vector3 _color;

        public CheckerboardSurface(Color color)
        {
            _color = color.ToVector3();
        }

        public float Shininess => 200;

        public Vector3 Diffuse(Vector3 position)
            => (Math.Floor(position.X) + Math.Floor(position.Z)) % 2 == 0 ? _color + new Vector3(0.3f, 0.2f, 0.1f) : _color;

        public float Reflect(Vector3 position)
            => 0.5f;

        public Vector3 Specular(Vector3 position)
            => new Vector3(0.1f);
    }
}
