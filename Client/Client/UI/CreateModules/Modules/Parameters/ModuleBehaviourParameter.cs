using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.DTOs.ModuleDTO;
using System.Collections.Generic;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class ModuleBehaviourParameter : ModuleParameter
    {
        private BehaviourBox behaviourBox;

        public ModuleBehaviourParameter(ContentManager contentManager, Vector2 position, List<BehaviourDTO> values, int type, string name, string description) : base(contentManager, position, type, name, description)
        {
            behaviourBox = new BehaviourBox(contentManager.Load<Texture2D>("UI/CreateModules/Behaviours/switch_Button"), contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                            values, position);
            moduleName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), name, true, new Vector2(position.X + 30, position.Y), 195, 43);
            moduleName.SetTextColor(Color.White);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moduleName.Draw(spriteBatch);
            behaviourBox.Draw(spriteBatch);
        }

        public override bool CheckLeftClick(Vector2 position)
        {
            return behaviourBox.CheckLeftClick(position);
        }

        public override void Update(Vector2 position) {}

        public override int GetValue()
        {
            return behaviourBox.GetSelectedBehaviourID();
        }

        public override string GetDescription()
        { 
            return behaviourBox.GetBehaviourDescription();
        }
    }
}
