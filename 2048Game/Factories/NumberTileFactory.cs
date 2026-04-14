using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class NumberTileFactory : TileFactory
    {
        private const double PROBABILITY_FOR_TWO = 0.9;
        private const int VALUE_TWO = 2;
        private const int VALUE_FOUR = 4;
        private const int DEFAULT_INITIAL_VALUE = 2;

        private int initialValue;
        private static Random random = new Random();

        public NumberTileFactory()
        {
            this.initialValue = random.NextDouble() < PROBABILITY_FOR_TWO ? VALUE_TWO : VALUE_FOUR;
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