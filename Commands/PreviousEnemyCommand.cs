namespace KirbyNightmareInDreamLand.Commands
{
    public class PreviousEnemyCommand : ICommand
    {
        private ObjectManager objectManager;

        public PreviousEnemyCommand()
        {
            this.objectManager = ObjectManager.Instance;
        }

        // Move to the previous enemy in the list
        public void Execute()
        {
            if (objectManager.currentEnemyIndex == 0)
            {
                objectManager.currentEnemyIndex = objectManager.enemyList.Length - 1;
            }
            else
            {
                objectManager.currentEnemyIndex--;
            }
        }
    }

}
