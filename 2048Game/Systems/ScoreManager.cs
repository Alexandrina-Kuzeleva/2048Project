namespace _2048Game.Systems
{
    public class ScoreManager
    {
        private int currentScore;
        private int highScore;
        private IScoreCalculator _scoreCalculator;

        public int CurrentScore => currentScore;
        public int HighScore => highScore;

        public ScoreManager()
        {
            currentScore = 0;
            highScore = 0;
            // По умолчанию используем базовый калькулятор
            _scoreCalculator = new BaseScoreCalculator();
        }

        // Метод для установки декоратора (цепочки)
        public void SetScoreCalculator(IScoreCalculator calculator)
        {
            _scoreCalculator = calculator;
        }

        // Добавляем очки с учетом модификаторов
        public void AddPointsWithModifier(int basePoints)
        {
            int finalPoints = _scoreCalculator.Calculate(basePoints);
            AddPoints(finalPoints);
        }

        public void AddPoints(int points)
        {
            if (points > 0)
            {
                currentScore += points;
                if (currentScore > highScore)
                {
                    highScore = currentScore;
                }
            }
        }

        public void ResetScore()
        {
            currentScore = 0;
        }

        public string GetScoreCalculatorDescription()
        {
            return _scoreCalculator.GetDescription();
        }
    }
}