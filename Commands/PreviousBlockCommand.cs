using KirbyNightmareInDreamLand.Block;

namespace KirbyNightmareInDreamLand.Commands
{
    public class PreviousBlockCommand : ICommand
    {
        public PreviousBlockCommand()
        {
        }

        public void Execute()
        {
            BlockList.Instance.ViewPrevious();
        }

        public void Undo()
        {

        }
    }
}

