using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;
using System;
using System.Threading.Tasks;

namespace Raytracer.Backends
{
    public class MultiThreadedSoftwareRaytracer : SoftwareRaytracerBase
    {
        private Color[]? _buffer;

        public override string Name => "Multi-threaded software raytracer";

        public override string Description => "For every frame will compute pixels in parallel batches. Slight performance boost compared to single-threaded version but still CPU limited.";

        public override void Draw(RenderTarget2D renderTarget, ITracingOptions tracingOptions, GameTime gameTime)
        {
            if (_buffer == null ||
                _buffer.Length != renderTarget.Width * renderTarget.Height)
                _buffer = new Color[renderTarget.Width * renderTarget.Height];

            var r = Parallel.For(0, renderTarget.Height, y =>
            {
                for (int x = 0; x < renderTarget.Width; x++)
                {
                    var ray = tracingOptions.Camera.GetRayForRasterPosition(x, y, renderTarget.Width - 1, renderTarget.Height - 1);
                    _buffer[x + y * renderTarget.Width] = CastRay(ray, tracingOptions);
                }
            });
            if (!r.IsCompleted)
                throw new NotSupportedException("Parallel loop must always complete");

            renderTarget.SetData(_buffer);
        }
    }
}
