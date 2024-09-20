using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{   
    public class PlayerMovement
    {
        private GraphicsDevice graphicsDevice;
        //var viewHeight = graphicsDevice.Viewport.Height;
        //var viewWidth = graphicsDevice.Viewport.Width;

        int floor = 200;
        //light hard coded physics
        //seperate movement and state 
        double yVel = 0;
        double jumpVel = 10;
        double xVel = 10;
        double gravity = -10;
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
        public void UpdatePosition(Player kirby)
        {
            float y = kirby.GetYPos();
            float newY =(float) (yVel * frameRate + .5 * gravity * frameRate * frameRate + y);
            kirby.SetXPos(kirby.GetXPos() + (float)xVel);
            kirby.SetYPos(newY);
            yVel += gravity *frameRate;
        }
        // checks palyer doesnt go out of frame (up and down)
        public void AdjustX()
        {

        }

        public void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(kirby.position.Y > floor)
            {
                yVel = 0;
                //kirby.position.Y = (float) floor;
            }

            //dont go through the ceiling
            if(kirby.GetYPos() < 0)
            {
                yVel = 0;
                //kirby.position.Y = 0 + 10;
            }

        }

        public void Adjust(Player kirby)
        {
            AdjustX();
            AdjustY(kirby);
        }
        //updates position and adjusts frame. 
        public void MovePlayer(Player kirby)
        {
            UpdatePosition(kirby);
            Adjust(kirby);
        }
    }
}
