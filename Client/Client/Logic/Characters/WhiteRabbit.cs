using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class WhiteRabbit : Character
    {
        public WhiteRabbit(Vector2 position, Color color)
            : base(position, color, 192, 192, 150f, 4, 12)
        {
            am = new AnimationManager(16);
            SpriteDrawingOffset = new Vector2(-80, -100);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 16), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
