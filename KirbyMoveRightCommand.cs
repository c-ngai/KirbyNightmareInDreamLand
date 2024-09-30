using Microsoft.Xna.Framework.Input;
using MasterGame.Time;
using MasterGame.Entities.Players;
using MasterGame.Controllers;

namespace MasterGame.Commands
{
    public class KirbyMoveRightCommand : ICommand
    {
        private Game1 game;
        private KeyboardController keyboard;
        private Keys key;

        private TimeCalculator timer;
        private double timeSinceMoveStopped;

        private IPlayer kirby;
        private bool isRunning;

        public KirbyMoveRightCommand(IPlayer player, Keys keyMapped, KeyboardController currentKeyboard, Game1 currentGame)
        {
            game = currentGame;
            keyboard = currentKeyboard;
            key = keyMapped;

            timer = new TimeCalculator();
            timeSinceMoveStopped = 0;

            kirby = player;
            isRunning = false;
        }

        public void Execute()
        {
            // Backlog: if it switches from moving right to moving left it needs a skid animation

            // if it stopped being pressed and is now re executed check if the elapsed time is a threshold value (currently 0.25s)
            if (timeSinceMoveStopped != 0)
            {
                double currentTime = timer.GetCurrentTimeInMS(game.time);
                isRunning = currentTime - timeSinceMoveStopped < Constants.Controller.RESPONSE_TIME;

                // reset timer
                timeSinceMoveStopped = 0;
            }

            if (isRunning)
            {
                kirby.RunRight();
            }
            else
            {
                kirby.MoveRight();
            }
        }

        public void Undo()
        {
            // if it just stopped being pressed set stop time
            if (keyboard.oldKeyStates[key])
            {
                timeSinceMoveStopped = timer.GetCurrentTimeInMS(game.time);
            }
            // if running switch to walking, if walking switch to stop moving
            kirby.StopMoving();
        }
    }
}
