using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveLeftCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.Players[0].MoveLeft();

            var keyboard = Game1.Instance.Keyboard;

            if (keyboard.stopKeys.ContainsKey(Keys.Left))
            {
                keyboard.stopKeys[Keys.Left].Execute();
            }
        }
    }
}