using System.Linq;
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
                AddRandomTile(forceNumberTile: true);
                AddRandomTile();
            }

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

        public void AddRandomTile(bool forceNumberTile = false)
        {
            List<TileFactory> spawnFactories = availableFactories;
            if (forceNumberTile || !HasNumberTile())
            {
                var numberFactories = availableFactories.Where(factory => factory is NumberTileFactory).ToList();
                if (numberFactories.Count > 0)
                {
                    spawnFactories = numberFactories;
                }
            }

            List<(int, int)> emptyCells = new List<(int, int)>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tile = grid[i, j];
                    // Ячейка пустая, если Value == 0 И это не BonusTile
                    if (tile.Value == 0 && !(tile is BonusTile))
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }

            if (emptyCells.Count > 0 && spawnFactories.Count > 0)
            {
                var (x, y) = emptyCells[random.Next(emptyCells.Count)];
                int factoryIndex = random.Next(spawnFactories.Count);
                TileFactory factory = spawnFactories[factoryIndex];

                Tile newTile = factory.CreateTileAtPosition(x, y);
                grid[x, y] = newTile;
            }
        }

        private bool HasNumberTile()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid[i, j] is NumberTile numberTile && numberTile.Value != 0)
                        return true;
                }
            }
            return false;
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
            // Бонус можно активировать при касании с любой непустой плиткой
            if (a is BonusTile || b is BonusTile)
            {
                if (a is BonusTile)
                    return !(b is NumberTile numberB && numberB.Value == 0 && !(b is BonusTile));
                return !(a is NumberTile numberA && numberA.Value == 0 && !(a is BonusTile));
            }

            // Препятствия можно повредить при столкновении с реальной плиткой
            if (a is ObstacleTile || b is ObstacleTile)
            {
                if (a is ObstacleTile && b is ObstacleTile)
                    return false;

                return !(a is NumberTile numberA && numberA.Value == 0 && !(a is BonusTile))
                    && !(b is NumberTile numberB && numberB.Value == 0 && !(b is BonusTile));
            }

            // Нельзя сливать с пустыми клетками
            if (a.Value == 0 || b.Value == 0)
                return false;

            // Обычные плитки - по значению
            return a.Value == b.Value && !a.IsMerged && !b.IsMerged;
        }

        private void MergeTiles(Tile a, Tile b, int row, int col, int targetRow, int targetCol)
        {
            // Обработка бонусной плитки (может быть как a, так и b)
            if (a is BonusTile)
            {
                _scoreManager?.AddPoints(100);
                Console.WriteLine($"★ Bonus activated! +100 points ★");

                // Плитка движется в бонус: бонус удаляется, движущаяся плитка занимает целевую позицию.
                if (!(b is BonusTile))
                {
                    grid[targetRow, targetCol] = b;
                }
                else
                {
                    grid[targetRow, targetCol] = new NumberTile(0);
                }

                grid[row, col] = new NumberTile(0);
                return;
            }

            if (b is BonusTile)
            {
                _scoreManager?.AddPoints(100);
                Console.WriteLine($"★ Bonus activated! +100 points ★");
                grid[row, col] = new NumberTile(0);
                return;
            }

            // Обработка препятствия (повреждение)
            if (a is ObstacleTile obstacleA)
            {
                obstacleA.OnMerge();
                return;
            }

            if (b is ObstacleTile obstacleB)
            {
                obstacleB.OnMerge();
                return;
            }

            // Слияние обычных плиток
            if (a is NumberTile numA && b is NumberTile numB)
            {
                int newValue = numA.Value + numB.Value;
                grid[targetRow, targetCol] = new NumberTile(newValue);
                grid[row, col] = new NumberTile(0);
                grid[targetRow, targetCol].IsMerged = true;

                _scoreManager?.AddPoints(newValue);
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
                    while (targetCol > 0)
                    {
                        var nextTile = grid[row, targetCol - 1];
                        // Останавливаемся если следующая клетка занята (не пустая)
                        if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                            break;
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

                    // Проверяем слияние для обычных плиток, бонусов и препятствий
                    bool canMergeWithBonus = (current is BonusTile || next is BonusTile);
                    bool canMergeWithObstacle = (current is ObstacleTile || next is ObstacleTile);

                    if ((current.Value != 0 || canMergeWithBonus || canMergeWithObstacle) && IsMovable(next) && CanMerge(current, next))
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
                        while (targetCol > 0)
                        {
                            var nextTile = grid[row, targetCol - 1];
                            if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                                break;
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
                    while (targetCol < size - 1)
                    {
                        var nextTile = grid[row, targetCol + 1];
                        if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                            break;
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

                    bool canMergeWithBonus = (current is BonusTile || prev is BonusTile);
                    bool canMergeWithObstacle = (current is ObstacleTile || prev is ObstacleTile);

                    if ((current.Value != 0 || canMergeWithBonus || canMergeWithObstacle) && IsMovable(prev) && CanMerge(current, prev))
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
                        while (targetCol < size - 1)
                        {
                            var nextTile = grid[row, targetCol + 1];
                            if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                                break;
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
                    while (targetRow > 0)
                    {
                        var nextTile = grid[targetRow - 1, col];
                        if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                            break;
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

                    bool canMergeWithBonus = (current is BonusTile || next is BonusTile);
                    bool canMergeWithObstacle = (current is ObstacleTile || next is ObstacleTile);

                    if ((current.Value != 0 || canMergeWithBonus || canMergeWithObstacle) && IsMovable(next) && CanMerge(current, next))
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
                        while (targetRow > 0)
                        {
                            var nextTile = grid[targetRow - 1, col];
                            if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                                break;
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
                    while (targetRow < size - 1)
                    {
                        var nextTile = grid[targetRow + 1, col];
                        if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                            break;
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

                    bool canMergeWithBonus = (current is BonusTile || prev is BonusTile);
                    bool canMergeWithObstacle = (current is ObstacleTile || prev is ObstacleTile);

                    if ((current.Value != 0 || canMergeWithBonus || canMergeWithObstacle) && IsMovable(prev) && CanMerge(current, prev))
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
                        while (targetRow < size - 1)
                        {
                            var nextTile = grid[targetRow + 1, col];
                            if (nextTile.Value != 0 || nextTile is BonusTile || (nextTile is ObstacleTile obstacle && !obstacle.IsDestroyed()))
                                break;
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