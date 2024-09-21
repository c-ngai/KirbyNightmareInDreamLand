namespace MasterGame
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
