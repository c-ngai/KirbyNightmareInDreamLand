using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;
public class MouseController : IController
{
    private Dictionary<MouseControllerState, ICommand> controllerMappings;     

    public MouseController()
    {
        controllerMappings = new Dictionary<MouseControllerState, ICommand>();
    }

    public void setQuadrant(Window windowInfo, MouseControllerState state)
    {
        int horizontalMidPoint = windowInfo.width / 2;
        int verticalMidPoint = windowInfo.height / 2;

        // TODO: could data drive it by combining a dictionary mapping to an array? 
        if (state.xPosition <= horizontalMidPoint / 2 && state.yPosition <= verticalMidPoint)
        {
            state.quadrant = 1;
        }
        else if (state.xPosition > horizontalMidPoint && state.yPosition <= verticalMidPoint)
        {
            state.quadrant = 2;
        }
        else if (state.xPosition <= horizontalMidPoint && state.yPosition > verticalMidPoint)
        {
            state.quadrant = 3;
        }
        else if (state.xPosition >= horizontalMidPoint && state.yPosition > verticalMidPoint)
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

        controllerMappings[simpleState].Execute();
    }

    private void Sync(MouseState state, MouseControllerState simpleState)
    {
        // LeftButton and RightButton are ButtonState enums
        simpleState.leftClick = (int) state.LeftButton;
        simpleState.rightClick = (int) state.RightButton;

        simpleState.xPosition = state.X;
        simpleState.yPosition = state.Y;
    }
}