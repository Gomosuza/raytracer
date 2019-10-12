using Microsoft.Xna.Framework;
using System.Linq;

namespace Raytracer.Components
{
    public class WindowTitle : GameComponent
    {
        private int _lastFps;
        private FpsCounter _fpsCounter;
        private PerformanceEvaluator _performanceEvaluator;
        private int _lastWidth, _lastHeight;

        public WindowTitle(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_fpsCounter == null)
            {
                _fpsCounter = Game.Components.OfType<FpsCounter>().First();
                _performanceEvaluator = Game.Components.OfType<PerformanceEvaluator>().First();
            }
            bool dirty = false;
            if (_lastFps != _fpsCounter.CurrentFps)
            {
                _lastFps = _fpsCounter.CurrentFps;
                dirty = true;
            }
            if (_performanceEvaluator.Width != _lastWidth ||
                _performanceEvaluator.Height != _lastHeight)
            {
                _lastWidth = _performanceEvaluator.Width;
                _lastHeight = _performanceEvaluator.Height;
                dirty = true;
            }
            if (dirty)
            {
                Game.Window.Title = $"Raytracer - {_lastFps:00} FPS ({_lastWidth}x{_lastHeight})";
            }
        }
    }
}
