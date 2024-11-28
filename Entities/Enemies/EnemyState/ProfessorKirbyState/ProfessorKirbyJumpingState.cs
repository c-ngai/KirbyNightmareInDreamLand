using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.ProfessorKirbyState
{
    public class ProfessorKirbyJumpingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public ProfessorKirbyJumpingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Jumping);
        }

        public void Update()
        {
            if (_enemy is WaddleDoo jumpableEnemy)
            {
                jumpableEnemy.Jump(); // Perform jump action
                

                if (!jumpableEnemy.IsJumping)
                {
                    _enemy.ChangeState(new ProfessorKirbyWalkingState(_enemy));
                }
            }
            else
            {
                // If the enemy cannot jump, transition back to walking
                _enemy.ChangeState(new ProfessorKirbyWalkingState(_enemy));
            }
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

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
