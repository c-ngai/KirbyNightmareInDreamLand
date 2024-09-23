using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterGame.Commands
{
    public class PreviousEnemyCommand : ICommand
    {
        private Game1 game;

        public PreviousEnemyCommand(Game1 game)
        {
            this.game = game;
        }

        // Move to the previous enemy in the list
        public void Execute()
        {
            if (game.currentEnemyIndex == 0)
            {
                game.currentEnemyIndex = game.enemyList.Length - 1;
            }
            else
            {
                game.currentEnemyIndex--;
            }
        }

        // Move forward to the next enemy
        public void Undo()
        {
            game.currentEnemyIndex = (game.currentEnemyIndex + 1) % game.enemyList.Length;
        }
    }

}
