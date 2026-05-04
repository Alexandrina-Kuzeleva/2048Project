using _2048Game.Entities;
using _2048Game.Systems;
using _2048Game.Core;

namespace _2048Game.Strategies
{
    // Агрессивное движение - бонусные очки за слияния
    public class AggressiveMovementStrategy : IMovementStrategy
    {
        private ScoreManager _scoreManager;
        private Random _random;

        public AggressiveMovementStrategy(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            _random = new Random();
        }

        public bool ExecuteMove(Board board, Direction direction)
        {
            Console.WriteLine($"Aggressive Moving {direction} with bonus potential!");

            bool moved = SimulateMove(board, direction);

            if (moved)
            {
                int bonus = _random.Next(10, 51);
                Console.WriteLine($"Aggressive Bonus points: +{bonus}!");
                _scoreManager.AddPoints(bonus);
            }

            return moved;
        }

        private bool SimulateMove(Board board, Direction direction)
        {
            return true;
        }

        public string GetStrategyName()
        {
            return "Aggressive Movement (Bonus points for moves!)";
        }

        public int GetScoreMultiplier()
        {
            return 2;
        }
    }
}