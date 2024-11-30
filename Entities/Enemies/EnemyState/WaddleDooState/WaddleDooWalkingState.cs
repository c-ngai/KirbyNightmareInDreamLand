using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooWalkingState : IEnemyState
    {
        private readonly Enemy _enemy;

        private int walkingDuration;
        private bool nextActionAttack; // true: attack next, false: jump next

        public WaddleDooWalkingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));

            // Set the duration of this walking state to a random value in the range
            walkingDuration = _enemy.random.Next(Constants.WaddleDoo.WALK_MIN_FRAMES, Constants.WaddleDoo.WALK_MAX_FRAMES);
            // Randomly decide the next action to be either an attack or a jump
            nextActionAttack = _enemy.random.Next(2) == 0;
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Walking);
        }

        public void Update()
        {
            _enemy.Move(); // Execute walking movement logic

            if (_enemy.FrameCounter >= walkingDuration)
            {
                if (nextActionAttack)
                {
                    _enemy.ChangeState(new WaddleDooChargingState(_enemy));
                }
                else
                {
                    _enemy.ChangeState(new WaddleDooJumpingState(_enemy));
                }
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
