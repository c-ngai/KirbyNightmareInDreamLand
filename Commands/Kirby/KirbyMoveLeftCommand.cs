using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Particles;
using KirbyNightmareInDreamLand.Time;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveLeftCommand : ICommand
    {
        // Reference to player list
        private List<IPlayer> _players;
        // Index of player to execute on
        private int playerIndex;
        // Reference to Game1 for gameTime and UpdateCounter
        private Game1 _game;

        // Frame count of last execution
        private int frameOfLastExecution;
        // Flag for if Kirby should be running or walking this frame
        private bool shouldRun;

        public KirbyMoveLeftCommand(int _playerIndex)
        {
            _players = ObjectManager.Instance.Players;
            playerIndex = _playerIndex;
            _game = Game1.Instance;

            frameOfLastExecution = 0;
            shouldRun = false;
        }

        public void Execute()
        {
            // If a player of this index exists
            if (playerIndex < _players.Count)
            {
                // Record current time and frame count
                int currentFrame = _game.UpdateCounter;

                // run this frame if the time since previous execution is less than the double-tap response time AND
                //   1. if shouldRun was FALSE the previous execution, then also if this execution is not happening on the update immediately after the previous
                shouldRun = shouldRun ?
                    (currentFrame - frameOfLastExecution < Constants.Controller.RESPONSE_FRAMES) :
                    (currentFrame - frameOfLastExecution < Constants.Controller.RESPONSE_FRAMES) && (currentFrame > frameOfLastExecution + 1);

                // If shouldRun, then run. If not, then walk.
                if (shouldRun)
                {
                    _players[playerIndex].RunLeft();
                }
                else
                {
                    _players[playerIndex].MoveLeft();
                }

                // Record time and frame count of this execution for the next execution to compare against
                frameOfLastExecution = currentFrame;
            }
        }
    }
}
