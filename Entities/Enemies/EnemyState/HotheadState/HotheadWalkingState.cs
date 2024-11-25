using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadWalkingState : IEnemyState
    {
        private readonly Hothead _hothead;

        public HotheadWalkingState(Hothead hothead)
        {
            _hothead = hothead ?? throw new ArgumentNullException(nameof(hothead));
        }

        public void Enter()
        {
            _hothead.ChangePose(EnemyPose.Walking);
            _hothead.ResetFrameCounter();
        }

        public void Update()
        {
            _hothead.Move(); // Execute walking movement logic
            _hothead.IncrementFrameCounter();

            if (_hothead.FrameCounter >= Constants.Hothead.WALK_FRAMES)
            {
                _hothead.UpdateDirection();
                _hothead.ChangeState(new HotheadChargingState(_hothead));
            }
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

        public void TakeDamage()
        {
            _hothead.ChangeState(new EnemyHurtState(_hothead));
        }

        public void ChangeDirection()
        {
            _hothead.ToggleDirection();
        }

        public void Dispose()
        {

        }

    }
}
