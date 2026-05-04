using _2048Game.Core;

namespace _2048Game.States
{
    public interface IGameState
    {
        string StateName { get; }
        void Enter();
        void Update();
        void Exit();
        void HandleInput(ConsoleKey key);
    }
}