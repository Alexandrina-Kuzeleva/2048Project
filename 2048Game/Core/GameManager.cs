using System;

namespace _2048Game.Core
{
    public class GameManager
    {
        private static GameManager _instance;

        public int MapSize { get; set; }
        public Difficulty GameDifficulty { get; set; }
        public bool IsSoundEnabled { get; set; }

        private GameManager()
        {
            MapSize = 4;
            GameDifficulty = Difficulty.Normal;
            IsSoundEnabled = true;
        }

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public void Run()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("       2048 GAME - SINGLETON DEMO      ");
            Console.WriteLine("========================================");
            Console.WriteLine($"Map Size: {MapSize}x{MapSize}");
            Console.WriteLine($"Difficulty: {GameDifficulty}");
            Console.WriteLine($"Sound: {(IsSoundEnabled ? "ON" : "OFF")}");
            Console.WriteLine("========================================");
            Console.WriteLine("Press ESC to exit");
            Console.WriteLine("========================================\n");

            bool isRunning = true;

            while (isRunning)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        isRunning = false;
                    }
                }
                Thread.Sleep(50);
            }

            Console.WriteLine("\nGame Over! Press any key to exit...");
            Console.ReadKey();
        }

        public void ShowSettings()
        {
            Console.WriteLine($"Current settings - Size: {MapSize}, Difficulty: {GameDifficulty}");
        }
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}