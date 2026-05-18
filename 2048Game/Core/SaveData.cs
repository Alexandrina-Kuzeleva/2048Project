using System.Collections.Generic;
using _2048Game.Entities;

namespace _2048Game.Core
{
    public class SaveData
    {
        public int MapSize { get; set; }
        public Difficulty GameDifficulty { get; set; }
        public int CurrentScore { get; set; }
        public int HighScore { get; set; }
        public List<TileData>? Grid { get; set; }
        public DateTime SaveTime { get; set; }
        public int Version { get; set; }
    }

    public class TileData
    {
        public string Type { get; set; } = string.Empty;
        public int Value { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDestroyed { get; set; }
        public int Health { get; set; }
    }
}