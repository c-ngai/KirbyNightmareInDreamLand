using MasterGame.Entities.Players;
namespace MasterGame.Commands
{
    public class KirbyChangeSparkCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyChangeSparkCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.ChangeToSpark();
        }

        public void Undo()
        {

        }
    }
}
