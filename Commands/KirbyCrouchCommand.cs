namespace MasterGame
{
    public class KirbyCrouchCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyCrouchCommand(IPlayer player)
        {
            kirby = player;
        }
        public void Execute()
        {
            kirby.Crouch();
        }
        public void Undo()
        {
            kirby.EndCrouch();
        }
    }
}
