using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFaceRightCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyFaceRightCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.SetDirectionRight();
        }

        public void Undo()
        {
        }
    }
}
