using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Raytracer.Scene.Camera;

namespace Raytracer.Components
{
    public class Player : GameComponent
    {
        private KeyboardState _previousState, _currentState;
        private MouseState _previousMouseState, _currentMouseState;
        private readonly ICamera _camera;
        private readonly float _movementSpeed;

        public Player(
            Game game,
            ICamera camera,
            float movementSpeed = 1f
            ) : base(game)
        {
            _camera = camera;
            _movementSpeed = movementSpeed;
        }

        public override void Initialize()
        {
            base.Initialize();
            CenterMouse();
            _previousMouseState = _currentMouseState = Mouse.GetState();
            _previousState = _currentState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Game.IsActive)
                return;

            _currentMouseState = Mouse.GetState();
            _currentState = Keyboard.GetState();

            Exit();
            Rotate(gameTime);
            Move(gameTime);

            _previousState = _currentState;
        }

        private void Exit()
        {
            if (IsPressed(Keys.Escape))
                Game.Exit();
        }

        private void Rotate(GameTime gameTime)
        {
            var delta = _currentMouseState.Position - _previousMouseState.Position;
            var x = delta.X;
            var y = delta.Y;
            if (x == 0 && y == 0)
                return;

            const float mouseSpeed = 0.1f;
            _camera.Rotate(x * mouseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, y * mouseSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            CenterMouse();
            _previousMouseState = Mouse.GetState();
        }

        private void CenterMouse()
            => Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);

        private void Move(GameTime gameTime)
        {
            var movement = Vector3.Zero;
            if (IsPressed(Keys.W))
                movement += Vector3.UnitZ;
            if (IsPressed(Keys.S))
                movement -= Vector3.UnitZ;

            if (IsPressed(Keys.A))
                movement -= Vector3.UnitX;
            if (IsPressed(Keys.D))
                movement += Vector3.UnitX;

            if (IsPressed(Keys.Space))
                movement += Vector3.UnitY;
            if (IsPressed(Keys.LeftShift))
                movement -= Vector3.UnitY;

            if (movement != Vector3.Zero)
                _camera.Move(Vector3.Normalize(movement) * _movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private bool IsPressed(Keys k)
            => _currentState.IsKeyDown(k);
    }
}
