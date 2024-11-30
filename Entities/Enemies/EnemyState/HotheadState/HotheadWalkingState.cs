using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadWalkingState : IEnemyState
    {
        private readonly Enemy _enemy;

        private int walkingDuration;

        public HotheadWalkingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            // Set the duration of this walking state to a random value in the range
            walkingDuration = _enemy.random.Next(Constants.Hothead.WALK_MIN_FRAMES, Constants.Hothead.WALK_MAX_FRAMES);
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
                _enemy.FaceNearestPlayer();
                _enemy.ChangeState(new HotheadChargingState(_enemy));
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
