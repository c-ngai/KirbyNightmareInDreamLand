using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.ProfessorKirbyState
{
    public class ProfessorKirbyWalkingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public ProfessorKirbyWalkingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Walking);
        }

        public void Update()
        {
            _enemy.Move(); // Execute walking movement logic

                if (_enemy.FrameCounter >= Constants.ProfessorKirby.WALK_FRAMES)
                {
                    _enemy.ChangeState(new ProfessorKirbyChargingState(_enemy));
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
