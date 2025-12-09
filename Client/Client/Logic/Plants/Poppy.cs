using Microsoft.Xna.Framework;

namespace Client.Logic.Plants
{
    public class Poppy : Plant
    {
        public Poppy(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(64, 32, 32, 32);
        }
    }
}
