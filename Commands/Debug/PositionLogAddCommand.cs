using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class PositionLogAddCommand : ICommand
    {
        private IPlayer _player;
        public PositionLogAddCommand()
        {
            _player = ObjectManager.Instance.Players[0];
        }

        public void Execute()
        {
            GameDebug.Instance.LogPosition(_player.GetKirbyPosition());
        }
    }
}
