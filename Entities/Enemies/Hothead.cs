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
        private int frameCounter = 0;
        private int walkFrames = 180;  //3 sec (if 60fps)
        private int stopFrames = 60;  //2 sec
        private int attackFrames = 100; //1 sec
        private int shootFrames = 100; //1 sec
        private List<IProjectile> fireballs; // all fireballs


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

            //eventual death pose/animation
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
            {
                frameCounter++;

                //walking
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();

                    if (frameCounter >= walkFrames)
                    {
                        //stateMachine.ChangePose(EnemyPose.Shooting);
                        stateMachine.ChangePose(EnemyPose.Charging);
                        frameCounter = 0;
                        UpdateTexture();
                    }
                }
                //shooting
                //else if (stateMachine.GetPose() == EnemyPose.Shooting)
                else if (stateMachine.GetPose() == EnemyPose.Charging)
                        {
                    if (frameCounter == 1) // fireball projectile
                    {
                        ShootProjectile();
                    }

                    if (frameCounter >= shootFrames)
                    {
                        // blow fire
                        stateMachine.ChangePose(EnemyPose.Attacking); // attacks (blows fire) after shooting
                        frameCounter = 0;
                        UpdateTexture();
                    }
                }
                //blows fire
                else if (stateMachine.GetPose() == EnemyPose.Attacking)
                {
                    if (frameCounter == 1) //blowing fire projectile
                    {
                        Attack();
                    }

                    if (frameCounter >= attackFrames)
                    {
                        // after attack, walk
                        stateMachine.ChangePose(EnemyPose.Walking);
                        frameCounter = 0;
                        UpdateTexture();
                    }
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
