using MasterGame.Entities.Players;
namespace MasterGame.Commands
{
    public class KirbyFaceLeftCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyFaceLeftCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.SetDirectionLeft();
        }

        public void Undo()
        {

        }
    }
}
