using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;
public class MouseController : IController
{
    private Dictionary<ButtonState, ICommand> controllerMappings;

    public MouseController()
    {
        controllerMappings = new Dictionary<ButtonState, ICommand>();
    }

    public void RegisterCommand(ButtonState button, ICommand command)
    {
        controllerMappings.Add(button, command);
    }

    public void Update()
    {
        MouseState state = Mouse.GetState();

        // Create Commands and Register Them

        foreach (KeyValuePair<ButtonState, ICommand> button in controllerMappings)
        {
            controllerMappings[button.Key].Execute();
        }
    }
}