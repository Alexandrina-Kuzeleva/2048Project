using System;
using _2048Game.Core;
using _2048Game.UI;

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
            var content = new List<string>
            {
                "",
                "Press ESC to resume game",
                "Press M to return to main menu",
                ""
            };
            FrameRenderer.DrawFrame("GAME PAUSED", content);
        }

        public void Update() { }

        public void Exit()
        {
            Console.Clear();
        }

        public void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Escape:
                    _context.SetState(_context.GameState);
                    break;

                case ConsoleKey.M:
                    _context.SetState(_context.MenuState);
                    break;
            }
        }
    }
}