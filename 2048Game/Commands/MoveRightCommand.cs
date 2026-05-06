using _2048Game.Core;
using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.Commands
{
    public class MoveRightCommand : ICommand
    {
        private Board _board;
        private ConsoleHUD _hud;

        public MoveRightCommand(Board board, ConsoleHUD hud)
        {
            _board = board;
            _hud = hud;
        }

        public void Execute()
        {
            _board.Move(Direction.Right);
            _hud.RefreshAll();
        }

        public string GetDescription() => "Move Right";
    }
}