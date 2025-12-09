using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class WhiteRabbit : Character
    {
        public WhiteRabbit(Vector2 position, int maxHealth, Color color)
            : base(position, color, 192, 192, 150f, maxHealth)
        {
            am = new AnimationManager(16);
            SpriteDrawingOffset = new Vector2(-80, -100);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (am.CheckDeadAnimation()) return;

            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture((int) am.ActiveAnimation, 16), GetPosition(), GetSourceRectangle(), Color.White);

            HealthBar.Draw(spriteBatch, Position, -3, -20);
        }
    }
}
