using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopCrouchCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyStopCrouchCommand(IPlayer player)
        {
            kirby = player;
        }
        public void Execute()
        {
            kirby.EndCrouch();
        }
    }
}