using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtFlyingSlowState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.FlyingSlow);
            enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update(Enemy enemy)
        {
            enemy.Move(); // Move logic

            // Transition to FlyingFast state after slow fly frames
            if (enemy.FrameCounter >= Constants.BrontoBurt.SLOW_FLY_FRAMES)
            {
                enemy.ChangeState(new BrontoBurtFlyingFastState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
