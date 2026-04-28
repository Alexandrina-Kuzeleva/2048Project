using _2048Game.Entities;
using _2048Game.Systems;

namespace _2048Game.Core
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
            Console.WriteLine($"Random Attempting to move {direction}...");

            if (_random.Next(100) < 20)
            {
                Console.WriteLine($"Random Movement failed! No points gained.");
                return false;
            }

            bool moved = SimulateMove(board, direction);

            if (moved)
            {
                int randomPoints = _random.Next(5, 31);
                Console.WriteLine($"Random Random points: +{randomPoints}");
                _scoreManager.AddPoints(randomPoints);
            }

            return moved;
        }

        private bool SimulateMove(Board board, Direction direction)
        {
            if (_random.Next(100) < 10)
            {
                Console.WriteLine($"Random Direction changed randomly!");
                return true;
            }
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