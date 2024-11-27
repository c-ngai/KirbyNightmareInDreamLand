﻿using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Particles;
using KirbyNightmareInDreamLand.StateMachines;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState
{
    public class EnemyHurtState : IEnemyState
    {
        private readonly Enemy _enemy;

        public EnemyHurtState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hurt);
            _enemy.ResetFrameCounter();

            _enemy.Health -= Constants.Enemies.DAMAGE_TAKEN;
        }

        public void Update()
        {

            _enemy.IncrementFrameCounter();

            _enemy.Vibrate = (float)(Constants.Enemies.HURT_FRAMES - _enemy.FrameCounter) / Constants.Enemies.HURT_FRAMES * Constants.Enemies.HURT_VIBRATE_MAX_MAGNITUDE;

            //TO-DO: CHANGE TO WHEN KIRBY + ENEMY COLLIDE
            if (_enemy.FrameCounter >= Constants.Enemies.HURT_FRAMES)
            {
                if (_enemy.Health <= 0)
                {
                    _enemy.Active = false;
                    _enemy.CollisionActive = false;
                    new StarExplode(_enemy.GetPosition());
                    SoundManager.Play("enemyexplode");
                }
            }
        }

        public void Exit() { }

        public void TakeDamage()
        {
            //handled in update
        }

        public void ChangeDirection()
        {
            //won't change direction while hurt
        }

        public void Dispose()
        {

        }

    }
}