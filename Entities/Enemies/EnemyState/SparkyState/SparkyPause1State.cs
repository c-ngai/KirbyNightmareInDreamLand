using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyPause1State : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Standing);
            enemy.ResetFrameCounter();
        }

        public void Update(Enemy enemy)
        {
            // Wait for a defined period of time
            if (enemy.FrameCounter >= Constants.Sparky.PAUSE_TIME)
            {
                enemy.ChangeState(new SparkyTallJumpState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
