using System.ComponentModel.DataAnnotations;

namespace Raytracer.Configuration
{
    public class SceneSettings
    {
        [Range(20, 180)]
        public int Fov { get; set; } = 90;

        public Sampling Sampling { get; set; }

        [Range(1, int.MaxValue)]
        public int SampleCount { get; set; } = 4;

        [Range(0, int.MaxValue)]
        public int ReflectionLimit { get; set; } = 4;

        public bool OnlyRedrawIfDirty { get; set; } = true;
    }
}
