using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.MapSelection
{
    public class MapData
    {
        public Texture2D Texture;
        private Rectangle rect;
        public string Name;

        public MapData(Texture2D texture, string name)
        {
            Texture = texture;
            rect = new Rectangle(377, 222, 526, 277);
            Name = name;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, rect, Color.White);
        }


    }
}
