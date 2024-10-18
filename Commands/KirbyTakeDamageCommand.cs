using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyTakeDamageCommand : ICommand
    { 

        public void Execute()
        {
            ObjectManager.Instance.players[0].TakeDamage();

        }
    }
}
