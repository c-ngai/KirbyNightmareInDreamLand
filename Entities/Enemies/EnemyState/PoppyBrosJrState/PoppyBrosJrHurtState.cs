using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.PoppyBrosJrState
{
    public class PoppyBrosJrHurtState : IEnemyState
    {
        private readonly Enemy _enemy;
        public PoppyBrosJrHurtState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }
        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hop);
            _enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update()
        {
            // Execute hopping logic
            _enemy.Move();

            // Transition to Hurt state after hop frames
            if (_enemy.FrameCounter >= Constants.PoppyBrosJr.HURT_FRAMES)
            {
                _enemy.ChangeState(new PoppyBrosJrHopState(_enemy));
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
        }
    }
}
