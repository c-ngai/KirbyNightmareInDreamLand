using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooHurtState : IEnemyState
    {
        private readonly Enemy _enemy;

        public WaddleDooHurtState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hurt);
            _enemy.ResetFrameCounter();
            _enemy.Health -= 1; 
        }

        public void Update()
        {

            _enemy.IncrementFrameCounter();

            //TO-DO: CHANGE TO WHEN KIRBY + ENEMY COLLIDE
            if (_enemy.FrameCounter >= Constants.WaddleDoo.HURT_FRAMES)
            {
                _enemy.ChangeState(new WaddleDooWalkingState(_enemy));
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
            //won't change direction while hurt
        }
    }
}
