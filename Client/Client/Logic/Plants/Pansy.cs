using Microsoft.Xna.Framework;

namespace Client.Logic.Plants
{
    public class Pansy : Plant
    {
        public Pansy(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(32, 32, 32, 32);
        }
    }
}
