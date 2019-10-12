using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raytracer.Backends
{
    public class SingleThreadedSoftwareRaytracer : IRaytracingBackend
    {
        public string Name => "Single-threaded software raytracer";

        public string Description => "For every frame will simply iterate all pixels in software and compute them. Easiest version to implement but also least performant.";

        public void Draw(TracingOptions tracingOptions, RenderTarget2D renderTarget, GameTime gameTime)
        {
        }
    }
}
