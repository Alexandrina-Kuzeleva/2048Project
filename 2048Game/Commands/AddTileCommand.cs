using _2048Game.Systems;
using _2048Game.UI;

namespace _2048Game.Commands
{
    public class AddTileCommand : ICommand
    {
        private Board _board;
        private ConsoleHUD _hud;

        public AddTileCommand(Board board, ConsoleHUD hud)
        {
            _board = board;
            _hud = hud;
        }

        public void Execute()
        {
            _board.AddRandomTile();
            _hud.RefreshAll();
        }

        public string GetDescription() => "Add Random Tile";
    }
}