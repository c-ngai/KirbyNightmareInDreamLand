using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{   
    public class PlayerMovement
    {
        private int gameWidth;
        private int gameHeight;
        int floor;
        //light hard coded physics
        //seperate movement and state 
        float yVel = 0;
        float xVel = 0;
        float jumpVel = 10f;
        float leftVel = -.375f;
        float rightVel = .375f;
        float gravity = -10f;
        double frameRate = (1.0/60.0);

        //change kirby velocity to go left
        public PlayerMovement()
        {
            gameWidth = Game1.self.windowWidth;
            gameHeight = Game1.self.windowHeight;
            floor =  gameHeight * 2/3;
        }
        public void MoveLeft()
        {
            xVel = leftVel;
        }
        //change kirby velocity to go right
        public void MoveRight()
        {
            xVel = rightVel;
        }
        //no kirby shouldnt move
        public void StopMoving()
        {
            xVel = 0;
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
            //float newY =(float) (yVel * frameRate + .5 * gravity * frameRate * frameRate + kirby.position.Y);
            kirby.position.X += xVel;
            //kirby.position.Y = newY;
            //yVel += gravity *frameRate;
        }

        // checks palyer doesnt go out of frame (up and down)
        public void AdjustX(Player kirby)
        {
            if(kirby.position.X > 200)
            {
                kirby.position.X  = 100;  
            }
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
            if(kirby.position.Y < 0)
            {
                yVel = 0;
                //kirby.position.Y = 0 + 10;
            }

        }
        //ensures sprite does not leave the window
        public void Adjust(Player kirby)
        {
            AdjustX(kirby);
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
