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

<<<<<<< HEAD
        public void Undo()
        {
=======
        // will go away soon 
        public void SetState()
        {
            throw new NotImplementedException();
>>>>>>> a8f6ada808a3e6cd4614f1eebc7fb9db548ee563
        }
    }
}

