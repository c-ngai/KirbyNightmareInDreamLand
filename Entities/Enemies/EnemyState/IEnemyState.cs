using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies.EnemyState
{
    public interface IEnemyState
    {
        void Enter();         // State enters, sets up necessary values
        void Update();        // Update logic for this state
        void Exit();          // Cleanup on exit from this state
        void TakeDamage();    // Handle taking damage in different states
        void ChangeDirection();  // Handle changing direction
    }

}
