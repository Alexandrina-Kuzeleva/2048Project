using System;
using _2048Game.Core;

namespace _2048Game.States
{
    public class PauseState : IGameState
    {
        private GameStateContext _context;

        public string StateName => "Pause";

        public PauseState(GameStateContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      GAME PAUSED                          ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║    Press ESC to resume game                               ║");
            Console.WriteLine("║    Press ESC again to continue...                        ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }

        public void Update()
        {

        }

        public void Exit()
        {
            Console.Clear();
        }

        public void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Escape:
                    // Возврат в игру
                    _context.SetState(_context.GameState);
                    break;
            }
        }
    }
}