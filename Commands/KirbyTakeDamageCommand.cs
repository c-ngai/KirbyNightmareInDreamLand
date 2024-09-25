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

        public void Undo()
        {
<<<<<<< HEAD
=======
            await Task.Delay(200);
>>>>>>> ea90984 (more kirby movement is functional now: jump (though buggy and crouch))
            kirby.StopMoving();
        }
    }
}
