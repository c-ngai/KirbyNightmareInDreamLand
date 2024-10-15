using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyShortJumpState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hop);
            enemy.ResetFrameCounter(); // Reset frame counter on entering the state

            if (enemy is Sparky sparky)
            {
                sparky.SetHopHeight(Constants.Sparky.SHORT_HOP_HEIGHT); // Set the short hop height
            }
        }

        public void Update(Enemy enemy)
        {
            enemy.Move(); // Execute movement logic for a short jump

            // Transition to pausing after the jump
            if (enemy.FrameCounter >= Constants.Sparky.HOP_FREQUENCY)
            {
                enemy.ChangeState(new SparkyPause1State());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
