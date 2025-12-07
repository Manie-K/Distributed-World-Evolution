using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class ColoredSprite : ScaledSprite
    {
        public Color color;

        public ColoredSprite(Texture2D texture, Vector2 position, int width, int height, Color color, Vector2 spriteDrawingOffset = default) 
            : base(texture, position, width, height, spriteDrawingOffset)
        {
            this.color = color;
        }

        public override void Draw(SpriteBatch spritebatch) {}
    }
}