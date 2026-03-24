using Xunit;
using _2048Game.Entities;

namespace _2048Game.Tests.Tests
{
    public class ObstacleTileTests
    {
        [Fact]
        public void Constructor_ShouldSetDefaultValues()
        {
            // Arrange & Act
            var tile = new ObstacleTile();

            // Assert
            Assert.Equal(-1, tile.Value);
            Assert.Equal("ObstacleTile", tile.Name);
            Assert.Equal(3, tile.GetHealth());
            Assert.False(tile.IsDestroyed());
        }

        [Fact]
        public void OnMerge_ShouldReduceHealthByOne()
        {
            // Arrange
            var tile = new ObstacleTile();
            Assert.Equal(3, tile.GetHealth());

            // Act
            tile.OnMerge();

            // Assert
            Assert.Equal(2, tile.GetHealth());
        }

        [Fact]
        public void OnMerge_TwoTimes_ShouldReduceHealthTo1()
        {
            // Arrange
            var tile = new ObstacleTile();

            // Act
            tile.OnMerge();
            tile.OnMerge();

            // Assert
            Assert.Equal(1, tile.GetHealth());
            Assert.False(tile.IsDestroyed());
        }

        [Fact]
        public void OnMerge_ThreeTimes_ShouldDestroyObstacle()
        {
            // Arrange
            var tile = new ObstacleTile();

            // Act
            tile.OnMerge();
            tile.OnMerge();
            tile.OnMerge();

            // Assert
            Assert.Equal(0, tile.GetHealth());
            Assert.True(tile.IsDestroyed());
            Assert.Equal(0, tile.Value);
        }

        [Fact]
        public void GetColor_WithFullHealth_ShouldBeDarkRed()
        {
            // Arrange
            var tile = new ObstacleTile();

            // Act & Assert
            Assert.Equal(ConsoleColor.DarkRed, tile.GetColor());
        }

        [Fact]
        public void GetColor_WithHealth2_ShouldBeRed()
        {
            // Arrange
            var tile = new ObstacleTile();

            // Act
            tile.OnMerge();

            // Assert
            Assert.Equal(ConsoleColor.Red, tile.GetColor());
        }

        [Fact]
        public void GetColor_WithHealth1_ShouldBeYellow()
        {
            // Arrange
            var tile = new ObstacleTile();

            // Act
            tile.OnMerge();
            tile.OnMerge();

            // Assert
            Assert.Equal(ConsoleColor.Yellow, tile.GetColor());
        }

        [Fact]
        public void GetColor_WhenDestroyed_ShouldBeDarkGray()
        {
            // Arrange
            var tile = new ObstacleTile();
            tile.OnMerge();
            tile.OnMerge();
            tile.OnMerge();

            // Act & Assert
            Assert.Equal(ConsoleColor.DarkGray, tile.GetColor());
        }

        [Fact]
        public void GetSymbol_WithHealth3_ShouldShowHealth()
        {
            // Arrange
            var tile = new ObstacleTile();

            // Act
            var symbol = tile.GetSymbol();

            // Assert
            Assert.Equal(" █3 ", symbol);
        }

        [Fact]
        public void GetSymbol_WithHealth2_ShouldShowHealth2()
        {
            // Arrange
            var tile = new ObstacleTile();
            tile.OnMerge();

            // Act
            var symbol = tile.GetSymbol();

            // Assert
            Assert.Equal(" █2 ", symbol);
        }

        [Fact]
        public void GetSymbol_WhenDestroyed_ShouldBeEmpty()
        {
            // Arrange
            var tile = new ObstacleTile();
            tile.OnMerge();
            tile.OnMerge();
            tile.OnMerge(); // destroyed

            // Act
            var symbol = tile.GetSymbol();

            // Assert
            Assert.Equal("    ", symbol);
        }
    }
}