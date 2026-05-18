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
        private ScoreManager? _scoreManager;

        public int Size => size;

        public Board() : this(true)
        {
        }

        public Board(bool initialize)
        {
            this.size = GameManager.Instance.MapSize;
            grid = new Tile[size, size];
            random = new Random();
            availableFactories = new List<TileFactory>();
            _scoreManager = new ScoreManager();

            InitializeFactories();
            InitializeEmptyBoard();

            if (initialize)
            {
                AddRandomTile();
                AddRandomTile();
            }

            Console.WriteLine($"Board created with size {size}");
            Console.WriteLine($"Available factories: {availableFactories.Count}");
        }

        public void SetScoreManager(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
        }

        private void InitializeEmptyBoard()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new NumberTile(0)
                    {
                        PositionX = i,
                        PositionY = j
                    };
                }
            }
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

        public void AddRandomTile()
        {
            List<(int, int)> emptyCells = new List<(int, int)>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid[i, j].Value == 0)
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }

            if (emptyCells.Count > 0)
            {
                var (x, y) = emptyCells[random.Next(emptyCells.Count)];
                int factoryIndex = random.Next(availableFactories.Count);
                TileFactory factory = availableFactories[factoryIndex];

                Tile newTile = factory.CreateTileAtPosition(x, y);
                grid[x, y] = newTile;

                string tileType = newTile is BonusTile ? "★ Bonus" :
                                  newTile is ObstacleTile ? "█ Obstacle" : "Number";
                Console.WriteLine($"Added {tileType} tile at ({x}, {y}) with value {newTile.Value}");
            }
        }

        // ============ ЛОГИКА ДВИЖЕНИЯ ============

        public bool Move(Direction direction)
        {
            // Сбрасываем флаги слияния
            ResetMergeFlags();

            bool moved = false;
            var beforeGrid = CopyGrid();

            switch (direction)
            {
                case Direction.Left:
                    MoveLeft();
                    break;
                case Direction.Right:
                    MoveRight();
                    break;
                case Direction.Up:
                    MoveUp();
                    break;
                case Direction.Down:
                    MoveDown();
                    break;
            }

            if (!GridsEqual(beforeGrid, grid))
            {
                moved = true;
                AddRandomTile();
            }

            return moved;
        }

        private void ResetMergeFlags()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid[i, j] is NumberTile)
                    {
                        grid[i, j].IsMerged = false;
                    }
                }
            }
        }

        private bool IsMovable(Tile tile)
        {
            // Препятствия нельзя перемещать
            if (tile is ObstacleTile obstacle)
            {
                if (!obstacle.IsDestroyed())
                    return false;
            }
            return true;
        }

        private bool CanMerge(Tile a, Tile b)
        {
            // Нельзя сливать с препятствиями
            if (a is ObstacleTile || b is ObstacleTile)
                return false;

            // Нельзя сливать с пустыми клетками
            if (a.Value == 0 || b.Value == 0)
                return false;

            // Бонусные плитки
            if (a is BonusTile || b is BonusTile)
            {
                // Бонус можно активировать при касании с любой плиткой
                return true;
            }

            // Обычные плитки - по значению
            return a.Value == b.Value && !a.IsMerged && !b.IsMerged;
        }

        private void MergeTiles(Tile a, Tile b, int row, int col, int targetRow, int targetCol)
        {
            if (a is BonusTile bonus)
            {
                bonus.OnMerge();
                _scoreManager?.AddPoints(100);
                Console.WriteLine($"★ Bonus activated! +100 points ★");

                // Бонусная плитка исчезает после активации
                grid[row, col] = new NumberTile(0);
                return;
            }

            if (a is NumberTile numA && b is NumberTile numB)
            {
                int newValue = numA.Value + numB.Value;
                grid[targetRow, targetCol] = new NumberTile(newValue);
                grid[row, col] = new NumberTile(0);
                grid[targetRow, targetCol].IsMerged = true;

                _scoreManager?.AddPoints(newValue);
                Console.WriteLine($"Merged! {numA.Value} + {numB.Value} = {newValue} (+{newValue} points)");
            }
        }

        private void MoveLeft()
        {
            for (int row = 0; row < size; row++)
            {
                // Сдвиг влево
                for (int col = 1; col < size; col++)
                {
                    var currentTile = grid[row, col];

                    // Пропускаем пустые клетки и непреодолимые препятствия
                    if (currentTile.Value == 0) continue;
                    if (!IsMovable(currentTile)) continue;

                    int targetCol = col;
                    while (targetCol > 0 && grid[row, targetCol - 1].Value == 0)
                    {
                        targetCol--;
                    }

                    if (targetCol != col)
                    {
                        grid[row, targetCol] = currentTile;
                        grid[row, col] = new NumberTile(0);
                    }
                }

                // Слияние
                for (int col = 0; col < size - 1; col++)
                {
                    var current = grid[row, col];
                    var next = grid[row, col + 1];

                    if (current.Value != 0 && CanMerge(current, next))
                    {
                        MergeTiles(current, next, row, col + 1, row, col);
                    }
                }

                // Второй проход для сдвига после слияния
                for (int col = 1; col < size; col++)
                {
                    var currentTile = grid[row, col];
                    if (currentTile.Value != 0 && IsMovable(currentTile))
                    {
                        int targetCol = col;
                        while (targetCol > 0 && grid[row, targetCol - 1].Value == 0)
                        {
                            targetCol--;
                        }

                        if (targetCol != col)
                        {
                            grid[row, targetCol] = currentTile;
                            grid[row, col] = new NumberTile(0);
                        }
                    }
                }
            }
        }

        private void MoveRight()
        {
            for (int row = 0; row < size; row++)
            {
                // Сдвиг вправо
                for (int col = size - 2; col >= 0; col--)
                {
                    var currentTile = grid[row, col];
                    if (currentTile.Value == 0) continue;
                    if (!IsMovable(currentTile)) continue;

                    int targetCol = col;
                    while (targetCol < size - 1 && grid[row, targetCol + 1].Value == 0)
                    {
                        targetCol++;
                    }

                    if (targetCol != col)
                    {
                        grid[row, targetCol] = currentTile;
                        grid[row, col] = new NumberTile(0);
                    }
                }

                // Слияние
                for (int col = size - 1; col > 0; col--)
                {
                    var current = grid[row, col];
                    var prev = grid[row, col - 1];

                    if (current.Value != 0 && CanMerge(current, prev))
                    {
                        MergeTiles(current, prev, row, col - 1, row, col);
                    }
                }

                // Второй проход
                for (int col = size - 2; col >= 0; col--)
                {
                    var currentTile = grid[row, col];
                    if (currentTile.Value != 0 && IsMovable(currentTile))
                    {
                        int targetCol = col;
                        while (targetCol < size - 1 && grid[row, targetCol + 1].Value == 0)
                        {
                            targetCol++;
                        }

                        if (targetCol != col)
                        {
                            grid[row, targetCol] = currentTile;
                            grid[row, col] = new NumberTile(0);
                        }
                    }
                }
            }
        }

        private void MoveUp()
        {
            for (int col = 0; col < size; col++)
            {
                // Сдвиг вверх
                for (int row = 1; row < size; row++)
                {
                    var currentTile = grid[row, col];
                    if (currentTile.Value == 0) continue;
                    if (!IsMovable(currentTile)) continue;

                    int targetRow = row;
                    while (targetRow > 0 && grid[targetRow - 1, col].Value == 0)
                    {
                        targetRow--;
                    }

                    if (targetRow != row)
                    {
                        grid[targetRow, col] = currentTile;
                        grid[row, col] = new NumberTile(0);
                    }
                }

                // Слияние
                for (int row = 0; row < size - 1; row++)
                {
                    var current = grid[row, col];
                    var next = grid[row + 1, col];

                    if (current.Value != 0 && CanMerge(current, next))
                    {
                        MergeTiles(current, next, row + 1, col, row, col);
                    }
                }

                // Второй проход
                for (int row = 1; row < size; row++)
                {
                    var currentTile = grid[row, col];
                    if (currentTile.Value != 0 && IsMovable(currentTile))
                    {
                        int targetRow = row;
                        while (targetRow > 0 && grid[targetRow - 1, col].Value == 0)
                        {
                            targetRow--;
                        }

                        if (targetRow != row)
                        {
                            grid[targetRow, col] = currentTile;
                            grid[row, col] = new NumberTile(0);
                        }
                    }
                }
            }
        }

        private void MoveDown()
        {
            for (int col = 0; col < size; col++)
            {
                // Сдвиг вниз
                for (int row = size - 2; row >= 0; row--)
                {
                    var currentTile = grid[row, col];
                    if (currentTile.Value == 0) continue;
                    if (!IsMovable(currentTile)) continue;

                    int targetRow = row;
                    while (targetRow < size - 1 && grid[targetRow + 1, col].Value == 0)
                    {
                        targetRow++;
                    }

                    if (targetRow != row)
                    {
                        grid[targetRow, col] = currentTile;
                        grid[row, col] = new NumberTile(0);
                    }
                }

                // Слияние
                for (int row = size - 1; row > 0; row--)
                {
                    var current = grid[row, col];
                    var prev = grid[row - 1, col];

                    if (current.Value != 0 && CanMerge(current, prev))
                    {
                        MergeTiles(current, prev, row - 1, col, row, col);
                    }
                }

                // Второй проход
                for (int row = size - 2; row >= 0; row--)
                {
                    var currentTile = grid[row, col];
                    if (currentTile.Value != 0 && IsMovable(currentTile))
                    {
                        int targetRow = row;
                        while (targetRow < size - 1 && grid[targetRow + 1, col].Value == 0)
                        {
                            targetRow++;
                        }

                        if (targetRow != row)
                        {
                            grid[targetRow, col] = currentTile;
                            grid[row, col] = new NumberTile(0);
                        }
                    }
                }
            }
        }

        private Tile[,] CopyGrid()
        {
            var copy = new Tile[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid[i, j] != null)
                        copy[i, j] = (Tile)grid[i, j].Clone();
                }
            }
            return copy;
        }

        private bool GridsEqual(Tile[,] grid1, Tile[,] grid2)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid1[i, j].Value != grid2[i, j].Value)
                        return false;
                }
            }
            return true;
        }

        public Tile? GetCell(int x, int y)
        {
            if (x >= 0 && x < size && y >= 0 && y < size)
                return grid[x, y];
            return null;
        }

        public bool IsGameOver()
        {
            // 1. Проверяем наличие возможных ходов
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var current = grid[i, j];

                    // Пропускаем препятствия и пустые клетки
                    if (current is ObstacleTile obstacle && !obstacle.IsDestroyed())
                        continue;
                    if (current.Value == 0)
                        continue;

                    // Проверяем соседей справа
                    if (j < size - 1)
                    {
                        var right = grid[i, j + 1];
                        // Если справа пусто или можно слить
                        if (right.Value == 0 || CanMerge(current, right))
                            return false;
                    }

                    // Проверяем соседей слева
                    if (j > 0)
                    {
                        var left = grid[i, j - 1];
                        if (left.Value == 0 || CanMerge(current, left))
                            return false;
                    }

                    // Проверяем соседа снизу
                    if (i < size - 1)
                    {
                        var down = grid[i + 1, j];
                        if (down.Value == 0 || CanMerge(current, down))
                            return false;
                    }

                    // Проверяем соседа сверху
                    if (i > 0)
                    {
                        var up = grid[i - 1, j];
                        if (up.Value == 0 || CanMerge(current, up))
                            return false;
                    }
                }
            }

            return true;
        }

        public void SetCell(int x, int y, Tile tile)
        {
            if (x >= 0 && x < size && y >= 0 && y < size)
            {
                grid[x, y] = tile;
            }
        }

    }
}