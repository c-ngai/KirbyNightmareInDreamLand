using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFaceLeftCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.players[0].SetDirectionLeft();
        }
    }
}
