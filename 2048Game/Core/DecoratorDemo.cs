using System;
using _2048Game.Core;
using _2048Game.Systems;

namespace _2048Game.Core
{
    public static class DecoratorDemo
    {
        public static void DemonstrateDecorator()
        {

            Console.WriteLine("DECORATOR PATTERN DEMO");


            int tileValue = 8;
            Console.WriteLine($"Tile value (base score): {tileValue}");

            var simpleScore = new BaseScoreCalculator();
            Console.WriteLine($"\n1. Base calculator: {simpleScore.Calculate(tileValue)} points");
            Console.WriteLine($"   Description: {simpleScore.GetDescription()}");

            var easyScore = new DifficultyBonusDecorator(new BaseScoreCalculator(), Difficulty.Easy);
            Console.WriteLine($"\n2. Easy mode: {easyScore.Calculate(tileValue)} points");
            Console.WriteLine($"   Description: {easyScore.GetDescription()}");

            var normalScore = new DifficultyBonusDecorator(new BaseScoreCalculator(), Difficulty.Normal);
            Console.WriteLine($"\n3. Normal mode: {normalScore.Calculate(tileValue)} points");
            Console.WriteLine($"   Description: {normalScore.GetDescription()}");

            var hardScore = new DifficultyBonusDecorator(new BaseScoreCalculator(), Difficulty.Hard);
            Console.WriteLine($"\n4. Hard mode: {hardScore.Calculate(tileValue)} points");
            Console.WriteLine($"   Description: {hardScore.GetDescription()}");

            Console.WriteLine("\n--- Using with ScoreManager ---");
            var scoreManager = new ScoreManager();

            scoreManager.SetScoreCalculator(new DifficultyBonusDecorator(
                new BaseScoreCalculator(),
                Difficulty.Normal
            ));

            Console.WriteLine($"Score calculator: {scoreManager.GetScoreCalculatorDescription()}");
            scoreManager.AddPointsWithModifier(tileValue);
            Console.WriteLine($"After adding tile value {tileValue}: Score = {scoreManager.CurrentScore}");

            scoreManager.AddPointsWithModifier(tileValue);
            Console.WriteLine($"After adding again: Score = {scoreManager.CurrentScore}");

        }
    }
}