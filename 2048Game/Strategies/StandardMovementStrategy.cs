using _2048Game.Entities;
using _2048Game.Systems;
using _2048Game.Core;

namespace _2048Game.Strategies
{
    public class StandardMovementStrategy : IMovementStrategy
    {
        private ScoreManager _scoreManager;

        public StandardMovementStrategy(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
        }

        public bool ExecuteMove(Board board, Direction direction)
        {
            Console.WriteLine($"Standard Moving {direction}");

            bool moved = SimulateMove(board, direction);

            if (moved)
            {
                Console.WriteLine($"Standard Standard scoring applied");
            }

            return moved;
        }

        private bool SimulateMove(Board board, Direction direction)
        {
            return true;
        }

        public string GetStrategyName()
        {
            return "Standard Movement (Classic 2048)";
        }

        public int GetScoreMultiplier()
        {
            return 1;
        }
    }
}