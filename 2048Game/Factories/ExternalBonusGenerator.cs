using _2048Game.Entities;

namespace _2048Game.Factories
{
    public class ExternalBonusGenerator
    {
        public static object GenerateBonus(int probability, int gameTime)
        {
            if (probability > 80 && gameTime > 10)
                return new { Type = "MegaBonus", Multiplier = 4 };
            else if (probability > 50)
                return new { Type = "Bonus", Multiplier = 2 };
            else
                return new { Type = "None", Multiplier = 1 };
        }
    }
}