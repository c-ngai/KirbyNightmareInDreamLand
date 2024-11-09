using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Audio;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class FloatingMovement : PlayerMovement
    {
        protected float floatVel = Constants.Physics.FLOAT_VEL;
        protected float floatGravity = Constants.Physics.FLOAT_GRAVITY;
        //protected new float gravity = Constants.Physics.FLOAT_GRAVITY2;
        private float floatFallingWindow = Constants.Graphics.FLOOR + 50;
        protected bool endFloat = false;
        public FloatingMovement(Vector2 pos) : base(pos)
        {
            landed = false;
        }
        public override void Walk(bool isLeft)
        {
            if (isLeft)
            {
                xVel = floatVel * -1; // times -1 to go in opposite direction
            }
            else
            {
                xVel = floatVel;
            }
        }

        public override void Run(bool isLeft)
        {
            Walk(isLeft);
        }

        //pressing x (jump) makes float go up
        public override void Jump(bool isLeft)
        {
            endFloat = false;
            landed = false;
            yVel = floatVel * -1; //go up
            
        }

        public void AdjustYPositionWhileFloating(Player kirby)
        {
            //dont go through the floor but float state as not been terminated
            if (landed)
            {   
                kirby.ChangePose(KirbyPose.FloatingGrounded);
            }
        }
        public void AdjustYPositionWhileNotFloating(Player kirby)
        {
            //dont go through the floor but floating was ended
            if (landed)
            {
                kirby.ChangePose(KirbyPose.Standing);
                kirby.ChangeMovement();
            }
        }

        public override void AdjustY(Player kirby)
        {
            if (endFloat)
            {
                AdjustYPositionWhileNotFloating(kirby);
            }
            else
            {
                AdjustYPositionWhileFloating(kirby);
            }
            //dont go through the ceiling
            if (position.Y < Constants.Kirby.CEILING)
            {
                yVel = 0;
                position.Y = Constants.Kirby.CEILING;
            }
             if(position.Y > Game1.Instance.Level.CurrentRoom.Height)
            {
                if(kirby.DEAD == true) // game over 
                {
                    Game1.Instance.Level.GameOver();
                    kirby.FillFullHealth();
                } else {
                    kirby.RestartKirby();
                    Game1.Instance.Level.LoadRoom(Game1.Instance.Level.CurrentRoom.Name);
                    Game1.Instance.Level.ChangeToPlaying();
                }
            }
        }

        public override void UpdatePosition(GameTime gameTime)
        {
            yVel += floatGravity * dt;
            position.X += xVel;
            position.Y += yVel;
        }
        //the animation of kirby letting air go
        public void FloatingEndAnimation(Player kirby)
        {
            kirby.ChangePose(KirbyPose.FloatingEnd);
        }

        public void FloatingFallingAnimation(Player kirby)
        {
            //floating doesnt go into the falling animation within a certain distance from floor
            if (position.Y < floatFallingWindow)
            {
                kirby.ChangePose(KirbyPose.FreeFall);

            }
        }

        //attack (or pressing z) undoes float
        public override void Attack(Player kirby)
        {
            if (!kirby.GetKirbyPose().Equals("FloatingGrounded"))
            {
                //SoundManager.Play("spitair");
                FloatingEndAnimation(kirby);
                floatGravity = gravity;
                endFloat = true;
                yVel = floatVel;
                FloatingFallingAnimation(kirby);
            }
            else
            {
                FloatingEndAnimation(kirby);
                endFloat = true;
            }
        }

    }
}
