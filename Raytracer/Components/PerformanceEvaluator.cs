using Microsoft.Xna.Framework;
using Raytracer.Configuration;
using System;

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
        private readonly Settings _settings;

        public PerformanceEvaluator(
            Game game,
            FpsCounter fpsCounter,
            Settings settings,
            TimeSpan? evalPeriod = null
            ) : base(game)
        {
            _fpsCounter = fpsCounter;
            Width = game.GraphicsDevice.Viewport.Width;
            Height = game.GraphicsDevice.Viewport.Height;
            _evalPeriod = evalPeriod ?? TimeSpan.FromSeconds(5);
            _settings = settings;
        }

        /// <summary>
        /// The (user set) Fps to hit.
        /// If not sustainable at the provided resolution, resolution will be lowered until it is met.
        /// </summary>
        public int TargetFps => _settings.Compute.FpsTarget;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (TargetFps < 0)
                return;

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
                    // TODO: also upscale if player input is stopped
                    Width *= 2;
                    Height *= 2;
                }
            }
        }
    }
}
