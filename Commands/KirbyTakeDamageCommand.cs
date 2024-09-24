using System.Threading.Tasks;
namespace MasterGame
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

        public async void Undo()
        {
            //await Task.Delay(100);
            kirby.StopMoving();
        }
    }
}
