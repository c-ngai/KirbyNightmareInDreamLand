using System;
namespace KirbyNightmareInDreamLand.Commands
{
    public class SelectQuitCommand : ICommand
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
