using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooHurtState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hurt);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state

        }

        public void Update(Enemy enemy)
        {
            if (enemy.FrameCounter >= Constants.WaddleDoo.HURT_FRAMES)
            {
                enemy.ChangeState(new WaddleDooJumpingState());
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
