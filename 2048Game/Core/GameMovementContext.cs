using _2048Game.Entities;
using _2048Game.Systems;
using _2048Game.Strategies;

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
        }

        public bool ExecuteMovement(Board board, Direction direction)
        {
            _moveCount++;

            bool result = _currentStrategy.ExecuteMove(board, direction);
            return result;
        }

        public string GetCurrentStrategyName()
        {
            return _currentStrategy.GetStrategyName();
        }

        public int GetMoveCount() => _moveCount;
    }
}