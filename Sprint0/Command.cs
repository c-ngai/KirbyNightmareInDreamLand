using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Sprint0
{
    public class Command : ICommand
    {
        private string Name;

        private Dictionary<string, Action> commandMapping;

        public Command(string name)
        {
            Name = name;
            commandMapping = new Dictionary<string, Action>();
            linkCommands(commandMapping);

        }

        public void linkCommands(Dictionary<string, Action> commands)
        {
            // must be updated with command changes
            commands.Add("Quit", Quit);
            commands.Add("UnanimatedUnmoving", UnanimatedUnmoving);
            commands.Add("AnimatedUnmoving", AnimatedUnmoving);
            commands.Add("UnanimatedMoving", UnanimatedMoving);
            commands.Add("AnimatedMoving", AnimatedMoving);
        }
        private void Quit()
        {
            Game1.self.Exit();
        }

        private void UnanimatedUnmoving()
        {

        }
        private void AnimatedUnmoving()
        {

        }
        private void UnanimatedMoving()
        {

        }
        private void AnimatedMoving()
        {

        }
        //private void DisplayText()
        //{

        //}

        public void Execute()
        {
            commandMapping[Name]();
        }
    }
}

