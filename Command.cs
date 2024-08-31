using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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
            commands.Add("UnanimatedMovingVertically", UnanimatedMovingVertically);
            commands.Add("AnimatedMovingHorizontally", AnimatedMovingHorizontally);
        }
        private void Quit()
        {
            Game1.self.Exit();
        }

        private void UnanimatedUnmoving()
        {

            Game1.self.state = 1;
        }
        private void AnimatedUnmoving()
        {
            Game1.self.state = 2;
            //Game1.self.animatedUnmovingSprite.Draw(Game1.self.spriteBatch, new Vector2(350, 200));
        }
        private void UnanimatedMovingVertically()
        {
            Game1.self.state = 3;
        }
        private void AnimatedMovingHorizontally()
        {
            Game1.self.state = 4;
        }

        public void Execute()
        {
            commandMapping[name]();
        }
    }
}

