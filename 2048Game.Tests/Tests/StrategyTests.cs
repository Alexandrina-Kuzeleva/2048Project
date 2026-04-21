using Xunit;
using _2048Game.Core;
using _2048Game.Systems;

namespace _2048Game.Tests.Tests
{
    public class StrategyTests
    {
        [Fact]
        public void StandardStrategy_ShouldHaveNameAndMultiplier()
        {
            // Arrange
            var strategy = new StandardMovementStrategy(new ScoreManager());

            // Act & Assert
            Assert.Contains("Standard", strategy.GetStrategyName());
            Assert.Equal(1, strategy.GetScoreMultiplier());
        }

        [Fact]
        public void AggressiveStrategy_ShouldHaveMultiplier2()
        {
            // Arrange
            var strategy = new AggressiveMovementStrategy(new ScoreManager());

            // Act & Assert
            Assert.Contains("Aggressive", strategy.GetStrategyName());
            Assert.Equal(2, strategy.GetScoreMultiplier());
        }

        [Fact]
        public void DefensiveStrategy_ShouldHaveMultiplier1()
        {
            // Arrange
            var strategy = new DefensiveMovementStrategy(new ScoreManager());

            // Act & Assert
            Assert.Contains("Defensive", strategy.GetStrategyName());
            Assert.Equal(1, strategy.GetScoreMultiplier());
        }

        [Fact]
        public void Context_ShouldAllowStrategyChange()
        {
            // Arrange
            var context = new GameMovementContext(new ScoreManager());
            string initialStrategy = context.GetCurrentStrategyName();

            // Act
            context.SetStrategy(new AggressiveMovementStrategy(new ScoreManager()));

            // Assert
            Assert.NotEqual(initialStrategy, context.GetCurrentStrategyName());
            Assert.Contains("Aggressive", context.GetCurrentStrategyName());
        }

        [Fact]
        public void Context_ExecuteMovement_ShouldReturnBool()
        {
            // Arrange
            var context = new GameMovementContext(new ScoreManager());
            var board = new Board();

            // Act
            bool result = context.ExecuteMovement(board, Direction.Up);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void RandomStrategy_Multiplier_ShouldBeBetween1And3()
        {
            // Arrange
            var strategy = new RandomMovementStrategy(new ScoreManager());

            // Act
            int multiplier = strategy.GetScoreMultiplier();

            // Assert
            Assert.InRange(multiplier, 1, 3);
        }

        [Fact]
        public void Strategy_Interface_IsImplemented()
        {
            // Arrange
            var strategies = new IMovementStrategy[]
            {
                new StandardMovementStrategy(new ScoreManager()),
                new AggressiveMovementStrategy(new ScoreManager()),
                new DefensiveMovementStrategy(new ScoreManager()),
                new RandomMovementStrategy(new ScoreManager())
            };

            // Act & Assert
            foreach (var strategy in strategies)
            {
                Assert.IsAssignableFrom<IMovementStrategy>(strategy);
            }
        }
    }
}