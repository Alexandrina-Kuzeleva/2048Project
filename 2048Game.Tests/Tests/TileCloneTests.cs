using Xunit;
using _2048Game.Entities;

namespace _2048Game.Tests.Tests
{
    public class TileCloneTests
    {
        [Fact]
        public void NumberTile_Clone_ShouldCreateSeparateCopy()
        {
            // Arrange
            var original = new NumberTile(8);
            original.PositionX = 1;
            original.PositionY = 2;

            // Act
            var clone = (Tile)original.Clone();
            clone.PositionX = 9;
            clone.PositionY = 9;

            // Assert
            Assert.NotSame(original, clone);
            Assert.Equal(8, original.Value);
            Assert.Equal(8, clone.Value);
            Assert.Equal(1, original.PositionX);
            Assert.Equal(9, clone.PositionX);
        }

        [Fact]
        public void NumberTile_Clone_ModifyingCloneShouldNotAffectOriginal()
        {
            // Arrange
            var original = new NumberTile(4);

            // Act
            var clone = (NumberTile)original.Clone();
            clone.OnMerge();

            // Assert
            Assert.Equal(4, original.Value);
            Assert.Equal(8, clone.Value);
        }

        [Fact]
        public void BonusTile_Clone_ShouldCopyActivationState()
        {
            // Arrange
            var original = new BonusTile();
            original.OnMerge();

            // Act
            var clone = (BonusTile)original.Clone();

            // Assert
            Assert.True(original.IsActivated());
            Assert.True(clone.IsActivated());
        }

        [Fact]
        public void BonusTile_Clone_ModifyingCloneShouldNotAffectOriginal()
        {
            // Arrange
            var original = new BonusTile();

            // Act
            var clone = (BonusTile)original.Clone();
            clone.OnMerge();

            // Assert
            Assert.False(original.IsActivated());
            Assert.True(clone.IsActivated());
        }

        [Fact]
        public void ObstacleTile_Clone_ShouldCopyHealthAndDestroyedState()
        {
            // Arrange
            var original = new ObstacleTile();
            original.OnMerge();

            // Act
            var clone = (ObstacleTile)original.Clone();

            // Assert
            Assert.Equal(2, original.GetHealth());
            Assert.Equal(2, clone.GetHealth());
        }

        [Fact]
        public void ObstacleTile_Clone_ModifyingCloneShouldNotAffectOriginal()
        {
            // Arrange
            var original = new ObstacleTile();

            // Act
            var clone = (ObstacleTile)original.Clone();
            clone.OnMerge();

            // Assert
            Assert.Equal(3, original.GetHealth());
            Assert.Equal(2, clone.GetHealth());
        }
    }
}