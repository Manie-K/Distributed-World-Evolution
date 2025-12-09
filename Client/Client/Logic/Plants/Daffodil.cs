using Microsoft.Xna.Framework;

namespace Client.Logic.Plants
{
    public class Daffodil : Plant
    {
        public Daffodil(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(32, 0, 32, 32);
        }
    }
}
