using Microsoft.Xna.Framework;
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
    }
}