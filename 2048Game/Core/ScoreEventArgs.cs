using System;

namespace _2048Game.Core
{
    public class ScoreEventArgs : EventArgs
    {
        public int OldScore { get; }
        public int NewScore { get; }
        public int PointsAdded { get; }
        public DateTime Timestamp { get; }

        public ScoreEventArgs(int oldScore, int newScore)
        {
            OldScore = oldScore;
            NewScore = newScore;
            PointsAdded = newScore - oldScore;
            Timestamp = DateTime.Now;
        }
    }
}