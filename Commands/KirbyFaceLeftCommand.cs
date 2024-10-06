using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFaceLeftCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyFaceLeftCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.SetDirectionLeft();
        }
    }
}
