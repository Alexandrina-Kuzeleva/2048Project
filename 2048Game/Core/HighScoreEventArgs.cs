using System;

namespace _2048Game.Core
{
    public class HighScoreEventArgs : EventArgs
    {
        public int OldHighScore { get; }
        public int NewHighScore { get; }
        public bool IsNewRecord { get; }

        public HighScoreEventArgs(int oldHighScore, int newHighScore)
        {
            OldHighScore = oldHighScore;
            NewHighScore = newHighScore;
            IsNewRecord = newHighScore > oldHighScore;
        }
    }
}