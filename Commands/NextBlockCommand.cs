using System;
namespace MasterGame.Commands
{
	public class NextBlockCommand : ICommand
    {
		public NextBlockCommand()
		{
		}

        public void Execute()
        {
            BlockList.Instance.viewNext();
        }

        public void Undo()
        {
        }
    }
}

