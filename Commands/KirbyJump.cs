namespace MasterGame
{
    public class KirbyJump : ICommand
    {
        private IPlayer kirby;

        public KirbyJump(IPlayer newPlayer)
        {
            kirby = newPlayer;
        }
        public void Execute()
        {
            kirby.ChangePose();
            //kirby.Jump();
        }
    }
}
