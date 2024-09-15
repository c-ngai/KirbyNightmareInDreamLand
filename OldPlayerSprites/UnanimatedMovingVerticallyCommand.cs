using Microsoft.Xna.Framework;
namespace MasterGame
{
    public class UnanimatedMovingVerticallyCommand : ICommand
    {
        public UnanimatedMovingVerticallyCommand(){}

        public void SetState()
        {
            Game1.self.state = 3;
        }
        public void Execute()
        {
            Game1.self.unanimatedMovingVerticallySprite.Draw(Game1.self.spriteBatch, new Vector2(350, 200));
        }
    }
}
