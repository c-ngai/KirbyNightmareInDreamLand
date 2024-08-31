using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint0;
public class MouseController : IController
{
    public int leftClickIndex { get; set; }
    public int rightClickIndex { get; set; }
    public int quadrantIndex { get; set; }

    public int leftClickPressed { get; set; }
    public int rightClickPressed { get; set; }
    public int quadrant { get; set; }

    private Dictionary<int[], ICommand> ControllerMappings;

    public MouseController()
    {
        ControllerMappings = new Dictionary<int[], ICommand>();
    }

    public void SetQuadrant(MouseState reference, int[] mouseState)
    {
        int horizontalMidPoint = Game1.self.windowWidth / 2;
        int verticalMidPoint = Game1.self.windowHeight / 2;
 
        // sets quadrants by dividing the game window into equal fourths
        if (reference.X <= horizontalMidPoint / 2 && reference.Y <= verticalMidPoint)
        {
            mouseState[quadrantIndex] = 1;
        }
        else if (reference.X > horizontalMidPoint && reference.Y <= verticalMidPoint)
        {
            mouseState[quadrantIndex] = 2;
        }
        else if (reference.X <= horizontalMidPoint && reference.Y > verticalMidPoint)
        {
            mouseState[quadrantIndex] = 3;
        }
        else if (reference.X > horizontalMidPoint && reference.Y > verticalMidPoint)
        {
            mouseState[quadrantIndex] = 4;
        } 
    }

    public void RegisterCommand(int[] mouseState, ICommand command)
    {
        ControllerMappings.Add(mouseState, command);
    }

    public void Update()
    {
        MouseState currentState = Mouse.GetState();

        int[] mouseState = new int[3];
        Sync(currentState, mouseState);
        SetQuadrant(currentState, mouseState);

        // determines command by quadrant and button click
        if (mouseState[quadrantIndex] == 1 && mouseState[leftClickIndex] == 1)
        {
            ICommand command = new Command("UnanimatedUnmoving");
            command.Execute();
        }
        else if (mouseState[quadrantIndex] == 2 && mouseState[leftClickIndex] == 1)
        {
            ICommand command = new Command("AnimatedUnmoving");
            command.Execute();
        }
        else if (mouseState[quadrantIndex] == 3 && mouseState[leftClickIndex] == 1)
        {
            ICommand command = new Command("UnanimatedMovingVertically");
            command.Execute();
        }
        else if (mouseState[quadrantIndex] == 4 && mouseState[leftClickIndex] == 1)
        {
            ICommand command = new Command("AnimatedMovingHorizontally");
            command.Execute();
        }
        else if (mouseState[rightClickIndex] == 1)
        {
            ICommand command = new Command("Quit");
            command.Execute();
        }

    }

    private void Sync(MouseState state, int[] mouseState)
    {
        // LeftButton and RightButton are ButtonState enums
        mouseState[leftClickIndex]= (int) state.LeftButton;

        mouseState[rightClickIndex] = (int) state.RightButton;
    }
}