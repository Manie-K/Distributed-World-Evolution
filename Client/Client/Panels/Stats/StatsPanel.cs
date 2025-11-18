using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Panels.Stats
{
    public class StatsPanel
    {
        private Texture2D panelBackground;
        private Bar healthBar;
        private Bar hungerBar;

        private Vector2 position;

        public StatsPanel(GameManager manager, Vector2 position)
        {
            panelBackground = manager.ContentManager.Load<Texture2D>("Panels/StatsPanel/StatsBG");
            this.position = position;   
            healthBar = new Bar(manager.ContentManager.Load<Texture2D>("Panels/StatsPanel/HealthBar"), new Vector2(position.X + 53, position.Y + 18));
            hungerBar = new Bar(manager.ContentManager.Load<Texture2D>("Panels/StatsPanel/HungerBar"), new Vector2(position.X + 53, position.Y + 52));
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(panelBackground, new Rectangle((int) position.X, (int) position.Y, 200, 94), Color.White);
            healthBar.Draw(spriteBatch);
            hungerBar.Draw(spriteBatch);
        }

        public void SetHungerBar(float range)
        {
            hungerBar.SetRangeBar(range);
        }

        public void SetHealthBar(float range)
        {
            healthBar.SetRangeBar(range);
        }
    }
}
