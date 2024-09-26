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
            /* For this sprint we could just fire the projectile and not implement the entire game sequence of commands */ 
            // if kirby is normal kirby and is floating
            // kirby.Exhale();
            // kirby.Fall();
            // kirby.StopMoving();
            kirby.Attack();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
