namespace MasterGame
{
    public class KirbyChangeBeamCommand : ICommand
    {
        IPlayer kirby;
        public KirbyChangeBeamCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            //kirby.ChangeToBeam();
        }

        public void Undo()
        {

        }
    }
}
