using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class AnimatedMovingHorizontallyCommand : ICommand
    {
        public AnimatedMovingHorizontallyCommand() { }

        public void SetState()
        {
            Game1.self.state = 4;
        }

        public void Execute() 
        {
            Game1.self.animatedMovingHorizontallySprite.Draw(Game1.self.spriteBatch, new Vector2(350, 200));
        }
    }
}
