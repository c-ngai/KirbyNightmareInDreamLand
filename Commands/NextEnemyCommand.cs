using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Commands
{
    public class NextEnemyCommand : ICommand
    {
        ObjectManager objectManager;

        public NextEnemyCommand()
        {
            objectManager = ObjectManager.Instance;
        }

        // Move to the next enemy in the list
        public void Execute()
        {
            objectManager.currentEnemyIndex = (objectManager.currentEnemyIndex + 1) % objectManager.enemyList.Length;
        }
    }
}
