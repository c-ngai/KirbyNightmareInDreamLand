using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtStandingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Standing);
            enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update(Enemy enemy)
        {
            // Logic for standing state (can be idle or wait for a condition)

            if (enemy.FrameCounter >= Constants.BrontoBurt.STANDING_FRAMES)
            {
                enemy.ChangeState(new BrontoBurtFlyingSlowState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
