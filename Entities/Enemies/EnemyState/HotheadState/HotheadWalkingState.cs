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
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Walking);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state
        }

        public void Update(Enemy enemy)
        {
            enemy.Move();

            // Transition to Charging state after specified frames
            if (enemy.FrameCounter >= Constants.Hothead.WALK_FRAMES)
            {
                enemy.ChangeState(new HotheadChargingState());
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
