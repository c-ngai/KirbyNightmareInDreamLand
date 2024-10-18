using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyJumpCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.players[0].Jump();
        }
    }
}
