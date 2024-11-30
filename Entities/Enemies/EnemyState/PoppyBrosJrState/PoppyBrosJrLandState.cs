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
    public class PoppyBrosJrLandState : IEnemyState
    {
        private readonly Enemy _enemy;
        public PoppyBrosJrLandState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }
        public void Enter()
        {
            _enemy.StopMoving();
        }

        public void Update()
        {
            // Wait for a defined period of time
            

            if (_enemy.FrameCounter >= Constants.PoppyBrosJr.PAUSE_TIME)
            {
                _enemy.ChangeState(new PoppyBrosJrHopState(_enemy));
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
