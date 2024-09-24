namespace MasterGame.Commands
{
    public class KirbyInhaleCommand : ICommand
    {
        IPlayer kirby;
        public KirbyInhaleCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            // kirby.Inhale();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
