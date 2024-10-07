using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Commands
{
    public class NextEnemyCommand : ICommand
    {
        private Game1 game;

        public NextEnemyCommand(Game1 game)
        {
            this.game = game;
        }

        // Move to the next enemy in the list
        public void Execute()
        {
            game.currentEnemyIndex = (game.currentEnemyIndex + 1) % game.enemyList.Length;
        }
    }
}
