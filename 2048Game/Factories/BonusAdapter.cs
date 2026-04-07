using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class BonusAdapter : TileFactory
    {
        private int _gameTime;

        public BonusAdapter(int gameTime = 0)
        {
            _gameTime = gameTime;
        }

        public override Tile CreateTile()
        {
            Random random = new Random();
            int probability = random.Next(0, 100);

            var bonusData = ExternalBonusGenerator.GenerateBonus(probability, _gameTime);

            string bonusType = bonusData.GetType().GetProperty("Type")?.GetValue(bonusData)?.ToString() ?? "None";
            int multiplier = (int)(bonusData.GetType().GetProperty("Multiplier")?.GetValue(bonusData) ?? 1);

            if (bonusType == "MegaBonus" || (bonusType == "Bonus" && multiplier > 2))
            {
                var bonusTile = new BonusTile();
                return bonusTile;
            }

            return new NumberTile(random.NextDouble() < 0.9 ? 2 : 4);
        }

        public void UpdateGameTime(int time)
        {
            _gameTime = time;
        }
    }
}