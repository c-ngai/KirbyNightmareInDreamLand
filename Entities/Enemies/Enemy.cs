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
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public abstract class Enemy : IEnemy, ICollidable
    {
        protected Vector2 position; //Where enemy is drawn on screen
        protected Vector2 spawnPosition; //Where enemy is first drawn on screen
        protected Vector2 velocity;
        protected float vibrate;

        protected int health; //Enemy health
        protected bool active;  //If enemy is dead
        protected Sprite enemySprite;
        public Random random;
        public EnemyStateMachine stateMachine { get;  private set; }
        protected IEnemyState currentState; // Current state of the enemy
        protected string oldState; //Previous state
        protected int frameCounter; // Frame counter for tracking state duration
        protected float gravity;
        protected bool affectedByGravity;
        protected bool isFalling;
        protected bool isBeingInhaled;

        public bool CollisionActive { get; set; }

        protected Enemy(Vector2 startPosition, EnemyType type)
        {
            spawnPosition = startPosition;
            gravity = Constants.Physics.GRAVITY;
            stateMachine = new EnemyStateMachine(type);
            random = new Random();
            ObjectManager.Instance.AddEnemy(this);
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

        public bool IsBeingInhaled
        {
            get => isBeingInhaled;
            set => isBeingInhaled = value;
        }

        public float Vibrate
        {
            get => vibrate;
            set => vibrate = value;
        }

        public int FrameCounter
        {
            get { return frameCounter; }
        }

        public void FaceNearestPlayer()
        {
            IPlayer nearestPlayer = ObjectManager.Instance.NearestPlayer(position);
            float nearestPlayerX = nearestPlayer.GetKirbyPosition().X;
            bool isLeft = nearestPlayerX < position.X;
            SetDirection(isLeft);
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
            // Call exit on the current state, if there is one, so that it may take care of any final business if it needs (stopping sounds, ending attacks, deallocating resources, etc)
            currentState?.Exit();
            currentState?.Dispose();
            // Set the current state to the given new state and call Enter on it
            currentState = newState;
            currentState.Enter();
            // Reset the state update counter
            frameCounter = 0;
        }

        public virtual void TakeDamage(ICollidable damageDealer, Rectangle intersection, Vector2 positionOfDamageSource)
        {
            if (!isBeingInhaled)
            {
                UpdateScore();

                currentState.TakeDamage();
                //positionOfDamageSource.Y += 8; // I like to shift the position of the damage source used to calculate the velocity down a little, otherwise hitting things straight on usually sends them down into the ground
                velocity = (GetHitBox().Center.ToVector2() - positionOfDamageSource) * Constants.Enemies.DAMAGE_OFFSET_TO_KNOCKBACK_VELOCITY_RATIO;
                CollisionActive = false;
                SoundManager.Play("enemydamage");
            }
        }

        public virtual void GetInhaled(Rectangle intersection, IPlayer player)
        {
            if (!isBeingInhaled)
            {
                isBeingInhaled = true;
                CollisionActive = true;
                velocity = Vector2.Zero;
                ChangeState(new EnemyInhaledState(this, player));
            }
        }


        public void GetSwallowed(Rectangle intersection)
        {
            UpdateScore();
            Dispose();
        }

        private void UpdateScore()
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

        public virtual void Spawn()
        {
            CollisionActive = true;
            active = true;
            isBeingInhaled = false;
            position = spawnPosition;
            velocity = Vector2.Zero;
            vibrate = 0;
            FaceNearestPlayer();
            health = Constants.Enemies.HEALTH;
            frameCounter = 0;
        }

        private void Despawn()
        {
            Dispose();
        }

        public void UpdatePosition()
        {
            if (affectedByGravity && !isBeingInhaled && health > 0)
            {
                velocity.Y += gravity;  // Increase vertical velocity by gravity
            }

            position.X += velocity.X;
            position.Y += velocity.Y;  // Apply the updated velocity to the enemy's Y position
        }

        public void DecelerateX(float deceleration)
        {
            if (velocity.X > 0)
            {
                velocity.X -= deceleration;
            }
            else if (velocity.X < 0)
            {
                velocity.X += deceleration;
            }
            if (velocity.X < deceleration / 2 && velocity.X > -deceleration / 2)
            {
                velocity.X = 0;
            }
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
                frameCounter++;
                currentState.Update();
                UpdateTexture();
                enemySprite.Update();

                UpdatePosition();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw if enemy is alive 
            if (active)
            {
                Vector2 vibratePos = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
                vibratePos.Normalize();
                enemySprite.Draw(position + vibratePos * vibrate, spriteBatch);

                if (Game1.Instance.DEBUG_TEXT_ENABLED)
                {
                    spriteBatch.DrawString(LevelLoader.Instance.Font, frameCounter.ToString(), (position + new Vector2(-8, -32)), Color.Black);
                }
                
            }
        }

        public virtual void Attack() { }

        public virtual void Jump() { }

        public virtual void Move() { }

        public virtual void StopMoving()
        {
            velocity.X = 0;
        }

        public void AccellerateTowards(Vector2 _position)
        {
            float magnitude = velocity.Length();
            velocity = _position - position;
            velocity.Normalize();
            velocity *= magnitude + 0.2f;
            //velocity.X += (_position.X - position.X) / 200;
            //velocity.Y += (_position.Y - position.Y) / 200;
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
            position.Y = intersection.Y + Constants.Collision.GROUND_COLLISION_OFFSET;
            velocity.Y = 0;
            isFalling = false;
        }

        public virtual void TopCollisionWithBlock(Rectangle intersection)
        {
            position.Y += intersection.Height;
        }
        public virtual void RightCollisionWithBlock(Rectangle intersection)
        {
            position.X -= intersection.Width;
            bool left = true;
            SetDirection(left);
        }

        public virtual void LeftCollisionWithBlock(Rectangle intersection)
        {
            position.X += intersection.Width;
            bool left = false;
            SetDirection(left);
        }

        public virtual void BottomCollisionWithPlatform(Rectangle intersection)
        {

            position.Y = intersection.Y + Constants.Collision.GROUND_COLLISION_OFFSET;
            velocity.Y = 0;
            isFalling = false;
        }
        public virtual void AdjustOnSlopeCollision(Tile tile, float slope, float yIntercept)
        {
            Rectangle intersection = tile.rectangle;
            if (position.X > intersection.Left && position.X < intersection.Right)
            {
                float offset = position.X - intersection.X;
                float slopeY = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
                if (position.Y > slopeY)
                {
                    position.Y = slopeY + Constants.Collision.GROUND_COLLISION_OFFSET;
                    velocity.Y = 0;
                }
            }
        }
        public void CollisionWithGentle1LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void CollisionWithGentle2LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_LEFT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void CollisionWithSteepLeftSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_LEFT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void CollisionWithGentle1RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_RIGHT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void CollisionWithGentle2RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_RIGHT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }
        public void CollisionWithSteepRightSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_RIGHT_YINTERCEPT;
            AdjustOnSlopeCollision(tile, slope, yIntercept);
        }


        public virtual void Dispose()
        {
            CollisionActive = false;
            Active = false;
            currentState?.Dispose();
        }

        public virtual KirbyType PowerType()
        {
            return KirbyType.Normal;
        }
    }
}