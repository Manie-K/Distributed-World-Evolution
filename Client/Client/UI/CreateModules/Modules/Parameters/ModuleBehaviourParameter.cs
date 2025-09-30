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
    public class ModuleBehaviourParameter : ModuleParameter
    {
        private BehaviourBox behaviourBox;

        public ModuleBehaviourParameter(ContentManager contentManager, Vector2 position, List<string> values, int type, string name, string description) : base(contentManager, position, type, name, description)
        {
            behaviourBox = new BehaviourBox(contentManager.Load<Texture2D>("UI/CreateModules/Behaviours/switch_Button"), contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                            values, position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            checkBox.Draw(spriteBatch);
            moduleName.Draw(spriteBatch);
            behaviourBox.Draw(spriteBatch);
        }

        public override void CheckLeftClick(Vector2 position)
        {
            checkBox.CheckLeftClick(position);
            behaviourBox.CheckLeftClick(position);
        }

        public override void Update(Vector2 position)
        {
            checkBox.Update(position);
        }
    }
}
