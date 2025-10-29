using Microsoft.Xna.Framework;


namespace Client.Logic.Plants
{
    public class Sunflower : Plant
    {
        public Sunflower(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(128, 32, 32, 32);
        }
    }
}
