using Microsoft.Xna.Framework;
using Raytracer.Input;

namespace Raytracer.Components
{
    public class Exit : GameComponent
    {
        private readonly IActionKeyMap _actionKeyMap;

        public Exit(
            IActionKeyMap actionKeyMap,
            Game game)
            : base(game)
        {
            _actionKeyMap = actionKeyMap;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_actionKeyMap.IsPressed(nameof(InputAction.Exit)))
                Game.Exit();
        }
    }
}
