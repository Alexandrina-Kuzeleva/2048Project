namespace _2048Game.Entities
{
    public class ObstacleTile : Tile
    {
        private bool isDestroyed;
        private int health;

        public ObstacleTile()
        {
            Value = -1;
            isDestroyed = false;
            health = 3;
            Name = "ObstacleTile";
        }
        private ObstacleTile(bool destroyed, int healthValue, string name)
        {
            Value = -1;
            isDestroyed = destroyed;
            health = healthValue;
            Name = name;
        }

        public override void OnMerge()
        {
            health--;

            if (health <= 0)
            {
                isDestroyed = true;
                Value = 0;
                Console.WriteLine("█ Препятствие разрушено! █");
            }
            else
            {
                Console.WriteLine($"█ Препятствие повреждено! Осталось здоровья: {health} █");
            }

        }

        public override ConsoleColor GetColor()
        {
            if (isDestroyed) return ConsoleColor.DarkGray;

            return health switch
            {
                3 => ConsoleColor.DarkRed,
                2 => ConsoleColor.Red,
                1 => ConsoleColor.Yellow,
                _ => ConsoleColor.DarkGray
            };
        }

        public override string GetSymbol()
        {
            if (isDestroyed) return "    ";
            return $" █{health} ";
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }

        public int GetHealth()
        {
            return health;
        }

        protected override Tile CloneInternal()
        {
            ObstacleTile clone = new ObstacleTile(this.isDestroyed, this.health, this.Name + " (clone)");

            CopyBaseProperties(clone);

            return clone;
        }
    }
}