using Xunit;
using _2048Game.States;
using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.Tests.Tests
{
    public class FSMTests
    {
        [Fact]
        public void StateNames_ShouldBeCorrect()
        {
            var scoreManager = new ScoreManager();
            var hud = new ConsoleHUD(scoreManager);
            var context = new GameStateContext();

            var menu = new MenuState(context);
            var game = new GameState(context, scoreManager, hud);
            var pause = new PauseState(context);
            var gameOver = new GameOverState(context, scoreManager, hud);

            Assert.Equal("Menu", menu.StateName);
            Assert.Equal("Game", game.StateName);
            Assert.Equal("Pause", pause.StateName);
            Assert.Equal("GameOver", gameOver.StateName);
        }
    }
}