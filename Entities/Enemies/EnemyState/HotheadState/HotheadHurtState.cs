using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState
{
    public class HotheadHurtState : IEnemyState
    {
            private readonly Hothead _hothead;

            public HotheadHurtState(Hothead hothead)
            {
                _hothead = hothead ?? throw new ArgumentNullException(nameof(hothead));
            }

            public void Enter()
            {
                _hothead.ChangePose(EnemyPose.Hurt);
                _hothead.ResetFrameCounter();
            }

            public void Update()
            {
                if (_hothead.FrameCounter >= Constants.Hothead.HURT_FRAMES)
                {
                    _hothead.ChangeState(new HotheadWalkingState(_hothead));
                }
            }

            public void Exit() { }

            public void TakeDamage()
            {
                // Optionally handle multiple hits
            }

            public void ChangeDirection()
            {
                _hothead.ToggleDirection();
            }
        }
    }
