using System;

namespace Raytracer.Scene
{
    public struct Intersection
    {
        public Intersection(ISceneObject o, float d)
        {
            IntersectedObject = o;
            Distance = d >= 0 ? d : throw new ArgumentOutOfRangeException(nameof(d));
        }

        public ISceneObject IntersectedObject { get; set; }

        public float Distance { get; set; }
    }
}
