namespace _2048Game.Systems
{
    public interface IScoreCalculator
    {
        int Calculate(int baseScore);
        string GetDescription();
    }
}