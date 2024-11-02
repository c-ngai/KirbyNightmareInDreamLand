using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopAttackingCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.Players[0].StopAttacking();
        }
    }
}