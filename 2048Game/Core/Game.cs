using System;

namespace _2048Game.Core
{
    public class Game
    {
        private bool isRunning;
        
        public Game()
        {
            isRunning = true;
        }
        
        public void Run()
        {
            while (isRunning)
            {
                HandleInput();
                Update();
                Render();
            }
        }
        
        private void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                
                if (key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
            }
        }
        
        private void Update()
        {
            // здесь когда то будет логика но пока нет))
        }
        
        private void Render()
        {
            Console.Clear();
            Console.WriteLine("2048 Game - Press ESC to exit");
        }
    }
}