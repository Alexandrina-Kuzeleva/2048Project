using _2048Game.Entities;

namespace _2048Game.Systems
{
    public class Board
    {
        private Tile[,] grid;
        private int size;

        public int Size => size;

        public Board(int size)
        {
            this.size = size;
            grid = new Tile[size, size];
        }

        public Tile GetCell(int x, int y)
        {
            return null; // заглушка
        }

        public void Move(Direction direction)
        {
            // будующем логика движения
        }
    }
}