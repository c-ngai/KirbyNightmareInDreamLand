using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
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
                _hothead.Health -= Constants.Enemies.DAMAGE_TAKEN;
        }

        public void Update()
        {

            _hothead.IncrementFrameCounter();

            //TO-DO: CHANGE TO WHEN KIRBY + ENEMY COLLIDE
            if (_hothead.FrameCounter >= Constants.Hothead.HURT_FRAMES)
            {
                _hothead.ChangeState(new HotheadWalkingState(_hothead));
                _hothead.UpdateTexture();

                if (_hothead.Health <= 0)
                {
                    _hothead.Active = false;
                    _hothead.CollisionActive = false;
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
                _hothead.ToggleDirection();
            }

        public void Dispose()
        {

        }

    }
    }
