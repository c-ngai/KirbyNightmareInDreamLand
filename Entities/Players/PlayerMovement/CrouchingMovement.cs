using System;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class CrouchingMovement : PlayerMovement
    {
        public CrouchingMovement(Vector2 pos) : base(pos){}

        private static int slideDistance = 70;
        private float distanceMoved = 0;
        private float startingX;

        private int timer = 0;

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
            {   Slide(kirby);
                if(distanceMoved == 0){
                    startingX = position.X;
                    distanceMoved = 1;
                }
            }
        }

        public override void Jump(bool isLeft)
        {
            //Slide(isLeft);
        }

        public void AdjustSlide(Player kirby)
        {
            if(kirby.IsSliding())
            {
                distanceMoved = Math.Abs(position.X - startingX);
                if(distanceMoved > slideDistance )
                {
                    StopMovement();
                    kirby.EndSlide();
                } 
            }
        }
        public override void Adjust(Player kirby)
        {
            AdjustSlide(kirby);
            //if kirby collides with a wall while crouching the slide ends
            //AdjustX(kirby);  // Turning this off temporarily  -Mark
            AdjustY(kirby);
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