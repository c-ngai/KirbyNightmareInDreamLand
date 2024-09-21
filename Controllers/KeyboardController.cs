using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MasterGame
{
    public class KeyboardController : IController
    {
        private Dictionary<Keys, ICommand> controllerMappings;

        private Dictionary<Keys, bool> oldKeyStates;

        public KeyboardController()
        {
            controllerMappings = new Dictionary<Keys, ICommand>();
            oldKeyStates = new Dictionary<Keys, bool>();
        }

        public void RegisterCommand(Keys key, ICommand command)
        {
            controllerMappings.Add(key, command);
            oldKeyStates.Add(key, false);
        }

        public void Update()
        {
            Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

            IDictionaryEnumerator enumerator = oldKeyStates.GetEnumerator();
            Dictionary<Keys, bool> tempDict = new Dictionary<Keys, bool>();

            for (int i = 0; i < pressedKeys.Length; i++)
            {
                tempDict.Add(pressedKeys[i], true);
            }

            // Only executes if there has been a state change for the key pressed
            while (enumerator.MoveNext())
            {
                Keys key = (Keys)enumerator.Key;

                // Executes the current key if it is being pressed, if a previous key that was being pressed
                // is now no longer pressed, undo that key command
                if (controllerMappings.ContainsKey(key))
                {
                    if (pressedKeys.Contains(key))
                    {
                        controllerMappings[key].Execute();
                    }
                    else if (oldKeyStates[key] != false)
                    {
                        controllerMappings[key].Undo();
                    }

                }

                if (!tempDict.ContainsKey(key)) tempDict.Add(key, false);
            }

            // Stores current iteration state for next iteration's use 
            oldKeyStates = tempDict;
        }
    }
}
