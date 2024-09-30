using MasterGame.Entities.Players;
namespace MasterGame.Commands
{
    public class KirbyChangeNormalCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyChangeNormalCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.ChangeToNormal();
        }

        public void Undo()
        {

        }
    }
}
