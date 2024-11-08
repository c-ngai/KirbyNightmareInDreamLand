﻿using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState
{
    public class SparkyTallJumpState : IEnemyState
    {
        private readonly Enemy _enemy;

        public SparkyTallJumpState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hop);
            _enemy.ResetFrameCounter(); // Reset frame counter on entering the state

            if (_enemy is Sparky sparky)
            {
                sparky.SetHopHeight(Constants.Sparky.TALL_HOP_HEIGHT); // Set the tall hop height
            }
        }

        public void Update()
        {
            _enemy.Move(); // Execute movement logic for a tall jump
            _enemy.IncrementFrameCounter();

            // Transition to pausing after the jump
            if (_enemy.FrameCounter >= Constants.Sparky.HOP_FREQUENCY)
            {
                _enemy.ChangeState(new SparkyPause2State(_enemy));
                _enemy.UpdateTexture();
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            _enemy.ChangeState(new SparkyHurtState(_enemy));
            _enemy.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }
    }
}