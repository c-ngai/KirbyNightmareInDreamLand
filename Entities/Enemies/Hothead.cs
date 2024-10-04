using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Hothead : Enemy
    {
        // Frame tracker
        private int frameCounter = 0;

        // All fireballs and flamethrower
        private readonly List<IProjectile> fireballs;
        private readonly EnemyFlamethrower flamethrower;

        //Checks if flamethrower attack is active
        private bool isFlamethrowerActive;

        public Hothead(Vector2 startPosition) : base(startPosition, EnemyType.Hothead)
        {
            //Initializes attacks and pose for enemy
            fireballs = new List<IProjectile>();
            flamethrower = new EnemyFlamethrower();
            isFlamethrowerActive = false;
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                //TO-DO: Change switch case into state pattern design.
                switch (stateMachine.GetPose())
                {
                    //Walk for certain number of frames, then transition to spitting projectile
                    case EnemyPose.Walking:
                        Move();
                        if (frameCounter >= Constants.Hothead.WALK_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Charging);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    //Spit projectile then transition to flamethrower
                    case EnemyPose.Charging:
                        if (frameCounter == 1) // Fireball projectile on frame 1
                        {
                            Attack();
                        }

                        if (frameCounter >= Constants.Hothead.SHOOT_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Attacking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    //Flamethrower attack then transition to walking
                    case EnemyPose.Attacking:
                        if (frameCounter == 1) // Start flamethrower attack on frame 1
                        {
                            Flamethrower(gameTime);
                        }

                        if (frameCounter >= Constants.Hothead.ATTACK_FRAMES)
                        {
                            isFlamethrowerActive = false; // Deactivate flamethrower after attack
                            flamethrower.ClearSegments(); // Clear fire
                            stateMachine.ChangePose(EnemyPose.Hurt); // After attack, walk
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    case EnemyPose.Hurt:
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= Constants.Hothead.HURT_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Walking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                }

                //Update all sprites
                UpdateTexture();
                enemySprite.Update();
                UpdateFireballs();

                // Update flamethrower if active
                if (isFlamethrowerActive)
                {
                    flamethrower.Update(gameTime, ProjectilePosition(), stateMachine.IsLeft() ? new Vector2(-1, 0) : new Vector2(1, 0));
                }
                else
                {
                    flamethrower.ClearSegments(); // Clear segments when not active
                    //NOTE: Need to find some way to remove off-screen fireballs
                }
            }
        }

        private Vector2 ProjectilePosition()
        {
            // Adjust flamethrower position based on Hothead's facing direction
            return stateMachine.IsLeft() ? new Vector2(position.X - 18, position.Y - 7 ) : new Vector2(position.X + 18, position.Y - 7);
        }

        protected override void Move()
        {
            // Walking back and forth in X axis 
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.Hothead.MOVE_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                    UpdateTexture();
                }
            }
            else
            {
                position.X += Constants.Hothead.MOVE_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                    UpdateTexture();
                }
            }
        }

        private void Flamethrower(GameTime gameTime)
        {
            //Shoots flamethrower if called while inactive
            if (!isFlamethrowerActive)
            {
                isFlamethrowerActive = true;

                // Set the start position for the flamethrower
                Vector2 flameDirection = stateMachine.IsLeft() ? new Vector2(-1, 0) : new Vector2(1, 0);
                flamethrower.Update(gameTime, ProjectilePosition(), flameDirection);
            }
        }

        public override void Attack()
        {
            //Shoots fireball projectile attack
            Vector2 projectileDirection = stateMachine.IsLeft() ? new Vector2(-1, -0.5f) : new Vector2(1, -0.5f);
            IProjectile newFireball = new EnemyFireball(ProjectilePosition(), projectileDirection);
            fireballs.Add(newFireball);
        }

        private void UpdateFireballs()
        {
            //Updates all fireballs on list
            foreach (var fireball in fireballs)
            {
                fireball.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw enemy, flamethrower, and projectile depending if alive or active
            if (!isDead)
            {
                if (isFlamethrowerActive)
                {
                    flamethrower.Draw(spriteBatch);
                }

                enemySprite.LevelDraw(position, spriteBatch);

                foreach (var fireball in fireballs)
                {
                    fireball.Draw(spriteBatch);
                }
            }
        }
    }
}