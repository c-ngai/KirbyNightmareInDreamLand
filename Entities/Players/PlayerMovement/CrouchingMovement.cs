using System;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class CrouchingMovement : PlayerMovement
    {
        public CrouchingMovement(Vector2 pos) : base(pos){}

        private double timer = 0;

        public override void Walk(bool isLeft)
        {
            //does nothing
        }

        public override void Run(bool isLeft)
        {
            //does nothing
        }
        //starts kirby is sliding
        public override void Attack(Player kirby)
        {
            kirby.Slide();
            if(kirby.IsSliding())
            {   
                xVel = kirby.IsLeft() ? runningVel * -1 :runningVel;
            }
        }

        public override void Jump(bool isLeft)
        {
            //Slide(isLeft);
        }

        public void AdjustSlide(Player kirby, GameTime gameTime)
        {
            if(kirby.IsSliding())
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds; 
                if(timer > .8)
                {
                    StopMovement();
                    kirby.EndSlide();
                    timer = 0;
                } 
            }
        }
        // public override void Adjust(Player kirby)
        // {
        //     AdjustSlide(kirby);
        //     //if kirby collides with a wall while crouching the slide ends
        //     //AdjustX(kirby);  // Turning this off temporarily  -Mark
        //     AdjustY(kirby);
        // }
        public override void MovePlayer(Player kirby, GameTime gameTime)
        {
            UpdatePosition(gameTime);
            AdjustX(kirby);
            AdjustSlide(kirby, gameTime);
        }
        public override void AdjustFromRightCollisionBlock(Rectangle intersection)
        {
            position.X -= intersection.Width;
            xVel = 0;
            
        }

        public override void AdjustFromLeftCollisionBlock(Rectangle intersection)
        {
            position.X += intersection.Width;
            xVel = 0;
        }

    }
}