using Xunit;
using _2048Game.Entities;

namespace _2048Game.Tests.Tests
{
    public class BonusTileTests
    {
        [Fact]
        public void Constructor_ShouldSetDefaultValues()
        {
            // Arrange & Act
            var tile = new BonusTile();

            // Assert
            Assert.Equal(0, tile.Value);
            Assert.Equal("BonusTile", tile.Name);
            Assert.Equal(2, tile.GetBonusMultiplier());
            Assert.False(tile.IsActivated());
        }
    }
}