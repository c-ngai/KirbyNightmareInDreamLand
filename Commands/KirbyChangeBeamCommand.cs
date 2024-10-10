using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
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
