using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeSparkCommand : ICommand
    {
        private IPlayer kirby;
        public KirbyChangeSparkCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.ChangeToSpark();
        }
    }
}
