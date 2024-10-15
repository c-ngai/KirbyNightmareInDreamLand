using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState
{
    public class WaddleDeeHurtState : IEnemyState
    {

        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hurt);
            enemy.UpdateTexture(); // Update sprite to hurt texture
            enemy.ResetFrameCounter(); // Reset enemy's frame counter
        }

        public void Update(Enemy enemy)
        {
            enemy.IncrementFrameCounter(); // Increment the enemy's frame counter

            // Transition back to walking after a certain number of frames
            if (enemy.FrameCounter >= Constants.WaddleDee.HURT_FRAMES)
            {
                enemy.ChangeState(new WaddleDeeWalkingState());
            }
        }

        public void Exit(Enemy enemy)
        {
            // Any logic that needs to occur when exiting the hurt state
        }
    }
}
