using _2048Game.Entities;
using _2048Game.Core;

namespace _2048Game.Systems
{
    public class Board
    {
        private Tile[,] grid;
        private int size;

        public int Size => size;

        public Board()
        {
            this.size = GameManager.Instance.MapSize;
            grid = new Tile[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = new Tile();
                }
            }

            Console.WriteLine($"Board created with size {size} from GameManager settings");
        }

        public Tile GetCell(int x, int y)
        {
            if (x >= 0 && x < size && y >= 0 && y < size)
                return grid[x, y];
            return null;
        }
    }
}