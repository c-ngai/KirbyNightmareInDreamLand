namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyChangeSparkCommand : ICommand
    {

        public void Execute()
        {
            ObjectManager.Instance.Players[0].ChangeToSpark();

        }
    }
}
