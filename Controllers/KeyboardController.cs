using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MasterGame
{
    public class KeyboardController : IController
    {
        private Dictionary<Keys, ICommand> controllerMappings;

        public KeyboardController()
        {
            controllerMappings = new Dictionary<Keys, ICommand>();
        }

        public void RegisterCommand(Keys key, ICommand command)
        {
            controllerMappings.Add(key, command);
        }

        public void Update()
        {
            Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

            // detects key state not transition (i.e. would move Mario, not change the sprite)
            // needs separate method for transition to animate (i.e. keep track of old state and see if current state is different)
            foreach (Keys key in pressedKeys)
            {
                controllerMappings[key].Execute();
            }
        }
    }
}
