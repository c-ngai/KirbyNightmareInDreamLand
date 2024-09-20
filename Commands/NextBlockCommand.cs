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

        // will go away soon 
        public void SetState()
        {
            throw new NotImplementedException();
        }
    }
}

