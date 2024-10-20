using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using System;
using KirbyNightmareInDreamLand.Levels;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public abstract class Enemy : IEnemy, ICollidable
    {
        protected Vector2 position; //Where enemy is drawn on screen
        protected int health; //Enemy health
        protected bool isDead;  //If enemy is dead
        protected Sprite enemySprite;
        protected EnemyStateMachine stateMachine;
        protected IEnemyState currentState; // Current state of the enemy
        protected string oldState; //Previous state
        protected int frameCounter; // Frame counter for tracking state duration
        protected float xVel;
        protected float yVel;
        protected float gravity;
        protected Boolean isFalling;

        public bool CollisionActive { get; set; } = true;

          public bool IsFalling
          {
              get => isFalling; 
          }

        protected Enemy(Vector2 startPosition, EnemyType type)
        {
            //Initialize all variables
            position = startPosition;
            health = 1;
            isDead = false;
            xVel = 0;
            yVel = 0;
            isFalling = false;
            gravity = Constants.Physics.GRAVITY;
            stateMachine = new EnemyStateMachine(type);
            oldState = string.Empty;
            currentState = new WaddleDooWalkingState(this); // Initialize with the walking state
            ObjectManager.Instance.RegisterDynamicObject(this);
            currentState.Enter();
            frameCounter = 0; 
        }

        public string GetObjectType()
        {
            return "Enemy";
        }

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }
        public Sprite EnemySprite
        {
            //Returns Sprite
            set { enemySprite = value; }
        }

        public int Health
        {
            get => health;
            set => health = value;
        }

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        public int FrameCounter
        {
            get { return frameCounter; }
        }
        public String GetCollisionType()
        {
            return "Enemy";
        }
        public void IncrementFrameCounter()
        {
            frameCounter++;
        }

        // Method to reset the frame counter
        public void ResetFrameCounter()
        {
            frameCounter = 0;
        }

        public void UpdateTexture()
        {
            if (!stateMachine.GetStateString().Equals(oldState))
            {
                enemySprite = SpriteFactory.Instance.CreateSprite(stateMachine.GetSpriteParameters());
                oldState = stateMachine.GetStateString();
            }
        }

        public void ChangeState(IEnemyState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void TakeDamage(Rectangle intersection)
        {
            currentState.TakeDamage(); // Delegate to current state
            CollisionActive = false;
        }

        public void ChangeDirection()
        {
            currentState.ChangeDirection(); // Delegate to current state
        }

        // Public methods to interact with stateMachine
        public void ChangePose(EnemyPose pose)
        {
            stateMachine.ChangePose(pose);
        }

        public void ToggleDirection()
        {
            stateMachine.ChangeDirection();
        }

        public string GetStateString()
        {
            return stateMachine.GetStateString();
        }

        public virtual void Update(GameTime gameTime) // Change to virtual
        {
            if (CollisionActive && !IsDead)
            {
                IncrementFrameCounter();
                currentState.Update();
                UpdateTexture();
                enemySprite.Update();
                GetHitBox();
            } else {
                CollisionActive = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw if enemy is alive
            if (CollisionActive && !IsDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
            else
            {
                ObjectManager.Instance.RemoveDynamicObject(this); // Deregister if dead
            }
        }

        public virtual void Attack() { }

        public virtual void Jump() { }

        public virtual void Fall()
        {
            // isFalling = true;
            yVel = gravity;
        }

        public abstract void Move();

        public virtual Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.ENTITY_WIDTH/2;
            float y = pos.Y - Constants.HitBoxes.ENTITY_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint; 
        }
        public virtual Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
        }

        public virtual void BottomCollisionWithBlock(Rectangle intersection)
        {
            isFalling = false;
            position.Y = intersection.Y;
            yVel = 0;
        }
        public void BottomCollisionWithAir(Rectangle intersection)
        {
            //if (state.ShouldFallThroughTile())
            //{
            //movement.ChangeKirbyLanded(false);
                Fall();
            //}
        }
        public void CollisionWithGentle1SlopeLeft(Tile tile)
        {

        }
    }
}