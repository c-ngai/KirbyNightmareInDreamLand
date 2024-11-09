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
            stateMachine.ChangePose(EnemyPose.Hop);
            ChangeState(new SparkyPause1State(this)); // Set initial state
            yVel = 0;
            xVel = Constants.Sparky.HOP_SPEED;
        }

        public override void Jump()
        {
            if (!isJumping)
            {
                isJumping = true;

                if (isTallJump)
                {
                    yVel = -Constants.Sparky.TALL_JUMP_VELOCITY;
                } else
                {
                    yVel = -Constants.Sparky.SHORT_JUMP_VELOCITY;
                }
                isTallJump = !isTallJump;
            }

            Move();
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
             if (!isDead)
             {
                 IncrementFrameCounter();
                 currentState.Update();
                 UpdateTexture();
                 enemySprite.Update();

                Fall();
                GetHitBox();            
                        
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
            isFalling = false;
            isJumping = false;
            position.Y = intersection.Y;

            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(SparkyJumpState))) {
                if (isTallJump)
                {
                    ChangeState(new SparkyPause1State(this));
                }
                else
                {
                    ChangeState(new SparkyPause2State(this));
                }
            }
            yVel = 0;
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
                        if (isTallJump)
                        {
                            ChangeState(new SparkyPause1State(this));
                        }
                        else
                        {
                            ChangeState(new SparkyPause2State(this));
                        }
                    }
                    yVel = 0;
                }
                //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
            }
        }


    }
}
