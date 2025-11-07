using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Vampire3 : Character
    {
        public Vampire3(Vector2 position, Color color)
            : base(position, color, 110, 110, 150f, 4, 12)
        {
            am = new AnimationManager(11);
            SpriteDrawingOffset = new Vector2(-37, -48);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 11), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
