using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadAttackingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Attacking);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state
        }

        public void Update(Enemy enemy)
        {

            if (enemy is Hothead hothead)
            {
                hothead.Flamethrower();
            }

            // Transition to Hurt state after specified frames
            if (enemy.FrameCounter >= Constants.Hothead.ATTACK_FRAMES)
            {
                enemy.ChangeState(new HotheadHurtState());
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
