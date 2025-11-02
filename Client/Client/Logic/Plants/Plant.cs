using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.Helpers;

namespace Client.Logic.Plants
{
    public abstract class Plant
    {
        public Rectangle SourceRect;
        public Vector2 Position;

        public Plant(Vector2 position)
        {
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetFlowersSpritesheet(), Position, SourceRect, Color.White);
        }
    }
}
