using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopAttackingCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.players[0].StopAttacking();
        }
    }
}