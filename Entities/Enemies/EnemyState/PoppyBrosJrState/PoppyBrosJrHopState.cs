using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.PoppyBrosJrState
{
    public class PoppyBrosJrHopState : IEnemyState
    {
        private readonly Enemy _enemy;
        public PoppyBrosJrHopState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }
        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hop);
            _enemy.ResetFrameCounter(); 
        }

        public void Update()
        {
            if (_enemy is PoppyBrosJr jumpableEnemy)
            {
                jumpableEnemy.Jump(); // Perform jump action
                _enemy.IncrementFrameCounter();

                if (!jumpableEnemy.IsJumping)
                {
                    _enemy.ChangeState(new PoppyBrosJrLandState(_enemy));
                   // _enemy.UpdateTexture();
                }
            }
            else
            {
                // If the enemy cannot jump, transition back to walking
                _enemy.ChangeState(new PoppyBrosJrLandState(_enemy));
                //_enemy.UpdateTexture();
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
