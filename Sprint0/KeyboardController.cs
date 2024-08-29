using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;
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

        foreach (Keys key in pressedKeys)
        {
            controllerMappings[key].Execute();
        }
    }
}
