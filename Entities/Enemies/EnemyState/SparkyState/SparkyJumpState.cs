using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyJumpState : IEnemyState
    {
        private readonly Enemy _enemy;

        public SparkyJumpState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hop);
            _enemy.ResetFrameCounter(); // Reset frame counter on entering the state
        }

        public void Update()
        {
            if (_enemy is Sparky jumpableEnemy)
            {
                jumpableEnemy.Jump(); // Perform jump action
                _enemy.IncrementFrameCounter();

                if (!jumpableEnemy.IsJumping)
                {
                    _enemy.ChangeState(new SparkyPause2State(_enemy));
                    _enemy.UpdateTexture();
                }
            }
            else
            {
                // If the enemy cannot jump, transition back to walking
                _enemy.ChangeState(new SparkyPause2State(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            _enemy.ChangeState(new SparkyHurtState(_enemy));
            _enemy.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }
    }
}
