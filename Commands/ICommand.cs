namespace KirbyNightmareInDreamLand.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
