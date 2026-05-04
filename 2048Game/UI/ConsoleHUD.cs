using System;
using _2048Game.Core;
using _2048Game.Systems;
using _2048Game.Events;

namespace _2048Game.UI
{
    public class ConsoleHUD : IDisposable
    {
        private ScoreManager _scoreManager;
        private BoardRenderer? _renderer;
        private int _lastScore;
        private int _lastHighScore;
        private bool _disposed;

        public ConsoleHUD(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            _lastScore = scoreManager.CurrentScore;
            _lastHighScore = scoreManager.HighScore;

            _scoreManager.ScoreChanged += OnScoreChanged;
            _scoreManager.HighScoreChanged += OnHighScoreChanged;

            DrawInitialHUD();
        }

        public void SetBoard(Board board)
        {
            _renderer = new BoardRenderer(board);
        }

        private void OnScoreChanged(object? sender, ScoreEventArgs e)
        {
            _lastScore = e.NewScore;

            if (e.PointsAdded > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(0, 8);
                Console.WriteLine($"+{e.PointsAdded} points! ".PadRight(50));
                Console.ResetColor();
            }

            DrawHUD();
        }

        private void OnHighScoreChanged(object? sender, HighScoreEventArgs e)
        {
            _lastHighScore = e.NewHighScore;

            if (e.IsNewRecord)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 9);
                Console.WriteLine($"NEW HIGH SCORE! {e.OldHighScore} → {e.NewHighScore}".PadRight(50));
                Console.ResetColor();
            }

            DrawHUD();
        }

        private void DrawInitialHUD()
        {
            Console.Clear();
            DrawHUD();
        }

        private void DrawHUD()
        {
            // Верхняя граница
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                        2048 GAME                          ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            // Строка со счетом
            Console.Write("║ SCORE: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{_lastScore,8}");
            Console.ResetColor();

            // Прогресс-бар к рекорду
            if (_lastHighScore > 0)
            {
                int progress = (int)((double)_lastScore / _lastHighScore * 100);
                progress = Math.Min(progress, 100);

                Console.Write("  [");
                for (int i = 0; i < 20; i++)
                {
                    if (i < progress / 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("█");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("░");
                    }
                }
                Console.Write($"] {progress,3}%");
            }
            Console.WriteLine(" ║");

            // Строка с рекордом
            Console.Write("║ HIGH SCORE: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{_lastHighScore,8}");
            Console.ResetColor();
            Console.WriteLine("                                              ║");

            // Нижняя граница HUD
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Controls: ESC=Exit | Arrows=Move | SPACE=Add Tile         ║");
            Console.WriteLine("║ Strategies: 1=Standard | 2=Aggressive | 3=Defensive | 4=Random ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }

        public void RefreshBoard()
        {
            if (_renderer != null)
            {
                _renderer.Draw();
            }
        }

        public void ShowGameOver()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      GAME OVER!                            ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Final Score: {_lastScore,8}                                    ║");
            Console.WriteLine($"║ High Score:  {_lastHighScore,8}                                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_scoreManager != null)
                {
                    _scoreManager.ScoreChanged -= OnScoreChanged;
                    _scoreManager.HighScoreChanged -= OnHighScoreChanged;
                }
                _disposed = true;
            }
        }
    }
}