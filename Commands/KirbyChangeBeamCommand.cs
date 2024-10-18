using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeBeamCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.players[0].ChangeToBeam();
        }
    }
}
