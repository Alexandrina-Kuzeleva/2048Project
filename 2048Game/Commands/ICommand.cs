namespace _2048Game.Commands
{
    public interface ICommand
    {
        void Execute();
        string GetDescription();
    }
}