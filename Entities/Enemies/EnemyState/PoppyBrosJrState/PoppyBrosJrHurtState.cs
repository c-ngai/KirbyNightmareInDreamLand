using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.PoppyBrosJrState
{
    public class PoppyBrosJrHurtState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hurt);
            enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update(Enemy enemy)
        {
            // Logic for when PoppyBrosJr is hurt

            if (enemy.FrameCounter >= Constants.PoppyBrosJr.HURT_FRAMES)
            {
                enemy.ChangeState(new PoppyBrosJrHopState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
