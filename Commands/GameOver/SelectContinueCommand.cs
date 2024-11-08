using System;
namespace KirbyNightmareInDreamLand.Commands.GameOver
{
	public class SelectContinueCommand
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

