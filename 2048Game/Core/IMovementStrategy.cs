using _2048Game.Entities;
using _2048Game.Systems;

namespace _2048Game.Core
{
    public interface IMovementStrategy
    {
        bool ExecuteMove(Board board, Direction direction);

        string GetStrategyName();

        int GetScoreMultiplier();
    }
}