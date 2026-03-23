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

    }
}