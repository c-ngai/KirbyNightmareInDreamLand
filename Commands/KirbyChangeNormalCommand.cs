namespace MasterGame
{
    public class KirbyChangeNormalCommand : ICommand
    {
        IPlayer kirby;
        public KirbyChangeNormalCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.ChangeToNormal();
        }

        public void Undo()
        {

        }
    }
}
