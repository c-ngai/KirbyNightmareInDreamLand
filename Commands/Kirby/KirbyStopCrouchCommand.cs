using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopCrouchCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.Players[0].EndCrouch();

        }
    }
}