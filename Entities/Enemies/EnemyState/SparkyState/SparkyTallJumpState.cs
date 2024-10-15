using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyTallJumpState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hop);
            enemy.ResetFrameCounter(); // Reset frame counter on entering the state

            // Cast to Sparky to access Sparky-specific methods
            if (enemy is Sparky sparky)
            {
                sparky.SetHopHeight(Constants.Sparky.TALL_HOP_HEIGHT); // Set the tall hop height
            }
        }

        public void Update(Enemy enemy)
        {
            enemy.Move(); // Execute movement logic for a tall jump

            // Transition to pausing after the jump
            if (enemy.FrameCounter >= Constants.Sparky.HOP_FREQUENCY)
            {
                enemy.ChangeState(new SparkyPause2State());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
