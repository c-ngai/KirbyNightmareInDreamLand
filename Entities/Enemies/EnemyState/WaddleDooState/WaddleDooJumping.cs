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
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state

        }

        public void Update(Enemy enemy)
        {
            if (enemy is IJumpable jumpableEnemy)
            {
                jumpableEnemy.Jump(); // Call the jump method if the enemy can jump

                if (!jumpableEnemy.IsJumping) // Check if the jump is finished
                {
                    enemy.ChangeState(new WaddleDooWalkingState());
                    enemy.UpdateTexture();
                }
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
