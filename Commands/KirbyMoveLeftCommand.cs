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
            // If it switches from moving left to moving right it needs a skid animation
            kirby.MoveLeft();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
