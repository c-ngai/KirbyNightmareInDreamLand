using KirbyNightmareInDreamLand.Block;

namespace KirbyNightmareInDreamLand.Commands
{
    public class NextBlockCommand : ICommand
    {
        public NextBlockCommand()
        {
        }

        public void Execute()
        {
            BlockList.Instance.ViewNext();
        }

        public void Undo()
        {
        }
    }
}

