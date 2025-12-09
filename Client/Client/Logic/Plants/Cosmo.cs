using Microsoft.Xna.Framework;

namespace Client.Logic.Plants
{
    public class Cosmo : Plant
    {
        public Cosmo(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(0, 0, 32, 32);
        }
    }
}
