using MasterGame.Entities.Players;
namespace MasterGame.Commands
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
