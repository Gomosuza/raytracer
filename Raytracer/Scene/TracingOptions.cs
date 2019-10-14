using Raytracer.Configuration;
using Raytracer.Scene.Camera;

namespace Raytracer.Scene
{
    public class TracingOptions : ITracingOptions
    {
        public TracingOptions(ICamera camera, IScene scene, Settings settings)
        {
            Camera = camera;
            Scene = scene;
            ReflectionLimit = settings.Scene.ReflectionLimit;
            SampleCount = settings.Scene.SampleCount;
            OnlyRedrawIfDirty = settings.Scene.OnlyRedrawIfDirty;
        }

        public int ReflectionLimit { get; set; }

        public int SampleCount { get; set; }

        public bool OnlyRedrawIfDirty { get; set; }

        public ICamera Camera { get; }

        public IScene Scene { get; }
    }
}
