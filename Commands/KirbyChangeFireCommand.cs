namespace MasterGame
{
    public class KirbyChangeFireCommand : ICommand
    {
        IPlayer kirby;
        public KirbyChangeFireCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            // kirby.ChangeToFire();
        }

        public void Undo()
        {

        }
    }
}
