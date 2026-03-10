using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class NumberTileFactory : TileFactory
    {
        private int initialValue;
        private static Random random = new Random();

        public NumberTileFactory()
        {
            this.initialValue = random.NextDouble() < 0.9 ? 2 : 4;
        }

        public NumberTileFactory(int initialValue)
        {
            this.initialValue = initialValue;
        }

        public override Tile CreateTile()
        {
            return new NumberTile(initialValue);
        }
    }
}