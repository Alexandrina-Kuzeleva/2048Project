using Xunit;
using _2048Game.Entities;

namespace _2048Game.Tests.Tests
{
    public class NumberTileTests
    {
        [Fact]
        public void Constructor_WithInitialValue_ShouldSetCorrectValue()
        {
            // Arrange
            int expectedValue = 4;

            // Act
            var tile = new NumberTile(expectedValue);

            // Assert
            Assert.Equal(expectedValue, tile.Value);
            Assert.Equal($"NumberTile_{expectedValue}", tile.Name);
        }

        [Fact]
        public void OnMerge_WithValue2_ShouldDoubleTo4()
        {
            // Arrange
            var tile = new NumberTile(2);

            // Act
            tile.OnMerge();

            // Assert
            Assert.Equal(4, tile.Value);
        }
    }
}