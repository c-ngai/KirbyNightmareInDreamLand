using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class ProfessorKirbyAttackingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public ProfessorKirbyAttackingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Attacking);
            _enemy.ResetFrameCounter();
            _enemy.Attack(); // Perform attack action immediately upon entering
        }

        public void Update()
        {
            _enemy.IncrementFrameCounter();

            if (_enemy.FrameCounter >= Constants.ProfessorKirby.ATTACK_FRAMES)
            {
                _enemy.ChangeState(new ProfessorKirbyJumpingState(_enemy));
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
