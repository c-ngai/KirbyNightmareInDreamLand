using MasterGame.Entities.Players;
namespace MasterGame.Commands
{
    public class KirbyFloatCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyFloatCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.Float();
        }

        public void Undo()
        {

        }
    }
}
