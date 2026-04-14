using _2048Game.Entities;
using _2048Game.Core;
using _2048Game.Factories;

namespace _2048Game.Systems
{
    public class Board
    {
        private Tile[,] grid;
        private int size;
        private Random random;
        private List<TileFactory> availableFactories;

        public int Size => size;

        public Board()
        {
            this.size = GameManager.Instance.MapSize;
            grid = new Tile[size, size];
            random = new Random();
            availableFactories = new List<TileFactory>();

            InitializeFactories();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new NumberTile(0);
                }
            }

            Console.WriteLine($"Board created with size {size} from GameManager settings");
            Console.WriteLine($"Available factories: {availableFactories.Count}");
        }

        private void InitializeFactories()
        {
            availableFactories.Add(new NumberTileFactory(2));
            availableFactories.Add(new NumberTileFactory(4));

            switch (GameManager.Instance.GameDifficulty)
            {
                case Difficulty.Easy:
                    availableFactories.Add(new BonusTileFactory());
                    break;

                case Difficulty.Hard:
                    availableFactories.Add(new ObstacleTileFactory());
                    break;

                case Difficulty.Normal:
                    availableFactories.Add(new BonusTileFactory());
                    availableFactories.Add(new ObstacleTileFactory());
                    break;
            }
        }

        private List<(int row, int col)> FindEmptyCells()
        {
            var emptyCells = new List<(int row, int col)>();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (grid[row, col].Value == 0)
                    {
                        emptyCells.Add((row, col));
                    }
                }
            }

            return emptyCells;
        }

        private (int row, int col)? GetRandomEmptyCell()
        {
            var emptyCells = FindEmptyCells();

            if (emptyCells.Count == 0)
                return null;

            return emptyCells[random.Next(emptyCells.Count)];
        }

        private TileFactory GetRandomFactory()
        {
            return availableFactories[random.Next(availableFactories.Count)];
        }

        public void AddRandomTile()
        {
            var emptyCell = GetRandomEmptyCell();

            if (emptyCell == null)
                return;

            var (row, col) = emptyCell.Value;
            TileFactory factory = GetRandomFactory();

            Tile newTile = factory.CreateTileAtPosition(row, col);
            grid[row, col] = newTile;

            Console.WriteLine($"Added {newTile.GetType().Name} at ({row}, {col}) with value {newTile.Value}");
        }

        public Tile? GetCell(int x, int y)
        {
            if (x >= 0 && x < size && y >= 0 && y < size)
                return grid[x, y];
            return null;
        }

        public void Move(Direction direction)
        {
            Console.WriteLine($"Move {direction} - will use polymorphic Tile methods");
        }
    }
}