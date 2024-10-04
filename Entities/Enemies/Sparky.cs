using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Sparky : Enemy
    {
        private int hopCounter = 0; //number of hops
        private int stateCounter = 0;   //frames that have passed in 1 state
        private string currentState = "HoppingForward"; // name of current state

        public Sparky(Vector2 startPosition) : base(startPosition, EnemyType.Sparky)
        {
            //initialize to hop
            stateMachine.ChangePose(EnemyPose.Hop);
        }

        public override void Attack()
        {
            //Change pose and texture for charge attack
            stateMachine.ChangePose(EnemyPose.Attacking);
            UpdateTexture();
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                stateCounter++;

                //TO-DO: Change switch case into state pattern design
                switch (currentState)
                {
                    //Short Hop forward. Transition to Pause for "slime" jumping effect
                    case "HoppingForward":
                        Move();
                        if (stateCounter >= Constants.Sparky.HOP_FREQUENCY) //short hop
                        {
                            stateCounter = 0;
                            currentState = "Pausing";
                        }
                        break;

                    //Pause/standing still for moment before next jump (taller jump)
                    case "Pausing":
                        if (stateCounter >= Constants.Sparky.PAUSE_TIME) // pause
                        {
                            stateCounter = 0;
                            currentState = "HoppingTall";
                            enemySprite.ResetAnimation();  // Mark addition: since hop is a non-looping animation that we want to repeat but we already have that sprite, just call ResetAnimation on it.
                        }
                        break;

                        //Taller jump. Transitons to pause
                    case "HoppingTall":
                        Move();
                        if (stateCounter >= Constants.Sparky.HOP_FREQUENCY) //tall hop
                        {
                            stateCounter = 0;
                            currentState = "PausingAgain";
                        }
                        break;
                        //Pauses for slime ffect, Transitions to attack
                    case "PausingAgain":
                        if (stateCounter >= Constants.Sparky.PAUSE_TIME) // Pause 
                        {
                            stateCounter = 0;
                            currentState = "Attacking";
                        }
                        break;
                        //Attacks for a few seconds while standing still. Transitions to hopping
                    case "Attacking":
                        Attack();
                        if (stateCounter >= Constants.Sparky.ATTACK_TIME) // Attack
                        {
                            stateCounter = 0;
                            currentState = "Hurt"; // back to hop
                            stateMachine.ChangePose(EnemyPose.Hurt);
                        }
                        break;
                        case "Hurt":
                        // Transition back to Hopping after hurtFrames
                        if (stateCounter >= Constants.Sparky.HURT_FRAMES)
                        {
                            stateCounter = 0;
                            currentState = "HoppingForward"; // back to hop
                            stateMachine.ChangePose(EnemyPose.Hop);
                        }
                        break;
                }

                // Update texture and enemy sprite
                UpdateTexture();
                enemySprite.Update();
            }
        }
        protected override void Move()
        {
            //Keeps track of number of hoops
            hopCounter++;

            //Calculates how tall jump should be depending if jumping state
            float height = (currentState == "HoppingForward") ? Constants.Sparky.SHORT_HOP_HEIGHT : Constants.Sparky.TALL_HOP_HEIGHT; // Choose height based on state
            float t = (float)hopCounter / Constants.Sparky.HOP_FREQUENCY;

            //Y movement calculations for smooth hops
            position.Y -= (float)(Math.Sin(t * Math.PI * 2) * height / 2);

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw enemy if alive
            if (!isDead)
            {
                enemySprite.LevelDraw(position, spriteBatch);
            }
        }
    }
}
