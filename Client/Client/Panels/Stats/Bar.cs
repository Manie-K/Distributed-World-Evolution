using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Panels.Stats
{
    public class Bar
    {
        private Texture2D barTexture;
        private Vector2 position;
        private float rangeBar;

        public Bar(Texture2D barTexture, Vector2 position)
        {
            this.barTexture = barTexture;
            this.position = position;
            rangeBar = 1.0f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                barTexture,
                new Rectangle((int)position.X, (int)position.Y, (int)(129 * rangeBar), 24),
                new Rectangle(0, 0, (int)(129 * rangeBar), 24),
                Color.White);
        }

        public void SetRangeBar(float range)
        {
            if (range < 0.0f) rangeBar = 0.0f;
            else if (range > 1.0f) rangeBar = 1.0f;
            else rangeBar = range;
        }
    }
}

