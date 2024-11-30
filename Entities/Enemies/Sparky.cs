using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Sparky : Enemy
    {
        private SparkyPlasma sparkyPlasma;
        private bool isPlasmaActive;
        private bool isJumping = false;
        private bool isTallJump = false;

        public bool IsJumping => isJumping;

        public Sparky(Vector2 startPosition) : base(startPosition, EnemyType.Sparky)
        {
            //initialize to hop
            //stateMachine.ChangePose(EnemyPose.Hop);
            //ChangeState(new SparkyPause1State(this)); // Set initial state
            //velocity.Y = 0;
            //velocity.X = Constants.Sparky.HOP_SPEED;
            affectedByGravity = true;
            gravity = Constants.Sparky.GRAVITY;
        }

        public override void Spawn()
        {
            base.Spawn();
            stateMachine.ChangePose(EnemyPose.Hop);
            currentState = new SparkyPauseState(this);
        }

        public override void TakeDamage(Rectangle intersection, Vector2 positionOfDamageSource)
        {
            base.TakeDamage(intersection, positionOfDamageSource);
            sparkyPlasma?.EndAttack();
        }


        public override void GetInhaled(Rectangle intersection, IPlayer player)
        {
            base.GetInhaled(intersection, player);
            sparkyPlasma?.EndAttack();
        }


        public override void Move()
        {
            base.Move();
            velocity.X = stateMachine.IsLeft() ? -Constants.Sparky.HOP_SPEED : Constants.Sparky.HOP_SPEED;
        }


        public override void Jump()
        {
            if (!isJumping)
            {
                isJumping = true;
                isTallJump = random.Next(0, 3) == 0; // one in three chance for a jump to be a tall jump
                velocity.Y = isTallJump ? -Constants.Sparky.TALL_JUMP_VELOCITY : -Constants.Sparky.SHORT_JUMP_VELOCITY;
                isTallJump = !isTallJump;
            }
        }

        public override Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.ENEMY_WIDTH / Constants.Sparky.HITBOX_RECTANGLE_OFFSET;
            float y = pos.Y - Constants.HitBoxes.ENEMY_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }

        public override Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENEMY_WIDTH, Constants.HitBoxes.ENEMY_HEIGHT);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); 
            
            if (active)
            {            
                 // Handle the beam if active
                 if (isPlasmaActive)
                 {
                     sparkyPlasma.Update();
                     if (sparkyPlasma.IsDone())
                     {
                        sparkyPlasma.EndAttack();
                        isPlasmaActive = false;
                     }
                 }
            }
        }

        public override void Attack()
        {
             if (!isPlasmaActive)
             {
                sparkyPlasma = new SparkyPlasma(position);
                isPlasmaActive = true;
             }
        }

        public override void BottomCollisionWithBlock(Rectangle intersection)
        {
            base.BottomCollisionWithBlock(intersection);
            
            isJumping = false;

            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(SparkyJumpState)))
            {
                ChangeState(new SparkyPauseState(this));
            }
        }

        public override void BottomCollisionWithPlatform(Rectangle intersection)
        {
            base.BottomCollisionWithPlatform(intersection);

            isJumping = false;

            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(SparkyJumpState)))
            {
                ChangeState(new SparkyPauseState(this));
            }
        }

        public override void AdjustOnSlopeCollision(Tile tile, float slope, float yIntercept)
        {

            //GameDebug.Instance.LogPosition(position);

            Rectangle intersection = tile.rectangle;
            if (position.X > intersection.Left && position.X < intersection.Right)
            {
                float offset = position.X - intersection.X;
                //Debug.WriteLine($"Starting Y position: {position.Y}");
                float slopeY = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
                //GameDebug.Instance.LogPosition(intersection.Location.ToVector2());
                if (position.Y > slopeY)
                {
                    position.Y = slopeY;

                    isFalling = false;
                    isJumping = false;
                    // TODO: remove band-aid. waddle doo is always still inside the slope on the first frame of his jump, but he shouldn't be. check the order that velocity and position changes happen. position should change by velocity ONCE at the end of an update
                    if (currentState.GetType().Equals(typeof(SparkyJumpState)) && frameCounter > 0)
                    {
                        ChangeState(new SparkyPauseState(this));
                    }
                    velocity.Y = 0;
                }
                //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            ObjectManager.Instance.RemoveDynamicObject(sparkyPlasma);
        }

        public override KirbyType PowerType()
        {
            return KirbyType.Spark;
        }

    }
}
