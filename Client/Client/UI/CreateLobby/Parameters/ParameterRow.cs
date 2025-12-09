using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.CreateLobby.Parameters
{
    public class ParameterRow
    {
        private Text parameterName;
        private Text parameterValue;
        private Texture2D background;
        private Rectangle rect;

        public string Description;

        public ParameterRow(ContentManager contentManager, string name, string value, string description, Vector2 position, int type)
        {
            parameterName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), name, true, position, 196, 46);
            Description = description;
            
            if(type==0)
            {
                background = contentManager.Load<Texture2D>("UI/CreateModules/Stats/box_Button");
                rect = new Rectangle((int)position.X + 220, (int)position.Y, 86, 46);
                parameterValue = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), value, true, new Vector2(position.X + 220, position.Y), 86, 46);
            }
            else if (type==1)
            {
                background = contentManager.Load<Texture2D>("UI/CreateLobby/Text_Box");
                rect = new Rectangle((int)position.X + 193, (int)position.Y, 141, 46);
                parameterValue = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), value, true, new Vector2(position.X + 193, position.Y), 141, 46);
            }

            parameterValue.SetTextColor(Color.White);
            parameterName.SetTextColor(Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            parameterName.Draw(spriteBatch);

            spriteBatch.Draw(background, rect, Color.White);
            parameterValue.Draw(spriteBatch);
        }

        public bool CheckLeftClick(Vector2 position)
        {
            return rect.Contains(position);
        }

        public string GetDescription()
        {
            return Description;
        }
    }
}
