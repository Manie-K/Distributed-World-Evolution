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
        protected CheckBox checkBox;
        protected Text moduleName;
        protected string description;
        protected int type;

        public ModuleParameter(ContentManager contentManager, Vector2 position, int type, string name, string description)
        {
            this.type = type;
            this.description = description;
            this.moduleName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), name, true, new Vector2(position.X + 85, position.Y), 160, 43);
            this.checkBox = new CheckBox(contentManager.Load<Texture2D>("UI/CreateModules/CheckBox/Box_Apply"),
                                         contentManager.Load<Texture2D>("UI/CreateModules/CheckBox/Box_Apply_Check"), new Vector2(position.X + 50, position.Y + 10), 25, 24, Color.Gold);

            moduleName.SetTextColor(Color.White);
        }

        public abstract void Update(Vector2 position);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void CheckLeftClick(Vector2 position);
    }
}
