namespace _2048Game.Entities
{
    public class BonusTile : Tile
    {
        private int bonusMultiplier;
        private bool isActivated;

        public BonusTile()
        {
            Value = 0;
            bonusMultiplier = 2;
            isActivated = false;
            Name = "BonusTile";
        }

        private BonusTile(int multiplier, bool activated, string name)
        {
            Value = 0;
            bonusMultiplier = multiplier;
            isActivated = activated;
            Name = name;
        }

        public override void OnMerge()
        {
            isActivated = true;
            Console.Beep(400, 100);
            Console.WriteLine("★ Бонус активирован! ★");
        }

        public override ConsoleColor GetColor()
        {
            return isActivated ? ConsoleColor.DarkMagenta : ConsoleColor.Magenta;
        }

        public override string GetSymbol()
        {
            return isActivated ? " ★★ " : " ★ ";
        }

        public int GetBonusMultiplier()
        {
            return bonusMultiplier;
        }

        public bool IsActivated()
        {
            return isActivated;
        }

        protected override Tile CloneInternal()
        {
            BonusTile clone = new BonusTile(this.bonusMultiplier, this.isActivated, this.Name + " (clone)");

            CopyBaseProperties(clone);

            return clone;
        }
    }
}