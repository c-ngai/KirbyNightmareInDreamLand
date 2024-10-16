﻿using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadHurtState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            enemy.StateMachine.ChangePose(EnemyPose.Hurt);
            enemy.ResetFrameCounter(); // Reset the frame counter upon entering the state
            
        }

        public void Update(Enemy enemy)
        {
            // Transition back to Walking after hurtFrames
            if (enemy.FrameCounter >= Constants.Hothead.HURT_FRAMES)
            {
                enemy.ChangeState(new HotheadWalkingState());
            }
        }

        public void Exit(Enemy enemy) { }
    }
}