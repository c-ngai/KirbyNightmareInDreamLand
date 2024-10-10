using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyAttackCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.Attack();
        }
    }
}
