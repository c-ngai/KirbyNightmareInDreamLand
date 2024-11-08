using System;
namespace KirbyNightmareInDreamLand.Commands.GameOver
{
    public class SelectQuitCommand
    {
        public SelectQuitCommand()
        {
        }

        public void Execute()
        {
            myGame.Exit();
        }
    }
}
