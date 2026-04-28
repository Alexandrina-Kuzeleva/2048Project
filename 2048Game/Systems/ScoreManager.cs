using System;
using _2048Game.Core;

namespace _2048Game.Systems
{
    public class ScoreManager
    {
        private int currentScore;
        private int highScore;

        public int CurrentScore => currentScore;
        public int HighScore => highScore;
        public event EventHandler<ScoreEventArgs>? ScoreChanged;
        public event EventHandler<HighScoreEventArgs>? HighScoreChanged;

        public ScoreManager()
        {
            currentScore = 0;
            highScore = 0;
        }

        public void AddPoints(int points)
        {
            if (points > 0)
            {
                int oldScore = currentScore;
                currentScore += points;

                OnScoreChanged(new ScoreEventArgs(oldScore, currentScore));

                if (currentScore > highScore)
                {
                    int oldHighScore = highScore;
                    highScore = currentScore;

                    OnHighScoreChanged(new HighScoreEventArgs(oldHighScore, highScore));
                }
            }
        }

        public void ResetScore()
        {
            int oldScore = currentScore;
            currentScore = 0;

            OnScoreChanged(new ScoreEventArgs(oldScore, currentScore));
        }

        protected virtual void OnScoreChanged(ScoreEventArgs e)
        {
            ScoreChanged?.Invoke(this, e);
        }

        protected virtual void OnHighScoreChanged(HighScoreEventArgs e)
        {
            HighScoreChanged?.Invoke(this, e);
        }
    }
}