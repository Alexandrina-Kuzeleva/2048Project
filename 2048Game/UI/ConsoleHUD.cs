using System;
using System.Collections.Generic;
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
        }

        public void SetBoard(Board board)
        {
            _renderer = new BoardRenderer(board);
        }

        private void OnScoreChanged(object? sender, ScoreEventArgs e)
        {
            _lastScore = e.NewScore;
            RefreshAll();
        }

        private void OnHighScoreChanged(object? sender, HighScoreEventArgs e)
        {
            _lastHighScore = e.NewHighScore;
            RefreshAll();
        }

        public void RefreshAll()
        {
            Console.Clear();
            DrawHUD();
            _renderer?.Draw();
        }

        private void DrawHUD()
        {
            // Прогресс-бар
            int progress = 0;
            if (_lastHighScore > 0)
            {
                progress = (int)((double)_lastScore / _lastHighScore * 100);
                progress = Math.Min(progress, 100);
            }

            string progressBar = "";
            for (int i = 0; i < 20; i++)
            {
                progressBar += i < progress / 5 ? "█" : "░";
            }

            var content = new List<string>
                {
                    $"SCORE: {_lastScore}  [{progressBar}] {progress}%",
                    $"HIGH SCORE: {_lastHighScore}",
                    "",
                    "Controls: Arrows=Move | SPACE=Add Tile | ESC=Menu | R=Remap Keys",
                    "",
                    "Press R to remap controls!"
                };

            FrameRenderer.DrawFrame("2048 GAME", content);
        }

        public void ShowGameOver()
        {
            Console.Clear();
            var content = new List<string>
            {
                $"Final Score: {_lastScore}",
                $"High Score: {_lastHighScore}",
                "",
                "Press any key to return to main menu..."
            };
            FrameRenderer.DrawFrame("GAME OVER!", content, ConsoleColor.Red);
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