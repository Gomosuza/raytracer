using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Components;
using System.Collections.Generic;
using System.Linq;

namespace Raytracer
{
    public class RaytracerGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private PerformanceEvaluator _performanceEvaluator;
        private IRaytracingBackend _selectedRaytracingBackend;
        private IRaytracingBackend[] _raytracingBackends;
        private TracingOptions _tracingOptions;
        private Texture2D _pixel;

        public RaytracerGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            Components.Add(_performanceEvaluator = new PerformanceEvaluator(this));
            Components.Add(new FpsCounter(this));
            Components.Add(new WindowTitle(this));

            var collection = new ServiceCollection();
            collection.Scan(scan =>
            {
                scan.FromAssemblyOf<IRaytracingBackend>()
                    .AddClasses(x => x.AssignableTo<IRaytracingBackend>())
                    .As<IRaytracingBackend>()
                    .WithSingletonLifetime();
            });

            var serviceProvider = collection.BuildServiceProvider();
            _raytracingBackends = serviceProvider.GetRequiredService<IEnumerable<IRaytracingBackend>>()
                .OrderBy(x => x.Name)
                .ToArray();
            _selectedRaytracingBackend = _raytracingBackends.First();

            _tracingOptions = new TracingOptions
            {
                ReflectionLimit = 0,
                SampleCount = 1
            };

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
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
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (_renderTarget == null)
                return;

            // fill rendertarget so we can see if tracing did not fill entirely
            GraphicsDevice.SetRenderTarget(_renderTarget);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_pixel, _renderTarget.Bounds, Color.Purple);
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            // raytrace
            _selectedRaytracingBackend.Draw(_tracingOptions, _renderTarget, gameTime);

            GraphicsDevice.SetRenderTarget(null);
            // simply upscale rendertarget to screen to draw scene
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
            _spriteBatch.End();
        }
    }
}
