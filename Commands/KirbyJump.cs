namespace MasterGame
{
    public class KirbyJump : ICommand
    {
        private IPlayer kirby;

        public KirbyJump(IPlayer newPlayer)
        {
            kirby = newPlayer;
        }

        public void SetState()
        {

        }
        public void Execute()
        {
            //kirby.Jump();
        }
    }
}
