using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Boar : Character
    {
        public Boar(Vector2 position, Color color)
            : base(position, color, 160, 160, 150f, 4, 12)
        {
            am = new AnimationManager(12);
            SpriteDrawingOffset = new Vector2(-65, -75);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 12), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
