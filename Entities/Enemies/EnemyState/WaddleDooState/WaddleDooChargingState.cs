using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooChargingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public WaddleDooChargingState(Enemy enemy)
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

            if (_enemy.FrameCounter >= Constants.WaddleDoo.STOP_FRAMES)
            {
                _enemy.ChangeState(new WaddleDooAttackingState(_enemy));
            }
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

        public void TakeDamage()
        {
            _enemy.ChangeState(new WaddleDooHurtState(_enemy));
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
