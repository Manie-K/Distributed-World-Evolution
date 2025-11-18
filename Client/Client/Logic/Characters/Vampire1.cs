using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Vampire1 : Character
    {
        public Vampire1(Vector2 position, Color color)
            : base(position, color, 110, 110, 150f, 4, 12)
        {
            am = new AnimationManager(9);
            SpriteDrawingOffset = new Vector2(-37, -48);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 9), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
