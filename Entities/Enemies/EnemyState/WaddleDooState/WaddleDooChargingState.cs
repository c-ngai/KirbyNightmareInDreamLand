using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class WaddleDooChargingState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Charging);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state

        }

        public void Update(Enemy enemy)
        {
            // Logic for charging (if needed)

            if (enemy.FrameCounter >= Constants.WaddleDoo.STOP_FRAMES)
            {
                enemy.ChangeState(new WaddleDooAttackingState());
                enemy.UpdateTexture();
            }
        }

        public void Exit(Enemy enemy) { }
    }
}
