namespace _2048Game.Entities
{
    public abstract class Tile : Entity
    {
        public int Value { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public abstract void OnMerge();
        public abstract ConsoleColor GetColor();
        public abstract string GetSymbol();

        public override void Update()
        {

        }

        public override void Draw()
        {
            Console.ForegroundColor = GetColor();
            Console.Write(GetSymbol());
            Console.ResetColor();
        }
    }
}