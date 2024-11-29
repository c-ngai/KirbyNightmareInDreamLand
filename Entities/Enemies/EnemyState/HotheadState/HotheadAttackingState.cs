using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadAttackingState : IEnemyState
    {
        private readonly Enemy _enemy;
        

        public HotheadAttackingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Attacking);
            _enemy.Attack(); //flamethrower
        }

        public void Update()
        {
            //if (_enemy.FrameCounter >= Constants.Hothead.ATTACK_FRAMES)
            //{
            //    _enemy.ChangeState(new HotheadWalkingState(_enemy));
            //}
        }

        public void Exit()
        {

        }

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
