using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Raytracer.Scene.Camera;
using System;

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _previousMouseState = _currentMouseState;
            _previousState = _currentState;

            _currentMouseState = Mouse.GetState();
            _currentState = Keyboard.GetState();

            if (IsPressed(Keys.Escape))
                Game.Exit();

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
