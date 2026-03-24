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

        [Fact]
        public void OnMerge_ShouldActivateBonus()
        {
            // Arrange
            var tile = new BonusTile();
            Assert.False(tile.IsActivated());

            // Act
            tile.OnMerge();

            // Assert
            Assert.True(tile.IsActivated());
        }

        [Fact]
        public void GetColor_BeforeActivation_ShouldBeMagenta()
        {
            // Arrange
            var tile = new BonusTile();

            // Act & Assert
            Assert.Equal(ConsoleColor.Magenta, tile.GetColor());
        }

        [Fact]
        public void GetColor_AfterActivation_ShouldBeDarkMagenta()
        {
            // Arrange
            var tile = new BonusTile();

            // Act
            tile.OnMerge();

            // Assert
            Assert.Equal(ConsoleColor.DarkMagenta, tile.GetColor());
        }

        [Fact]
        public void GetSymbol_BeforeActivation_ShouldReturnSingleStar()
        {
            // Arrange
            var tile = new BonusTile();

            // Act
            var symbol = tile.GetSymbol();

            // Assert
            Assert.Equal(" ★ ", symbol);
        }

        [Fact]
        public void GetSymbol_AfterActivation_ShouldReturnDoubleStar()
        {
            // Arrange
            var tile = new BonusTile();

            // Act
            tile.OnMerge();
            var symbol = tile.GetSymbol();

            // Assert
            Assert.Equal(" ★★ ", symbol);
        }
    }
}