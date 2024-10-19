using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyTakeDamageCommand : ICommand
    { 

        public void Execute()
        {
            ObjectManager.Instance.Players[0].TakeDamage();

        }
    }
}
