using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Time;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveRightCommand : ICommand
    {
        // Reference to player list
        private List<IPlayer> _players;
        // Index of player to execute on
        private int playerIndex;
        // Reference to Game1 for gameTime and UpdateCounter
        private Game1 _game;

        // Timer to keep track of time since last execution
        private TimeCalculator timer;
        // Time and frame count of last execution
        private double timeOfLastExecution;
        private int frameOfLastExecution;
        // Flag for if Kirby should be running or walking this frame
        private bool shouldRun;
        private bool wasDashing;

        public KirbyMoveRightCommand(int _playerIndex)
        {
            _players = ObjectManager.Instance.Players;
            playerIndex = _playerIndex;
            _game = Game1.Instance;
            timer = new TimeCalculator();

            timeOfLastExecution = 0;
            frameOfLastExecution = 0;
            shouldRun = false;
            wasDashing = false;
        }

        public void Execute()
        {
            // If a player of this index exists
            if (playerIndex < _players.Count)
            {
                // Record current time and frame count
                double currentTime = timer.GetCurrentTimeInMS(_game.time);
                int currentFrame = _game.UpdateCounter;

                // run this frame if the time since previous execution is less than the double-tap response time AND
                //   1. if shouldRun was FALSE the previous execution, then also if this execution is not happening on the update immediately after the previous
                shouldRun = shouldRun ?
                    (currentTime - timeOfLastExecution < Constants.Controller.RESPONSE_TIME) :
                    (currentTime - timeOfLastExecution < Constants.Controller.RESPONSE_TIME) && (currentFrame > frameOfLastExecution + 1);

                if (shouldRun && !wasDashing)
                {
                    SoundManager.Play("dash");
                }

                // Update wasDashing to match shouldRun for the next frame
                wasDashing = shouldRun;

                // If shouldRun, then run. If not, then walk.
                if (shouldRun)
                {
                    _players[playerIndex].RunRight();
                }
                else
                {
                    _players[playerIndex].MoveRight();
                }

                // Record time and frame count of this execution for the next execution to compare against
                timeOfLastExecution = currentTime;
                frameOfLastExecution = currentFrame;
            }
        }
    }
}
