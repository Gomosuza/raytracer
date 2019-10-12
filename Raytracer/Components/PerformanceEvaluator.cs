using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Raytracer.Components
{
    /// <summary>
    /// Helper that returns the performance target and evaluates if it is matched.
    /// </summary>
    public class PerformanceEvaluator : GameComponent
    {
        private FpsCounter _fpsCounter;
        private readonly TimeSpan _evalPeriod;
        private TimeSpan _passedTime;

        public PerformanceEvaluator(Game game, TimeSpan? evalPeriod = null) : base(game)
        {
            Width = game.GraphicsDevice.Viewport.Width;
            Height = game.GraphicsDevice.Viewport.Height;
            _evalPeriod = evalPeriod ?? TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// The (user set) Fps to hit.
        /// If not sustainable at the provided resolution, resolution will be lowered until it is met.
        /// </summary>
        public int TargetFps { get; set; } = 55;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (_fpsCounter == null)
                _fpsCounter = Game.Components.OfType<FpsCounter>().First();

            _passedTime += gameTime.ElapsedGameTime;
            if (_passedTime >= _evalPeriod)
            {
                _passedTime -= _evalPeriod;

                if (_fpsCounter.CurrentFps < TargetFps)
                {
                    // performance target not hit. half res
                    Width /= 2;
                    Height /= 2;
                }
                else if (_fpsCounter.CurrentFps > TargetFps * 2)
                {
                    // performance target exceeded. increase res
                    Width *= 2;
                    Height *= 2;
                }
            }

            base.Update(gameTime);
        }
    }
}
