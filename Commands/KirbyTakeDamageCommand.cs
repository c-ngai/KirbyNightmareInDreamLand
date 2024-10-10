using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyTakeDamageCommand : ICommand
    { 
        private IPlayer kirby;

        public KirbyTakeDamageCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.TakeDamage();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
