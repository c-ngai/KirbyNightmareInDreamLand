using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadChargingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public HotheadChargingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Charging);
            _enemy.FaceNearestPlayer();
            _enemy.StopMoving();
        }

        public void Update()
        {
            if (_enemy.FrameCounter >= Constants.Hothead.CHARGE_FRAMES)
            {
                _enemy.ChangeState(new HotheadAttackingState(_enemy));
            }
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
