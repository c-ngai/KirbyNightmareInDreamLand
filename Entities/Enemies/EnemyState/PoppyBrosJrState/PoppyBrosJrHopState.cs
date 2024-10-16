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
            _enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update()
        {
            // Execute hopping logic
            _enemy.Move();

            // Transition to Hurt state after hop frames
            if (_enemy.FrameCounter >= Constants.PoppyBrosJr.HOP_FRAMES)
            {
                _enemy.ChangeState(new PoppyBrosJrHurtState(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            _enemy.ChangeState(new WaddleDooHurtState(_enemy));
            _enemy.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }
    }
}
