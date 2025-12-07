using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Logic.Plants
{
    public abstract class Plant
    {
        public Rectangle SourceRect;
        public Vector2 Position;
        public bool IsDead;

        public Plant(Vector2 position)
        {
            Position = position;
            IsDead = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetFlowersSpritesheet(), Position, SourceRect, Color.White);
        }
    }
}
