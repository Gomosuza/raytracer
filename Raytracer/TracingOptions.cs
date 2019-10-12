namespace Raytracer
{
    public struct TracingOptions
    {
        /// <summary>
        /// Max number of reflections a single ray may cast.
        /// Higher count = more realistic at increased computation cost.
        /// </summary>
        public int ReflectionLimit;

        /// <summary>
        /// Number of rays cast per pixel.
        /// 1 = hard shadow, increased count = soft shadows.
        /// </summary>
        public int SampleCount;
    }
}
