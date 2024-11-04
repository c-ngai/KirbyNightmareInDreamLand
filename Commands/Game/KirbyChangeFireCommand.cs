using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeFireCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.Players[0].ChangeToFire();

        }
    }
}
