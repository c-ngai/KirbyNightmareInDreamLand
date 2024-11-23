﻿using Microsoft.Xna.Framework;
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
        protected int health; //Enemy health
        protected bool active;  //If enemy is dead
        protected Sprite enemySprite;
        public EnemyStateMachine stateMachine { get;  private set; }
        protected IEnemyState currentState; // Current state of the enemy
        protected string oldState; //Previous state
        protected int frameCounter; // Frame counter for tracking state duration
        protected float gravity;
        protected bool affectedByGravity;
        protected Boolean isFalling;
        protected bool isBeingInhaled;

        public bool CollisionActive { get; set; }

        protected Enemy(Vector2 startPosition, EnemyType type)
        {
            //Initialize all variables
            spawnPosition = startPosition;
            //position = spawnPosition;
            //health = Constants.Enemies.HEALTH;
            //active = true;
            //velocity = Vector2.Zero;
            //isFalling = true;
            //isBeingInhaled = false;
            gravity = Constants.Physics.GRAVITY;
            stateMachine = new EnemyStateMachine(type);
            //oldState = string.Empty;
            //currentState = new WaddleDooWalkingState(this); // Initialize with the walking state
            ObjectManager.Instance.AddEnemy(this);
            //currentState.Enter();
            //frameCounter = 0;
            //UpdateTexture();
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
            CollisionActive = false;
            SoundManager.Play("enemydamage");
            await Task.Delay(Constants.Enemies.DELAY);
            SoundManager.Play("enemyexplode");
        }


        public void GetInhaled(Rectangle intersection, IPlayer player)
        {
            isBeingInhaled = true;
            ChangeState(new EnemyInhaledState(this, player));
        }


        public void GetSwallowed(Rectangle intersection)
        {
            //currentState.TakeDamage();
            //this.TakeDamage(intersection);
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

        public virtual void Spawn()
        {
            CollisionActive = true;
            active = true;
            isBeingInhaled = false;
            position = spawnPosition;
            velocity = Vector2.Zero;
            bool isLeft = ObjectManager.Instance.NearestPlayerDirection(position);
            stateMachine.SetDirection(isLeft);
            health = Constants.Enemies.HEALTH;
            frameCounter = 0;
        }

        private void Despawn()
        {
            CollisionActive = false;
            active = false;
        }

        public void UpdatePosition()
        {
            if (affectedByGravity && !isBeingInhaled)
            {
                velocity.Y += gravity;  // Increase vertical velocity by gravity
            }

            position.X += velocity.X;
            position.Y += velocity.Y;  // Apply the updated velocity to the enemy's Y position
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

                UpdatePosition();
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

        public virtual void Move()
        {
            //// Walking back and forth in X axis 
            //if (stateMachine.IsLeft())
            //{
            //    position.X -= velocity.X;
            //}
            //else
            //{
            //    position.X += velocity.X;
            //}
            //UpdateTexture();
        }

        public void AccellerateTowards(Vector2 _position)
        {
            float magnitude = velocity.Length();
            velocity = _position - position;
            velocity.Normalize();
            velocity *= magnitude * 1.1f;
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
            position.Y = intersection.Y; // Note: +1 removed because in Game1.Update I made level update before collision is called. This makes collisions happen after preexisting momentum is applied -Mark
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
        // Commented out the inside for now. Is this necessary? -Mark
        public void BottomCollisionWithAir(Rectangle intersection)
        {
            //isFalling = true;
            //Fall();
        }

        public void BottomCollisionWithPlatform(Rectangle intersection)
        {

            position.Y = intersection.Y;
            velocity.Y = 0;
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
                    velocity.Y = 0;
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
            Active = false;
            currentState.Dispose();
        }

        public virtual KirbyType PowerType()
        {
            return KirbyType.Normal;
        }
    }
}