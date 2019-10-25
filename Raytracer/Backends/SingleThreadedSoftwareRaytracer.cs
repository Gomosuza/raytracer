using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;

namespace Raytracer.Backends
{
    public class SingleThreadedSoftwareRaytracer : SoftwareRaytracerBase
    {
        private Color[]? _buffer;

        public SingleThreadedSoftwareRaytracer(
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {

        }

        public override string Name => "Single-threaded software raytracer";

        public override string Description => "For every frame will simply iterate all pixels in software and compute them. Easiest version to implement but also least performant.";

        public override void Draw(RenderTarget2D renderTarget, ITracingOptions tracingOptions, GameTime gameTime)
        {
            if (_buffer == null ||
                _buffer.Length != renderTarget.Width * renderTarget.Height)
                _buffer = new Color[renderTarget.Width * renderTarget.Height];

            for (int y = 0; y < renderTarget.Height; y++)
            {
                for (int x = 0; x < renderTarget.Width; x++)
                {
                    var ray = tracingOptions.Camera.GetRayForRasterPosition(x, y, renderTarget.Width - 1, renderTarget.Height - 1);
                    _buffer[x + y * renderTarget.Width] = CastRay(ray, tracingOptions);
                }
            }

            // TODO: SetData perf is "horrible"
            // compare against pixel/line drawing
            renderTarget.SetData(_buffer);
        }
    }
}
