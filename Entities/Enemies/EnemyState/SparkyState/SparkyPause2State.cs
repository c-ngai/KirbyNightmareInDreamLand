using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyPause2State : IEnemyState
    {
            private readonly Enemy _enemy;

            public SparkyPause2State(Enemy enemy)
            {
                _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            }

            public void Enter()
            {
                _enemy.UpdateDirection();
                _enemy.ChangePose(EnemyPose.Standing);
                _enemy.ResetFrameCounter();
            }

            public void Update()
            {
                // Wait for a defined period of time
                _enemy.IncrementFrameCounter();

                if (_enemy.FrameCounter >= Constants.Sparky.PAUSE_TIME)
                {
                    _enemy.ChangeState(new SparkyAttackingState(_enemy));
                }
            }

            public void Exit() { }

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
