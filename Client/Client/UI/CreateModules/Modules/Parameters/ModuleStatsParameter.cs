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
    public class ModuleStatsParameter : ModuleParameter
    {
        private StatsBox statsBox;
        public ModuleStatsParameter(ContentManager contentManager, Vector2 position, int type, string name, string description) : base(contentManager, position, type, name, description)
        {
            statsBox = new StatsBox(contentManager.Load<Texture2D>("UI/CreateModules/Stats/box_Button"), contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), 100, position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moduleName.Draw(spriteBatch);
            statsBox.Draw(spriteBatch);
        }

        public override bool CheckLeftClick(Vector2 position)
        {
            return statsBox.CheckLeftClick(position);
        }

        public override void Update(Vector2 position)
        {
            statsBox.Update();
        }
        public override int GetValue()
        {
            return statsBox.GetParameter();
        }
    }
}
