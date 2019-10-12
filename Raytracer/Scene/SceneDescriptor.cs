using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Raytracer.Scene
{
    /// <summary>
    /// Description container for a full scene.
    /// </summary>
    public class SceneDescriptor : IScene
    {
        private readonly List<ISceneObject> _sceneObjects;
        private readonly List<ILight> _lights;

        public SceneDescriptor()
        {
            _sceneObjects = new List<ISceneObject>();
            _lights = new List<ILight>();
        }

        public IReadOnlyList<ISceneObject> SceneObjects => _sceneObjects;

        public IReadOnlyList<ILight> Lights => _lights;

        public void Add(ISceneObject sceneObject)
        {
            _sceneObjects.Add(sceneObject);
        }

        public void Add(ILight light)
        {
            _lights.Add(light);
        }

        public IReadOnlyList<Intersection> GetIntersections(Ray ray)
        {
            var intersections = new List<Intersection>();
            foreach (var o in SceneObjects)
            {
                var d = o.Intersects(ray);
                if (d.HasValue && d.Value >= 0)
                    intersections.Add(new Intersection(o, d.Value));
            }
            return intersections;
        }
    }
}
