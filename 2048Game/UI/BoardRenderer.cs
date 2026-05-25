using System;
using _2048Game.Entities;
using _2048Game.Core;
using _2048Game.Systems;

namespace _2048Game.UI
{
    public class BoardRenderer
    {
        private Board _board;

        public BoardRenderer(Board board)
        {
            _board = board;
        }

        public void Draw()
        {
            int size = _board.Size;
            int cellWidth = CalculateCellWidth();

            // Верхняя граница
            Console.Write("╔");
            for (int col = 0; col < size; col++)
            {
                Console.Write(new string('═', cellWidth));
                if (col < size - 1) Console.Write("╦");
            }
            Console.WriteLine("╗");

            // Рисуем строки
            for (int row = 0; row < size; row++)
            {
                Console.Write("║");
                for (int col = 0; col < size; col++)
                {
                    var tile = _board.GetCell(row, col);
                    DrawCellContent(tile, cellWidth);
                    if (col < size - 1) Console.Write("║");
                }
                Console.WriteLine("║");

                // Разделитель между строками
                if (row < size - 1)
                {
                    Console.Write("╠");
                    for (int col = 0; col < size; col++)
                    {
                        Console.Write(new string('═', cellWidth));
                        if (col < size - 1) Console.Write("╬");
                    }
                    Console.WriteLine("╣");
                }
            }

            // Нижняя граница
            Console.Write("╚");
            for (int col = 0; col < size; col++)
            {
                Console.Write(new string('═', cellWidth));
                if (col < size - 1) Console.Write("╩");
            }
            Console.WriteLine("╝");
        }

        private int CalculateCellWidth()
        {
            int maxValue = 0;
            for (int row = 0; row < _board.Size; row++)
            {
                for (int col = 0; col < _board.Size; col++)
                {
                    var tile = _board.GetCell(row, col);
                    if (tile != null && tile.Value > maxValue)
                    {
                        maxValue = tile.Value;
                    }
                }
            }
            int valueLength = maxValue.ToString().Length;
            return Math.Max(valueLength + 2, 4); // Минимум 4, +2 для пробелов
        }

        private void DrawCellContent(Tile? tile, int cellWidth)
        {
            if (tile == null)
            {
                Console.Write(new string(' ', cellWidth));
                return;
            }

            if (tile is ObstacleTile obstacle)
            {
                if (obstacle.IsDestroyed())
                {
                    Console.Write("░░".PadRight(cellWidth));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    int health = obstacle.GetHealth();
                    string healthBar = health switch
                    {
                        3 => "███",
                        2 => "██░",
                        1 => "█░░",
                        _ => "░░░"
                    };
                    Console.Write(healthBar.Substring(0, Math.Min(healthBar.Length, cellWidth)).PadRight(cellWidth));
                    Console.ResetColor();
                }
                return;
            }

            if (tile is BonusTile bonus)
            {
                Console.ForegroundColor = bonus.IsActivated() ? ConsoleColor.Magenta : ConsoleColor.DarkMagenta;
                Console.Write((bonus.IsActivated() ? "★★" : "★").PadRight(cellWidth));
                Console.ResetColor();
                return;
            }

            if (tile.Value == 0)
            {
                Console.Write(new string(' ', cellWidth));
                return;
            }

            string valueStr = tile.Value.ToString();
            Console.ForegroundColor = tile.GetColor();

            int padding = (cellWidth - valueStr.Length) / 2;
            Console.Write(new string(' ', padding));
            Console.Write(valueStr);
            Console.Write(new string(' ', cellWidth - padding - valueStr.Length));

            Console.ResetColor();
        }
    }
}