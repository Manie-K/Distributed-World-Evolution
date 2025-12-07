using Microsoft.Xna.Framework;

namespace Client.Logic.Plants
{
    public class Lily : Plant
    {
        public Lily(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(128, 0, 32, 32);
        }
    }
}
