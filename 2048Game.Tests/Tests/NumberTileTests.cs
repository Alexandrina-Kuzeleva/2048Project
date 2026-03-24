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

        [Fact]
        public void OnMerge_WithValue8_ShouldDoubleTo16()
        {
            // Arrange
            var tile = new NumberTile(8);

            // Act
            tile.OnMerge();

            // Assert
            Assert.Equal(16, tile.Value);
        }

        [Fact]
        public void OnMerge_WithValue1024_ShouldDoubleTo2048()
        {
            // Arrange
            var tile = new NumberTile(1024);

            // Act
            tile.OnMerge();

            // Assert
            Assert.Equal(2048, tile.Value);
        }

        [Fact]
        public void OnMerge_MultipleTimes_ShouldKeepDoubling()
        {
            // Arrange
            var tile = new NumberTile(2);

            // Act
            tile.OnMerge();
            tile.OnMerge();

            // Assert
            Assert.Equal(8, tile.Value);
        }

        [Fact]
        public void GetColor_ForDifferentValues_ShouldReturnCorrectColor()
        {
            // Arrange & Act & Assert
            var tile2 = new NumberTile(2);
            Assert.Equal(ConsoleColor.DarkGreen, tile2.GetColor());

            var tile4 = new NumberTile(4);
            Assert.Equal(ConsoleColor.Green, tile4.GetColor());

            var tile8 = new NumberTile(8);
            Assert.Equal(ConsoleColor.DarkYellow, tile8.GetColor());

            var tile16 = new NumberTile(16);
            Assert.Equal(ConsoleColor.Yellow, tile16.GetColor());

            var tile32 = new NumberTile(32);
            Assert.Equal(ConsoleColor.DarkRed, tile32.GetColor());

            var tile64 = new NumberTile(64);
            Assert.Equal(ConsoleColor.Red, tile64.GetColor());

            var tile128 = new NumberTile(128);
            Assert.Equal(ConsoleColor.Magenta, tile128.GetColor());

            var tile256 = new NumberTile(256);
            Assert.Equal(ConsoleColor.Cyan, tile256.GetColor());

            var tile512 = new NumberTile(512);
            Assert.Equal(ConsoleColor.Blue, tile512.GetColor());

            var tile1024 = new NumberTile(1024);
            Assert.Equal(ConsoleColor.DarkBlue, tile1024.GetColor());

            var tile2048 = new NumberTile(2048);
            Assert.Equal(ConsoleColor.White, tile2048.GetColor());
        }

        [Fact]
        public void GetSymbol_ShouldReturnValuePaddedTo4()
        {
            // Arrange
            var tile = new NumberTile(2);

            // Act
            var symbol = tile.GetSymbol();

            // Assert
            Assert.Equal("   2", symbol);
        }

        [Fact]
        public void GetSymbol_WithLargerValue_ShouldReturnProperPadding()
        {
            // Arrange
            var tile = new NumberTile(2048);

            // Act
            var symbol = tile.GetSymbol();

            // Assert
            Assert.Equal("2048", symbol.Trim());
        }
    }
}