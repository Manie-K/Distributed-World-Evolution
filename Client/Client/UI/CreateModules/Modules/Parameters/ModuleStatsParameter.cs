using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class ModuleStatsParameter : ModuleParameter
    {
        private StatsBox statsBox;
        public ModuleStatsParameter(ContentManager contentManager, Vector2 position, int type, string name, string description) : base(contentManager, position, type, name, description)
        {
            statsBox = new StatsBox(contentManager.Load<Texture2D>("UI/CreateModules/Stats/box_Button"), contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), 100, position);
            moduleName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), name, true, new Vector2(position.X + 80, position.Y), 205, 43);
            moduleName.SetTextColor(Color.White);
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

        public override string GetDescription()
        { 
            return Description;
        }
    }
}
