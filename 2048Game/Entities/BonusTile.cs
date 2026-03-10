namespace _2048Game.Entities
{
    public class BonusTile : Tile
    {
        private int bonusMultiplier;

        public BonusTile()
        {
            Value = 0;
            bonusMultiplier = 2;
            Name = "BonusTile";
        }

        public override void OnMerge()
        {
            Console.Beep(400, 100);
            Console.WriteLine("Бонус активирован!");
        }

        public override ConsoleColor GetColor()
        {
            return ConsoleColor.Magenta;
        }

        public override string GetSymbol()
        {
            return "*";
        }

        public int GetBonusMultiplier()
        {
            return bonusMultiplier;
        }
    }
}