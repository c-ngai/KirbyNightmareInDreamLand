using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MasterGame
{
    public enum ExecutionType { Pressed, StartingPress, StoppingPress }
    public class KeyboardController : IController
    {
        private Dictionary<Keys, (ICommand, ExecutionType)> controllerMappings;

        private Dictionary<Keys, bool> oldKeyStates;

        // TODO: change this to a public property later;
        public Keys[] currentState;

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

        public void Update()
        {
            currentState = Keyboard.GetState().GetPressedKeys();

            IDictionaryEnumerator enumerator = oldKeyStates.GetEnumerator();
            Dictionary<Keys, bool> tempDict = new Dictionary<Keys, bool>();


            for (int i = 0; i < currentState.Length; i++)
            {
                tempDict.Add(currentState[i], true);
            }

            // TODO: Very ugly rn, I will make it prettier later 
            while (enumerator.MoveNext())
            {
                Keys key = (Keys)enumerator.Key;

                // Determines command execution by the type of command it is
                if (controllerMappings.ContainsKey(key))
                {
                    (ICommand, ExecutionType) commandMapping = (controllerMappings[key]);
                    ICommand command = commandMapping.Item1;
                    ExecutionType type = commandMapping.Item2;

                    // Pressed execution type: executes the command when pressed
                    // Starting press execution type: executes the command when it shifts from not being pressed to pressed
                    if (currentState.Contains(key) && ((type == ExecutionType.Pressed) || (!oldKeyStates[key] && type == ExecutionType.StartingPress)))
                    {
                        command.Execute();
                    }
                    // Pressed execution type: undos the command if it is no longer pressed 
                    else if (oldKeyStates[key] && type == ExecutionType.Pressed)
                    {
                        command.Undo();
                    }

                }

                if (!tempDict.ContainsKey(key)) tempDict.Add(key, false);
            }

            // Stores current iteration state for next iteration's use 
            oldKeyStates = tempDict;
        }
    }
}
