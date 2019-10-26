using Raytracer.Scene.Camera;

namespace Raytracer.Scene
{
    public interface ITracingOptions
    {
        int Fov { get; }

        /// <summary>
        /// Max number of reflections a single ray may cast.
        /// Higher count = more realistic at increased computation cost.
        /// </summary>
        int ReflectionLimit { get; set; }

        /// <summary>
        /// Number of rays cast per pixel.
        /// 1 = hard shadow, increased count = soft shadows.
        /// </summary>
        int SampleCount { get; set; }

        /// <summary>
        /// If true, no redraw is performed when the scene didn't change.
        /// (E.g. when no user input occurs).
        /// </summary>
        bool OnlyRedrawIfDirty { get; set; }

        ICamera Camera { get; }

        IScene Scene { get; }
    }
}
