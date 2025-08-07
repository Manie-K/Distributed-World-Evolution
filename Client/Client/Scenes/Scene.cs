using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public interface IScene
    {
        public void Load();
        public void Update(GameTime gametime);
        public void Draw(SpriteBatch spriteBatch, Camera2D camera);
    }
}
