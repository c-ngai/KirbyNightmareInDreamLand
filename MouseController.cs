using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;
public class MouseController : IController
{
    private Dictionary<MouseControllerState, ICommand> controllerMappings;

    private Window window;

    public MouseController()
    {
        controllerMappings = new Dictionary<MouseControllerState, ICommand>();
        window = new Window();
    }

    public void SetQuadrant(Window windowInfo, MouseState reference,  MouseControllerState state)
    {
        int horizontalMidPoint = 800 / 2;
        int verticalMidPoint = 450 / 2;

        // TODO: could data drive it by combining a dictionary mapping to an array? 
        if (reference.X <= horizontalMidPoint / 2 && reference.Y <= verticalMidPoint)
        {
            state.quadrant = 1;
        }
        else if (reference.X > horizontalMidPoint && reference.Y <= verticalMidPoint)
        {
            state.quadrant = 2;
        }
        else if (reference.X <= horizontalMidPoint && reference.Y > verticalMidPoint)
        {
            state.quadrant = 3;
        }
        else if (reference.X > horizontalMidPoint && reference.Y > verticalMidPoint)
        {
            state.quadrant = 4;
        } 
    }

    public void RegisterCommand(MouseControllerState state, ICommand command)
    {
        controllerMappings.Add(state, command);
    }

    public void Update()
    {
        MouseState currentState = Mouse.GetState();

        MouseControllerState simpleState = new MouseControllerState();
        Sync(currentState, simpleState);
        SetQuadrant(window, currentState, simpleState);

        if (simpleState.quadrant == 1 && simpleState.leftClick == 1)
        {
            ICommand command = new Command("UnanimatedUnmoving");
            command.Execute();
        } else if (simpleState.quadrant == 2 && simpleState.leftClick == 1)
        {
            ICommand command = new Command("AnimatedUnmoving");
            command.Execute();
        } 
        else if (simpleState.quadrant == 3 && simpleState.leftClick == 1)
        {
            ICommand command = new Command("UnanimatedMovingVertically");
            command.Execute();
        }
        else if (simpleState.quadrant == 4 && simpleState.leftClick == 1)
        {
            ICommand command = new Command("AnimatedMovingHorizontally");
            command.Execute();
        }
        else if(simpleState.rightClick == 1)
        {
            ICommand command = new Command("Quit");
            command.Execute();
        }
    }

    private void Sync(MouseState state, MouseControllerState simpleState)
    {
        // LeftButton and RightButton are ButtonState enums
        simpleState.leftClick = (int) state.LeftButton;
        simpleState.rightClick = (int) state.RightButton;
    }
}