using MasterGame.Entities.Players;
namespace MasterGame.Commands
{
    public class KirbyChangeBeamCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyChangeBeamCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.ChangeToBeam();
        }

        public void Undo()
        {

        }
    }
}
