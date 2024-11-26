using System;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class CrouchingMovement : PlayerMovement
    {
        public CrouchingMovement(Vector2 pos, Vector2 vel) : base(pos, vel) { }

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
                velocity.X = kirby.IsLeft() ? runningVel * -1 :runningVel;
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
                if(timer > Constants.Kirby.SLIDE_TIME)
                {
                    StopMovement();
                    kirby.EndSlide();
                    timer = 0;
                    kirby.attack?.EndAttack();
                } 
            }
        }
        public override void EndSlide()
        {
            StopMovement();
            timer = 0;
        }
        public override void MovePlayer(Player kirby, GameTime gameTime)
        {
            UpdatePosition(kirby);
            AdjustX(kirby);
            AdjustY(kirby);
            AdjustSlide(kirby, gameTime);
            DeathBarrierCheck(kirby);
        }
        public override void AdjustFromRightCollisionBlock(Rectangle intersection)
        {
            position.X -= intersection.Width;
            velocity.X = 0;
            
        }

        public override void AdjustFromLeftCollisionBlock(Rectangle intersection)
        {
            position.X += intersection.Width;
            velocity.X = 0;
        }

    }
}