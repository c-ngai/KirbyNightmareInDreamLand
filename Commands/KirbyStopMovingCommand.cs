using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopMovingCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.Players[0].StopMoving();
        }
    }
}