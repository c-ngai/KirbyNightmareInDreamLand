using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Commands
{
    public class NextEnemyCommand : ICommand
    {
        private ObjectManager manager;

        public NextEnemyCommand()
        {
            manager = ObjectManager.Instance;
        }

        // Move to the next enemy in the list
        public void Execute()
        {
            manager.CurrentEnemyIndex = (manager.CurrentEnemyIndex + 1) % manager.EnemyList.Length;
        }
    }
}
