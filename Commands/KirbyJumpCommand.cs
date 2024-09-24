using System.Threading.Tasks;
namespace MasterGame
{
    public class KirbyJumpCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyJumpCommand(IPlayer newPlayer)
        {
            kirby = newPlayer;
        }

        public void Execute()
        {
            kirby.JumpY();
        }
        public void Undo()
        {
            //kirby.StopMoving();
        }
    }
}
