namespace _2048Game.Entities
{
    public abstract class Entity
    {
        public string Name { get; set; }
        
        public abstract void Update();
        public abstract void Draw();
    }
}