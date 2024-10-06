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
        private KeyboardController keyboard;
        private Keys key;
        private TimeCalculator timer;
        private double timeSinceMoveStopped;
        private IPlayer kirby;
        private bool isRunning;
        public KirbyRunningRightCommand(Game1 newGame, KeyboardController newKeyboard, Keys newKey, IPlayer player)
        {
            game = newGame;
            keyboard = newKeyboard;
            key = newKey;
            kirby = player;
            timer = new TimeCalculator();
            isRunning = false;
            timeSinceMoveStopped = 0;
        }
        public void Execute()
        {
            // Walking command alerts us Kirby is moving and elapased time since stopping is 0
            if (keyboard.currentState.Contains(key) && timeSinceMoveStopped != 0)
            {
                double currentTime = timer.GetCurrentTimeInMS(game.time);
                // Kirby should run if elapsed time is within predetermined response time
                isRunning = currentTime - timeSinceMoveStopped < Constants.Controller.RESPONSE_TIME;
                // reset timer
                timeSinceMoveStopped = 0;
            }
            // Called by the stop key mapping and needs to store stopped time
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