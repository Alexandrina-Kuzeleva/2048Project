using System;
using System.IO;
using System.Text.Json;
using _2048Game.Entities;
using _2048Game.Systems;

namespace _2048Game.Core
{
    public class SaveManager
    {
        private readonly string _savePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public SaveManager()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            _savePath = Path.Combine(appData, ".2048game", "save.json");

            // Создаем директорию если её нет
            string directory = Path.GetDirectoryName(_savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = false
            };
        }

        public SaveData SaveGame(Board board, ScoreManager scoreManager)
        {
            var saveData = new SaveData
            {
                MapSize = GameManager.Instance.MapSize,
                GameDifficulty = GameManager.Instance.GameDifficulty,
                CurrentScore = scoreManager.CurrentScore,
                HighScore = scoreManager.HighScore,
                SaveTime = DateTime.Now,
                Version = 1,
                Grid = new List<TileData>()
            };

            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    var tile = board.GetCell(i, j);
                    if (tile != null && tile.Value != 0)
                    {
                        saveData.Grid.Add(CreateTileData(tile));
                    }
                }
            }

            string json = JsonSerializer.Serialize(saveData, _jsonOptions);
            File.WriteAllText(_savePath, json);

            return saveData;
        }

        public SaveData? LoadGame()
        {
            if (!File.Exists(_savePath))
            {
                Console.WriteLine("No save file found!");
                return null;
            }

            try
            {
                string json = File.ReadAllText(_savePath);
                var saveData = JsonSerializer.Deserialize<SaveData>(json, _jsonOptions);
                return saveData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading save: {ex.Message}");
                return null;
            }
        }

        private TileData CreateTileData(Tile tile)
        {
            var tileData = new TileData
            {
                PositionX = tile.PositionX,
                PositionY = tile.PositionY,
                Value = tile.Value,
                Type = tile.GetType().Name
            };

            if (tile is BonusTile bonus)
            {
                tileData.IsActivated = bonus.IsActivated();
            }
            else if (tile is ObstacleTile obstacle)
            {
                tileData.IsDestroyed = obstacle.IsDestroyed();
                tileData.Health = obstacle.GetHealth();
            }

            return tileData;
        }

        public bool SaveExists()
        {
            return File.Exists(_savePath);
        }

        public void DeleteSave()
        {
            if (File.Exists(_savePath))
            {
                File.Delete(_savePath);
            }
        }
    }
}