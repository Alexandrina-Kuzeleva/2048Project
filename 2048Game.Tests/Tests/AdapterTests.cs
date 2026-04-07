using Xunit;
using _2048Game.Factories;
using _2048Game.Entities;

namespace _2048Game.Tests.Tests
{
    public class AdapterTests
    {
        [Fact]
        public void SpecialTileAdapter_ShouldImplementTileFactory()
        {
            // Arrange
            var adapter = new SpecialTileAdapter();

            // Assert
            Assert.IsAssignableFrom<TileFactory>(adapter);
        }

        [Fact]
        public void SpecialTileAdapter_CreateTile_ShouldReturnTile()
        {
            // Arrange
            var adapter = new SpecialTileAdapter();

            // Act
            Tile tile = adapter.CreateTile();

            // Assert
            Assert.NotNull(tile);
            Assert.IsAssignableFrom<Tile>(tile);
        }

        [Fact]
        public void SpecialTileAdapter_CreateTile_ShouldReturnValidTileType()
        {
            // Arrange
            var adapter = new SpecialTileAdapter();

            // Act
            Tile tile = adapter.CreateTile();

            // Assert
            Assert.True(tile is NumberTile || tile is BonusTile || tile is ObstacleTile);
        }

        [Fact]
        public void BonusAdapter_ShouldImplementTileFactory()
        {
            // Arrange
            var adapter = new BonusAdapter();

            // Assert
            Assert.IsAssignableFrom<TileFactory>(adapter);
        }

        [Fact]
        public void BonusAdapter_CreateTile_ShouldReturnTile()
        {
            // Arrange
            var adapter = new BonusAdapter();

            // Act
            Tile tile = adapter.CreateTile();

            // Assert
            Assert.NotNull(tile);
            Assert.IsAssignableFrom<Tile>(tile);
        }

        [Fact]
        public void SpecialTileAdapter_WithDifferentLevels_ShouldWork()
        {
            // Arrange
            var adapterLow = new SpecialTileAdapter(currentLevel: 1);
            var adapterHigh = new SpecialTileAdapter(currentLevel: 5);

            // Act & Assert
            Tile tile1 = adapterLow.CreateTile();
            Tile tile2 = adapterHigh.CreateTile();

            Assert.NotNull(tile1);
            Assert.NotNull(tile2);
        }

        [Fact]
        public void BonusAdapter_WithDifferentGameTime_ShouldWork()
        {
            // Arrange
            var adapterShort = new BonusAdapter(gameTime: 5);
            var adapterLong = new BonusAdapter(gameTime: 30);

            // Act & Assert
            Tile tile1 = adapterShort.CreateTile();
            Tile tile2 = adapterLong.CreateTile();

            Assert.NotNull(tile1);
            Assert.NotNull(tile2);
        }

        [Fact]
        public void Adapter_CanBeUsedInFactoryList()
        {
            // Arrange
            var factories = new System.Collections.Generic.List<TileFactory>
            {
                new NumberTileFactory(2),
                new SpecialTileAdapter(),
                new BonusAdapter()
            };

            // Act & Assert
            foreach (var factory in factories)
            {
                Tile tile = factory.CreateTile();
                Assert.NotNull(tile);
            }
        }
    }
}