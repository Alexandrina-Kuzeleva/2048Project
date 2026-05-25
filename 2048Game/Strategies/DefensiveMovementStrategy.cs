using _2048Game.Entities;
using _2048Game.Systems;
using _2048Game.Core;

namespace _2048Game.Strategies
{
    // Защитное движение сохраняет поле, добавляет защитные плитки
    public class DefensiveMovementStrategy : IMovementStrategy
    {
        private ScoreManager _scoreManager;
        private Random _random;
        private int _defenseCounter;

        public DefensiveMovementStrategy(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            _random = new Random();
            _defenseCounter = 0;
        }

        public bool ExecuteMove(Board board, Direction direction)
        {
            bool moved = SimulateMove(board, direction);

            if (moved)
            {
                _defenseCounter++;

                if (_defenseCounter % 3 == 0)
                {
                    _scoreManager.AddPoints(50);
                }
            }

            return moved;
        }

        private bool SimulateMove(Board board, Direction direction)
        {
            return true;
        }

        public string GetStrategyName()
        {
            return "Defensive Movement (Shield every 3 moves!)";
        }

        public int GetScoreMultiplier()
        {
            return 1;
        }

        public int GetDefenseCounter() => _defenseCounter;
    }
}