namespace MasterGame
{
    public class KirbyMoveRightCommand : ICommand
    {
        private Player kirby;

        public KirbyMoveRightCommand(Player newPlayer)
        {
            kirby = newPlayer;
        }
        public void SetState()
        {
            Game1.self.state = 1;
        }
        public void Execute()
        {
            kirby.MoveRight();
        }
    }
}
