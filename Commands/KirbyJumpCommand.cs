using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
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
            kirby.Jump();
        }
    }
}
