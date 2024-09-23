using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class ResetCommand : ICommand
    {
        private Game1 game;
        public ResetCommand(Game1 currentGame)
        {
            game = currentGame;
        }

        // is there a better way to do this beyond resetting every single game object?
        public void Execute()
        {
            //game.kirby = new Player(new Vector2(30, game.gameHeight * 4 / 5));
            //game.kirby.PlayerSprite = SpriteFactory.Instance.createSprite("kirby_normal_standing_right");
        }
        public void Undo()
        {

        }

    }
}
