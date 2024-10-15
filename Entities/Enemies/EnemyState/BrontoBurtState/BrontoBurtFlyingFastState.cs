using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtFlyingFastState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.FlyingFast);
            enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update(Enemy enemy)
        {
            enemy.Move(); // Move logic

            // Transition to Hurt state after fast fly frames
            if (enemy.FrameCounter >= Constants.BrontoBurt.FAST_FLY_FRAMES)
            {
                enemy.ChangeState(new BrontoBurtHurtState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
