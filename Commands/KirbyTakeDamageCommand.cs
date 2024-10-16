using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyTakeDamageCommand : ICommand
    { 

        public void Execute()
        {
            Game1.Instance.players[0].TakeDamage();

        }
    }
}
