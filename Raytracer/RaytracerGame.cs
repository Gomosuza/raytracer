using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Components;
using Raytracer.Scene;
using Raytracer.Scene.Camera;
using Raytracer.Scene.Objects;
using Raytracer.Scene.Surfaces;
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
        private IRaytracer _selectedRaytracingBackend;
        private IRaytracer[] _raytracingBackends;
        private TracingOptions _tracingOptions;
        private Texture2D _pixel;
        private SceneDescriptor _scene;
        private ICamera _camera;

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
                scan.FromAssemblyOf<IRaytracer>()
                    .AddClasses(x => x.AssignableTo<IRaytracer>())
                    .As<IRaytracer>()
                    .WithSingletonLifetime();
            });

            var serviceProvider = collection.BuildServiceProvider();
            _raytracingBackends = serviceProvider.GetRequiredService<IEnumerable<IRaytracer>>()
                .OrderBy(x => x.Name)
                .ToArray();
            _selectedRaytracingBackend = _raytracingBackends.First();

            _camera = new FpsCamera(new Vector3(0, 2.5f, -5f), Vector3.Zero);

            _scene = LoadScene();
            _tracingOptions = new TracingOptions
            {
                ReflectionLimit = 0,
                SampleCount = 1,
                Camera = _camera,
                Scene = _scene
            };

            base.LoadContent();
        }

        private SceneDescriptor LoadScene()
        {
            var descriptor = new SceneDescriptor();

            descriptor.Add(new Sphere(Vector3.Zero, 2, new SolidColorSurface(Color.Red)));
            return descriptor;
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
            _selectedRaytracingBackend.Draw(_renderTarget, _tracingOptions, gameTime);

            GraphicsDevice.SetRenderTarget(null);
            // simply upscale rendertarget to screen to draw scene
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
            _spriteBatch.End();
        }
    }
}
