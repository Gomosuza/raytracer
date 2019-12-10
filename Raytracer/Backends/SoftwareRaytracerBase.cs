using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;
using System;

namespace Raytracer.Backends
{
    public abstract class SoftwareRaytracerBase : IRaytracer
    {
        private readonly Random _random = new Random();
        private readonly GraphicsDevice _graphicsDevice;

        public SoftwareRaytracerBase(
            GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public abstract string Name { get; }
        public abstract string Description { get; }

        public RenderTarget2D ChangeSize(int newWidth, int newHeight)
            => new RenderTarget2D(_graphicsDevice, newWidth, newHeight);

        public abstract void Draw(RenderTarget2D renderTarget, ITracingOptions tracingOptions, GameTime gameTime);

        protected Color CastRay(Ray ray, ITracingOptions tracingOptions)
        {
            Vector3 color = Vector3.Zero;
            for (int i = 0; i < tracingOptions.SampleCount; i++)
            {
                var c = GetColorVectorForRay(ray, tracingOptions, 0, i);
                c = Vector3.Clamp(c, Vector3.Zero, Vector3.One);
                color += c;
            }
            color /= tracingOptions.SampleCount;
            return new Color(color);
        }

        private Vector3 GetColorVectorForRay(Ray ray, ITracingOptions tracingOptions, int depth, int sampleIndex)
        {
            var ix = CheckIntersection(ray, tracingOptions.Scene);
            if (!ix.HasValue)
                return Vector3.Zero;

            var intersection = ix.Value;
            var intersectionLocation = ray.Position + ray.Direction * intersection.Distance;
            var intersectionNormal = intersection.IntersectedObject.Normal(intersectionLocation);

            var color = CalculateNaturalColor(intersectionLocation, intersectionNormal, tracingOptions, intersection.IntersectedObject.Surface, sampleIndex);

            if (depth >= tracingOptions.ReflectionLimit)
            {
                return color * Vector3.One / 2f;
            }
            // TODO: make feature of surface
            var reflectionDir = ray.Direction - 2 * Vector3.Dot(intersectionNormal, ray.Direction) * intersectionNormal;
            return color + GetReflectionColor(intersection.IntersectedObject.Surface, intersectionLocation, reflectionDir, tracingOptions, depth + 1, sampleIndex);
        }

        private Vector3 GetReflectionColor(
            ISurface surface,
            Vector3 position,
            Vector3 reflectionDir,
            ITracingOptions tracingOptions,
            int depth,
            int sampleIndex)
        {
            var ray = new Ray(position + reflectionDir * 0.001f, reflectionDir);
            return surface.Reflect(position) * GetColorVectorForRay(ray, tracingOptions, depth + 1, sampleIndex);
        }

        private Vector3 CalculateNaturalColor(
            Vector3 position,
            Vector3 intersectionNormal,
            ITracingOptions tracingOptions,
            ISurface surface,
            int sampleIndex)
        {
            // use Phong shading model to determine color
            // https://en.wikipedia.org/wiki/Blinn%E2%80%93Phong_shading_model

            // use minimal ambient
            var color = new Vector3(0.2f);
            foreach (var light in tracingOptions.Scene.Lights)
            {
                var offset = Vector3.Zero;
                if (sampleIndex > 0)
                {
                    const float offsetFactor = 0.1f;
                    offset = new Vector3(-offsetFactor + offsetFactor * 2 * (float)_random.NextDouble(),
                                         -offsetFactor + offsetFactor * 2 * (float)_random.NextDouble(),
                                         -offsetFactor + offsetFactor * 2 * (float)_random.NextDouble());
                }
                var lightDistance = light.Position + offset - position;
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

                var illumination = MathHelper.Clamp(Vector3.Dot(lightDir, intersectionNormal), 0, float.MaxValue);
                var c = illumination * light.Color.ToVector3() * light.Intensity;
                color += c * surface.Diffuse(position);

                var specular = MathHelper.Clamp(Vector3.Dot(lightDir, intersectionNormal), 0, float.MaxValue);
                color += specular * c * (float)Math.Pow(specular, surface.Shininess) * surface.Specular(position);
            }
            return color / tracingOptions.Scene.Lights.Count;
        }

        private Intersection? CheckIntersection(Ray ray, IScene scene)
            => scene.GetClosestIntersection(ray);
    }
}
