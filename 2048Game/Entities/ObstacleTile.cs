namespace _2048Game.Entities
{
    public class ObstacleTile : Tile
    {
        private bool isDestroyed;

        public ObstacleTile()
        {
            Value = -1;
            isDestroyed = false;
            Name = "ObstacleTile";
        }

        public override void OnMerge()
        {
            isDestroyed = true;
            Value = 0;
            Console.Beep(100, 100);
        }

        public override ConsoleColor GetColor()
        {
            return isDestroyed ? ConsoleColor.DarkGray : ConsoleColor.DarkRed;
        }

        public override string GetSymbol()
        {
            return isDestroyed ? "    " : " ██ ";
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }
    }
}