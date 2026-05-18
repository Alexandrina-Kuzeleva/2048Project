using System;
using _2048Game.Systems;
using _2048Game.UI;
using _2048Game.Core;
using _2048Game.Commands;
using _2048Game.Entities;

namespace _2048Game.States
{
    public class GameState : IGameState
    {
        private GameStateContext _context;
        private Board? _board;
        private ScoreManager _scoreManager;
        private ConsoleHUD _hud;
        private BoardRenderer? _boardRenderer;
        private InputHandler _inputHandler;
        private bool _isInitialized = false;

        public string StateName => "Game";

        public GameState(GameStateContext context, ScoreManager scoreManager, ConsoleHUD hud)
        {
            _context = context;
            _scoreManager = scoreManager;
            _hud = hud;
            _inputHandler = new InputHandler();
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

                if (GameManager.Instance.HasSave() && AskLoadSavedGame())
                {
                    LoadSavedGame();
                }
                else
                {
                    _board = new Board();
                    _board.SetScoreManager(_scoreManager);
                    _hud.SetBoard(_board);
                    _boardRenderer = new BoardRenderer(_board);
                    _scoreManager.ResetScore();
                }

                SetupDefaultBindings();
                _isInitialized = true;
            }

            _hud.RefreshAll();
            _inputHandler.ShowBindings();
        }

        private void SetupDefaultBindings()
        {
            if (_board == null) return;

            _inputHandler.BindCommand(ConsoleKey.UpArrow, new MoveUpCommand(_board, _hud));
            _inputHandler.BindCommand(ConsoleKey.DownArrow, new MoveDownCommand(_board, _hud));
            _inputHandler.BindCommand(ConsoleKey.LeftArrow, new MoveLeftCommand(_board, _hud));
            _inputHandler.BindCommand(ConsoleKey.RightArrow, new MoveRightCommand(_board, _hud));
            _inputHandler.BindCommand(ConsoleKey.Spacebar, new AddTileCommand(_board, _hud));
        }

        public void Update() { }

        public void Exit()
        {
            // Ничего не делаем
        }

        public void HandleInput(ConsoleKey key)
        {
            if (_board == null) return;

            switch (key)
            {
                case ConsoleKey.Escape:
                    _context.SetState(_context.PauseState);
                    break;

                case ConsoleKey.R:
                    ShowRemapMenu();
                    break;

                case ConsoleKey.F5:
                    GameManager.Instance.SaveGame(_board, _scoreManager);
                    break;

                case ConsoleKey.F9:
                    LoadSavedGame();
                    break;

                default:
                    _inputHandler.HandleInput(key);
                    break;
            }

            if (_board.IsGameOver())
            {
                _context.SetState(_context.GameOverState);
            }
        }

        private void ShowRemapMenu()
        {
            Console.Clear();
            Console.WriteLine("=== REMAP CONTROLS ===");
            Console.WriteLine("Available commands:");
            Console.WriteLine("  1 - Move Up");
            Console.WriteLine("  2 - Move Down");
            Console.WriteLine("  3 - Move Left");
            Console.WriteLine("  4 - Move Right");
            Console.WriteLine("  5 - Add Tile");
            Console.WriteLine("  ESC - Cancel");
            Console.Write("\nSelect command to remap: ");

            var choice = Console.ReadKey(true).Key;
            ICommand? selectedCommand = null;

            switch (choice)
            {
                case ConsoleKey.D1:
                    selectedCommand = new MoveUpCommand(_board!, _hud);
                    break;
                case ConsoleKey.D2:
                    selectedCommand = new MoveDownCommand(_board!, _hud);
                    break;
                case ConsoleKey.D3:
                    selectedCommand = new MoveLeftCommand(_board!, _hud);
                    break;
                case ConsoleKey.D4:
                    selectedCommand = new MoveRightCommand(_board!, _hud);
                    break;
                case ConsoleKey.D5:
                    selectedCommand = new AddTileCommand(_board!, _hud);
                    break;
                default:
                    return;
            }

            Console.Write($"\nSelected: {selectedCommand.GetDescription()}");
            Console.Write("\nPress new key to bind: ");
            var newKey = Console.ReadKey(true).Key;

            foreach (var binding in _inputHandler.GetAllBindings())
            {
                if (binding.Value.GetDescription() == selectedCommand.GetDescription())
                {
                    _inputHandler.RemapCommand(binding.Key, newKey, selectedCommand);
                    break;
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            _hud.RefreshAll();
        }

        public void ResetGame()
        {
            _isInitialized = false;
            _board = null;
        }

        private bool AskLoadSavedGame()
        {
            Console.WriteLine("A saved game was found. Load it? (Y/N)");
            var key = Console.ReadKey(true).Key;
            return key == ConsoleKey.Y || key == ConsoleKey.Enter;
        }

        private void LoadSavedGame()
        {
            var saveData = GameManager.Instance.LoadGame();
            if (saveData == null) return;

            GameManager.Instance.MapSize = saveData.MapSize;
            GameManager.Instance.GameDifficulty = saveData.GameDifficulty;

            _board = new Board(false);
            _board.SetScoreManager(_scoreManager);

            if (saveData.Grid != null)
            {
                foreach (var tileData in saveData.Grid)
                {
                    if (tileData.Value == 0)
                        continue;

                    Tile tile = tileData.Type switch
                    {
                        "BonusTile" => new BonusTile(),
                        "ObstacleTile" => new ObstacleTile(),
                        _ => new NumberTile(tileData.Value)
                    };

                    tile.PositionX = tileData.PositionX;
                    tile.PositionY = tileData.PositionY;

                    if (tile is BonusTile bonusTile)
                    {
                        bonusTile.RestoreActivation(tileData.IsActivated);
                    }

                    if (tile is ObstacleTile obstacleTile)
                    {
                        obstacleTile.RestoreState(tileData.Health, tileData.IsDestroyed);
                    }

                    _board.SetCell(tileData.PositionX, tileData.PositionY, tile);
                }
            }

            _scoreManager.SetScore(saveData.CurrentScore);
            _scoreManager.SetHighScore(saveData.HighScore);

            _hud.SetBoard(_board);
            _boardRenderer = new BoardRenderer(_board);
            SetupDefaultBindings();
            _hud.RefreshAll();

            Console.WriteLine("Game loaded!");
            Console.ReadKey(true);
        }
    }
}