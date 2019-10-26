using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.ComputeShader;
using Raytracer.Scene;
using System;
using System.Linq;

namespace Raytracer.Backends
{
    public class ComputeShaderRaytracer : IRaytracer
    {
        private readonly static uint[] _pow = Enumerable.Range(1, 20).Select(x => (uint)Math.Pow(2, x)).ToArray();
        private readonly ComputeShader _shader;
        private readonly GraphicsDevice _graphicsDevice;

        public ComputeShaderRaytracer(
            GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _shader = new ComputeShader(graphicsDevice, "Shaders/raytracer.glslcs");
        }

        public string Name => "Compute shader based raytracer";

        public string Description => "Compute shaders allow executing arbitrary workloads in parallel on the GPU. This is ideal for raytracers where each pixel can be calculated independently.";

        public RenderTarget2D ChangeSize(int newWidth, int newHeight)
            => new RenderTarget2D(_graphicsDevice, newWidth, newHeight, false, SurfaceFormat.Rgba64, DepthFormat.None, 0, RenderTargetUsage.DiscardContents, true, 1);

        public void Draw(RenderTarget2D renderTarget, ITracingOptions tracingOptions, GameTime gameTime)
        {
            _shader.Begin(renderTarget);

            // compute shader requires batching in power of 2, so must input next power of 2
            // shader will auto. discard anything outside of texture range
            var x = _pow.First(x => x >= renderTarget.Width);
            var y = _pow.First(y => y >= renderTarget.Height);

            var width = renderTarget.Width - 1;
            var height = renderTarget.Height - 1;
            // inject parameters
            _shader.SetParameter("eye", tracingOptions.Camera.Position);
            _shader.SetParameter("direction", tracingOptions.Camera.Direction);
            _shader.SetParameter("sampleCount", tracingOptions.SampleCount);
            _shader.SetParameter("reflectionLimit", tracingOptions.ReflectionLimit);
            _shader.SetParameter("fov", tracingOptions.Fov);

            // hardcoded to chunks of 8x8 in compute shader
            _shader.Execute(x / 8, y / 8, 1);
            _shader.End();
        }
    }
}
