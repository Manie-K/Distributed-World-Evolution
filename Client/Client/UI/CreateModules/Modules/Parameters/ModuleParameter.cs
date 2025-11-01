using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public abstract class ModuleParameter
    {
        protected Text moduleName;
        public string Description;
        public int Type;

        public ModuleParameter(ContentManager contentManager, Vector2 position, int type, string name, string description)
        {
            this.Type = type;
            this.Description = description;
            this.moduleName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), name, true, new Vector2(position.X + 85, position.Y), 160, 43);
          

            moduleName.SetTextColor(Color.White);
        }

        public abstract void Update(Vector2 position);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract bool CheckLeftClick(Vector2 position);
        public abstract int GetValue();
    }
}
