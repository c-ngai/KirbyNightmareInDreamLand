using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackPressedCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyAttackPressedCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.AttackPressed();
        }
    }
}
