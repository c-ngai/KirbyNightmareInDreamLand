using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeNormalCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.Players[0].ChangeToNormal();
        }
    }
}
