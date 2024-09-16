using Microsoft.Xna.Framework;
namespace MasterGame
{
    public class AnimatedUnmovingCommand : ICommand
    {
        public AnimatedUnmovingCommand(){}

        public void SetState()
        {
            Game1.self.state = 2;
        }

        public void Execute()
        {
            Game1.self.animatedUnmovingSprite.Draw(Game1.self.spriteBatch, new Vector2(350, 200));
        }

    }
}
