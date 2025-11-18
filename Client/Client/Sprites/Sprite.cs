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

        public Vector2 GetPosition()
        {
            return new Vector2((int)(Position.X + SpriteDrawingOffset.X), (int)(Position.Y + SpriteDrawingOffset.Y));
        }

        public virtual void Update() { }
        
        public virtual void Draw(SpriteBatch spritebatch) { }
    }
}
