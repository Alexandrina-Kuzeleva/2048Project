namespace _2048Game.Systems
{
    public class ScoreManager
    {
        private int currentScore;
        private int highScore;

        public int CurrentScore => currentScore;
        public int HighScore => highScore;

        public ScoreManager()
        {
            currentScore = 0;
            highScore = 0;
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
    }
}