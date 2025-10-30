using Microsoft.Xna.Framework;


namespace Client.Logic.Plants
{
    public class Daisy : Plant
    {
        public Daisy(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(64, 0, 32, 32);
        }
    }
}
