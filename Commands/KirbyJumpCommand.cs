namespace MasterGame
{
    public class KirbyJumpCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyJumpCommand(IPlayer newPlayer)
        {
            kirby = newPlayer;
        }

        public void Execute()
        {
            //kirby.Jump();
        }
        public void Undo()
        {

        }
    }
}
