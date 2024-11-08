using KirbyNightmareInDreamLand.Audio;
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
        private readonly Hothead _hothead;
        private SoundInstance sound;

        public HotheadAttackingState(Hothead hothead)
        {
            _hothead = hothead ?? throw new ArgumentNullException(nameof(hothead));
        }

        public void Enter()
        {
            _hothead.ChangePose(EnemyPose.Attacking);
            _hothead.ResetFrameCounter();
            sound = SoundManager.CreateInstance("hotheadflamethrowerattack");
            sound.Play();
        }

        public void Update()
        {
            _hothead.Flamethrower();

            if (_hothead.FrameCounter >= Constants.Hothead.ATTACK_FRAMES)
            {
                _hothead.ChangeState(new HotheadWalkingState(_hothead));
            }
        }

        public void Exit() {
            sound.Stop();
        }

        public void TakeDamage()
        {
            _hothead.ChangeState(new HotheadHurtState(_hothead));
            _hothead.UpdateTexture();
        }

        public void ChangeDirection()
        {
            _hothead.ToggleDirection();
        }
    }
}
