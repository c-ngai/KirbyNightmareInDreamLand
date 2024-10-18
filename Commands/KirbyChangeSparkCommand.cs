using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeSparkCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.players[0].ChangeToSpark();

        }
    }
}
