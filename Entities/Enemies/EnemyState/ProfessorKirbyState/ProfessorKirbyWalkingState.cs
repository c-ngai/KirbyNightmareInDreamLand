﻿using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState
{
    public class ProfessorKirbyWalkingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public ProfessorKirbyWalkingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Walking);
            _enemy.ResetFrameCounter();
        }

        public void Update()
        {
            _enemy.Move(); // Execute walking movement logic

                if (_enemy.FrameCounter >= Constants.ProfessorKirby.WALK_FRAMES)
                {
                    _enemy.ChangeState(new ProfessorKirbyChargingState(_enemy));
                    _enemy.UpdateTexture();
                }
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

        public void TakeDamage()
        {
            _enemy.ChangeState(new ProfessorKirbyHurtState(_enemy));
            _enemy.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }

        public void Dispose()
        {

        }

    }
}