using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtFlyingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public BrontoBurtFlyingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            
        }

        public void Update()
        {
            _enemy.Move(); // Move logic
        }

        public void Exit() { }

        public void TakeDamage()
        {
            // Handle damage logic, e.g., transition to hurt state
            _enemy.ChangeState(new EnemyHurtState(_enemy));
        }

        public void ChangeDirection()
        {
            // Implement direction change logic, if applicable
            _enemy.ToggleDirection();
        }

        public void Dispose()
        {

        }

    }
}
