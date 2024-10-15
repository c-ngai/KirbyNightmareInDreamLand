using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState
{
    public class WaddleDeeWalkingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Walking);
            enemy.UpdateTexture(); // Update sprite to walking texture
            enemy.ResetFrameCounter(); // Reset enemy's frame counter
        }

        public void Update(Enemy enemy)
        {
            enemy.IncrementFrameCounter(); // Increment the enemy's frame counter

            // Move logic
            enemy.Move();

            // Transition to hurt state after a certain number of frames
            if (enemy.FrameCounter >= Constants.WaddleDee.WALK_FRAMES)
            {
                enemy.ChangeState(new WaddleDeeHurtState());
            }
        }

        public void Exit(Enemy enemy)
        {
            // Any logic that needs to occur when exiting the walking state
        }

    }
}
