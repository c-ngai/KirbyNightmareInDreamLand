using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtFlyingFastState : IEnemyState
    {
        private readonly Enemy _enemy;

        public BrontoBurtFlyingFastState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.FlyingFast);
            _enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update()
        {
            _enemy.Move(); // Move logic

            // Transition to Hurt state after fast fly frames
            if (_enemy.FrameCounter >= Constants.BrontoBurt.FAST_FLY_FRAMES)
            {
                _enemy.ChangeState(new BrontoBurtHurtState(_enemy));
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
            _enemy.ToggleDirection();
        }
    }
}
