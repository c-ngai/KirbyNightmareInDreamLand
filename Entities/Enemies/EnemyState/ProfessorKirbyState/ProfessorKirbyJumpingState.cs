﻿using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.ProfessorKirbyState
{
    public class ProfessorKirbyJumpingState : IEnemyState
    {
        private readonly Enemy _enemy;

        public ProfessorKirbyJumpingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.FaceNearestPlayer();
            _enemy.ChangePose(EnemyPose.Jumping);
            _enemy.Jump();
        }

        public void Update()
        {
            _enemy.Move();
        }

        public void Exit()
        {
            // Cleanup logic if necessary
        }

        public void TakeDamage()
        {
            _enemy.ChangeState(new EnemyHurtState(_enemy));
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
