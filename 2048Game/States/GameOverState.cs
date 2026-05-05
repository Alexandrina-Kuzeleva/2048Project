using System;
using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.States
{
    public class GameOverState : IGameState
    {
        private GameStateContext _context;
        private ScoreManager _scoreManager;
        private ConsoleHUD? _hud;

        public string StateName => "GameOver";

        public GameOverState(GameStateContext context)
        {
            _context = context;
            _scoreManager = new ScoreManager();
        }

        public void Enter()
        {
            _hud = new ConsoleHUD(_scoreManager);
            _hud.ShowGameOver();
        }

        public void Update()
        {

        }

        public void Exit()
        {
            _hud?.Dispose();
            Console.Clear();
        }

        public void HandleInput(ConsoleKey key)
        {
            _context.SetState(_context.MenuState);
        }
    }
}