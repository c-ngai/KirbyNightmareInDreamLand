using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState
{
    public class EnemyInhaledState : IEnemyState
    {
        private readonly Enemy _enemy;
        private readonly IPlayer _player;

        public EnemyInhaledState(Enemy enemy, IPlayer player)
        {
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            _player = player;
        }

        public void Enter()
        {
            _enemy.ChangePose(EnemyPose.Hurt);
            _enemy.ResetFrameCounter();
        }

        public void Update()
        {

            _enemy.IncrementFrameCounter();

            _enemy.AccellerateTowards(_player.GetKirbyPosition());

            //if (_enemy.FrameCounter >= Constants.WaddleDee.HURT_FRAMES)
            //{
            //    _enemy.ChangeState(new WaddleDeeWalkingState(_enemy));
            //    _enemy.UpdateTexture();

            //    if (_enemy.Health <= 0)
            //    {
            //        _enemy.Active = false;
            //        _enemy.CollisionActive = false;
            //    }
            //}
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
