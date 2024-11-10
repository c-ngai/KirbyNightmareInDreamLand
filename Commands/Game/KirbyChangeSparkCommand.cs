using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeSparkCommand : ICommand
    {

        public void Execute()
        {
            foreach (IPlayer player in ObjectManager.Instance.Players)
            {
                player.ChangeToSpark();
            }
        }
    }
}
