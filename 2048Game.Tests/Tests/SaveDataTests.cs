using System.Text.Json;
using Xunit;
using _2048Game.Core;

namespace _2048Game.Tests.Tests
{
    public class SaveDataTests
    {
        [Fact]
        public void SaveData_Serialization_ShouldRoundTripWithoutDisk()
        {
            // Arrange
            var original = new SaveData
            {
                MapSize = 4,
                GameDifficulty = Difficulty.Hard,
                CurrentScore = 256,
                HighScore = 512,
                SaveTime = new System.DateTime(2026, 5, 18, 12, 0, 0),
                Version = 1,
                Grid = new System.Collections.Generic.List<TileData>
                {
                    new TileData
                    {
                        Type = "NumberTile",
                        Value = 8,
                        PositionX = 0,
                        PositionY = 0,
                        IsActivated = false,
                        IsDestroyed = false,
                        Health = 0
                    },
                    new TileData
                    {
                        Type = "BonusTile",
                        Value = 0,
                        PositionX = 0,
                        PositionY = 1,
                        IsActivated = true,
                        IsDestroyed = false,
                        Health = 0
                    },
                    new TileData
                    {
                        Type = "ObstacleTile",
                        Value = -1,
                        PositionX = 0,
                        PositionY = 2,
                        IsActivated = false,
                        IsDestroyed = false,
                        Health = 2
                    }
                }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            // Act
            string json = JsonSerializer.Serialize(original, options);
            var restored = JsonSerializer.Deserialize<SaveData>(json, options);

            // Assert
            Assert.NotNull(restored);
            Assert.Equal(original.MapSize, restored!.MapSize);
            Assert.Equal(original.GameDifficulty, restored.GameDifficulty);
            Assert.Equal(original.CurrentScore, restored.CurrentScore);
            Assert.Equal(original.HighScore, restored.HighScore);
            Assert.Equal(original.SaveTime, restored.SaveTime);
            Assert.Equal(original.Version, restored.Version);
            Assert.NotNull(restored.Grid);
            Assert.Equal(original.Grid.Count, restored.Grid.Count);

            for (int i = 0; i < original.Grid.Count; i++)
            {
                var expectedTile = original.Grid[i];
                var actualTile = restored.Grid[i];

                Assert.Equal(expectedTile.Type, actualTile.Type);
                Assert.Equal(expectedTile.Value, actualTile.Value);
                Assert.Equal(expectedTile.PositionX, actualTile.PositionX);
                Assert.Equal(expectedTile.PositionY, actualTile.PositionY);
                Assert.Equal(expectedTile.IsActivated, actualTile.IsActivated);
                Assert.Equal(expectedTile.IsDestroyed, actualTile.IsDestroyed);
                Assert.Equal(expectedTile.Health, actualTile.Health);
            }
        }
    }
}
