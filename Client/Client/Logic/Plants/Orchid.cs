using Microsoft.Xna.Framework;


namespace Client.Logic.Plants
{
    public class Orchid : Plant
    {
        public Orchid(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(0, 32, 32, 32);
        }
    }
}
