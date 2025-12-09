using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Orc2 : Character
    {
        public Orc2(Vector2 position, int maxHealth, Color color)
            : base(position, color, 100, 100, 150f, maxHealth)
        {
            am = new AnimationManager(1);
            SpriteDrawingOffset = new Vector2(-35, -37);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (am.CheckDeadAnimation()) return;

            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture((int) am.ActiveAnimation, 1), GetPosition(), GetSourceRectangle(), Color.White);
            HealthBar.Draw(spriteBatch, Position, -3, -30);
        }
    }
}
