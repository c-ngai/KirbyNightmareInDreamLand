namespace MasterGame.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
