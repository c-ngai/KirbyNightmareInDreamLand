using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyEnterDoorCommand : ICommand
    {
        Game1 _game;
        IPlayer _player;

        public KirbyEnterDoorCommand() {
            _game = Game1.Instance;
            _player = _game.players[0];
        }


        public void Execute()
        {
            _game.level.nextRoom(_player.GetKirbyPosition());
        }
    }
}
