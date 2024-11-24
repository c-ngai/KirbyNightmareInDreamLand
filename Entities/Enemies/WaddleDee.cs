using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDee : Enemy
    {
        public WaddleDee(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDee)
        {
            currentState = new WaddleDeeWalkingState(this);
            //stateMachine.ChangeDirection();
            velocity.X = Constants.WaddleDee.MOVE_SPEED;
            affectedByGravity = true;
        }

        public override void Spawn()
        {
            base.Spawn();
            currentState = new WaddleDeeWalkingState(this);
            velocity.X = Constants.WaddleDee.MOVE_SPEED;
        }

        public override void Move()
        {
            base.Move();
            if (stateMachine.IsLeft())
            {
                velocity.X = -Constants.WaddleDee.MOVE_SPEED;
            }
            else
            {
                velocity.X = Constants.WaddleDee.MOVE_SPEED;
            }
        }

    }
}
