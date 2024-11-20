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
using KirbyNightmareInDreamLand.Audio;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Actions;
using Microsoft.VisualBasic;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public abstract class Enemy : IEnemy, ICollidable
    {
        protected Vector2 position; //Where enemy is drawn on screen
        protected Vector2 spawnPosition; //Where enemy is first drawn on screen
        protected int health; //Enemy health
        protected bool active;  //If enemy is dead
        protected Sprite enemySprite;
        public EnemyStateMachine stateMachine { get;  private set; }
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
            spawnPosition = startPosition;
            health = Constants.Enemies.HEALTH;
            active = true;
            xVel = 0;
            yVel = 0;
            isFalling = true;
            gravity = Constants.Physics.GRAVITY;
            stateMachine = new EnemyStateMachine(type);
            oldState = string.Empty;
            currentState = new WaddleDooWalkingState(this); // Initialize with the walking state
            ObjectManager.Instance.AddEnemy(this);
            ObjectManager.Instance.RegisterDynamicObject(this);
            currentState.Enter();
            frameCounter = 0;
            UpdateTexture();
        }

        public CollisionType GetCollisionType()
        {
            return CollisionType.Enemy;
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

        public bool Active
        {
            get => active;
            set => active = value;
        }

        public int FrameCounter
        {
            get { return frameCounter; }
        }
        public void IncrementFrameCounter()
        {
            frameCounter++;
        }
        public void UpdateDirection()
        {
            if(ObjectManager.Instance.Players[0].GetKirbyPosition().X < this.position.X){
                stateMachine.FaceLeft();
            } else {
                stateMachine.FaceRight();
            }
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

        public async void TakeDamage(Rectangle intersection)
        {

            int points = 0;

            // Determine points based on the type of enemy
            if (this is WaddleDoo || this is BrontoBurt || this is Hothead || this is Sparky)
            {
                points = Constants.Enemies.STRONG_ENEMY_POINTS;
            }
            else if (this is WaddleDee || this is PoppyBrosJr)
            {
                points = Constants.Enemies.WEAK_ENEMY_POINTS;
            }

            // Update the score in ObjectManager
            Game1.Instance.manager.UpdateScore(points);

            currentState.TakeDamage();
            Dispose();
            SoundManager.Play("enemydamage");
            await Task.Delay(Constants.Enemies.DELAY);
            SoundManager.Play("enemyexplode");
        }

        public void GetSwallowed(Rectangle intersection)
        {
            currentState.TakeDamage();
            this.TakeDamage(intersection);
            Dispose();
        }

        public virtual void ChangeDirection()
        {
            currentState.ChangeDirection();
        }

        public void SetDirection(bool facingLeft)
        {
            stateMachine.SetDirection(facingLeft);
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

        private void Spawn()
        {
            CollisionActive = true;
            active = true;
            bool isLeft = ObjectManager.Instance.NearestPlayerDirection(position);
            stateMachine.SetDirection(isLeft);
            health = Constants.Enemies.HEALTH;
            position = spawnPosition;
            frameCounter = 0;
            UpdateTexture();
        }

        private void Despawn()
        {
            CollisionActive = false;
            active = false;
        }

        public virtual void Update(GameTime gameTime) 
        {

            //respawn enemy if dead but just outside camera bounds
            if (!active && Camera.InAnyEnemyRespawnBounds(spawnPosition))
            {
                Spawn();
            }
            else if (active && !Camera.InAnyActiveEnemyBounds(position))
            {
                Despawn();
            }

            if (active)
            {
                IncrementFrameCounter();
                currentState.Update();
                UpdateTexture();
                enemySprite.Update();

                Fall();
            }
            else
            {
                Dispose();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw if enemy is alive 
            if (active)
            {
                enemySprite.Draw(position, spriteBatch);
                //spriteBatch.DrawString(LevelLoader.Instance.Font, frameCounter.ToString(), (position + new Vector2(-8, -32)), Color.Black);
            }
        }

        public virtual void Attack() { }

        public virtual void Jump() { }

        public virtual void Fall()
        {
            yVel += gravity / Constants.Enemies.GRAVITY_OFFSET;  // Increase vertical velocity by gravity
            position.Y += yVel;  // Apply the updated velocity to the enemy's Y position
        }

        public virtual void Move()
        {
            // Walking back and forth in X axis 
            if (stateMachine.IsLeft())
            {
                position.X -= xVel;
            }
            else
            {
                position.X += xVel;
            }
            UpdateTexture();
        }

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

        public virtual Vector2 GetPosition()
        {
            return position;
        }

        public virtual void BottomCollisionWithBlock(Rectangle intersection)
        {
            position.Y = intersection.Y; // Note: +1 removed because in Game1.Update I made level update before collision is called. This makes collisions happen after preexisting momentum is applied -Mark
            yVel = 0;
            isFalling = false;
        }

        public virtual void TopCollisionWithBlock(Rectangle intersection)
        {
            position.Y += intersection.Height;
        }
        // Commented out the inside for now. Is this necessary? -Mark
        public void BottomCollisionWithAir(Rectangle intersection)
        {
            //isFalling = true;
            //Fall();
        }

        public void BottomCollisionWithPlatform(Rectangle intersection)
        {

            position.Y = intersection.Y;
            yVel = 0;
            isFalling = false;
        }
        public virtual void AdjustOnSlopeCollision(Tile tile, float slope, float yIntercept)
        {
            Rectangle intersection = tile.rectangle;
            if (position.X > intersection.Left && position.X < intersection.Right)
            {
                float offset = position.X - intersection.X;
                //Debug.WriteLine($"Starting Y position: {position.Y}");
                float slopeY = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
                //GameDebug.Instance.LogPosition(new Vector2(position.X, position.Y));
                if (position.Y > slopeY)
                {
                    position.Y = slopeY;
                    yVel = 0;
                }
                //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
            }
        }
        public void AdjustGentle1SlopeLeftCollision(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void AdjustGentle2SlopeLeftCollision(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_LEFT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void AdjustSteepSlopeLeftCollision(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_LEFT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void AdjustGentle1SlopeRightCollision(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_RIGHT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void AdjustGentle2SlopeRightCollision(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_RIGHT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void AdjustSteepSlopeRightCollision(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_RIGHT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }


        public virtual void Dispose()
        {
            CollisionActive = false;
            currentState.Dispose();
        }

        public virtual KirbyType PowerType()
        {
            return KirbyType.Normal;
        }
    }
}