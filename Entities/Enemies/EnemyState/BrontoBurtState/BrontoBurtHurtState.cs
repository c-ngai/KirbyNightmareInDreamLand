using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtHurtState : IEnemyState
    {
        private readonly Enemy _enemy;

        public BrontoBurtHurtState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hurt);
            _enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update()
        {
            if (_enemy.FrameCounter >= Constants.BrontoBurt.HURT_FRAMES)
            {
                _enemy.ChangeState(new BrontoBurtStandingState(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            _enemy.Health -= 1; // Accessing Health property

            if (_enemy.Health <= 0) // Accessing Health property
            {
                _enemy.IsDead = true; // Accessing IsDead property
                // Optionally, transition to a dead state
            }
        }

        public void ChangeDirection()
        {
            // Implement direction change logic, if applicable
            _enemy.ToggleDirection();
        }
    }
}
