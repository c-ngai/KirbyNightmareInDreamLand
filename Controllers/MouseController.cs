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

    public MouseController(){}

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

    public void Update()
    {
        MouseState currentState = Mouse.GetState();

        int[] mouseState = new int[3];
        Sync(currentState, mouseState);
        SetQuadrant(currentState, mouseState);

        // determines command by quadrant and button click
        if (mouseState[quadrantIndex] == 1 && mouseState[leftClickIndex] == 1)
        {
            Game1.self.unanimatedUnmoving.SetState();
        }
        else if (mouseState[quadrantIndex] == 2 && mouseState[leftClickIndex] == 1)
        {
            Game1.self.animatedUnmoving.SetState();
        }
        else if (mouseState[quadrantIndex] == 3 && mouseState[leftClickIndex] == 1)
        {
            Game1.self.movingVertically.SetState();
        }
        else if (mouseState[quadrantIndex] == 4 && mouseState[leftClickIndex] == 1)
        {
            Game1.self.movingHorizontally.SetState();
        }
        else if (mouseState[rightClickIndex] == 1)
        {
            Game1.self.quit.SetState();
        }

    }

    private void Sync(MouseState state, int[] mouseState)
    {
        // LeftButton and RightButton are ButtonState enums
        mouseState[leftClickIndex]= (int) state.LeftButton;

        mouseState[rightClickIndex] = (int) state.RightButton;
    }
}