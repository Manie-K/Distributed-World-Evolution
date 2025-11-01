using Microsoft.Xna.Framework;


namespace Client.Logic.Plants
{
    public class LilyOfTheValley : Plant
    {
        public LilyOfTheValley(Vector2 position) : base(position)
        {
            SourceRect = new Rectangle(160, 0, 32, 32);
        }
    }
}
