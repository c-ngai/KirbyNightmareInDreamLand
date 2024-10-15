using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState
{
    public class BrontoBurtHurtState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hurt);
            enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update(Enemy enemy)
        {
            // Logic for when BrontoBurt is hurt

            if (enemy.FrameCounter >= Constants.BrontoBurt.HURT_FRAMES)
            {
                enemy.ChangeState(new BrontoBurtStandingState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
