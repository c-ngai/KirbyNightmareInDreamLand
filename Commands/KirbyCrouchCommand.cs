namespace MasterGame
{
    public class KirbyCrouchCommand
    {
        private IPlayer kirby;

        public KirbyCrouchCommand(IPlayer player)
        {
            kirby = player;
        }
        public void Execute()
        {
        
        }
        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
