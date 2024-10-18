using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFloatCommand : ICommand
    {
        Game1 _game;
        IPlayer _player;

        public KirbyFloatCommand()
        {
            _game = Game1.Instance;
            _player = ObjectManager.Instance.players[0];
        }

        public void Execute()
        {
            // If kirby is not in a door, float
            if (!_game.level.atDoor(_player.GetKirbyPosition()))
            {
                _player.Float();
            }
        }
    }
}
