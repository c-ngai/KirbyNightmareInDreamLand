using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackPressedCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.players[0].AttackPressed();

        }
    }
}
