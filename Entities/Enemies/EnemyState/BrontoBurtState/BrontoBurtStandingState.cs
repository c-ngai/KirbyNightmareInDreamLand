using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtStandingState : IEnemyState
    {
            private readonly Enemy _enemy;

            public BrontoBurtStandingState(Enemy enemy)
            {
                _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            }

            public void Enter()
            {
                _enemy.ChangePose(EnemyPose.Standing);
                _enemy.ResetFrameCounter(); // Reset frame counter when entering the state
            }

            public void Update()
            {
                // Logic for standing state (can be idle or wait for a condition)

                if (_enemy.FrameCounter >= Constants.BrontoBurt.STANDING_FRAMES)
                {
                    _enemy.ChangeState(new BrontoBurtFlyingSlowState(_enemy));
                    _enemy.UpdateTexture();
                }
            }

            public void Exit() { }

            public void TakeDamage()
            {
                // Transition to hurt state on taking damage
                _enemy.ChangeState(new BrontoBurtHurtState(_enemy));
                _enemy.UpdateTexture();
            }

            public void ChangeDirection()
            {
            // Implement direction change logic, if applicable
                _enemy.ToggleDirection();
        }


        public void Dispose()
        {

        }

    }
    }
