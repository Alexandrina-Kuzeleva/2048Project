using Xunit;
using _2048Game.Systems;
using _2048Game.Core;

namespace _2048Game.Tests.Tests
{
    public class DecoratorTests
    {
        [Fact]
        public void BaseCalculator_ShouldReturnSameValue()
        {
            // Arrange
            var calculator = new BaseScoreCalculator();

            // Act
            int result = calculator.Calculate(10);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void DifficultyBonusDecorator_Normal_ShouldDouble()
        {
            // Arrange
            var calculator = new DifficultyBonusDecorator(
                new BaseScoreCalculator(),
                Difficulty.Normal
            );

            // Act
            int result = calculator.Calculate(10);

            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public void DifficultyBonusDecorator_Hard_ShouldTriple()
        {
            // Arrange
            var calculator = new DifficultyBonusDecorator(
                new BaseScoreCalculator(),
                Difficulty.Hard
            );

            // Act
            int result = calculator.Calculate(10);

            // Assert
            Assert.Equal(30, result);
        }

        [Fact]
        public void DifficultyBonusDecorator_Easy_ShouldNotChange()
        {
            // Arrange
            var calculator = new DifficultyBonusDecorator(
                new BaseScoreCalculator(),
                Difficulty.Easy
            );

            // Act
            int result = calculator.Calculate(10);

            // Assert
            Assert.Equal(10, result);
        }
    }
}