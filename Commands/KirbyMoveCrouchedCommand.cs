using Microsoft.Xna.Framework.Input;
using System.Linq;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveCrouchedCommand : ICommand
    {
        private IPlayer kirby;

        private KeyboardController keyboard;
        private Keys crouchKey;
        private Keys attackKey;

        public KirbyMoveCrouchedCommand(IPlayer player, Keys keyMapped, Keys currentAttackKey, KeyboardController currentKeyboard)
        {
            kirby = player;
            crouchKey = keyMapped;
            attackKey = currentAttackKey;
            keyboard = currentKeyboard;
        }
        public void Execute()
        {
            kirby.Crouch();
            if (keyboard.currentState.Contains(attackKey)) keyboard.stopKeys[crouchKey].Execute();
        }
    }
}
