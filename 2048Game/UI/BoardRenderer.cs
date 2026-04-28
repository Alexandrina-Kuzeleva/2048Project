using System;
using _2048Game.Entities;
using _2048Game.Core;
using _2048Game.Systems;

namespace _2048Game.UI
{
    public class BoardRenderer
    {
        private const int CELL_WIDTH = 6;
        private const int CELL_HEIGHT = 3;

        private Board _board;

        public BoardRenderer(Board board)
        {
            _board = board;
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, 12); // Начинаем рисовать после HUD

            int size = _board.Size;

            // Верхняя граница
            Console.Write("╔");
            for (int col = 0; col < size; col++)
            {
                Console.Write(new string('═', CELL_WIDTH));
                if (col < size - 1) Console.Write("╦");
            }
            Console.WriteLine("╗");

            // Рисуем строки
            for (int row = 0; row < size; row++)
            {
                // Верхняя часть клеток (содержимое)
                Console.Write("║");
                for (int col = 0; col < size; col++)
                {
                    var tile = _board.GetCell(row, col);
                    DrawCellContent(tile);
                    if (col < size - 1) Console.Write("║");
                }
                Console.WriteLine("║");

                // Разделитель между строками
                if (row < size - 1)
                {
                    Console.Write("╠");
                    for (int col = 0; col < size; col++)
                    {
                        Console.Write(new string('═', CELL_WIDTH));
                        if (col < size - 1) Console.Write("╬");
                    }
                    Console.WriteLine("╣");
                }
            }

            // Нижняя граница
            Console.Write("╚");
            for (int col = 0; col < size; col++)
            {
                Console.Write(new string('═', CELL_WIDTH));
                if (col < size - 1) Console.Write("╩");
            }
            Console.WriteLine("╝");
        }

        private void DrawCellContent(Tile? tile)
        {
            if (tile == null)
            {
                Console.Write("      ");
                return;
            }

            // Препятствия
            if (tile is ObstacleTile obstacle)
            {
                if (obstacle.IsDestroyed())
                {
                    Console.Write("  ░░  ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("  ██  ");
                    Console.ResetColor();
                }
                return;
            }

            // Бонусные плитки
            if (tile is BonusTile bonus)
            {
                Console.ForegroundColor = bonus.IsActivated() ? ConsoleColor.Magenta : ConsoleColor.DarkMagenta;
                Console.Write(bonus.IsActivated() ? "  ★★  " : "  ★   ");
                Console.ResetColor();
                return;
            }

            // Обычные числовые плитки
            if (tile.Value == 0)
            {
                Console.Write("      ");
                return;
            }

            string valueStr = tile.Value.ToString();
            Console.ForegroundColor = tile.GetColor();

            int padding = (CELL_WIDTH - valueStr.Length) / 2;
            Console.Write(new string(' ', padding));
            Console.Write(valueStr);
            Console.Write(new string(' ', CELL_WIDTH - padding - valueStr.Length));

            Console.ResetColor();
        }

        public void DrawSimple()
        {
            // Простая версия отрисовки (без рамок)
            Console.WriteLine();
            for (int row = 0; row < _board.Size; row++)
            {
                for (int col = 0; col < _board.Size; col++)
                {
                    var tile = _board.GetCell(row, col);
                    if (tile != null && tile.Value != 0)
                    {
                        Console.ForegroundColor = tile.GetColor();
                        Console.Write($"[{tile.Value,4}] ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("[    ] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}