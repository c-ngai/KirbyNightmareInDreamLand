using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MasterGame
{
    public class Hothead : IEnemy
    {
        private Vector2 position;
        private int health;
        private bool isDead;
        private Sprite enemySprite;
        private EnemyStateMachine stateMachine;
        private Vector2 leftBoundary = new Vector2(100, 100);
        private Vector2 rightBoundary = new Vector2(230, 100);
        private string oldState;

        //frame tracker
        private int frameCounter = 0;
        private int walkFrames = 180; 
        private int stopFrames = 60;
        private int attackFrames = 100;
        private int shootFrames = 100;

        // all fireballs
        private List<IProjectile> fireballs;


        public Hothead(Vector2 startPosition)
        {
            position = startPosition;
            health = 100;
            isDead = false;
            stateMachine = new EnemyStateMachine(EnemyType.WaddleDoo);
            //stateMachine = new EnemyStateMachine(EnemyType.Hothead);
            stateMachine.ChangePose(EnemyPose.Walking);
            fireballs = new List<IProjectile>();
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Sprite EnemySprite
        {
            set { enemySprite = value; }
        }

        public void TakeDamage()
        {
            stateMachine.ChangePose(EnemyPose.Hurt);
            health -= 10;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            stateMachine.ChangePose(EnemyPose.Hurt);
            UpdateTexture();
        }

        public void Attack()
        {
            stateMachine.ChangePose(EnemyPose.Attacking);
            UpdateTexture();
        }

        public void UpdateTexture()
        {
            if (!stateMachine.GetStateString().Equals(oldState))
            {
                enemySprite = SpriteFactory.Instance.createSprite(stateMachine.GetSpriteParameters());
                oldState = stateMachine.GetStateString();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!isDead)
                if (!isDead)
                {
                    frameCounter++;

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
                            if (frameCounter == 1) // fireball projectile
                            {
                                ShootProjectile();
                            }

                            if (frameCounter >= shootFrames)
                            {
                                stateMachine.ChangePose(EnemyPose.Attacking); // attacks (blows fire) after shooting
                                frameCounter = 0;
                                UpdateTexture();
                            }
                            break;

                        case EnemyPose.Attacking:
                            if (frameCounter == 1) // blowing fire projectile
                            {
                                Attack();
                            }

                            if (frameCounter >= attackFrames)
                            {
                                stateMachine.ChangePose(EnemyPose.Walking); // after attack, walk
                                frameCounter = 0;
                                UpdateTexture();
                            }
                            break;
                    }

                    UpdateTexture();
                    enemySprite.Update();

                    //update fireballs
                    UpdateFireballs();
                }
        }
    

    private void Move()
        {
            //walking back and forth
            if (stateMachine.IsLeft())
            {
                position.X -= 0.5f;
                if (position.X <= leftBoundary.X)
                {
                    stateMachine.ChangeDirection();
                    UpdateTexture();
                }
            }
            else
            {
                position.X += 0.5f;
                if (position.X >= rightBoundary.X)
                {
                    stateMachine.ChangeDirection();
                    UpdateTexture();
                }
            }
        }

        private void ShootProjectile()
        {

            Vector2 projectileDirection;

            if (stateMachine.IsLeft())
            {
                projectileDirection = new Vector2(-1, -0.5f); // aim left
            }
            else
            {
                projectileDirection = new Vector2(1, -0.5f); // aim right
            }

            IProjectile newFireball = new EnemyFireball(position, projectileDirection);
            fireballs.Add(newFireball);
        }

        private void UpdateFireballs()
        {
            foreach (var fireball in fireballs)
            {
                fireball.Update();
            }

            //should prob eventually remove fireballs from list after off screen
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);

                foreach (var fireball in fireballs)
                {
                    fireball.Draw(spriteBatch);
                }
            }
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection();
        }
    }
}
