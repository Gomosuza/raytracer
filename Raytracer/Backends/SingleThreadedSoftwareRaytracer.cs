using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;
using System.Linq;

namespace Raytracer.Backends
{
    public class SingleThreadedSoftwareRaytracer : IRaytracer
    {
        private Color[]? _buffer;

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
            var ix = CheckIntersection(ray, tracingOptions.Scene);
            if (!ix.HasValue)
                return Vector3.Zero;

            var intersection = ix.Value;
            var intersectionLocation = ray.Position + ray.Direction * intersection.Distance;

            var color = CalculateNaturalColor(intersectionLocation, tracingOptions, intersection.IntersectedObject.Surface);

            return color;
        }

        private Vector3 CalculateNaturalColor(Vector3 position, ITracingOptions tracingOptions, ISurface surface)
        {
            var color = Vector3.Zero;
            foreach (var light in tracingOptions.Scene.Lights)
            {
                var lightDistance = light.Position - position;
                var lightDir = Vector3.Normalize(lightDistance);

                // check if light source is reachable from current position or not
                // must move away from object slightly, otherwise collision will be current point
                var ray = new Ray(position + lightDir * 0.001f, lightDir);
                var ix = CheckIntersection(ray, tracingOptions.Scene);
                if (ix.HasValue)
                {
                    var intersection = ix.Value;
                    var isInShadow = intersection.Distance * intersection.Distance < lightDistance.LengthSquared();
                    if (isInShadow)
                        continue;
                }

                color += surface.Diffuse(Vector3.Zero);
            }
            return color;
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
