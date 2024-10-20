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
            yVel = 0;
            xVel = Constants.Sparky.HOP_SPEED;
        }

        public override void Move()
        {
            //Keeps track of number of hoops
            hopCounter++;

            float t = (float)hopCounter / Constants.Sparky.HOP_FREQUENCY;

            //Y movement calculations for smooth hops
            yVel = (float)(Math.Sin(t * Math.PI * 2) * currentHopHeight / 2);
            position.Y -= yVel;

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
            float x;
            float y;
            if (stateMachine.GetPose() == EnemyPose.Attacking)
            {
                x = pos.X - Constants.HitBoxes.SPARKY_ATTACK_WIDTH / 2;
                y = pos.Y - Constants.HitBoxes.SPARKY_ATTACK_HEIGHT + Constants.HitBoxes.SPARKY_ATTACK_OFFSET;
            }
            else
            {
                x = pos.X - Constants.HitBoxes.ENEMY_WIDTH / 2;
                y = pos.Y - Constants.HitBoxes.ENEMY_HEIGHT;
            }
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }

        public override Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);

            // Check if the enemy is in the attacking state
            if (stateMachine.GetPose() == EnemyPose.Attacking)
            {
                // Return the larger hitbox when attacking
                return new Rectangle((int)rectPoint.X,(int)rectPoint.Y,Constants.HitBoxes.SPARKY_ATTACK_WIDTH,Constants.HitBoxes.SPARKY_ATTACK_HEIGHT);
            }
            else
            {
                // Return the normal hitbox for other states
                return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENEMY_WIDTH, Constants.HitBoxes.ENEMY_HEIGHT);
            }
        }

    }
}
