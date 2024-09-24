namespace MasterGame
{
    public class KirbyMoveRightCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyMoveRightCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            // if it switches from moving right to moving left it needs a skid animation 
            kirby.MoveRight();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
