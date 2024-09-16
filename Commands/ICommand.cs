namespace MasterGame
{
    public interface ICommand
    {
        void SetState();
        void Execute();
    }
}
