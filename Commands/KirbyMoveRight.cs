namespace MasterGame
{
    public class KirbyMoveRightCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyMoveRightCommand(IPlayer newPlayer)
        {
            kirby = newPlayer;
        }
        public void Execute()
        {
            //kirby.MoveRight();
        }
    }
}
