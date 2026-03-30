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

        private Board? board;
        private List<Tile> clonedTiles;
        private Tile? lastAddedTile;

        private GameManager()
        {
            MapSize = 4;
            GameDifficulty = Difficulty.Normal;
            IsSoundEnabled = true;

            clonedTiles = new List<Tile>();
            lastAddedTile = null;
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

            TilePrototypeDemo.DemonstratePrototype();

            DecoratorDemo.DemonstrateDecorator();

            Console.WriteLine("\n========================================");
            Console.WriteLine("CONTROLS:");
            Console.WriteLine("  ESC - Exit");
            Console.WriteLine("  SPACE - Add random tile");
            Console.WriteLine("  C - Clone the last added tile (demonstrate Prototype)");
            Console.WriteLine("  D - Display all clones");
            Console.WriteLine("========================================\n");

            board = new Board();

            bool isRunning = true;

            while (isRunning)
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
                            board.AddRandomTile();
                            UpdateLastAddedTile();
                            Console.WriteLine($"Current board state - Press SPACE again");
                            break;

                        case ConsoleKey.C:
                            if (lastAddedTile != null)
                            {
                                DemonstrateCloning(lastAddedTile);
                            }
                            else
                            {
                                Console.WriteLine("No tile to clone. Press SPACE first to add a tile.");
                            }
                            break;

                        case ConsoleKey.D:
                            DisplayAllClones();
                            break;
                    }
                }
                Thread.Sleep(50);
            }

            Console.WriteLine("\nGame Over! Press any key to exit...");
            Console.ReadKey();
        }

        private void DemonstrateFactoryMethod()
        {
            Console.WriteLine("\n--- FACTORY METHOD DEMONSTRATION ---");

            List<TileFactory> factories = new List<TileFactory>
            {
                new NumberTileFactory(2),
                new NumberTileFactory(4),
                new NumberTileFactory(8),
                new BonusTileFactory(),
                new ObstacleTileFactory()
            };

            Console.WriteLine("Creating tiles through factories:");
            foreach (var factory in factories)
            {
                Tile tile = factory.CreateTile();

                Console.WriteLine($"  {factory.GetType().Name,-20} -> {tile.GetType().Name,-15} | " +
                                $"Value: {tile.Value,3} | Symbol: {tile.GetSymbol()}");
            }

            Console.WriteLine("\nPolymorphic method calls:");
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

            Console.WriteLine("--- END OF FACTORY METHOD DEMONSTRATION ---\n");
        }

        private void DemonstrateCloning(Tile originalTile)
        {
            Console.WriteLine("\n--- PROTOTYPE DEMONSTRATION: Live Cloning ---");

            int originalValue = originalTile.Value;
            int originalX = originalTile.PositionX;
            int originalY = originalTile.PositionY;
            string originalType = originalTile.GetType().Name;

            Console.WriteLine($"Original tile: Type={originalType}, Value={originalValue}, Pos=({originalX},{originalY})");

            Tile clone = (Tile)originalTile.Clone();

            clone.PositionX = 9;
            clone.PositionY = 9;

            if (clone is NumberTile numClone)
            {
                numClone.OnMerge();
            }
            else if (clone is BonusTile bonusClone)
            {
                bonusClone.OnMerge();
            }
            else if (clone is ObstacleTile obstacleClone)
            {
                obstacleClone.OnMerge();
            }

            Console.WriteLine($"Cloned tile:   Type={clone.GetType().Name}, Value={clone.Value}, Pos=({clone.PositionX},{clone.PositionY})");
            Console.WriteLine($"Are same object? {originalTile == clone}");
            Console.WriteLine($"Original hash: {originalTile.GetHashCode()}, Clone hash: {clone.GetHashCode()}");

            Console.WriteLine($"\nPROOF - Original tile after cloning:");
            Console.WriteLine($"  Original Value: {originalTile.Value} ({(originalTile.Value == originalValue ? "UNCHANGED ✓" : "CHANGED ✗")})");
            Console.WriteLine($"  Original Position: ({originalTile.PositionX},{originalTile.PositionY}) ({(originalTile.PositionX == originalX ? "UNCHANGED ✓" : "CHANGED ✗")})");

            if (originalTile is BonusTile originalBonus && clone is BonusTile clonedBonus)
            {
                Console.WriteLine($"  Original Bonus Activated: {originalBonus.IsActivated()}");
                Console.WriteLine($"  Cloned Bonus Activated: {clonedBonus.IsActivated()}");
            }
            else if (originalTile is ObstacleTile originalObstacle && clone is ObstacleTile clonedObstacle)
            {
                Console.WriteLine($"  Original Obstacle Health: {originalObstacle.GetHealth()}, Destroyed: {originalObstacle.IsDestroyed()}");
                Console.WriteLine($"  Cloned Obstacle Health: {clonedObstacle.GetHealth()}, Destroyed: {clonedObstacle.IsDestroyed()}");
            }

            clonedTiles.Add(clone);
            Console.WriteLine($"\nClone saved to list. Total clones: {clonedTiles.Count}");
            Console.WriteLine("--- End of Prototype Demonstration ---\n");
        }

        private void UpdateLastAddedTile()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    var tile = board.GetCell(i, j);
                    if (tile != null && tile.Value != 0)
                    {
                        lastAddedTile = tile;
                        return;
                    }
                }
            }
        }

        private void DisplayAllClones()
        {
            if (clonedTiles.Count == 0)
            {
                Console.WriteLine("No clones created yet. Press C to create a clone.");
                return;
            }

            Console.WriteLine($"\n--- All Clones ({clonedTiles.Count}) ---");
            for (int i = 0; i < clonedTiles.Count; i++)
            {
                var clone = clonedTiles[i];
                string typeName = clone.GetType().Name;
                string extraInfo = "";

                if (clone is BonusTile bonus)
                    extraInfo = $", Activated: {bonus.IsActivated()}";
                else if (clone is ObstacleTile obstacle)
                    extraInfo = $", Health: {obstacle.GetHealth()}, Destroyed: {obstacle.IsDestroyed()}";

                Console.WriteLine($"  Clone #{i + 1}: {typeName}, Value={clone.Value}, Pos=({clone.PositionX},{clone.PositionY}){extraInfo}");
            }
            Console.WriteLine("---\n");
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