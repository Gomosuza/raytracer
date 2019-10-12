using Raytracer.Scene;
using Raytracer.Scene.Camera;

namespace Raytracer
{
    public interface ITracingOptions
    {
        /// <summary>
        /// Max number of reflections a single ray may cast.
        /// Higher count = more realistic at increased computation cost.
        /// </summary>
        int ReflectionLimit { get; }

        /// <summary>
        /// Number of rays cast per pixel.
        /// 1 = hard shadow, increased count = soft shadows.
        /// </summary>
        int SampleCount { get; }

        ICamera Camera { get; }

        IScene Scene { get; }
    }
}
