using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class ProfessorKirbyChargingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public ProfessorKirbyChargingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Charging);
            _enemy.ResetFrameCounter();
        }

        public void Update()
        {
            // Implement charging behavior, e.g., increased speed or special attacks
            _enemy.IncrementFrameCounter();

            if (_enemy.FrameCounter >= Constants.ProfessorKirby.STOP_FRAMES)
            {
                _enemy.ChangeState(new ProfessorKirbyAttackingState(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

        public void TakeDamage()
        {
            _enemy.ChangeState(new EnemyHurtState(_enemy));
            _enemy.UpdateTexture();
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
