using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooJumpingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Jumping);
            //  enemy.Jump(); // Start jumping
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state

        }

        public void Update(Enemy enemy)
        {

           // enemy.Jump(); // Update jumping logic

             //Check if the jump is finished to go back to walking
           // if (!enemy.IsJumping) // Assuming IsJumping is a public property
           // {
                enemy.ChangeState(new WaddleDooWalkingState());
                 enemy.UpdateTexture();
            //  }
        }

        public void Exit(Enemy enemy) { }
    }
}
