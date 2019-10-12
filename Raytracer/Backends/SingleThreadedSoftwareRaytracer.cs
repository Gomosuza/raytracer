using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;
using System.Linq;

namespace Raytracer.Backends
{
    public class SingleThreadedSoftwareRaytracer : IRaytracer
    {
        private Color[] _buffer;

        public string Name => "Single-threaded software raytracer";

        public string Description => "For every frame will simply iterate all pixels in software and compute them. Easiest version to implement but also least performant.";

        public void Draw(RenderTarget2D renderTarget, ITracingOptions tracingOptions, GameTime gameTime)
        {
            if (_buffer == null ||
                _buffer.Length != renderTarget.Width * renderTarget.Height)
                _buffer = new Color[renderTarget.Width * renderTarget.Height];

            for (int y = 0; y < renderTarget.Height; y++)
            {
                for (int x = 0; x < renderTarget.Width; x++)
                {
                    var ray = tracingOptions.Camera.GetRayForRasterPosition(x, y, renderTarget.Width - 1, renderTarget.Height - 1);
                    Vector3 color = Vector3.Zero;
                    for (int i = 0; i < tracingOptions.SampleCount; i++)
                    {
                        var c = GetColorVectorForRay(ray, tracingOptions);
                        c = Vector3.Clamp(c, Vector3.Zero, Vector3.One);
                        color += c;
                    }
                    color /= tracingOptions.SampleCount;
                    _buffer[x + y * renderTarget.Width] = new Color(color);
                }
            }

            // TODO: SetData perf is "horrible"
            // compare against pixel/line drawing
            renderTarget.SetData(_buffer);
        }

        private Vector3 GetColorVectorForRay(Ray ray, ITracingOptions tracingOptions)
        {
            var intersection = CheckIntersection(ray, tracingOptions.Scene);
            if (!intersection.HasValue)
                return Vector3.Zero;

            return intersection.Value.IntersectedObject.Surface.Diffuse(Vector3.Zero);
        }

        private Intersection? CheckIntersection(Ray ray, IScene scene)
        {
            var intersections = scene.GetIntersections(ray);
            if (intersections.Count == 0)
                return null;

            return intersections.OrderBy(x => x.Distance).First();
        }
    }
}
