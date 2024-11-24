using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Projectiles;
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
        private readonly Enemy _enemy;
        private SoundInstance sound;

        public SparkyAttackingState(Enemy enemy)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Attacking);
            _enemy.ResetFrameCounter();
            sound = SoundManager.CreateInstance("sparkyattack");
            sound.Play();
        }

        public void Update()
        {
            _enemy.Attack(); // Perform the attack

            // Transition to hurt state after the attack frames
            if (_enemy.FrameCounter >= Constants.Sparky.ATTACK_TIME)
            {
                _enemy.ChangeState(new SparkyPause1State(_enemy));
            }
        }

        public void Exit() {
            sound.Stop();
        }

        public void TakeDamage()
        {
            _enemy.ChangeState(new SparkyHurtState(_enemy));
            sound.Stop();
        }

        public void ChangeDirection()
        {
            _enemy.ToggleDirection();
        }

        public void Dispose()
        {
            sound.Dispose();
        }

    }
}
