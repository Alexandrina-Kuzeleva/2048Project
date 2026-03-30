using _2048Game.Core;

namespace _2048Game.Systems
{
    public class DifficultyBonusDecorator : ScoreCalculatorDecorator
    {
        private Difficulty _difficulty;
        private int _multiplier;

        public DifficultyBonusDecorator(IScoreCalculator calculator, Difficulty difficulty)
            : base(calculator)
        {
            _difficulty = difficulty;

            _multiplier = difficulty switch
            {
                Difficulty.Easy => 1,
                Difficulty.Normal => 2,
                Difficulty.Hard => 3,
                _ => 1
            };
        }

        public override int Calculate(int baseScore)
        {
            int originalScore = _calculator.Calculate(baseScore);
            return originalScore * _multiplier;
        }

        public override string GetDescription()
        {
            return $"{_calculator.GetDescription()} ×{_multiplier} ({_difficulty})";
        }
    }
}