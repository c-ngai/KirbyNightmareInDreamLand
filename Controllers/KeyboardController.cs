﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Commands;

namespace KirbyNightmareInDreamLand.Controllers
{
    public enum ExecutionType { Pressed, StartingPress, StoppingPress }
    public class KeyboardController : IController
    {
        private Dictionary<Keys, ICommand> controllerMappings;
        private Dictionary<Keys, ICommand> pressedKeys;
        private Dictionary<Keys, ICommand> startKeys;
        public Dictionary<Keys, ICommand> stopKeys { get; set; }

        public Dictionary<Keys, bool> oldKeyStates { get; set; }

        public Keys[] currentState { get; set; }

        public KeyboardController()
        {
            controllerMappings = new Dictionary<Keys, ICommand>();
            pressedKeys = new Dictionary<Keys, ICommand>();
            startKeys = new Dictionary<Keys, ICommand>();
            stopKeys = new Dictionary<Keys, ICommand>();
            oldKeyStates = new Dictionary<Keys, bool>();
        }

        public void RegisterCommand(Keys key, ICommand command, ICommand stop, ExecutionType type)
        {
            if (type == ExecutionType.Pressed)
            {
                pressedKeys.Add(key, command);
                if (stop != null) stopKeys.Add(key, stop);
            }
            else if (type == ExecutionType.StartingPress)
            {
                startKeys.Add(key, command);
            }
            else
            {
                stopKeys.Add(key, command);
            }

            controllerMappings.Add(key, command);
            oldKeyStates.Add(key, false);
        }

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
                    // Execute pressed keys if it is currently being pressed
                    if (pressedKeys.ContainsKey(key) && currentState.Contains(key)) pressedKeys[key].Execute();

                    // Execute start keys if is is now pressed and was not pressed last update
                    if (startKeys.ContainsKey(key) && currentState.Contains(key) && !oldKeyStates[key]) startKeys[key].Execute();

                    // Execute stop keys if it is no longer pressed and was pressed last update
                    if (stopKeys.ContainsKey(key) && !currentState.Contains(key) && oldKeyStates[key]) stopKeys[key].Execute();
                }
                // If the temporary dictionary does not have the key already then it has not been pressed
                if (!tempDict.ContainsKey(key)) tempDict.Add(key, false);
            }

            // Stores current iteration state for next iteration's use 
            oldKeyStates = tempDict;
        }
    }
}
