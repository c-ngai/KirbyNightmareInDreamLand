using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopMovingCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyStopMovingCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.StopMoving();
        }
    }
}