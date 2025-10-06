using Client.UI.CreateLobby.Parameters;
using Client.UI.CreateModules.Modules.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;


namespace Client
{
    public class CreateLobbyRow
    {
        private ModuleData moduleData;
        private CheckBox checkBox;
        private Text creatureName;
        private Rectangle bounds;

        private Texture2D verifyIcon;
        private Rectangle iconBounds;

        public CreateLobbyRow(SpriteFont font, Texture2D checkboxBG, Texture2D checkboxApply, Texture2D verifyIcon, ModuleData module, Vector2 position, int width, int height)
        {

            moduleData = module;
            creatureName = new Text(font, module.ModuleName, true, new Vector2(position.X + 70, position.Y), width - 70, height);

            bounds = new Rectangle((int)position.X + 70, (int)position.Y, width, height);

            this.checkBox = new CheckBox(checkboxBG, checkboxApply, new Vector2(position.X + 22, position.Y + 17), 25, 24, Color.Gold);

            this.verifyIcon = verifyIcon;
            iconBounds = new Rectangle((int) position.X + width + 15, (int) position.Y + 13, 33, 32);
        }

        public bool Update(Vector2 cursorPosition, bool isPressed, bool isSelected)
        {
            checkBox.Update(cursorPosition);
            if (isPressed) checkBox.CheckLeftClick(cursorPosition);

            if (isSelected)
            {
                creatureName.SetTextColor(Color.Cyan);
            }
            else if (bounds.Contains(cursorPosition))
            {
                creatureName.SetTextColor(Color.Gold);
                return true;
            }
            else
            {
                creatureName.SetTextColor(Color.White);
            }
            return false;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            creatureName.Draw(spriteBatch);
            checkBox.Draw(spriteBatch);
            if(moduleData.IsOfficial) spriteBatch.Draw(verifyIcon, iconBounds, Color.White);
        }

        public ModuleData GetData()
        {
            return moduleData;
        }
    }
}
