using KirbyNightmareInDreamLand.StateMachines;
using System;

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
            _enemy.Health -= Constants.Enemies.DAMAGE_TAKEN;
        }

        public void Update()
        {
            _enemy.IncrementFrameCounter();

                if (_enemy.Health <= 0)
                {
                    _enemy.Active = false;
                    _enemy.CollisionActive = false;
                }          
        }

        public void Exit() { }

        public void TakeDamage()
        {
            //handled in update
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
