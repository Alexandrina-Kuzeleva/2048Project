using System;
using _2048Game.Systems;
using _2048Game.UI;
using _2048Game.Core;

namespace _2048Game.States
{
    public class GameState : IGameState
    {
        private GameStateContext _context;
        private Board? _board;
        private ScoreManager _scoreManager;
        private ConsoleHUD _hud;
        private BoardRenderer? _boardRenderer;
        private bool _isInitialized = false;

        public string StateName => "Game";

        public GameState(GameStateContext context, ScoreManager scoreManager, ConsoleHUD hud)
        {
            _context = context;
            _scoreManager = scoreManager;
            _hud = hud;
        }

        public void Enter()
        {
            if (!_isInitialized)
            {
                Console.Clear();
                Console.WriteLine("Starting game...");
                Console.WriteLine($"Map Size: {GameManager.Instance.MapSize}x{GameManager.Instance.MapSize}");
                Console.WriteLine($"Difficulty: {GameManager.Instance.GameDifficulty}");
                Console.WriteLine("\nPress any key to begin...");
                Console.ReadKey(true);

                _board = new Board();
                _board.SetScoreManager(_scoreManager);
                _hud.SetBoard(_board);
                _boardRenderer = new BoardRenderer(_board);

                _scoreManager.ResetScore();
                _isInitialized = true;
            }
            else
            {
                _hud.RefreshAll();
            }
        }

        public void Update() { }

        public void Exit()
        {

        }

        public void HandleInput(ConsoleKey key)
        {
            if (_board == null) return;

            switch (key)
            {
                case ConsoleKey.Escape:
                    _context.SetState(_context.PauseState);
                    break;

                case ConsoleKey.Spacebar:
                    _board.AddRandomTile();
                    RefreshDisplay();
                    break;

                case ConsoleKey.UpArrow:
                    _board.Move(Direction.Up);
                    RefreshDisplay();
                    break;

                case ConsoleKey.DownArrow:
                    _board.Move(Direction.Down);
                    RefreshDisplay();
                    break;

                case ConsoleKey.LeftArrow:
                    _board.Move(Direction.Left);
                    RefreshDisplay();
                    break;

                case ConsoleKey.RightArrow:
                    _board.Move(Direction.Right);
                    RefreshDisplay();
                    break;

                case ConsoleKey.S:
                    ShowGameStatus();
                    break;
            }

            if (_board.IsGameOver())
            {
                _context.SetState(_context.GameOverState);
            }
        }

        private void RefreshDisplay()
        {
            _hud.RefreshAll();
        }

        private void ShowGameStatus()
        {
            Console.WriteLine();
            Console.WriteLine($"Score: {_scoreManager.CurrentScore}");
            Console.WriteLine($"High score: {_scoreManager.HighScore}");
            Console.WriteLine();
        }

        public void ResetGame()
        {
            _isInitialized = false;
            _board = null;
        }
    }
}