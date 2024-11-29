using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyPauseState : IEnemyState
    {
        private readonly Enemy _enemy;

        private bool nextActionAttack; // true: attack next, false: jump next

        public SparkyPauseState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            nextActionAttack = _enemy.random.Next(0, 4) == 0; // 1 in 4 chance for the next action to be an attack
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Standing);
        }

        public void Update()
        {
            // Wait for a defined period of time
            _enemy.DecelerateX(Constants.Physics.X_DECELERATION);

            if (_enemy.FrameCounter >= Constants.Sparky.PAUSE_TIME)
            {
                // If the next action is an attack
                if (nextActionAttack)
                {
                    _enemy.ChangeState(new SparkyAttackingState(_enemy));
                }
                else
                {
                    _enemy.ChangeState(new SparkyJumpState(_enemy));
                }
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            _enemy.ChangeState(new EnemyHurtState(_enemy));
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }

        public void Dispose()
        {

        }

    }
}
