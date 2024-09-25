namespace MasterGame
{
    public class KirbyChangeSparkCommand : ICommand
    {
        IPlayer kirby;
        public KirbyChangeSparkCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            // kirby.ChangeToSpark();
        }

        public void Undo()
        {

        }
    }
}
