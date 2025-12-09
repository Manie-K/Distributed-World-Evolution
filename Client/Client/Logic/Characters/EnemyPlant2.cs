using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class EnemyPlant2 : Character
    {
        public EnemyPlant2(Vector2 position, int maxHealth, Color color)
            : base(position, color, 100, 100, 150f, maxHealth)
        {
            am = new AnimationManager(4);
            SpriteDrawingOffset = new Vector2(-32, -47);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (am.CheckDeadAnimation()) return;

            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture((int) am.ActiveAnimation, 4), GetPosition(), GetSourceRectangle(), Color.White);
            HealthBar.Draw(spriteBatch, Position, -1, -35);
        }
    }
}
