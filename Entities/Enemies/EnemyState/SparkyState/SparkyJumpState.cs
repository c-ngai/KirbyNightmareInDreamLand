using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyJumpState : IEnemyState
    {
        private readonly Enemy _enemy;

        public SparkyJumpState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.FaceNearestPlayer();
            _enemy.ChangePose(EnemyPose.Hop); // Reset frame counter on entering the state
            _enemy.Jump(); // Perform jump action
        }

        public void Update()
        {
            _enemy.Move();
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
