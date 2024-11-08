using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyAttackingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public SparkyAttackingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Attacking);
            _enemy.ResetFrameCounter();
        }

        public void Update()
        {
            _enemy.Attack(); // Perform the attack
            _enemy.IncrementFrameCounter();

            // Transition to hurt state after the attack frames
            if (_enemy.FrameCounter >= Constants.Sparky.ATTACK_TIME)
            {
                _enemy.ChangeState(new SparkyPause1State(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            _enemy.ChangeState(new SparkyHurtState(_enemy));
            _enemy.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }
    }
}
