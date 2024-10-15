using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooWalkingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Walking);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state
        }

        public void Update(Enemy enemy)
        {
            // Movement logic for walking
            enemy.Move();

            // Transition to charging if the frame count reaches the threshold
            if (enemy.FrameCounter >= Constants.WaddleDoo.WALK_FRAMES)
            {
                enemy.ChangeState(new WaddleDooChargingState());
            }
        }

        public void Exit(Enemy enemy)
        {
            // Cleanup if needed
        }
    }
}
