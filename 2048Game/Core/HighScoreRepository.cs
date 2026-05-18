using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2048Game.Core
{
    public class HighScoreEntry
    {
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public Difficulty Difficulty { get; set; }
        public int BoardSize { get; set; }
        public DateTime Date { get; set; }
    }

    public class HighScoreRepository
    {
        private readonly string _scoresPath;
        private List<HighScoreEntry> _scores;

        public HighScoreRepository()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string directory = Path.Combine(appData, ".2048game");
            _scoresPath = Path.Combine(directory, "scores.txt");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            LoadScores();
        }

        private void LoadScores()
        {
            _scores = new List<HighScoreEntry>();

            if (!File.Exists(_scoresPath))
            {
                return;
            }

            try
            {
                var lines = File.ReadAllLines(_scoresPath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length != 5)
                        continue;

                    if (!int.TryParse(parts[1].Trim(), out int score))
                        continue;

                    if (!Enum.TryParse(parts[2].Trim(), out Difficulty difficulty))
                        difficulty = Difficulty.Normal;

                    if (!int.TryParse(parts[3].Trim(), out int boardSize))
                        boardSize = 4;

                    if (!DateTime.TryParse(parts[4].Trim(), out DateTime date))
                        date = DateTime.Now;

                    _scores.Add(new HighScoreEntry
                    {
                        PlayerName = parts[0].Trim(),
                        Score = score,
                        Difficulty = difficulty,
                        BoardSize = boardSize,
                        Date = date
                    });
                }
            }
            catch
            {
                _scores = new List<HighScoreEntry>();
            }
        }

        private void SaveScores()
        {
            var lines = _scores.Select(x => $"{x.PlayerName} | {x.Score} | {x.Difficulty} | {x.BoardSize} | {x.Date:O}");
            File.WriteAllLines(_scoresPath, lines);
        }

        public void AddScore(string playerName, int score, Difficulty difficulty, int boardSize)
        {
            var existingIndex = _scores.FindIndex(x =>
                string.Equals(x.PlayerName, playerName, StringComparison.OrdinalIgnoreCase)
                && x.Difficulty == difficulty
                && x.BoardSize == boardSize);

            if (existingIndex >= 0)
            {
                var existingScore = _scores[existingIndex].Score;
                if (score <= existingScore)
                {
                    return; // Не перезаписываем, если новый результат хуже или равен.
                }

                _scores.RemoveAt(existingIndex);
            }

            var entry = new HighScoreEntry
            {
                PlayerName = playerName,
                Score = score,
                Difficulty = difficulty,
                BoardSize = boardSize,
                Date = DateTime.Now
            };

            _scores.Add(entry);
            _scores = _scores.OrderByDescending(x => x.Score).Take(10).ToList();
            SaveScores();
        }

        public List<HighScoreEntry> GetTopScores(int count = 5)
        {
            return _scores.OrderByDescending(x => x.Score).Take(count).ToList();
        }

        public List<string> GetUniquePlayerNames(int count = 5)
        {
            return _scores
                .Select(x => x.PlayerName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .Take(count)
                .ToList();
        }
    }
}