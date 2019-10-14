using Raytracer.Backends;
using System.ComponentModel.DataAnnotations;

namespace Raytracer.Configuration
{
    public class ComputeSettings
    {
        public string Backend { get; set; } = nameof(SingleThreadedSoftwareRaytracer);

        [Range(-1, int.MaxValue)]
        public int FpsTarget { get; set; } = 60;
    }
}
