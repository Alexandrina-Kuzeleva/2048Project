using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class SpecialTileAdapter : TileFactory
    {
        private LegacySpecialTileGenerator _legacyGenerator;
        private int _currentLevel;
        private Random _random;

        public SpecialTileAdapter(int currentLevel = 1)
        {
            _legacyGenerator = new LegacySpecialTileGenerator();
            _currentLevel = currentLevel;
            _random = new Random();
        }

        public override Tile CreateTile()
        {
            int seed = _random.Next(1000, 9999);
            string tileType = _legacyGenerator.GenerateSpecialTileType(seed, _currentLevel);

            int baseValue = _random.NextDouble() < 0.9 ? 2 : 4;
            int value = _legacyGenerator.GetTileValue(tileType, baseValue);

            Tile tile = tileType switch
            {
                "BonusTile" => new BonusTile(),
                "ObstacleTile" => new ObstacleTile(),
                _ => new NumberTile(value)
            };

            if (tile is BonusTile bonusTile)
            {
                string symbol = _legacyGenerator.GetTileSymbol(tileType, false);
            }

            return tile;
        }

        public void UpdateLevel(int level)
        {
            _currentLevel = level;
        }
    }
}