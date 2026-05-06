using _2048Game.Core;
using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.Commands
{
    public class MoveUpCommand : ICommand
    {
        private Board _board;
        private ConsoleHUD _hud;

        public MoveUpCommand(Board board, ConsoleHUD hud)
        {
            _board = board;
            _hud = hud;
        }

        public void Execute()
        {
            _board.Move(Direction.Up);
            _hud.RefreshAll();
        }

        public string GetDescription() => "Move Up";
    }
}