using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Sparky : Enemy
    {
        private int hopCounter = 0; //number of hops
        private int stateCounter = 0;   //frames that have passed in 1 state
        private float currentHopHeight; // Store the current hop height

        public Sparky(Vector2 startPosition) : base(startPosition, EnemyType.Sparky)
        {
            //initialize to hop
            stateMachine.ChangePose(EnemyPose.Hop);
            ChangeState(new SparkyPause1State(this)); // Set initial state
        }

        public override void Move()
        {
            //Keeps track of number of hoops
            hopCounter++;
  
            float t = (float)hopCounter / Constants.Sparky.HOP_FREQUENCY;

            //Y movement calculations for smooth hops
            position.Y -= (float)(Math.Sin(t * Math.PI * 2) * currentHopHeight / 2);

            // X movement. Check direction for boundaries 
            if (!stateMachine.IsLeft())
            {
                position.X += Constants.Sparky.HOP_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X -= Constants.Sparky.HOP_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
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

    }
}
