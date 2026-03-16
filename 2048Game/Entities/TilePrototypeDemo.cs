using System;
using System.Collections.Generic;

namespace _2048Game.Entities
{
    public static class TilePrototypeDemo
    {
        public static void DemonstratePrototype()
        {
            Console.WriteLine("PROTOTYPE PATTERN DEMONSTRATION");

            // ШАГ 1: Создаем оригинальные плитки
            Console.WriteLine("STEP 1: Creating original tiles");

            Tile originalNumber = new NumberTile(8);
            originalNumber.PositionX = 1;
            originalNumber.PositionY = 1;

            Tile originalBonus = new BonusTile();
            originalBonus.PositionX = 2;
            originalBonus.PositionY = 2;

            Tile originalObstacle = new ObstacleTile();
            originalObstacle.PositionX = 3;
            originalObstacle.PositionY = 3;

            DisplayTile("Original Number Tile", originalNumber);
            DisplayTile("Original Bonus Tile", originalBonus);
            DisplayTile("Original Obstacle Tile", originalObstacle);

            // ШАГ 2: Клонируем плитки через Prototype
            Console.WriteLine("\nSTEP 2: Cloning tiles using Prototype pattern (ICloneable)");

            Tile clonedNumber = (Tile)originalNumber.Clone();
            Tile clonedBonus = (Tile)originalBonus.Clone();
            Tile clonedObstacle = (Tile)originalObstacle.Clone();

            DisplayTile("Cloned Number Tile", clonedNumber);
            DisplayTile("Cloned Bonus Tile", clonedBonus);
            DisplayTile("Cloned Obstacle Tile", clonedObstacle);

            // ШАГ 3: Доказываем, что это разные объекты
            Console.WriteLine("\nSTEP 3: Proving clones are separate objects");
            Console.WriteLine($"Original Number hash: {originalNumber.GetHashCode()}");
            Console.WriteLine($"Cloned Number hash:   {clonedNumber.GetHashCode()}");
            Console.WriteLine($"Different objects: {originalNumber != clonedNumber}");

            Console.WriteLine($"Original Bonus hash: {originalBonus.GetHashCode()}");
            Console.WriteLine($"Cloned Bonus hash:   {clonedBonus.GetHashCode()}");
            Console.WriteLine($"Different objects: {originalBonus != clonedBonus}");

            Console.WriteLine($"Original Obstacle hash: {originalObstacle.GetHashCode()}");
            Console.WriteLine($"Cloned Obstacle hash:   {clonedObstacle.GetHashCode()}");
            Console.WriteLine($"Different objects: {originalObstacle != clonedObstacle}");

            // ШАГ 4: Модифицируем клоны (демонстрация Deep Copy)
            Console.WriteLine("\nSTEP 4: Modifying cloned tiles (DEEP COPY demonstration)");

            // 4.1 NumberTile модификация
            Console.WriteLine("\nNumber Tile");
            Console.WriteLine($"BEFORE modification:");
            Console.WriteLine($"Original Number value: {originalNumber.Value}");
            Console.WriteLine($"Cloned Number value:   {clonedNumber.Value}");

            if (clonedNumber is NumberTile numClone)
            {
                numClone.OnMerge();
                numClone.PositionX = 5;
                numClone.PositionY = 5;
            }

            Console.WriteLine($"\n AFTER modifying clone:");
            Console.WriteLine($"Original Number value: {originalNumber.Value} (UNCHANGED!)");
            Console.WriteLine($"Cloned Number value:   {clonedNumber.Value} (CHANGED!)");
            Console.WriteLine($"Original Position: ({originalNumber.PositionX},{originalNumber.PositionY}) (UNCHANGED!)");
            Console.WriteLine($"Cloned Position:   ({clonedNumber.PositionX},{clonedNumber.PositionY}) (CHANGED!)");

            // 4.2 BonusTile модификаци
            Console.WriteLine("\nBonus Tile");

            if (clonedBonus is BonusTile bonusClone)
            {
                Console.WriteLine($"BEFORE - Bonus activated: {bonusClone.IsActivated()}");
                bonusClone.OnMerge();
                Console.WriteLine($"AFTER  - Bonus activated: {bonusClone.IsActivated()}");

                BonusTile originalBonusTile = (BonusTile)originalBonus;
                Console.WriteLine($"Original Bonus activated: {originalBonusTile.IsActivated()} (UNCHANGED!)");
            }

            // 4.3 ObstacleTile модификация
            Console.WriteLine("\nObstacle Tile");

            if (clonedObstacle is ObstacleTile obstacleClone)
            {
                Console.WriteLine($"BEFORE modification - Health: {obstacleClone.GetHealth()}, Destroyed: {obstacleClone.IsDestroyed()}");
                obstacleClone.OnMerge();
                obstacleClone.OnMerge();
                Console.WriteLine($"AFTER modification - Health: {obstacleClone.GetHealth()}, Destroyed: {obstacleClone.IsDestroyed()}");

                ObstacleTile originalObstacleTile = (ObstacleTile)originalObstacle;
                Console.WriteLine($"Original Obstacle - Health: {originalObstacleTile.GetHealth()}, Destroyed: {originalObstacleTile.IsDestroyed()} (UNCHANGED!)");
            }
        }

        private static void DisplayTile(string label, Tile tile)
        {
            string typeName = tile.GetType().Name;
            string symbol = tile.GetSymbol().Trim();
            string pos = $"({tile.PositionX},{tile.PositionY})";
            string extraInfo = "";

            if (tile is BonusTile bonus)
                extraInfo = $" | Activated: {bonus.IsActivated()}";
            else if (tile is ObstacleTile obstacle)
                extraInfo = $" | Health: {obstacle.GetHealth()}";

            Console.WriteLine($"   {label,-30}: {typeName,-15} | " +
                            $"Value: {tile.Value,3} | Pos: {pos,-5} | Symbol: {symbol}{extraInfo}");
        }
    }
}