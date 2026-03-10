namespace _2048Game.Entities
{
    public class NumberTile : Tile
    {
        public NumberTile(int initialValue = 2)
        {
            Value = initialValue;
            Name = $"NumberTile_{Value}";
        }

        public override void OnMerge()
        {
            Value *= 2;
            Console.Beep(200, 50); // звук слияния плитки 
        }

        public override ConsoleColor GetColor()
        {
            return Value switch
            {
                2 => ConsoleColor.DarkGreen,
                4 => ConsoleColor.Green,
                8 => ConsoleColor.DarkYellow,
                16 => ConsoleColor.Yellow,
                32 => ConsoleColor.DarkRed,
                64 => ConsoleColor.Red,
                128 => ConsoleColor.Magenta,
                256 => ConsoleColor.Cyan,
                512 => ConsoleColor.Blue,
                1024 => ConsoleColor.DarkBlue,
                2048 => ConsoleColor.White,
                _ => ConsoleColor.Gray
            };
        }

        public override string GetSymbol()
        {
            return Value.ToString().PadLeft(4);
        }
    }
}