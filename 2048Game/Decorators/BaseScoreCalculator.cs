using _2048Game.Systems;
namespace _2048Game.Decorators
{
    public class BaseScoreCalculator : IScoreCalculator
    {
        public int Calculate(int baseScore)
        {
            return baseScore;
        }

        public string GetDescription()
        {
            return "Base Score";
        }
    }
}