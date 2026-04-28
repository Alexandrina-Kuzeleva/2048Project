using Xunit;
using _2048Game.Core;
using _2048Game.Systems;

namespace _2048Game.Tests.Tests
{
    public class EventsTests
    {
        [Fact]
        public void ScoreChanged_Event_ShouldBeRaisedWhenPointsAdded()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            bool eventRaised = false;
            ScoreEventArgs? eventArgs = null;


            scoreManager.ScoreChanged += (sender, e) =>
            {
                eventRaised = true;
                eventArgs = e;
            };

            // Act
            scoreManager.AddPoints(100);

            // Assert
            Assert.True(eventRaised);
            Assert.NotNull(eventArgs);
            Assert.Equal(0, eventArgs!.OldScore);
            Assert.Equal(100, eventArgs.NewScore);
            Assert.Equal(100, eventArgs.PointsAdded);
        }

        [Fact]
        public void HighScoreChanged_Event_ShouldBeRaisedWhenRecordBroken()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            bool eventRaised = false;
            HighScoreEventArgs? eventArgs = null;

            scoreManager.HighScoreChanged += (sender, e) =>
            {
                eventRaised = true;
                eventArgs = e;
            };

            // Act
            scoreManager.AddPoints(150);

            // Assert
            Assert.True(eventRaised);
            Assert.NotNull(eventArgs);
            Assert.Equal(0, eventArgs!.OldHighScore);
            Assert.Equal(150, eventArgs.NewHighScore);
            Assert.True(eventArgs.IsNewRecord);
        }

        [Fact]
        public void ScoreChanged_Event_ShouldNotBeRaisedForZeroPoints()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            int eventCount = 0;

            scoreManager.ScoreChanged += (sender, e) => eventCount++;

            // Act
            scoreManager.AddPoints(0);

            // Assert
            Assert.Equal(0, eventCount);
        }

        [Fact]
        public void HighScoreChanged_Event_ShouldNotBeRaisedForSameScore()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            int eventCount = 0;

            scoreManager.AddPoints(100);
            scoreManager.HighScoreChanged += (sender, e) => eventCount++;

            // Act

            scoreManager.ResetScore();
            scoreManager.HighScoreChanged += (sender, e) => eventCount++;

            scoreManager.AddPoints(80);
            Assert.Equal(0, eventCount);
        }

        [Fact]
        public void ScoreEventArgs_ShouldContainCorrectData()
        {
            // Arrange
            var args = new ScoreEventArgs(50, 75);

            // Assert
            Assert.Equal(50, args.OldScore);
            Assert.Equal(75, args.NewScore);
            Assert.Equal(25, args.PointsAdded);
        }

        [Fact]
        public void HighScoreEventArgs_ShouldContainCorrectData()
        {
            // Arrange
            var args = new HighScoreEventArgs(100, 150);

            // Assert
            Assert.Equal(100, args.OldHighScore);
            Assert.Equal(150, args.NewHighScore);
            Assert.True(args.IsNewRecord);
        }

        [Fact]
        public void Subscriber_ShouldReceiveMultipleEvents()
        {
            // Arrange
            var scoreManager = new ScoreManager();
            int eventCount = 0;

            scoreManager.ScoreChanged += (sender, e) => eventCount++;

            // Act
            scoreManager.AddPoints(10);
            scoreManager.AddPoints(20);
            scoreManager.AddPoints(30);

            // Assert
            Assert.Equal(3, eventCount);
        }
    }
}