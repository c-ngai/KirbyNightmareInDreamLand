using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Time;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyRunningRightCommand : ICommand
    {
        private Game1 game;
        private TimeCalculator timer;
        private double timeSinceMoveStopped;
        private bool isRunning;

        public KirbyRunningRightCommand()
        {
            game = Game1.Instance;
            timer = new TimeCalculator();
            isRunning = false;
            timeSinceMoveStopped = 0;
        }

        public void Execute()
        {
            KeyboardController keyboard = game.KeyboardController;
            IPlayer kirby = game.players[0]; // Assuming single-player mode for now
            Keys key = Keys.Right;

            if (keyboard.currentState.Contains(key) && timeSinceMoveStopped != 0)
            {
                double currentTime = timer.GetCurrentTimeInMS(game.time);
                isRunning = currentTime - timeSinceMoveStopped < Constants.Controller.RESPONSE_TIME;
                timeSinceMoveStopped = 0;
            }

            if (!keyboard.currentState.Contains(key))
            {
                timeSinceMoveStopped = timer.GetCurrentTimeInMS(game.time);
                isRunning = false;
            }

            if (isRunning)
            {
                kirby.RunRight();
            }
            else if (!keyboard.currentState.Contains(key))
            {
                kirby.StopMoving();
            }
        }
    }

}