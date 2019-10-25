using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Components;
using Raytracer.Configuration;
using Raytracer.Scene;
using Raytracer.Scene.Camera;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Raytracer
{
    public class SceneRenderer : DrawableGameComponent
    {
        private readonly PerformanceEvaluator _performanceEvaluator;
        private readonly IRaytracer[] _raytracingBackends;
        private readonly IRaytracer _selectedRaytracingBackend;
        private readonly ITracingOptions _tracingOptions;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly ICamera _camera;
        private readonly Settings _settings;

        private RenderTarget2D? _renderTarget;
        private bool _initialDraw = true;
        private Sampling _sampling;
        private SamplerState _samplerState = SamplerState.PointClamp;

        public SceneRenderer(
            Game game,
            PerformanceEvaluator performanceEvaluator,
            ITracingOptions tracingOptions,
            ICamera camera,
            IEnumerable<IRaytracer> raytracerBackends,
            Settings settings
            ) : base(game)
        {
            _performanceEvaluator = performanceEvaluator;
            _tracingOptions = tracingOptions;
            _raytracingBackends = raytracerBackends.OrderBy(x => x.Name).ToArray();
            _selectedRaytracingBackend = _raytracingBackends
                .FirstOrDefault(x => x.GetType().Name.Equals(settings.Compute.Backend, StringComparison.OrdinalIgnoreCase)) ??
                throw new NotSupportedException($"Unsupported backend {settings.Compute.Backend} requested. " +
                $"  Supported backends are: {string.Join(",", _raytracingBackends.Select(x => x.Name))}");

            _camera = camera;
            _settings = settings;

            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_sampling != _settings.Scene.Sampling)
            {
                _sampling = _settings.Scene.Sampling;
                _samplerState = _sampling switch
                {
                    Sampling.Linear => SamplerState.PointClamp,
                    Sampling.Anisotropic => SamplerState.PointClamp,
                    _ => SamplerState.PointClamp
                };
            }

            ResizeRenderTarget(_performanceEvaluator.Width, _performanceEvaluator.Height);
        }

        private void ResizeRenderTarget(int w, int h)
        {
            if (_renderTarget == null ||
                _renderTarget.Width != w ||
                _renderTarget.Height != h)
            {
                _renderTarget = _selectedRaytracingBackend.ChangeSize(w, h);
                _initialDraw = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (_renderTarget == null)
                return;

            var dirty = HasSceneChanged();
            if (dirty || _initialDraw)
            {
                _initialDraw = false;
                // fill rendertarget so we can see if tracing did not fill entirely
                GraphicsDevice.SetRenderTarget(_renderTarget);
                GraphicsDevice.Clear(Color.Purple);
                GraphicsDevice.SetRenderTarget(null);

                // raytrace
                _selectedRaytracingBackend.Draw(_renderTarget, _tracingOptions, gameTime);
            }

            GraphicsDevice.SetRenderTarget(null);
            // simply upscale rendertarget to screen to draw scene
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, _samplerState);
            _spriteBatch.Draw(_renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
            _spriteBatch.End();
        }

        private bool HasSceneChanged()
        {
            if (!_tracingOptions.OnlyRedrawIfDirty)
                return true;

            return _camera.IsDirty;
        }
    }
}
