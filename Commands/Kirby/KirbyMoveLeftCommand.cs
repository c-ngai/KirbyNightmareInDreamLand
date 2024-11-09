using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Particles;
using KirbyNightmareInDreamLand.Time;
using System.Collections.Generic;
using System.Numerics;

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

        // Timer to keep track of time since last execution
        private TimeCalculator timer;
        // Time and frame count of last execution
        private double timeOfLastExecution;
        private int frameOfLastExecution;
        // Flag for if Kirby should be running or walking this frame
        private bool shouldRun;
        //old state
        private bool wasDashing;

        public KirbyMoveLeftCommand(int _playerIndex)
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

                // Play dash sound and animation on transition from walking to dashing 
                if (shouldRun && !wasDashing)
                {
                    SoundManager.Play("dash");
                    bool isLeft = true;
                    IParticle cloud = new Cloud(_players[playerIndex].movement.GetPosition(), isLeft);
                }

                // Update wasDashing to match shouldRun for the next frame
                wasDashing = shouldRun;

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
                timeOfLastExecution = currentTime;
                frameOfLastExecution = currentFrame;
            }
        }
    }
}
