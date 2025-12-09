using Microsoft.Xna.Framework;

namespace Client.Logic.Plants
{
    public class Tulip : Plant
    {
        public Tulip(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(160, 32, 32, 32);
        }
    }
}
