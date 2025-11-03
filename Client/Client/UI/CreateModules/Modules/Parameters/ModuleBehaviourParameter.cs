using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.DTOs.ModuleDTO;
using System.Collections.Generic;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class ModuleBehaviourParameter : ModuleParameter
    {
        private CheckBox checkBox;
        private BehaviourBox behaviourBox;

        public ModuleBehaviourParameter(ContentManager contentManager, Vector2 position, List<BehaviourDTO> values, int type, string name, string description) : base(contentManager, position, type, name, description)
        {
            behaviourBox = new BehaviourBox(contentManager.Load<Texture2D>("UI/CreateModules/Behaviours/switch_Button"), contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                            values, position);
            this.checkBox = new CheckBox(contentManager.Load<Texture2D>("UI/CreateModules/CheckBox/Box_Apply"),
                                       contentManager.Load<Texture2D>("UI/CreateModules/CheckBox/Box_Apply_Check"), new Vector2(position.X + 50, position.Y + 10), 25, 24, Color.Gold);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            checkBox.Draw(spriteBatch);
            moduleName.Draw(spriteBatch);
            behaviourBox.Draw(spriteBatch);
        }

        public override bool CheckLeftClick(Vector2 position)
        {
            checkBox.CheckLeftClick(position);
            return behaviourBox.CheckLeftClick(position);
        }

        public override void Update(Vector2 position)
        {
            checkBox.Update(position);
        }

        public override int GetValue()
        {
            return behaviourBox.GetSelectedBehaviourID();
        }

        public bool IsPicked()
        {
            return checkBox.GetValue();
        }

        public override string GetDescription()
        { 
            return behaviourBox.GetBehaviourDescription();
        }
    }
}
