using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeFireCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyChangeFireCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.ChangeToFire();
        }
    }
}
