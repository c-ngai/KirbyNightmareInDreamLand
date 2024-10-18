using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFaceRightCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.players[0].SetDirectionRight();
        }
    }
}
