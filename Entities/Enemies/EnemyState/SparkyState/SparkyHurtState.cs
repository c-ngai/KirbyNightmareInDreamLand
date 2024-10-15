using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyHurtState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hurt);
            enemy.ResetFrameCounter();
        }

        public void Update(Enemy enemy)
        {
            // Logic for when Sparky is hurt

            if (enemy.FrameCounter >= Constants.Sparky.HURT_FRAMES)
            {
                enemy.ChangeState(new SparkyShortJumpState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
