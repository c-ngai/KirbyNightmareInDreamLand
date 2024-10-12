using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyStopAttackingCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyStopAttackingCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.StopAttacking();
        }
    }
}