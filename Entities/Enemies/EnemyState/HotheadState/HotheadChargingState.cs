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
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Charging);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state
        }

        public void Update(Enemy enemy)
        {
            if (enemy.FrameCounter == 1) // Fireball on frame 1
            {
                enemy.Attack();
            }

            // Transition to Attacking state after specified frames
            if (enemy.FrameCounter >= Constants.Hothead.SHOOT_FRAMES)
            {
                enemy.ChangeState(new HotheadAttackingState());
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
