using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyFaceLeftCommand : ICommand
    {
        public void Execute()
        {
            Game1.Instance.players[0].SetDirectionLeft();
        }
    }
}
