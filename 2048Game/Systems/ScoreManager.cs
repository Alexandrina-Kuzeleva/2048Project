namespace _2048Game.Systems
{
    public class ScoreManager
    {
        public int CurrentScore { get; set; }
        public int HighScore { get; set; }

        public ScoreManager()
        {
            CurrentScore = 0;
            HighScore = 0;
        }

        public void AddPoints(int points)
        {
            // метод для добавления поинтов
        }
    }
}