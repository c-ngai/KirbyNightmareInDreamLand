using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
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
            _enemy.ResetFrameCounter(); 
            _enemy.Health -= 1;
        }

        public void Update()
        {
            _enemy.IncrementFrameCounter();

            //TO-DO: CHANGE TO WHEN KIRBY + ENEMY COLLIDE
            if (_enemy.FrameCounter >= Constants.PoppyBrosJr.HURT_FRAMES)
            {
                _enemy.ChangeState(new PoppyBrosJrHopState(_enemy));
                _enemy.UpdateTexture();

                if (_enemy.Health <= 0)
                {
                    _enemy.IsDead = true;
                }
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            //handled in update
        }

        public void ChangeDirection()
        {
        }
    }
}
