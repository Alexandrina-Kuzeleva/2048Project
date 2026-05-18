using System;
using _2048Game.Core;
using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.States
{
    public class GameOverState : IGameState
    {
        private GameStateContext _context;
        private ScoreManager _scoreManager;
        private ConsoleHUD _hud;

        public string StateName => "GameOver";

        public GameOverState(GameStateContext context, ScoreManager scoreManager, ConsoleHUD hud)
        {
            _context = context;
            _scoreManager = scoreManager;
            _hud = hud;
        }

        public void Enter()
        {
            Console.Clear();
            SaveFinalScore();
            _hud.ShowGameOver();
        }

        private void SaveFinalScore()
        {
            var repository = new HighScoreRepository();
            string playerName = GameManager.Instance.CurrentPlayerName;
            if (string.IsNullOrWhiteSpace(playerName))
            {
                playerName = "Anonymous";
            }

            repository.AddScore(playerName, _scoreManager.CurrentScore, GameManager.Instance.GameDifficulty, GameManager.Instance.MapSize);
        }

        public void Update() { }

        public void Exit() { }

        public void HandleInput(ConsoleKey key)
        {
            _context.SetState(_context.MenuState);
        }
    }
}