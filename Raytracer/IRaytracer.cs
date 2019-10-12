using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Scene;

namespace Raytracer
{
    /// <summary>
    /// Interface for raytracing implementations.
    /// </summary>
    public interface IRaytracer
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
        /// <param name="renderTarget"></param>
        /// <param name="tracingOptions"></param>
        /// <param name="gameTime"></param>
        void Draw(RenderTarget2D renderTarget, ITracingOptions tracingOptions, GameTime gameTime);
    }
}
