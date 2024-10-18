using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtFlyingSlowState : IEnemyState
    {
        private readonly Enemy _enemy;

        public BrontoBurtFlyingSlowState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.FlyingSlow);
            _enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update()
        {
            _enemy.Move(); // Move logic

            // Transition to FlyingFast state after slow fly frames
            if (_enemy.FrameCounter >= Constants.BrontoBurt.SLOW_FLY_FRAMES)
            {
                _enemy.ChangeState(new BrontoBurtFlyingFastState(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            // Handle damage logic, e.g., transition to hurt state
            _enemy.ChangeState(new BrontoBurtHurtState(_enemy));
            _enemy.UpdateTexture(); // Update texture on state change
        }

        public void ChangeDirection()
        {
            // Implement direction change logic, if applicable
            _enemy.ToggleDirection();
        }
    }
}
