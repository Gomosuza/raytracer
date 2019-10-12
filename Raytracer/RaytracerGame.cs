using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;
using Raytracer.Scene.Camera;
using Raytracer.Scene.Objects;
using Raytracer.Scene.Surfaces;
using System;
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
            collection.AddSingleton<ICamera>(new FpsCamera(GraphicsDevice, new Vector3(0, 2, -5f), new Vector3(0, 2, 0)));
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

            descriptor.Add(new Scene.Objects.Plane(Vector3.Up, 0, new SolidColorSurface(Parse("A34400"))));
            descriptor.Add(new Sphere(new Vector3(0, 2, 0), 2, new SolidColorSurface(Parse("36D215"))));
            descriptor.Add(new Sphere(new Vector3(3, 2, 0), 1, new SolidColorSurface(Parse("1F51A7"))));
            descriptor.Add(new Sphere(new Vector3(-2, 1.5f, -2), 0.75f, new SolidColorSurface(Parse("FFA86A"))));
            return descriptor;
        }

        public Color Parse(string hex)
        {
            var rgb = Convert.ToInt32(hex, 16);
            return new Color((rgb & 0xff0000) >> 16, (rgb & 0x00ff00) >> 8, rgb & 0x0000ff);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
