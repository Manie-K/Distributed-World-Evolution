using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class Sprite
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 SpriteDrawingOffset;

        public Sprite(Texture2D texture, Vector2 position, Vector2 spriteDrawingOffset = default)
        {
            Texture = texture;
            Position = position;
            SpriteDrawingOffset = spriteDrawingOffset;
        }

        public virtual void Update() { }
        
        public virtual void Draw(SpriteBatch spritebatch) { }
    }
}
