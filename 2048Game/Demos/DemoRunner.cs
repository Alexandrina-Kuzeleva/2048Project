using System;
using System.Collections.Generic;
using _2048Game.Systems;
using _2048Game.Entities;
using _2048Game.Factories;
using _2048Game.Adapters;
using _2048Game.Decorators;
using _2048Game.Events;
using _2048Game.Strategies;

namespace _2048Game.Demos
{
    public static class DemoRunner
    {

        public static void RunFactoryMethodDemo()
        {

            Console.WriteLine("--- Factory Method ---");

            List<TileFactory> factories = new List<TileFactory>
            {
                new NumberTileFactory(2),
                new NumberTileFactory(4),
                new NumberTileFactory(8),
                new BonusTileFactory(),
                new ObstacleTileFactory()
            };

            foreach (var factory in factories)
            {
                Tile tile = factory.CreateTile();
                Console.WriteLine($"{factory.GetType().Name,-20} -> {tile.GetType().Name,-15} | Value: {tile.Value,3} | Symbol: {tile.GetSymbol()}");
            }

            Console.WriteLine("\nOnMerge behavior:");
            List<Tile> tiles = new List<Tile>
            {
                new NumberTileFactory(2).CreateTile(),
                new NumberTileFactory(4).CreateTile(),
                new BonusTileFactory().CreateTile(),
                new ObstacleTileFactory().CreateTile()
            };

            foreach (Tile tile in tiles)
            {
                int oldValue = tile.Value;
                tile.OnMerge();
                Console.WriteLine($"{tile.GetType().Name}: {oldValue} -> {tile.Value}");
            }
            Console.WriteLine();
        }

        public static void RunPrototypeDemo()
        {

            Console.WriteLine("--- Prototype ---");

            Tile originalNumber = new NumberTile(8);
            originalNumber.PositionX = 1;
            originalNumber.PositionY = 1;

            Tile originalBonus = new BonusTile();
            originalBonus.PositionX = 2;
            originalBonus.PositionY = 2;

            Tile originalObstacle = new ObstacleTile();
            originalObstacle.PositionX = 3;
            originalObstacle.PositionY = 3;

            Tile clonedNumber = (Tile)originalNumber.Clone();
            Tile clonedBonus = (Tile)originalBonus.Clone();
            Tile clonedObstacle = (Tile)originalObstacle.Clone();

            Console.WriteLine($"NumberTile original: {originalNumber.Value} at ({originalNumber.PositionX},{originalNumber.PositionY}), clone: {clonedNumber.Value} at ({clonedNumber.PositionX},{clonedNumber.PositionY})");
            Console.WriteLine($"BonusTile original activated: {((BonusTile)originalBonus).IsActivated()}, clone: {((BonusTile)clonedBonus).IsActivated()}");
            Console.WriteLine($"ObstacleTile original health: {((ObstacleTile)originalObstacle).GetHealth()}, clone: {((ObstacleTile)clonedObstacle).GetHealth()}");

            ((NumberTile)clonedNumber).OnMerge();
            ((BonusTile)clonedBonus).OnMerge();
            ((ObstacleTile)clonedObstacle).OnMerge();

            Console.WriteLine($"After modifying clones: original number {originalNumber.Value} unchanged, clone now {clonedNumber.Value}");
            Console.WriteLine();
        }

        public static void RunAdapterDemo()
        {

            Console.WriteLine("--- Adapter ---");

            var specialAdapter = new SpecialTileAdapter(currentLevel: 3);
            Console.WriteLine("SpecialTileAdapter:");
            for (int i = 0; i < 3; i++)
            {
                var tile = specialAdapter.CreateTile();
                Console.WriteLine($"  {tile.GetType().Name,-15} Value: {tile.Value,3} Symbol: {tile.GetSymbol()}");
            }

            var bonusAdapter = new BonusAdapter(gameTime: 20);
            Console.WriteLine("\nBonusAdapter:");
            for (int i = 0; i < 3; i++)
            {
                var tile = bonusAdapter.CreateTile();
                Console.WriteLine($"  {tile.GetType().Name,-15} Value: {tile.Value,3} Symbol: {tile.GetSymbol()}");
            }
            Console.WriteLine();
        }

        public static void RunStrategyDemo()
        {

            Console.WriteLine("--- Strategy ---");

            var strategies = new IMovementStrategy[]
            {
                new StandardMovementStrategy(new ScoreManager()),
                new AggressiveMovementStrategy(new ScoreManager()),
                new DefensiveMovementStrategy(new ScoreManager()),
                new RandomMovementStrategy(new ScoreManager())
            };

            foreach (var strategy in strategies)
            {
                Console.WriteLine($"{strategy.GetStrategyName(),-25} | Multiplier: x{strategy.GetScoreMultiplier()}");
            }
            Console.WriteLine();
        }

        public static void RunEventsDemo()
        {

            Console.WriteLine("--- Events ---");

            var scoreManager = new ScoreManager();

            scoreManager.ScoreChanged += (sender, e) =>
            {
                Console.WriteLine($"Score: {e.OldScore} -> {e.NewScore} (+{e.PointsAdded})");
            };

            scoreManager.HighScoreChanged += (sender, e) =>
            {
                if (e.IsNewRecord)
                    Console.WriteLine($"High score: {e.OldHighScore} -> {e.NewHighScore}");
            };

            scoreManager.AddPoints(50);
            scoreManager.AddPoints(30);
            scoreManager.AddPoints(100);
            Console.WriteLine();
        }

        public static void RunAllDemos()
        {

            Console.Clear();
            Console.WriteLine("=== Pattern Demonstrations ===");

            RunFactoryMethodDemo();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
            Console.Clear();

            RunPrototypeDemo();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
            Console.Clear();

            RunAdapterDemo();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
            Console.Clear();

            RunStrategyDemo();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
            Console.Clear();

            RunEventsDemo();
            Console.WriteLine("Press any key to return to game...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}