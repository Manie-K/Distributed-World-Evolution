using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Orc3 : Character
    {
        public Orc3(Vector2 position, Color color)
           : base(position, color, 100, 100, 150f, 4, 12)
        {
            am = new AnimationManager(2);
            SpriteDrawingOffset = new Vector2(-35, -37);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 2), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
