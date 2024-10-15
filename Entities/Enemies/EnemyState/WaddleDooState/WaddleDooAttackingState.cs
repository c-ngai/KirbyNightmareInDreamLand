using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooAttackingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Attacking);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state
        }

        public void Update(Enemy enemy)
        {
            enemy.Attack(); // Attack immediately on entering the state    

            if (enemy.FrameCounter >= Constants.WaddleDoo.ATTACK_FRAMES)
            {
                enemy.ChangeState(new WaddleDooHurtState());
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
