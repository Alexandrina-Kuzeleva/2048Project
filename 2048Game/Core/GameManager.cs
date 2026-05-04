using System;
using System.Collections.Generic;
using _2048Game.Systems;
using _2048Game.Entities;
using _2048Game.Factories;
using _2048Game.UI;
using _2048Game.Demos;
using _2048Game.Strategies;

namespace _2048Game.Core
{
    public class GameManager
    {
        private static GameManager? _instance;

        public int MapSize { get; set; }
        public Difficulty GameDifficulty { get; set; }

        private Board? _board;
        private ScoreManager _scoreManager;
        private GameMovementContext _movementContext;
        private ConsoleHUD? _hud;
        private BoardRenderer? _boardRenderer;

        private List<Tile> _clonedTiles;
        private Tile? _lastAddedTile;

        private GameManager()
        {
            MapSize = 4;
            GameDifficulty = Difficulty.Normal;

            _scoreManager = new ScoreManager();
            _movementContext = new GameMovementContext(_scoreManager);
            _clonedTiles = new List<Tile>();
            _lastAddedTile = null;

        }

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public void Run()
        {
            while (true)
            {
                ShowMainMenu();
                var choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        StartGame();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ShowDemosMenu();
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        ShowSettingsMenu();
                        break;

                    case ConsoleKey.Escape:
                        Console.WriteLine("\nGoodbye!");
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice. Press any key...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("2048 Game");
            Console.WriteLine();
            Console.WriteLine("MAIN MENU");
            Console.WriteLine();
            Console.WriteLine("  [1] Start Game");
            Console.WriteLine("  [2] View Pattern Demonstrations");
            Console.WriteLine("  [3] Settings");
            Console.WriteLine("  [ESC] Exit");
            Console.WriteLine();
            Console.Write("Select option: ");
        }

        private void ShowDemosMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Pattern Demonstrations");
                Console.WriteLine();
                Console.WriteLine("  [1] Factory Method");
                Console.WriteLine("  [2] Prototype");
                Console.WriteLine("  [3] Adapter");
                Console.WriteLine("  [4] Strategy");
                Console.WriteLine("  [5] Events");
                Console.WriteLine("  [0] Run All Demonstrations");
                Console.WriteLine("  [ESC] Back to Main Menu");
                Console.WriteLine();
                Console.Write("Select demo: ");

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        DemoRunner.RunFactoryMethodDemo();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        DemoRunner.RunPrototypeDemo();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        DemoRunner.RunAdapterDemo();
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        DemoRunner.RunStrategyDemo();
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        DemoRunner.RunEventsDemo();
                        break;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        DemoRunner.RunAllDemos();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }

        private void ShowSettingsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Settings");
                Console.WriteLine();
                Console.WriteLine($"  [1] Board Size: {MapSize}x{MapSize}");
                Console.WriteLine($"  [2] Difficulty: {GameDifficulty}");
                Console.WriteLine($"  [3] Reset to Defaults");
                Console.WriteLine($"  [ESC] Back to Main Menu");
                Console.WriteLine();
                Console.Write("Select option: ");

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Console.Write("\nEnter board size (3-8): ");
                        if (int.TryParse(Console.ReadLine(), out int newSize) && newSize >= 3 && newSize <= 8)
                        {
                            MapSize = newSize;
                            Console.WriteLine($"Board size set to {MapSize}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid size. Must be 3-8.");
                        }
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.WriteLine("\nSelect difficulty: 1=Easy, 2=Normal, 3=Hard");
                        var diffKey = Console.ReadKey(true).Key;
                        GameDifficulty = diffKey switch
                        {
                            ConsoleKey.D1 or ConsoleKey.NumPad1 => Difficulty.Easy,
                            ConsoleKey.D3 or ConsoleKey.NumPad3 => Difficulty.Hard,
                            _ => Difficulty.Normal
                        };
                        Console.WriteLine($"Difficulty set to {GameDifficulty}");
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        MapSize = 4;
                        GameDifficulty = Difficulty.Normal;
                        Console.WriteLine("Settings restored to defaults.");
                        break;

                    case ConsoleKey.Escape:
                        return;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }

        private void StartGame()
        {
            Console.Clear();
            Console.WriteLine("Starting game...");
            Console.WriteLine($"Map Size: {MapSize}x{MapSize}");
            Console.WriteLine($"Difficulty: {GameDifficulty}");
            Console.WriteLine("\nPress any key to begin...");
            Console.ReadKey(true);

            _board = new Board();
            _board.SetScoreManager(_scoreManager);
            _hud = new ConsoleHUD(_scoreManager);
            _hud.SetBoard(_board);

            _boardRenderer = new BoardRenderer(_board);

            _scoreManager.ResetScore();
            _clonedTiles.Clear();
            _lastAddedTile = null;

            Console.Clear();
            _hud?.RefreshBoard();
            _boardRenderer?.Draw();

            bool isRunning = true;
            bool gameOver = false;

            while (isRunning && !gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.Escape:
                            isRunning = false;
                            break;

                        case ConsoleKey.Spacebar:
                            _board.AddRandomTile();
                            UpdateLastAddedTile();
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

                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            _movementContext.SetStrategy(new StandardMovementStrategy(_scoreManager));
                            break;

                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            _movementContext.SetStrategy(new AggressiveMovementStrategy(_scoreManager));
                            break;

                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            _movementContext.SetStrategy(new DefensiveMovementStrategy(_scoreManager));
                            break;

                        case ConsoleKey.D4:
                        case ConsoleKey.NumPad4:
                            _movementContext.SetStrategy(new RandomMovementStrategy(_scoreManager));
                            break;

                        case ConsoleKey.S:
                            ShowGameStatus();
                            break;

                        case ConsoleKey.C:
                            if (_lastAddedTile != null)
                            {
                                DemonstrateCloning(_lastAddedTile);
                                RefreshDisplay();
                            }
                            else
                            {
                                Console.WriteLine("No tile to clone. Press SPACE first.");
                            }
                            break;

                        case ConsoleKey.D:
                            DisplayAllClones();
                            break;
                    }

                    // ПРОВЕРКА ОКОНЧАНИЯ ИГРЫ - используем HUD
                    if (_board.IsGameOver())
                    {
                        gameOver = true;
                        _hud?.ShowGameOver();  // Используем HUD для отображения Game Over
                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey(true);
                    }
                }
                Thread.Sleep(50);
            }

            _hud?.Dispose();
        }

        private void ShowGameStatus()
        {
            Console.WriteLine();
            Console.WriteLine($"Strategy: {_movementContext.GetCurrentStrategyName()}");
            Console.WriteLine($"Moves: {_movementContext.GetMoveCount()}");
            Console.WriteLine($"Score: {_scoreManager.CurrentScore}");
            Console.WriteLine($"High score: {_scoreManager.HighScore}");
            Console.WriteLine();
        }

        private void UpdateLastAddedTile()
        {
            if (_board == null) return;

            for (int row = 0; row < MapSize; row++)
            {
                for (int col = 0; col < MapSize; col++)
                {
                    var tile = _board.GetCell(row, col);
                    if (tile != null && tile.Value != 0)
                    {
                        _lastAddedTile = tile;
                        return;
                    }
                }
            }
        }

        private void DemonstrateCloning(Tile originalTile)
        {
            Console.WriteLine("--- Clone demo ---");
            Tile clone = (Tile)originalTile.Clone();
            clone.PositionX = 9;
            clone.PositionY = 9;

            if (clone is NumberTile numClone)
                numClone.OnMerge();
            else if (clone is BonusTile bonusClone)
                bonusClone.OnMerge();
            else if (clone is ObstacleTile obstacleClone)
                obstacleClone.OnMerge();

            Console.WriteLine($"Original: {originalTile.Value}");
            Console.WriteLine($"Clone: {clone.Value}");
            Console.WriteLine("---");

            _clonedTiles.Add(clone);
        }

        private void DisplayAllClones()
        {
            if (_clonedTiles.Count == 0)
            {
                Console.WriteLine("No clones created.");
                return;
            }

            Console.WriteLine($"--- Clones ({_clonedTiles.Count}) ---");
            for (int i = 0; i < _clonedTiles.Count; i++)
            {
                var clone = _clonedTiles[i];
                string info = clone is BonusTile bonus ? $", activated: {bonus.IsActivated()}" :
                              clone is ObstacleTile obstacle ? $", health: {obstacle.GetHealth()}" : "";
                Console.WriteLine($"  {i + 1}: {clone.GetType().Name}, value={clone.Value}{info}");
            }
            Console.WriteLine("---");
        }

        public void ShowSettings()
        {
            Console.WriteLine($"Settings: size={MapSize}, difficulty={GameDifficulty}");
        }

        private void RefreshDisplay()
        {
            _hud?.RefreshBoard();
            _boardRenderer?.Draw();
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}