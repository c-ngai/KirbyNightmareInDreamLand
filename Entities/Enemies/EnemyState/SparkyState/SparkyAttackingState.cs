using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyAttackingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Attacking);
            enemy.ResetFrameCounter();
        }

        public void Update(Enemy enemy)
        {
            enemy.Attack(); // Perform the attack

            // Transition to hurt state after the attack frames
            if (enemy.FrameCounter >= Constants.Sparky.ATTACK_TIME)
            {
                enemy.ChangeState(new SparkyHurtState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
