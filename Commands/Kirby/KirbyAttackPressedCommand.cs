using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackPressedCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.Players[0].AttackPressed();

        }
    }
}
