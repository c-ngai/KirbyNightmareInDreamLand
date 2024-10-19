namespace KirbyNightmareInDreamLand.Commands
{
    public class PreviousEnemyCommand : ICommand
    {
        private ObjectManager manager;

        public PreviousEnemyCommand()
        {
            manager = ObjectManager.Instance;
        }

        // Move to the previous enemy in the list
        public void Execute()
        {
            if (manager.CurrentEnemyIndex == 0)
            {
                manager.CurrentEnemyIndex = manager.EnemyList.Length - 1;
            }
            else
            {
                manager.CurrentEnemyIndex--;
            }
        }
    }

}
