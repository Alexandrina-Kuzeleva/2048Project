using Xunit;
using _2048Game.Systems;

namespace _2048Game.Tests.Tests
{
    public class ScoreManagerTests
    {
        [Fact]
        public void Constructor_ShouldInitializeZeroScore()
        {
            // Arrange & Act
            var scoreManager = new ScoreManager();

            // Assert
            Assert.Equal(0, scoreManager.CurrentScore);
            Assert.Equal(0, scoreManager.HighScore);
        }

        [Fact]
        public void AddPoints_WithPositiveAmount_ShouldIncreaseCurrentScore()
        {
            // Arrange
            var scoreManager = new ScoreManager();

            // Act
            scoreManager.AddPoints(100);

            // Assert
            Assert.Equal(100, scoreManager.CurrentScore);
        }

        [Fact]
        public void AddPoints_MultipleTimes_ShouldAccumulate()
        {
            // Arrange
            var scoreManager = new ScoreManager();

            // Act
            scoreManager.AddPoints(50);
            scoreManager.AddPoints(30);
            scoreManager.AddPoints(20);

            // Assert
            Assert.Equal(100, scoreManager.CurrentScore);
        }

        [Fact]
        public void AddPoints_WithZero_ShouldNotChangeScore()
        {
            // Arrange
            var scoreManager = new ScoreManager();

            // Act
            scoreManager.AddPoints(0);

            // Assert
            Assert.Equal(0, scoreManager.CurrentScore);
        }

        [Fact]
        public void AddPoints_WithNegativeAmount_ShouldNotChangeScore()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            scoreManager.AddPoints(100);

            // Act
            scoreManager.AddPoints(-50);

            // Assert
            Assert.Equal(100, scoreManager.CurrentScore);
        }

        [Fact]
        public void ResetScore_ShouldSetScoreToZero()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            scoreManager.AddPoints(100);
            Assert.Equal(100, scoreManager.CurrentScore);

            // Act
            scoreManager.ResetScore();

            // Assert
            Assert.Equal(0, scoreManager.CurrentScore);
        }

        [Fact]
        public void AddPoints_WhenExceedingHighScore_ShouldUpdateHighScore()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            scoreManager.AddPoints(50);
            Assert.Equal(50, scoreManager.HighScore);

            // Act
            scoreManager.AddPoints(60);

            // Assert
            Assert.Equal(110, scoreManager.CurrentScore);
            Assert.Equal(110, scoreManager.HighScore);
        }

        [Fact]
        public void HighScore_ShouldUpdateWhenCurrentScoreExceedsIt()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            scoreManager.AddPoints(100);
            Assert.Equal(100, scoreManager.HighScore);

            // Act
            scoreManager.AddPoints(20);

            // Assert
            Assert.Equal(120, scoreManager.CurrentScore);
            Assert.Equal(120, scoreManager.HighScore);
        }
    }
}