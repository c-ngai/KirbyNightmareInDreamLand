using System;
using MasterGame.Block;
using Microsoft.Xna.Framework;

namespace MasterGame.Commands
{
	public class PreviousBlockCommand : ICommand
	{
		public PreviousBlockCommand()
		{
		}

        public void Execute()
        {
            BlockList.Instance.viewPrevious();
        }

        public void Undo()
        {
        }
    }
}

