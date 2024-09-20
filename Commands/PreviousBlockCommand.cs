using System;
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

        // will go away soon 
        public void SetState()
        {
            throw new NotImplementedException();
        }
    }
}

