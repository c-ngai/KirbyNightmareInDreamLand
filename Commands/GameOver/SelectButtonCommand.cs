using System;
namespace KirbyNightmareInDreamLand.Commands
{
    public class SelectButtonCommand : ICommand
    {
        public SelectButtonCommand()
        {
        }
        public void Execute()
        {
            Game1.Instance.Level.SelectButton();
            System.Diagnostics.Debug.WriteLine("losing my mind rn");
        }
    }

}