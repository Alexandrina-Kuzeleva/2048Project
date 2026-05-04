using _2048Game.Entities;
using _2048Game.Systems;
using _2048Game.Core;

namespace _2048Game.Strategies
{
    public interface IMovementStrategy
    {
        bool ExecuteMove(Board board, Direction direction);

        string GetStrategyName();

        int GetScoreMultiplier();
    }
}