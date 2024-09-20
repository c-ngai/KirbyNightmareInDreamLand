namespace MasterGame
{
    public class KirbyMoveLeftCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyMoveLeftCommand(IPlayer newPlayer)
        {
            kirby = newPlayer;
        }

        public void SetState()
        {

        }
        public void Execute()
        {
            //kirby.MoveLeft();
        }
    }
}
