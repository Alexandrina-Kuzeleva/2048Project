using System;

namespace _2048Game.Entities
{
    public abstract class Tile : Entity, ICloneable
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

        public object Clone()
        {
            return CloneInternal();
        }

        protected abstract Tile CloneInternal();

        protected void CopyBaseProperties(Tile target)
        {
            target.Value = this.Value;
            target.PositionX = this.PositionX;
            target.PositionY = this.PositionY;
            target.Name = this.Name + " (clone)";
        }
    }
}