namespace _2048Game.Systems
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