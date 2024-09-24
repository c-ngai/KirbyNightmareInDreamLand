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
            /* Depends on if move left, move right, jump, or double jump is pressed */
            // Also has longer jump if jump key is held
            // Can only be interrupted by: another jump command (aka now float), move right or move left, or float
        }
        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}
