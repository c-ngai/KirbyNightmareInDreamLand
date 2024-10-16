﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDee : Enemy, ICollidable
    {
        //Keep track of current frame
        //private int frameCounter = 0;
        public WaddleDee(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDee)
        {
            //Set pose and sprite
            stateMachine.ChangePose(EnemyPose.Walking);
            UpdateTexture();
            currentState = new WaddleDeeWalkingState(this);
            CollisionDetection.Instance.RegisterDynamicObject(this);
        }

        public override void Move()
        {
            //X movement logic. Moves until boundaries
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.WaddleDee.MOVE_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection(); // Change direction if hitting left boundary
                }
            }
            else
            {
                position.X += Constants.WaddleDee.MOVE_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection(); // Change direction if hitting right boundary
                }
            }
        }

        public override void Attack()
        {
            //WaddleDee has no attack sprite
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw if alive
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                GetHitBox();
                currentState.Update(); //update state
                UpdateTexture(); // Update the texture if the state has changed
                enemySprite.Update(); // Update the enemy sprite
            }
        }
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.ENTITY_WIDTH/2;
            float y = pos.Y - Constants.HitBoxes.ENTITY_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint; 
        }
        // Update the bounding box based on the player's current position and size
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
        }
    }
}
