using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Projectiles;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Sparky : Enemy
    {
        private int hopCounter = 0; //number of hops
        private int stateCounter = 0;   //frames that have passed in 1 state
        private float currentHopHeight; // Store the current hop height
        private SparkyPlasma sparkyPlasma;
        private bool isPlasmaActive;

        public Sparky(Vector2 startPosition) : base(startPosition, EnemyType.Sparky)
        {
            //initialize to hop
            stateMachine.ChangePose(EnemyPose.Hop);
            ChangeState(new SparkyPause1State(this)); // Set initial state
            yVel = 0;
            xVel = Constants.Sparky.HOP_SPEED;
        }


        public override void Move()
        {
            //Keeps track of number of hoops
            hopCounter++;

            //float t = (float)hopCounter / Constants.Sparky.HOP_FREQUENCY;
            //Y movement calculations for smooth hops
            //yVel = (float)(Math.Sin(t * Math.PI * 2) * currentHopHeight / 2);
            // position.Y -= yVel;

            //Y movement calculations for smooth hops (upwards during hop)
            if (hopCounter < Constants.Sparky.HOP_FREQUENCY / 2) // Going up in the first half of the hop cycle
            {
                yVel = (float)(Math.Sin((float)hopCounter / Constants.Sparky.HOP_FREQUENCY * Math.PI) * currentHopHeight);
                position.Y -= yVel; // Move upwards
            }
            else
            {
                Fall(); // Apply gravity/fall in the second half of the hop cycle
            }

            // X movement. Check direction for boundaries 
            if (!stateMachine.IsLeft())
            {
                //position.X += Constants.Sparky.HOP_SPEED;
                position.X += xVel;
            }
            else
            {
                //position.X -= Constants.Sparky.HOP_SPEED;
                position.X -= xVel;
            }

            // Reset and repeat hops
            if (hopCounter >= Constants.Sparky.HOP_FREQUENCY)
            {
                hopCounter = 0;
            }
        }

        public void SetHopHeight(float height)
        {
            currentHopHeight = height; // Set the current hop height
        }

        public override Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.ENEMY_WIDTH / 2;
            float y = pos.Y - Constants.HitBoxes.ENEMY_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }

        public override Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENEMY_WIDTH, Constants.HitBoxes.ENEMY_HEIGHT);
        }


        
 public override void Update(GameTime gameTime)
 {
     if (!isDead)
     {
         IncrementFrameCounter();
         currentState.Update();
         UpdateTexture();
         enemySprite.Update();

         //Fall();
         GetHitBox();

         // Handle the beam if active
         if (isPlasmaActive)
         {
             sparkyPlasma.Update();
             if (sparkyPlasma.IsDone())
             {
                 isPlasmaActive = false;
             }
         } 
     }
 }

         public override void Attack()
 {
     if (!isPlasmaActive)
     {
         sparkyPlasma = new SparkyPlasma(position);
         isPlasmaActive = true;
     }
 }

        public override void Fall()
        {
            if (yVel > 0)  // Only fall if we're coming down (positive yVel indicates downward movement)
            {
                yVel += gravity / 100;  // Increase vertical velocity by gravity
                position.Y += yVel;  // Apply the updated velocity to the enemy's Y position
            }
        }



    }
}
