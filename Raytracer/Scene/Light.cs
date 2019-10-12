using Microsoft.Xna.Framework;

namespace Raytracer.Scene
{
    public class Light : ILight
    {
        public Light(Vector3 pos, float intensity, Color color)
        {
            Position = pos;
            Intensity = intensity;
            Color = color;
        }

        public Vector3 Position { get; }

        public float Intensity { get; }

        public Color Color { get; }
    }
}
