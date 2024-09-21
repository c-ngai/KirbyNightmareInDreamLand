namespace MasterGame
{
    public class KirbyMoveLeftCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyMoveLeftCommand(IPlayer newPlayer)
        {
            kirby = newPlayer;
        }

        public void Execute()
        {
            kirby.MoveLeft();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
