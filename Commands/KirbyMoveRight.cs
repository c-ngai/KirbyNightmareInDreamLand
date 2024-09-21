namespace MasterGame
{
    public class KirbyMoveRightCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyMoveRightCommand()
        {
            kirby = Game1.self.kirby;
        }
        // public void SetState()
        // {
        //     Game1.self.state = 1;
        // }
        public void Execute()
        {
            kirby.MoveRight();
        }
    }
}
