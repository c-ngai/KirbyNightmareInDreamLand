using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{   
    public class PlayerMovement
    {
        private GraphicsDevice graphicsDevice;
        var viewHeight = graphicsDevice.Viewport.Height;
        var viewWidth = graphicsDevice.Viewport.Width;

        int floor = viewHeight * (2/3);
        //light hard coded physics
        //seperate movement and state 
        int yVel = 0
        int jumpVel = 10;
        int xVel = 10;
        int gravity = -10;
        double frameRate = (1.0/60.0);

        //change kirby velocity to go left
        public void MoveLeft()
        {
            xVel = - 10;
        }
        //change kirby velocity to go right
        public void MoveRight()
        {
            xVel = 10;
        }
        //change kirby yVelcoity to jump
        public void Jump() 
        {
            if(yVel == 0){
                yVel = jumpVel;
            }
        }

        //floats kirby
        public void Float()
        {
            
        }

        //update kirby position in UI
        public void UpdatePosition()
        {
            position.X += xVel;
            position.Y += yVel * frameRate + .5* gravity * frameRate * frameRate;
            yVel += gravity *frameRate;
        }
        // checks palyer doesnt go out of frame (up and down)
        public void AdjustX()
        {

        }

        public void AdjustY()
        {
            //dont go through the floor
            if(position.Y > floor)
            {
                yVel = 0;
                position.Y = floor;
            }

            //dont go through the ceiling
            if(position.Y < 0)
            {
                yVel = 0;
                position.Y = 0 + 10;
            }

        }

        public void Adjust()
        {
            AdjustX();
            AdjustY();
        }
        //updates position and adjusts frame. 
        public void MovePlayer()
        {
            UpdatePosition();
            Adjust();
        }
    }
}
