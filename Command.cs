using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Sprint0
{
    public class Command : ICommand
    {
        private string name;

        private Dictionary<string, Action> commandMapping;

        public Command(string name)
        {
            this.name = name;
            commandMapping = new Dictionary<string, Action>();
            LinkCommands(commandMapping);

        }

        public void LinkCommands(Dictionary<string, Action> commands)
        {
            // must be updated with command changes
            commands.Add("Quit", Quit);
            commands.Add("UnanimatedUnmoving", UnanimatedUnmoving);
            commands.Add("AnimatedUnmoving", AnimatedUnmoving);
            commands.Add("UnanimatedMoving", UnanimatedMoving);
            commands.Add("AnimatedMoving", AnimatedMoving);
            commands.Add("DisplayText", DisplayText);
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
        private void DisplayText()
        {

        }

        public void Execute()
        {
            commandMapping[name]();
        }
    }
}

