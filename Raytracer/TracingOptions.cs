using Raytracer.Scene;
using Raytracer.Scene.Camera;

namespace Raytracer
{
    public class TracingOptions : ITracingOptions
    {
        public int ReflectionLimit { get; set; }

        public int SampleCount { get; set; }

        public ICamera Camera { get; set; }

        public IScene Scene { get; set; }
    }
}
