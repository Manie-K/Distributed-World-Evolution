using Microsoft.Xna.Framework;

namespace Client.Logic.Plants
{
    public class Rose : Plant
    {
        public Rose(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(96, 32, 32, 32);
        }
    }
}
