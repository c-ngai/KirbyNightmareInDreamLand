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
            kirby.MoveRight();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
