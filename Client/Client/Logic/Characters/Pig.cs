using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Pig : Character
    {
        public Pig(Vector2 position, Color color)
            : base(position, color, 173, 173, 150f, 4, 12)
        {
            am = new AnimationManager(14);
            SpriteDrawingOffset = new Vector2(-72, -80);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 14), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
