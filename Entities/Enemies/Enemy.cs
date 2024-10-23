using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using System;
using KirbyNightmareInDreamLand.Levels;
using System.Diagnostics;

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

        protected Enemy(Vector2 startPosition, EnemyType type)
        {
            //Initialize all variables
            position = startPosition;
            health = 1;
            isDead = false;
            xVel = 0;
            yVel = 0;
            isFalling = true;
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
            currentState.TakeDamage();
            CollisionActive = false;
        }

        public void ChangeDirection()
        {
            currentState.ChangeDirection();
        }

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

        public virtual void Update(GameTime gameTime) 
        {
            if (CollisionActive && !IsDead)
            {
                IncrementFrameCounter();
                currentState.Update();
                UpdateTexture();
                enemySprite.Update();

                Fall();

                GetHitBox(); // Ensure hitbox is updated
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
            yVel += gravity / 100;  // Increase vertical velocity by gravity
            position.Y += yVel;  // Apply the updated velocity to the enemy's Y position
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
            position.Y = intersection.Y + 1; // TODO: fix jank, the +1 is a total bandaid
            yVel = 0;
            isFalling = false;
        }
        public void BottomCollisionWithAir(Rectangle intersection)
        {
            isFalling = true;
            Fall();
        }
        public void AdjustGentle1SlopeLeftCollision(Tile tile)
        {
            Rectangle intersection = tile.rectangle;
            float offset = position.X - intersection.X;
            //Debug.WriteLine($"Starting Y position: {position.Y}");
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            position.Y = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
            //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
        }
        public void AdjustGentle2SlopeLeftCollision(Tile tile)
        {
            Rectangle intersection = tile.rectangle;
            float offset = position.X - intersection.X;
            //Debug.WriteLine($"Starting Y position: {position.Y}");
            float slope = Constants.Collision.GENTLE2_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_LEFT_YINTERCEPT;
            position.Y = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
            //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
        }

        public void AdjustSteepSlopeLeftCollision(Tile tile)
        {
            Rectangle intersection = tile.rectangle;
            float offset = position.X - intersection.X;
            //Debug.WriteLine($"Starting Y position: {position.Y}");
            float slope = Constants.Collision.STEEP_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_LEFT_YINTERCEPT;
            position.Y = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
            //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
        }

        public void AdjustGentle1SlopeRightCollision(Tile tile)
        {
            Rectangle intersection = tile.rectangle;
            float offset = position.X - intersection.X;
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            position.Y = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
            //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
        }

        public void AdjustGentle2SlopeRightCollision(Tile tile)
        {
            Rectangle intersection = tile.rectangle;
            float offset = position.X - intersection.X;
            float slope = Constants.Collision.GENTLE2_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_RIGHT_YINTERCEPT;
            position.Y = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
            //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
        }

        public void AdjustSteepSlopeRightCollision(Tile tile)
        {
            Rectangle intersection = tile.rectangle;
            float offset = position.X - intersection.X;
            //Debug.WriteLine($"Starting Y position: {position.Y}");
            float slope = Constants.Collision.STEEP_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_RIGHT_YINTERCEPT;
            position.Y = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
        }
    }
}