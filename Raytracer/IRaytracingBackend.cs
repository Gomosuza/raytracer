using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raytracer
{
    /// <summary>
    /// Interface for raytracing implementations.
    /// </summary>
    public interface IRaytracingBackend
    {
        /// <summary>
        /// User friendly name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// User friendly description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// When called must perform a full trace and store the output in the rendertarget.
        /// </summary>
        /// <param name="tracingOptions"></param>
        /// <param name="renderTarget"></param>
        /// <param name="gameTime"></param>
        void Draw(TracingOptions tracingOptions, RenderTarget2D renderTarget, GameTime gameTime);
    }
}
