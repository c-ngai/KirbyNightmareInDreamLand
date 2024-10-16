using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyHurtState : IEnemyState
    {
        private readonly Enemy _enemy;

        public SparkyHurtState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hurt);
            _enemy.ResetFrameCounter();
        }

        public void Update()
        {
            // Logic for when Sparky is hurt
            _enemy.IncrementFrameCounter();

            if (_enemy.FrameCounter >= Constants.Sparky.HURT_FRAMES)
            {
                _enemy.ChangeState(new SparkyShortJumpState(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            // You might want to do nothing or handle something specific here
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }
    }
}
