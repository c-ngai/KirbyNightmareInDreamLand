using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeBeamCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.Players[0].ChangeToBeam();
        }
    }
}
