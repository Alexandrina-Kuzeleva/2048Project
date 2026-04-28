using _2048Game.Entities;
using _2048Game.Systems;

namespace _2048Game.Core
{
    public class GameMovementContext
    {
        private IMovementStrategy _currentStrategy;
        private ScoreManager _scoreManager;
        private int _moveCount;

        public GameMovementContext(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            _moveCount = 0;
            _currentStrategy = new StandardMovementStrategy(scoreManager);
        }

        public void SetStrategy(IMovementStrategy strategy)
        {
            _currentStrategy = strategy;
            Console.WriteLine($"Strategy Changed Now using: {_currentStrategy.GetStrategyName()}");
            Console.WriteLine($"Multiplier Score multiplier: x{_currentStrategy.GetScoreMultiplier()}\n");
        }

        public bool ExecuteMovement(Board board, Direction direction)
        {
            _moveCount++;
            Console.WriteLine($"Move #{_moveCount}");

            bool result = _currentStrategy.ExecuteMove(board, direction);

            if (result)
            {
                int multiplier = _currentStrategy.GetScoreMultiplier();
                if (multiplier > 1)
                {
                    Console.WriteLine($"Multiplier applied: x{multiplier}");
                }
            }

            return result;
        }

        public string GetCurrentStrategyName()
        {
            return _currentStrategy.GetStrategyName();
        }

        public int GetMoveCount() => _moveCount;
    }
}