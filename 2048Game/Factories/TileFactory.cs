using _2048Game.Entities;

namespace _2048Game.Factories
{
    public abstract class TileFactory
    {
        public abstract Tile CreateTile();

        public Tile CreateTileAtPosition(int x, int y)
        {
            Tile tile = CreateTile();
            tile.PositionX = x;
            tile.PositionY = y;
            return tile;
        }
    }
}