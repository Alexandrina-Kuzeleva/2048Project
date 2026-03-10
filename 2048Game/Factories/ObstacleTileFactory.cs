using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class ObstacleTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new ObstacleTile();
        }
    }
}