using Xunit;
using _2048Game.States;

namespace _2048Game.Tests.Tests
{
    public class FSMTests
    {
        [Fact]
        public void StateNames_ShouldBeCorrect()
        {
            var menu = new MenuState(null!);
            var game = new GameState(null!);
            var pause = new PauseState(null!);
            var gameOver = new GameOverState(null!);

            Assert.Equal("Menu", menu.StateName);
            Assert.Equal("Game", game.StateName);
            Assert.Equal("Pause", pause.StateName);
            Assert.Equal("GameOver", gameOver.StateName);
        }
    }
}