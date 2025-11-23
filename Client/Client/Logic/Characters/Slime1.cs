using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Slime1 : Character
    {
        public Slime1(Vector2 position, int maxHealth, Color color)
            : base(position, color, 100, 100, 150f, maxHealth)
        {
            am = new AnimationManager(6);
            SpriteDrawingOffset = new Vector2(-35, -37);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 6), GetPosition(), GetSourceRectangle(), Color.White);
            HealthBar.Draw(spriteBatch, Position, -3, -15);
        }
    }
}
