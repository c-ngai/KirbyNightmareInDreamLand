using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState
{
    public interface IEnemyState
    {
        void Update(Enemy enemy);
        void Enter(Enemy enemy);
        void Exit(Enemy enemy);
    }
}
