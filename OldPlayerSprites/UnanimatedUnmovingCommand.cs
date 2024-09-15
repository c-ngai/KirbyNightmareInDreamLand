using Microsoft.Xna.Framework;
namespace Sprint0
{
    public class UnanimatedUnmovingCommand : ICommand
    {
        public UnanimatedUnmovingCommand(){}
        public void SetState()
        {
            Game1.self.state = 1;
        }

        public void Execute()
        {
            Game1.self.unanimatedUnmovingSprite.Draw(Game1.self.spriteBatch, new Vector2(350, 200));
        }

    }
}
