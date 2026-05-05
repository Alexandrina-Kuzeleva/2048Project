using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048Game.UI
{
    public static class FrameRenderer
    {
        public static void DrawFrame(string title, List<string> contentLines, ConsoleColor? titleColor = null, int? fixedWidth = null)
        {
            int width = fixedWidth ?? CalculateWidth(title, contentLines);
            width = Math.Max(width, 50); // Минимальная ширина

            if (titleColor.HasValue)
            {
                Console.ForegroundColor = titleColor.Value;
            }

            // Верхняя граница
            Console.Write("╔");
            Console.Write(new string('═', width - 2));
            Console.WriteLine("╗");

            // Заголовок
            if (!string.IsNullOrEmpty(title))
            {
                string titleLine = $" {title} ";
                int padding = (width - titleLine.Length - 2) / 2;
                Console.Write("║");
                Console.Write(new string(' ', padding));
                Console.Write(titleLine);
                Console.Write(new string(' ', width - 2 - padding - titleLine.Length));
                Console.WriteLine("║");

                // Разделитель
                Console.Write("╠");
                Console.Write(new string('═', width - 2));
                Console.WriteLine("╣");
            }

            // Содержимое
            foreach (var line in contentLines)
            {
                string paddedLine = string.IsNullOrEmpty(line) ? "" : $" {line} ";
                Console.Write("║");
                Console.Write(paddedLine.PadRight(width - 2));
                Console.WriteLine("║");
            }

            // Нижняя граница
            Console.Write("╚");
            Console.Write(new string('═', width - 2));
            Console.WriteLine("╝");

            if (titleColor.HasValue)
            {
                Console.ResetColor();
            }
        }

        private static int CalculateWidth(string title, List<string> contentLines)
        {
            int maxContentWidth = 0;
            foreach (var line in contentLines)
            {
                int lineWidth = (string.IsNullOrEmpty(line) ? 0 : line.Length + 2);
                if (lineWidth > maxContentWidth)
                    maxContentWidth = lineWidth;
            }

            int titleWidth = string.IsNullOrEmpty(title) ? 0 : title.Length + 4;
            return Math.Max(maxContentWidth, titleWidth) + 2;
        }
    }
}