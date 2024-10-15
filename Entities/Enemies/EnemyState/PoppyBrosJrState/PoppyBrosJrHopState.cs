using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.PoppyBrosJrState
{
    public class PoppyBrosJrHopState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hop);
            enemy.ResetFrameCounter(); // Reset frame counter when entering the state
        }

        public void Update(Enemy enemy)
        {
            // Execute hopping logic
            enemy.Move();

            // Transition to Hurt state after hop frames
            if (enemy.FrameCounter >= Constants.PoppyBrosJr.HOP_FRAMES)
            {
                enemy.ChangeState(new PoppyBrosJrHurtState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
