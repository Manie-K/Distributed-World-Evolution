using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Panels
{
    public class HealthBar
    {
        private Texture2D healthFrame;
        private Texture2D healthBar;
        private float rangeBar;

        public HealthBar(GameManager manager)
        {
            healthFrame = manager.ContentManager.Load<Texture2D>("Panels/HealthBar/HealthBar");
            healthBar = manager.ContentManager.Load<Texture2D>("Panels/HealthBar/HealthBar_RedArea");
            rangeBar = 1.0f;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(healthFrame, new Rectangle(10, 10, 214, 78), Color.White);
            spriteBatch.Draw(
                healthBar,
                new Rectangle(28, 36, (int)(175 * rangeBar), 26),
                new Rectangle(0, 0, (int)(175 * rangeBar), 26),
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

