using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Time;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyRunningLeftCommand : ICommand
    {
        private Game1 game;
        private TimeCalculator timer;
        private double timeSinceMoveStopped;
        private bool isRunning;

        public KirbyRunningLeftCommand()
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
            Keys key = Keys.Left;

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
                kirby.RunLeft();
            }
            else if (!keyboard.currentState.Contains(key))
            {
                kirby.StopMoving();
            }
        }
    }
}