using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeNormalCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyChangeNormalCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.ChangeToNormal();
        }
    }
}
