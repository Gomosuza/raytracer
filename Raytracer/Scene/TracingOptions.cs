using Raytracer.Scene.Camera;

namespace Raytracer.Scene
{
    public class TracingOptions : ITracingOptions
    {
        public TracingOptions(ICamera camera, IScene scene)
        {
            Camera = camera;
            Scene = scene;
        }

        public int ReflectionLimit { get; set; } = 0;

        public int SampleCount { get; set; } = 1;

        public ICamera Camera { get; }

        public IScene Scene { get; }
    }
}
