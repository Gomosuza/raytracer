using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;
using Raytracer.Scene.Camera;
using Raytracer.Scene.Objects;
using Raytracer.Scene.Surfaces;
using System.Collections.Generic;

namespace Raytracer
{
    public class RaytracerGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        public RaytracerGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            var collection = new ServiceCollection();
            collection.AddSingleton<Game>(this);
            collection.AddSingleton(_graphicsDeviceManager);
            collection.AddSingleton<IGraphicsDeviceManager>(_graphicsDeviceManager);
            collection.AddSingleton<IGraphicsDeviceService>(_graphicsDeviceManager);
            collection.AddSingleton<ITracingOptions, TracingOptions>();
            collection.AddSingleton<ICamera>(new FpsCamera(GraphicsDevice, new Vector3(0, 0, -5f), Vector3.Zero));
            collection.AddSingleton(LoadScene());

            collection.Scan(scan =>
            {
                scan
                .FromAssemblyOf<IRaytracer>()
                .AddClasses(x => x.AssignableTo<IRaytracer>())
                .As<IRaytracer>()
                .WithSingletonLifetime()
                .AddClasses(x => x.AssignableTo<GameComponent>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime();
            });

            using var serviceProvider = collection.BuildServiceProvider();

            var tracingOptions = serviceProvider.GetRequiredService<ITracingOptions>();
            tracingOptions.ReflectionLimit = 0;
            tracingOptions.SampleCount = 1;

            foreach (var c in serviceProvider.GetRequiredService<IEnumerable<IGameComponent>>())
                Components.Add(c);
        }

        private IScene LoadScene()
        {
            // TODO: load from file
            var descriptor = new SceneDescriptor();

            descriptor.Add(new Sphere(Vector3.Zero, 2, new SolidColorSurface(Color.Red)));
            return descriptor;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
