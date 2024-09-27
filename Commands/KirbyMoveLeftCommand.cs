﻿using Microsoft.Xna.Framework.Input;

namespace MasterGame
{
    public class KirbyMoveLeftCommand : ICommand
    {
        private Game1 game;
        private KeyboardController keyboard;
        private Keys key;

        private TimeCalculator timer;
        private double timeSinceMoveStopped;

        private IPlayer kirby;
        private bool isRunning;

        public KirbyMoveLeftCommand(IPlayer player, Keys keyMapped, KeyboardController currentKeyboard, Game1 currentGame)
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
            // Backlog: If it switches from moving left to moving right it needs a skid animation
            // if it stopped being pressed and is now re executed check if the elapsed time is less than 0.5 second
            if (timeSinceMoveStopped != 0)
            {
                double currentTime = timer.GetCurrentTimeInMS(game.time);
                isRunning = currentTime - timeSinceMoveStopped < Constants.Controller.RESPONSE_TIME;

                // reset timer
                timeSinceMoveStopped = 0;
            }

            if (isRunning)
            {
                kirby.RunLeft();
            }
            else
            {
                kirby.MoveLeft();
            }
        }

        public void Undo()
        {
            // if it just stopped being pressed set stop time
            if (keyboard.oldKeyStates[key])
            {
                timeSinceMoveStopped = timer.GetCurrentTimeInMS(game.time);
            }
            
            kirby.StopMoving();
        }
    }
}
