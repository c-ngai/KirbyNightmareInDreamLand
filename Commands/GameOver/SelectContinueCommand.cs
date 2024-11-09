using System;
namespace KirbyNightmareInDreamLand.Commands
{
	public class SelectContinueCommand : ICommand
    {
		public SelectContinueCommand()
		{
		}

		public void Execute()
		{
            Game1.Instance.Level.SelectContinue();

        }
    }
}

