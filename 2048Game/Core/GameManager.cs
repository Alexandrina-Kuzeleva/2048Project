using System;
using System.Collections.Generic;
using _2048Game.Systems;
using _2048Game.Entities;
using _2048Game.Factories;

namespace _2048Game.Core
{
    public class GameManager
    {
        private static GameManager? _instance;

        public int MapSize { get; set; }
        public Difficulty GameDifficulty { get; set; }
        public bool IsSoundEnabled { get; set; }

        private GameManager()
        {
            MapSize = 4;
            GameDifficulty = Difficulty.Normal;
            IsSoundEnabled = true;
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
            Console.WriteLine("========================================");
            Console.WriteLine("   2048 GAME - FACTORY METHOD DEMO     ");
            Console.WriteLine("========================================");
            Console.WriteLine($"Map Size: {MapSize}x{MapSize}");
            Console.WriteLine($"Difficulty: {GameDifficulty}");
            Console.WriteLine($"Sound: {(IsSoundEnabled ? "ON" : "OFF")}");
            Console.WriteLine("========================================");

            DemonstrateFactoryMethod();

            Console.WriteLine("\n========================================");
            Console.WriteLine("Press ESC to exit, SPACE to add tile");
            Console.WriteLine("========================================\n");

            Board board = new Board();
            bool isRunning = true;

            while (isRunning)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        isRunning = false;
                    }
                    else if (key == ConsoleKey.Spacebar)
                    {
                        board.AddRandomTile();
                        Console.WriteLine($"Current board state - Press SPACE again");
                    }
                }
                Thread.Sleep(50);
            }

            Console.WriteLine("\nGame Over! Press any key to exit...");
            Console.ReadKey();
        }

        private void DemonstrateFactoryMethod()
        {
            Console.WriteLine("\n--- ДЕМОНСТРАЦИЯ ФАБРИЧНОГО МЕТОДА ---");

            List<TileFactory> factories = new List<TileFactory>
            {
                new NumberTileFactory(2),
                new NumberTileFactory(4),
                new NumberTileFactory(8),
                new BonusTileFactory(),
                new ObstacleTileFactory()
            };

            Console.WriteLine("Создание плиток через фабрики:");
            foreach (var factory in factories)
            {
                Tile tile = factory.CreateTile();

                Console.WriteLine($"  {factory.GetType().Name,-20} -> {tile.GetType().Name,-15} | " +
                                $"Value: {tile.Value,3} | Symbol: {tile.GetSymbol()}");
            }

            Console.WriteLine("\nПолиморфный вызов методов:");
            List<Tile> tiles = new List<Tile>();

            tiles.Add(new NumberTileFactory(2).CreateTile());
            tiles.Add(new NumberTileFactory(4).CreateTile());
            tiles.Add(new BonusTileFactory().CreateTile());
            tiles.Add(new ObstacleTileFactory().CreateTile());

            foreach (Tile tile in tiles)
            {
                Console.Write($"  {tile.GetType().Name}: ");
                tile.OnMerge();
                Console.WriteLine($" -> Value: {tile.Value}");
            }

            Console.WriteLine("--- КОНЕЦ ДЕМОНСТРАЦИИ ---\n");
        }

        public void ShowSettings()
        {
            Console.WriteLine($"Current settings - Size: {MapSize}, Difficulty: {GameDifficulty}");
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}