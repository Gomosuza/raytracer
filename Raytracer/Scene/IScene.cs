using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Raytracer.Scene
{
    public interface IScene
    {
        IReadOnlyList<ISceneObject> SceneObjects { get; }

        IReadOnlyList<ILight> Lights { get; }

        void Add(ISceneObject sceneObject);

        void Add(ILight light);

        Intersection? GetClosestIntersection(Ray ray);
    }
}
