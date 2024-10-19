using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyEnterDoorCommand : ICommand
    {
        Game1 _game;
        IPlayer _player;

        public KirbyEnterDoorCommand() {
            _game = Game1.Instance;
            _player = ObjectManager.Instance.Players[0];
        }


        public void Execute()
        {
            _game.Level.nextRoom(_player.GetKirbyPosition());
        }
    }
}
