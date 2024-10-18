using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopCrouchCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.players[0].EndCrouch();

        }
    }
}