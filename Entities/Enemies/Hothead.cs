using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private EnemyFlamethrower flamethrower;
        private bool isFlamethrowerActive;
        private bool canUseFlamethrower;

        public Hothead(Vector2 startPosition) : base(startPosition, EnemyType.Hothead)
        {
            health = 100;
            fireballs = new List<IProjectile>();
            flamethrower = new EnemyFlamethrower();
            isFlamethrowerActive = false;
            canUseFlamethrower = true;
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                //TO-DO: Should I use statemachine to avoid switch cases?
                //Changes behavior depending on pose
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
                        if (frameCounter == 1) // Start flamethrower attack
                        {
                            Flamethrower(gameTime);
                        }

                        if (frameCounter >= attackFrames)
                        {
                            isFlamethrowerActive = false; // Deactivate flamethrower after attack
                            flamethrower.ClearSegments(); // Clear
                            stateMachine.ChangePose(EnemyPose.Walking); // After attack, walk
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                }

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
                }
            }
        }

        private Vector2 ProjectilePosition()
        {
            // Adjust flamethrower position based on Hothead's facing direction
            return stateMachine.IsLeft() ? new Vector2(position.X - 18, position.Y) : new Vector2(position.X + 18, position.Y); // TODO: I think these values need to be changed to be accurate. Check how far the position for hothead is from the edges of the sprite.
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
            if (!isFlamethrowerActive)
            {
                isFlamethrowerActive = true;

                // Set the start position for the flamethrower
                Vector2 flameDirection = stateMachine.IsLeft() ? new Vector2(-1, 0) : new Vector2(1, 0);
                flamethrower.Update(gameTime, ProjectilePosition(), flameDirection);

                canUseFlamethrower = false; // Prevent re-activation
            }
        }

        public override void Attack()
        {
            Vector2 projectileDirection = stateMachine.IsLeft() ? new Vector2(-1, -0.5f) : new Vector2(1, -0.5f);
            IProjectile newFireball = new EnemyFireball(ProjectilePosition(), projectileDirection);
            fireballs.Add(newFireball);
        }

        private void UpdateFireballs()
        {
            foreach (var fireball in fireballs)
            {
                fireball.Update();
            }
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