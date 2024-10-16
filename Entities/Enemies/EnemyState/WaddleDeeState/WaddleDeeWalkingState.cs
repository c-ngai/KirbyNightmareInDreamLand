using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState
{
    public class WaddleDeeWalkingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public WaddleDeeWalkingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Walking);
            _enemy.ResetFrameCounter();
        }

        public void Update()
        {
           _enemy.Move(); // Execute walking movement logic
            _enemy.IncrementFrameCounter(); // Increment the enemy's frame counter

            /* 
            if (_enemy.FrameCounter >= Constants.WaddleDee.WALK_FRAMES)
            {
                _enemy.ChangeState(new WaddleDeeHurtState(_enemy));
            }
            */
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

        public void TakeDamage()
        {
            _enemy.ChangeState(new WaddleDeeHurtState(_enemy));
            _enemy.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }
    }
}
