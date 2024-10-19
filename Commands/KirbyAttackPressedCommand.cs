using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackPressedCommand : ICommand
    {
        public void Execute()
        {
            Game1.Instance.Players[0].AttackPressed();

        }
    }
}
