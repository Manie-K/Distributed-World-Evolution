using Microsoft.Xna.Framework;


namespace Client.Logic.Plants
{
    public class Lavender : Plant
    {
        public Lavender(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(96, 0, 32, 32);
        }
    }
}
