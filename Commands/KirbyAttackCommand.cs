using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework.Audio;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.Players[0].Attack();
        }
    }
}
