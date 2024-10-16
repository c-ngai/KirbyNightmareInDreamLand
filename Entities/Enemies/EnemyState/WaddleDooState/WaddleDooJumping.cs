using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooJumpingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public WaddleDooJumpingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Jumping);
            _enemy.ResetFrameCounter();
        }

        public void Update()
        {
            if (_enemy is IJumpable jumpableEnemy)
            {
                jumpableEnemy.Jump(); // Perform jump action
                _enemy.IncrementFrameCounter();

                if (!jumpableEnemy.IsJumping)
                {
                    _enemy.ChangeState(new WaddleDooWalkingState(_enemy));
                    _enemy.UpdateTexture();
                }
            }
            else
            {
                // If the enemy cannot jump, transition back to walking
                _enemy.ChangeState(new WaddleDooWalkingState(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

        public void TakeDamage()
        {
            _enemy.ChangeState(new WaddleDooHurtState(_enemy));
            _enemy.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }
    }
}
