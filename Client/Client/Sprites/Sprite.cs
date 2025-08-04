using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Sprite
    {
        public Texture2D Texture;
        public Vector2 Position;

        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public virtual void Update() { }
        
        public virtual void Draw(SpriteBatch spritebatch) { }
    }
}
