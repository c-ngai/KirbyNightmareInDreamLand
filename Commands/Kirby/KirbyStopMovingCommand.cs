using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopMovingCommand : ICommand
    {
        private IPlayer _player;
        public KirbyStopMovingCommand()
        {
            _player = ObjectManager.Instance.Players[0];
        }

        public void Execute()
        {
            _player.StopMoving();
        }
    }
}