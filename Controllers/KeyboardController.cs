using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using MasterGame.Commands;

namespace MasterGame.Controllers
{
    public enum ExecutionType { Pressed, StartingPress, StoppingPress }
    public class KeyboardController : IController
    {
        private Dictionary<Keys, (ICommand, ExecutionType)> controllerMappings;

        public Dictionary<Keys, bool> oldKeyStates { get; set; }

        public Keys[] currentState { get; set; }

        public KeyboardController()
        {
            controllerMappings = new Dictionary<Keys, (ICommand, ExecutionType)>();
            oldKeyStates = new Dictionary<Keys, bool>();
        }

        public void RegisterCommand(Keys key, ICommand command, ExecutionType type)
        {
            (ICommand, ExecutionType) commandMapping = (command, type);
            controllerMappings.Add(key, commandMapping);
            oldKeyStates.Add(key, false);
        }

        // This will all be refactored after Sprint2
        public void Update()
        {
            currentState = Keyboard.GetState().GetPressedKeys();

            IDictionaryEnumerator enumerator = oldKeyStates.GetEnumerator();
            // A temporary dictionary was used because there isn't an update map method
            Dictionary<Keys, bool> tempDict = new Dictionary<Keys, bool>();

            // Adds all currently pressed keys into the temporary dictionary
            for (int i = 0; i < currentState.Length; i++)
            {
                tempDict.Add(currentState[i], true);
            }

            // Iterates through all relevant keybinds
            while (enumerator.MoveNext())
            {
                Keys key = (Keys)enumerator.Key;

                // Checks it is a relevant keybind 
                if (controllerMappings.ContainsKey(key))
                {
                    (ICommand, ExecutionType) commandMapping = (controllerMappings[key]);
                    ICommand command = commandMapping.Item1;
                    ExecutionType type = commandMapping.Item2;

                    /* A little less efficient to move all of these out of the if-else conditions but done so for readability */
                    // Pressed execution type: executes the command when pressed
                    bool isPressedCommand = currentState.Contains(key) && type == ExecutionType.Pressed;
                    // Starting press execution type: executes the command when it shifts from not being pressed to pressed
                    bool isStartingPressComamnd = currentState.Contains(key) && !oldKeyStates[key] && type == ExecutionType.StartingPress;
                    // Pressed execution type: undos the command if it is no longer pressed 
                    bool isNoLongerPressed = oldKeyStates[key] && type == ExecutionType.Pressed;

                    if (isPressedCommand || isStartingPressComamnd)
                    {
                        command.Execute();
                    }
                    else if (isNoLongerPressed)
                    {
                        command.Undo();
                    }

                }
                // If the temporary dictionary does not have the key already then it has not been pressed
                if (!tempDict.ContainsKey(key)) tempDict.Add(key, false);
            }

            // Stores current iteration state for next iteration's use 
            oldKeyStates = tempDict;
        }
    }
}
