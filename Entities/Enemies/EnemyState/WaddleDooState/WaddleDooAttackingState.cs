using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooAttackingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public WaddleDooAttackingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Attacking);
            _enemy.Attack(); // Perform attack action immediately upon entering
        }

        public void Update()
        {
            

            if (_enemy.FrameCounter >= Constants.WaddleDoo.ATTACK_FRAMES)
            {
                _enemy.ChangeState(new WaddleDooWalkingState(_enemy));
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
