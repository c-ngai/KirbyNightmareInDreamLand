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
            affectedByGravity = true;
        }

        public override void Spawn()
        {
            base.Spawn();
            stateMachine.ChangePose(EnemyPose.Walking);
            currentState = new WaddleDeeWalkingState(this);
        }

        public override void Move()
        {
            base.Move();
            velocity.X = stateMachine.IsLeft() ? - Constants.WaddleDee.MOVE_SPEED : Constants.WaddleDee.MOVE_SPEED;
        }

    }
}
