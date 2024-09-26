namespace MasterGame
{
    public class KirbyAttackCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyAttackCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.Attack();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
