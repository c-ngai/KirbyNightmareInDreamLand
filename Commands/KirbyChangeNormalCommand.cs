using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeNormalCommand : ICommand
    {
        public void Execute()
        {
            Game1.Instance.Players[0].ChangeToNormal();
        }
    }
}
