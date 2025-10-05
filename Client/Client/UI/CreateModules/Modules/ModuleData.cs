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
        private Texture2D texture;
        private Rectangle rect;
        public ModuleType type;
        public string Name;
        public int GraphicIndex;

        public ModuleData(Texture2D texture, string name, ModuleType type, int graphicIndex)
        {
            this.texture = texture;
            rect = new Rectangle(193, 210, 368, 200);
            Name = name;
            this.type = type;
            GraphicIndex = graphicIndex;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }

    }
}
