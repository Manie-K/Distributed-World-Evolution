using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateModules.Modules
{
    public enum ModuleType
    {
        Animal,
        Plant
    }

    public class ModuleData
    {
        public Texture2D Texture;
        private Rectangle Rect;
        public ModuleType type;
        public string Name;

        public ModuleData(Texture2D texture, string name, ModuleType type)
        {
            Texture = texture;
            Rect = new Rectangle(193, 210, 368, 200);
            Name = name;
            this.type = type;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color.White);
        }

    }
}
