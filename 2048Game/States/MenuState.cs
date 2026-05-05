using System;
using _2048Game.Core;
using _2048Game.Demos;
using _2048Game.UI;

namespace _2048Game.States
{
    public class MenuState : IGameState
    {
        private GameStateContext _context;
        private bool _showDemoMenu = false;

        public string StateName => "Menu";

        public MenuState(GameStateContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            Console.Clear();
            ShowMainMenu();
        }

        public void Update()
        {

        }

        public void Exit()
        {
            Console.Clear();
        }

        public void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    var gameState = (GameState)_context.GameState;
                    gameState.ResetGame();
                    _context.SetState(_context.GameState);
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    ShowDemoMenu();
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    ShowSettings();
                    break;

                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }

        private void ShowMainMenu()
        {
            var content = new List<string>
                {
                    "",
                    "[1] Start Game",
                    "[2] Pattern Demonstrations",
                    "[3] Settings",
                    "[ESC] Exit",
                    ""
                };
            FrameRenderer.DrawFrame("2048 GAME", content);
            Console.Write("\nSelect option: ");
        }

        private void ShowDemoMenu()
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
                case ConsoleKey.D1: DemoRunner.RunFactoryMethodDemo(); break;
                case ConsoleKey.D2: DemoRunner.RunPrototypeDemo(); break;
                case ConsoleKey.D3: DemoRunner.RunAdapterDemo(); break;
                case ConsoleKey.D4: DemoRunner.RunStrategyDemo(); break;
                case ConsoleKey.D5: DemoRunner.RunEventsDemo(); break;
                case ConsoleKey.D0: DemoRunner.RunAllDemos(); break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            Enter();
        }

        private void ShowSettings()
        {
            Console.Clear();
            Console.WriteLine("Settings");
            Console.WriteLine();
            Console.WriteLine($"  [1] Board Size: {GameManager.Instance.MapSize}x{GameManager.Instance.MapSize}");
            Console.WriteLine($"  [2] Difficulty: {GameManager.Instance.GameDifficulty}");
            Console.WriteLine($"  [3] Reset to Defaults");
            Console.WriteLine($"  [ESC] Back to Main Menu");
            Console.WriteLine();
            Console.Write("Select option: ");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                    Console.Write("\nEnter board size (3-8): ");
                    if (int.TryParse(Console.ReadLine(), out int newSize) && newSize >= 3 && newSize <= 8)
                    {
                        GameManager.Instance.MapSize = newSize;
                    }
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("\nSelect difficulty: 1=Easy, 2=Normal, 3=Hard");
                    var diffKey = Console.ReadKey(true).Key;
                    GameManager.Instance.GameDifficulty = diffKey switch
                    {
                        ConsoleKey.D1 or ConsoleKey.NumPad1 => Difficulty.Easy,
                        ConsoleKey.D3 or ConsoleKey.NumPad3 => Difficulty.Hard,
                        _ => Difficulty.Normal
                    };
                    break;
                case ConsoleKey.D3:
                    GameManager.Instance.MapSize = 4;
                    GameManager.Instance.GameDifficulty = Difficulty.Normal;
                    break;
            }

            Enter();
        }
    }
}