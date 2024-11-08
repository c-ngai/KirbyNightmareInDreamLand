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
            Game1.Instance.Level.SelectQuit();
        }
    }
}
