using System;
using _2048Game.Factories;
using _2048Game.Entities;

namespace _2048Game.Core
{
    public static class AdapterDemo
    {
        public static void DemonstrateAdapterPattern()
        {
            Console.WriteLine("ADAPTER PATTERN DEMONSTRATION");

            Console.WriteLine("DEMO 1: Using SpecialTileAdapter (adapts LegacySpecialTileGenerator)");
            Console.WriteLine("Problem: LegacySpecialTileGenerator has method GenerateSpecialTileType(seed, level)");
            Console.WriteLine("Solution: Adapter converts it to CreateTile()\n");

            var adapter = new SpecialTileAdapter(currentLevel: 3);

            Console.WriteLine("Generating 5 tiles through adapter:");
            for (int i = 0; i < 5; i++)
            {
                Tile tile = adapter.CreateTile();
                Console.WriteLine($"Tile {i + 1}: {tile.GetType().Name,-15} Value: {tile.Value,3} Symbol: {tile.GetSymbol()}");
            }

            Console.WriteLine("\nDEMO 2: Using BonusAdapter (adapts ExternalBonusGenerator)");
            Console.WriteLine("Problem: ExternalBonusGenerator.GenerateBonus(probability, gameTime) returns object");
            Console.WriteLine("Solution: Adapter converts it to CreateTile()\n");

            var bonusAdapter = new BonusAdapter(gameTime: 15);

            Console.WriteLine("Generating 5 tiles through bonus adapter:");
            for (int i = 0; i < 5; i++)
            {
                Tile tile = bonusAdapter.CreateTile();
                Console.WriteLine($"   Tile {i + 1}: {tile.GetType().Name,-15} Value: {tile.Value,3} Symbol: {tile.GetSymbol()}");
            }

            Console.WriteLine("\nDEMO 3: Integration with existing Board system");
            Console.WriteLine("The adapter can be used anywhere TileFactory is expected!");

            var factories = new System.Collections.Generic.List<TileFactory>
            {
                new NumberTileFactory(2),
                new NumberTileFactory(4),
                new SpecialTileAdapter(currentLevel: 5),
                new BonusAdapter(gameTime: 20)
            };

            Console.WriteLine("\nMixed factories (including adapters):");
            foreach (var factory in factories)
            {
                Tile tile = factory.CreateTile();
                string factoryType = factory.GetType().Name;
                Console.WriteLine($"   {factoryType,-25} → {tile.GetType().Name,-15} | Value: {tile.Value,3}");
            }
        }
    }
}