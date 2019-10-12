using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raytracer.Components;

namespace Raytracer
{
    public class RaytracerGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _renderTarget;
        private PerformanceEvaluator _performanceEvaluator;

        public RaytracerGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Components.Add(_performanceEvaluator = new PerformanceEvaluator(this));
            Components.Add(new FpsCounter(this));
            Components.Add(new WindowTitle(this));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ResizeRenderTarget(_performanceEvaluator.Width, _performanceEvaluator.Height);
        }

        private void ResizeRenderTarget(int w, int h)
        {
            if (_renderTarget == null ||
                _renderTarget.Width != w ||
                _renderTarget.Height != h)
            {
                _renderTarget = new Texture2D(GraphicsDevice, w, h);
                var empty = new Color[w * h];
                for (int i = 0; i < empty.Length; i++)
                    empty[i] = Color.Purple;
                _renderTarget.SetData(empty);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (_renderTarget == null)
                return;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
            _spriteBatch.End();
        }
    }
}
