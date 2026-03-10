using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class BonusTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new BonusTile();
        }
    }
}