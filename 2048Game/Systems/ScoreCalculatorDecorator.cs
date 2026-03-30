namespace _2048Game.Systems
{
    public abstract class ScoreCalculatorDecorator : IScoreCalculator
    {
        protected IScoreCalculator _calculator;

        protected ScoreCalculatorDecorator(IScoreCalculator calculator)
        {
            _calculator = calculator;
        }

        public virtual int Calculate(int baseScore)
        {
            return _calculator.Calculate(baseScore);
        }

        public virtual string GetDescription()
        {
            return _calculator.GetDescription();
        }
    }
}