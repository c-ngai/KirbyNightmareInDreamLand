using System;
using System.Diagnostics;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class CrouchingMovement : PlayerMovement
    {
        public CrouchingMovement(Vector2 pos, Vector2 vel) : base(pos, vel) { }

        private float slideVel = Constants.Kirby.SLIDE_VEL;
        private int frameCounter = 0;

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
                velocity.X = kirby.IsLeft() ? slideVel * -1 : slideVel;
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
                frameCounter++; 
                if(frameCounter > Constants.Kirby.SLIDE_FRAMES)
                {
                    kirby.EndSlide();
                    frameCounter = 0;
                    kirby.attack?.EndAttack();
                    Debug.WriteLine("END SLIDE");
                }
                Debug.WriteLine(frameCounter);
            }
            else if (!kirby.state.IsCrouching()) // this is dumb and i hate it but it kind of works so i'm leaving it. 
            {
                kirby.EndSlide();
                kirby.attack?.EndAttack();
            }
        }
        public override void EndSlide()
        {
            //StopMovement();
            frameCounter = 0;
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