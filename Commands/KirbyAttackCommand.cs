using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.players[0].Attack();
        }
    }
}
