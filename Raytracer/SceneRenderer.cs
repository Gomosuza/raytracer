using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Components;
using Raytracer.Scene;
using Raytracer.Scene.Camera;
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

        private RenderTarget2D? _renderTarget;
        private bool _initialDraw = true;
        private readonly ICamera _camera;

        public SceneRenderer(
            Game game,
            PerformanceEvaluator performanceEvaluator,
            ITracingOptions tracingOptions,
            ICamera camera,
            IEnumerable<IRaytracer> raytracerBackends
            ) : base(game)
        {
            _performanceEvaluator = performanceEvaluator;
            _tracingOptions = tracingOptions;

            _raytracingBackends = raytracerBackends.OrderBy(x => x.Name).ToArray();
            _selectedRaytracingBackend = _raytracingBackends[0];

            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            _camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ResizeRenderTarget(_performanceEvaluator.Width, _performanceEvaluator.Height);
        }

        private void ResizeRenderTarget(int w, int h)
        {
            if (_renderTarget == null ||
                _renderTarget.Width != w ||
                _renderTarget.Height != h)
            {
                _renderTarget = new RenderTarget2D(GraphicsDevice, w, h);
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
                _spriteBatch.Begin();
                _spriteBatch.Draw(_pixel, _renderTarget.Bounds, Color.Purple);
                _spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // raytrace
                _selectedRaytracingBackend.Draw(_renderTarget, _tracingOptions, gameTime);
            }

            GraphicsDevice.SetRenderTarget(null);
            // simply upscale rendertarget to screen to draw scene
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
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
