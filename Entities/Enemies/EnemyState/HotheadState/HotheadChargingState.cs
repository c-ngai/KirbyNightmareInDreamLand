using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadChargingState : IEnemyState
    {
            private readonly Hothead _hothead;

            public HotheadChargingState(Hothead hothead)
            {
                _hothead = hothead ?? throw new ArgumentNullException(nameof(hothead));
            }

            public void Enter()
            {
                _hothead.ChangePose(EnemyPose.Charging);
                _hothead.ResetFrameCounter();
            }

            public void Update()
            {
                if (_hothead.FrameCounter == Constants.Hothead.FRAME_1) // Fireball on frame 1
                {
                    _hothead.Attack();
                }

                if (_hothead.FrameCounter >= Constants.Hothead.SHOOT_FRAMES)
                {
                    _hothead.ChangeState(new HotheadAttackingState(_hothead));
                }
            }

            public void Exit() { }

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
