using KirbyNightmareInDreamLand.Audio;
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

            _enemy.Health -= Constants.Enemies.DAMAGE_TAKEN;
        }

        public void Update()
        {
            _enemy.Vibrate = (float)(Constants.Enemies.HURT_FRAMES - _enemy.FrameCounter) / Constants.Enemies.HURT_FRAMES * Constants.Enemies.HURT_VIBRATE_MAX_MAGNITUDE;

            // Just before disappearing, start the explosion particle
            if (_enemy.FrameCounter == Constants.Enemies.HURT_FRAMES - Constants.Particle.ENEMYEXPLODE_START_FRAME && _enemy.Health <= 0)
            {
                new EnemyExplode(_enemy);
                SoundManager.Play("enemyexplode");
            }
            // When hurt frames are up, disappear
            if (_enemy.FrameCounter >= Constants.Enemies.HURT_FRAMES && _enemy.Health <= 0)
            {
                _enemy.Active = false;
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
