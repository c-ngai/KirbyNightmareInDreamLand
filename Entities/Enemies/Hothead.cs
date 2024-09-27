using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MasterGame
{
    public class Hothead : Enemy
    {
        private const float MoveSpeed = 0.5f;

        // Frame tracker
        private int frameCounter = 0;
        private int walkFrames = 180;
        private int stopFrames = 60;
        private int attackFrames = 100;
        private int shootFrames = 100;

        // All fireballs and flamethrower
        private List<IProjectile> fireballs;
        private KirbyFlamethrower flamethrower;
        private bool isFlamethrowerActive;

        public Hothead(Vector2 startPosition) : base(startPosition, EnemyType.Hothead)
        {
            //initilize variables
            health = 100;
            fireballs = new List<IProjectile>();
            flamethrower = new KirbyFlamethrower();
            isFlamethrowerActive = false;
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                //TO-DO: Ask if State Machine should handle this
                //Depending on pose, will play specific number of frames and swap to other pose
                switch (stateMachine.GetPose())
                {
                    case EnemyPose.Walking:
                        Move();
                        if (frameCounter >= walkFrames)
                        {
                            stateMachine.ChangePose(EnemyPose.Charging);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    case EnemyPose.Charging:
                        if (frameCounter == 1) // Fireball projectile
                        {
                            Attack();
                        }

                        if (frameCounter >= shootFrames)
                        {
                            stateMachine.ChangePose(EnemyPose.Attacking); // Attacks after shooting
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    case EnemyPose.Attacking:
                        if (frameCounter == 1) // Blowing fire projectile
                        {
                            Flamethrower(gameTime);
                        }

                        if (frameCounter >= attackFrames)
                        {
                            isFlamethrowerActive = false;
                            stateMachine.ChangePose(EnemyPose.Walking); // After attack, walk
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                }

                UpdateTexture();
                enemySprite.Update();
                UpdateFireballs();

                // If attacking, update flamethrower
                if (stateMachine.GetPose() == EnemyPose.Attacking && isFlamethrowerActive)
                {
                    flamethrower.Update(gameTime, position, stateMachine.IsLeft() ? new Vector2(-1, 0) : new Vector2(1, 0));
                }
            }
        }

        protected override void Move()
        {
            // Walking back and forth
            if (stateMachine.IsLeft())
            {
                position.X -= MoveSpeed;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                    UpdateTexture();
                }
            }
            else
            {
                position.X += MoveSpeed;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                    UpdateTexture();
                }
            }
        }

        private void Flamethrower(GameTime gameTime)
        {
            //TO-DO: check if need these lines
            //stateMachine.ChangePose(EnemyPose.Attacking);
            //UpdateTexture();

            isFlamethrowerActive = true;

            // Blow fire/flamethrower left or right
            Vector2 flameDirection = stateMachine.IsLeft() ? new Vector2(-1, 0) : new Vector2(1, 0);
        }

        //Fireball attack
        public override void Attack()
        {
            Vector2 projectileDirection = stateMachine.IsLeft() ? new Vector2(-1, -0.5f) : new Vector2(1, -0.5f); // Aim left or right

            IProjectile newFireball = new EnemyFireball(position, projectileDirection);
            fireballs.Add(newFireball);
        }

        private void UpdateFireballs()
        {
            foreach (var fireball in fireballs)
            {
                fireball.Update();
            }

            //TO-DO: Remove fireballs from list after off screen logic can be added here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                if (isFlamethrowerActive)
                {
                    flamethrower.Draw(spriteBatch);
                }

                enemySprite.Draw(position, spriteBatch);

                foreach (var fireball in fireballs)
                {
                    fireball.Draw(spriteBatch);
                }
            }
        }
    }
}
