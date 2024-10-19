using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.Players[0].Attack();
        }
    }
}
