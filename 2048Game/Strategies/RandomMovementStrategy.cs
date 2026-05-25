using _2048Game.Entities;
using _2048Game.Systems;
using _2048Game.Core;

namespace _2048Game.Strategies
{
    // Случайное движение
    public class RandomMovementStrategy : IMovementStrategy
    {
        private ScoreManager _scoreManager;
        private Random _random;

        public RandomMovementStrategy(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            _random = new Random();
        }

        public bool ExecuteMove(Board board, Direction direction)
        {
            if (_random.Next(100) < 20)
            {
                return false;
            }

            bool moved = SimulateMove(board, direction);

            if (moved)
            {
                int randomPoints = _random.Next(5, 31);
                _scoreManager.AddPoints(randomPoints);
            }

            return moved;
        }

        private bool SimulateMove(Board board, Direction direction)
        {
            return true;
        }

        public string GetStrategyName()
        {
            return "Random Movement (Unpredictable!)";
        }

        public int GetScoreMultiplier()
        {
            return _random.Next(1, 4);
        }
    }
}