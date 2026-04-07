using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class LegacySpecialTileGenerator
    {
        private Random _random;

        public LegacySpecialTileGenerator()
        {
            _random = new Random();
        }

        public string GenerateSpecialTileType(int seed, int level)
        {
            int randomValue = _random.Next(0, 100);

            if (randomValue < 20 && level > 2)
                return "BonusTile";
            else if (randomValue < 40 && level > 1)
                return "ObstacleTile";
            else
                return "NumberTile";
        }

        public int GetTileValue(string tileType, int baseValue)
        {
            return tileType switch
            {
                "BonusTile" => 0,
                "ObstacleTile" => -1,
                _ => baseValue
            };
        }

        public string GetTileSymbol(string tileType, bool isSpecial)
        {
            return tileType switch
            {
                "BonusTile" => isSpecial ? " ★★ " : " ★ ",
                "ObstacleTile" => " ██ ",
                _ => "    "
            };
        }
    }
}